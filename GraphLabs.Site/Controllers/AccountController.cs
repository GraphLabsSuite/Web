using System;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.Dal.Ef.Extensions;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.Models;
using GraphLabs.Site.Models.Account;
using GraphLabs.Site.Models.Groups;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Контроллер учётных записей </summary>
    [GLAuthorize]
    public class AccountController : GraphLabsController
    {
        #region Зависимости

        private readonly IMembershipEngine _membershipEngine;
        private readonly IAuthenticationSavingService _authSavingService;
        private readonly IEntityQuery _query;
        private readonly ISystemDateService _dateService;

        #endregion

        /// <summary> Контроллер учётных записей </summary>
        public AccountController(
            IMembershipEngine membershipEngine,
            IAuthenticationSavingService authSavingService,
            IEntityQuery query,
            ISystemDateService dateService)
        {
            _membershipEngine = membershipEngine;
            _authSavingService = authSavingService;
            _query = query;
            _dateService = dateService;
        }


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
                var success = model.ForceMode
                    ? _membershipEngine.TryForceLogin(model.Email, model.Password, Request.GetClientIP(), out session)
                    : _membershipEngine.TryLogin(model.Email, model.Password, Request.GetClientIP(), out session);

                if (success == LoginResult.Success)
                {
                    _authSavingService.SignIn(model.Email, session);
                    return RedirectToLocal(returnUrl);
                }

                if (success == LoginResult.LoggedInWithAnotherSessionId)
                {
                    model.ForceMode = true;
                    return View(model);
                }

                if (success == LoginResult.InvalidLoginPassword)
                {
                    model.ForceMode = false;
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError(STD_VALIDATION_MSG_KEY, UserMessages.LOGIN_PASSWORD_NOT_FOUND);
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
            var sessionInfo = _authSavingService.GetSessionInfo();
            if (!sessionInfo.IsEmpty())
            {
                _membershipEngine.Logout(sessionInfo.Email, sessionInfo.SessionGuid, Request.GetClientIP());
            }
            _authSavingService.SignOut();

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

        public sealed class NameValueModel
        {
            public long Id { get; set; }

            public string Name { get; set; }

            public NameValueModel(long id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        private void FillGroups(object selectedValue = null)
        {
            var groups = _query.OfEntities<Group>()
                .Where(g => g.IsOpen)
                .ToArray()
                .Select(t => new NameValueModel(t.Id, t.GetName(_dateService)))
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
                var success = _membershipEngine.RegisterNewStudent(reg.Email, reg.Name, reg.FatherName, reg.Surname, reg.Password, reg.IdGroup);
                if (success)
                    return RedirectToAction("Index", "Home", new
                        {
                            StatusMessage = UserMessages.REGISTRATION_COMPLETE,
                            StatusDescription = UserMessages.AccountController_Register_Вы_сможете_войти_когда_преподаватель_подтвердит_вашу_учётную_запись_о_чём_будет_сообщено_дополнительно_по_e_mail
                        });
                else
                {
                    ModelState.AddModelError(STD_VALIDATION_MSG_KEY, UserMessages.DUPLICATE_LOGIN);
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
                var success = model.ConfirmPassword == model.NewPassword;
                var sessionInfo = _authSavingService.GetSessionInfo();
                
                success &= _membershipEngine.ChangePassword(
                    sessionInfo.Email,
                    sessionInfo.SessionGuid,
                    Request.GetClientIP(),
                    model.OldPassword,
                    model.NewPassword);

                if (success)
                {
                    return RedirectToAction("Manage", new { StatusMessage = UserMessages.PASSWORD_CHANGED });
                }

                ModelState.AddModelError(STD_VALIDATION_MSG_KEY, UserMessages.ILLEGAL_PASSWORD);
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
