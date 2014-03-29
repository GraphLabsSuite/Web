using System;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.Models;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Контроллер учётных записей </summary>
    [GLAuthorize]
    public class AccountController : GraphLabsController
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();

        #region Зависимости

        private IMembershipEngine MembershipEngine
        {
            get { return DependencyResolver.GetService<IMembershipEngine>(); }
        }

        private IAuthenticationSavingService AuthSavingService
        {
            get { return DependencyResolver.GetService<IAuthenticationSavingService>(); }
        }

        private IGroupRepository GroupRepository
        {
            get { return DependencyResolver.GetService<IGroupRepository>(); }
        }

        #endregion


        #region Login

        //
        // GET: /Account/Login
        /// <summary> Начальная отрисовка формы входа </summary>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }

            SetReturnUrl(returnUrl);
            return View();
        }

        private void SetReturnUrl(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
        }

        //
        // POST: /Account/Login
        /// <summary> Вход </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AuthModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }

            if (ModelState.IsValid)
            {
                Guid session;
                var success = MembershipEngine.TryLogin(model.Email, model.Password, Request.GetClientIP(), out session);
                if (success)
                {
                    AuthSavingService.SignIn(model.Email, session);
                    return RedirectToLocal(returnUrl);
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", UserMessages.LOGIN_PASSWORD_NOT_FOUND);
            return View(model);
        }

        #endregion

        //
        // POST: /Account/LogOff
        /// <summary> Выход </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            var sessionInfo = AuthSavingService.GetSessionInfo();
            if (!sessionInfo.IsEmpty())
            {
                MembershipEngine.Logout(sessionInfo.Email, sessionInfo.SessionGuid, Request.GetClientIP());
            }
            AuthSavingService.SignOut();

            return RedirectToHome();
        }


        #region Register

        //
        // GET: /Account/Register
        /// <summary> Начальная отрисовка формы регистрации </summary>
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToHome();
            }

            FillGroups();
            return View();
        }

        private void FillGroups(object selectedValue = null)
        {
            var groups = GroupRepository.GetOpenGroups()
                .Select(t => new GroupModel(t))
                .ToArray();
            ViewBag.GroupsList = new SelectList(groups, "Id", "Name", selectedValue);
        }

        //
        // POST: /Account/Register
        /// <summary> Submit регистрации </summary>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistrationModel reg)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToHome();
            }

            if (ModelState.IsValid)
            {
                var success = MembershipEngine.RegisterNewStudent(reg.Email, reg.Name, reg.FatherName, reg.Surname, reg.Password, reg.IdGroup);
                if (success)
                    return RedirectToAction("Index", "Home", new { Message = UserMessages.REGISTRATION_COMPLETE });
                else
                {
                    ModelState.AddModelError("", UserMessages.DUPLICATE_LOGIN);
                }
            }

            FillGroups(reg.IdGroup);
            return View(reg);
        }

        #endregion

        //
        // GET: /Account/Manage
        /// <summary> Управление аккаунтом - смена пароля </summary>
        public ActionResult Manage(string message)
        {
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
            if (ModelState.IsValid)
            {
                throw new NotImplementedException();
                //var user = Session.GetUser(_ctx);
                //var HashCalculator = DependencyResolver.GetService<IHashCalculator>();
                //var oldHash = HashCalculator.Crypt(model.OldPassword);
                //if (oldHash == user.PasswordHash)
                //{
                //    var salt = HashCalculator.GenerateSalt();
                //    var hash = HashCalculator.Crypt(model.NewPassword);

                //    user.PasswordHash = hash;
                //    _ctx.SaveChanges();

                //    return RedirectToAction("Manage", new { Message = UserMessages.PASSWORD_CHANGED });
                //}
             
                ModelState.AddModelError("", UserMessages.ILLEGAL_PASSWORD);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private RedirectToRouteResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
