using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Utils;
using GraphLabs.Site.Models;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Контроллер учётных записей </summary>
    public class AccountController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();

        //
        // GET: /Account/Login
        /// <summary> Начальная отрисовка формы входа </summary>
        public ActionResult Login(string returnUrl)
        {
            if (this.IsAuthenticated(_ctx))
                RedirectToLocal(returnUrl);
            this.AllowAnonymous(_ctx);

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        /// <summary> Вход </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AuthModel model, string returnUrl)
        {
            if (this.IsAuthenticated(_ctx))
                RedirectToLocal(returnUrl);
            else if (ModelState.IsValid && this.Login(_ctx, model.Email, model.Password))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", UserMessages.LOGIN_PASSWORD_NOT_FOUND);
            this.AllowAnonymous(_ctx);
            return View(model);
        }

        //
        // POST: /Account/LogOff
        /// <summary> Выход </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            this.Logout(_ctx);

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        /// <summary> Начальная отрисовка формы регистрации </summary>
        public ActionResult Register()
        {
            if (this.IsAuthenticated(_ctx))
                RedirectToAction("Index", "Home");
            this.AllowAnonymous(_ctx);

            FillGroups();
            return View();
        }

        //
        // POST: /Account/Register
        /// <summary> Submit регистрации </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistrationModel reg)
        {
            if (this.IsAuthenticated(_ctx))
                RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var salt = HashCalculator.GenerateRandomSalt();
                var student = new Student
                {
                    PasswordHash = HashCalculator.GenerateSaltedHash(reg.Password, salt),
                    HashSalt = salt,
                    Name = reg.Name,
                    Surname = reg.Surname,
                    FatherName = reg.FatherName,
                    Email = reg.Email,
                    IsVerified = false
                };
                _ctx.Users.Add(student);
                var group = _ctx.Groups.Single(g => g.Id == reg.ID_Group);
                group.Students.Add(student);
                try
                {
                    _ctx.SaveChanges();
                    return RedirectToAction("Index", "Home", new { Message = UserMessages.REGISTRATION_COMPLETE });
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", UserMessages.DUPLICATE_LOGIN);
                }
            }

            this.AllowAnonymous(_ctx);
            FillGroups(reg.ID_Group);
            return View(reg);
        }

        private void FillGroups(object selectedValue = null)
        {
            var dateService = ServiceLocator.Locator.Get<ISystemDateService>();
            var groups = _ctx.Groups.Where(g => g.IsOpen)
                .ToArray() // делаем ToArray(), т.к. GetName - метод расширения, и его невозможно выполнить в БД.
                .OrderBy(g => g.GetName(dateService));

            ViewBag.ID_Group = new SelectList(groups, "ID_Group", "Name", selectedValue);
        }

        //
        // GET: /Account/Manage
        /// <summary> Управление аккаунтом - смена пароля </summary>
        public ActionResult Manage(string message)
        {
            if (!this.IsAuthenticated(_ctx))
            {
                ViewBag.ReturnUrl = Url.Action("Manage");
                return RedirectToAction("Login", new { Message = UserMessages.AUTH_REQUIRED });
            }

            ViewBag.StatusMessage = message;
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        /// <summary> Смена пароля - submit </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ChangePasswordModel model)
        {
            if (!this.IsAuthenticated(_ctx))
            {
                ViewBag.ReturnUrl = Url.Action("Manage");
                return RedirectToAction("Login", new { Message = UserMessages.AUTH_REQUIRED });
            }

            if (ModelState.IsValid)
            {
                var user = Session.GetUser(_ctx);

                var oldHash = HashCalculator.GenerateSaltedHash(model.OldPassword, user.HashSalt);
                if (oldHash == user.PasswordHash)
                {
                    var salt = HashCalculator.GenerateRandomSalt();
                    var hash = HashCalculator.GenerateSaltedHash(model.NewPassword, salt);

                    user.HashSalt = salt;
                    user.PasswordHash = hash;
                    _ctx.SaveChanges();

                    return RedirectToAction("Manage", new { Message = UserMessages.PASSWORD_CHANGED });
                }
             
                ModelState.AddModelError("", UserMessages.ILLEGAL_PASSWORD);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #region Helpers

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
            base.Dispose(disposing);
        }
    }
}
