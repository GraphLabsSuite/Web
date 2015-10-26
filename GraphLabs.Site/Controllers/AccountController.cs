using System;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.Models;
using GraphLabs.Site.Models.Account;
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
        private readonly IGroupRepository _groupRepository;
        private readonly ISystemDateService _dateService;

        #endregion

        /// <summary> Контроллер учётных записей </summary>
        public AccountController(
            IMembershipEngine membershipEngine,
            IAuthenticationSavingService authSavingService,
            IGroupRepository groupRepository,
            ISystemDateService dateService)
        {
            _membershipEngine = membershipEngine;
            _authSavingService = authSavingService;
            _groupRepository = groupRepository;
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
                var success = _membershipEngine.TryLogin(model.Email, model.Password, Request.GetClientIP(), out session);
                if (success)
                {
                    _authSavingService.SignIn(model.Email, session);
                    return RedirectToLocal(returnUrl);
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

        private void FillGroups(object selectedValue = null)
        {
            var groups = _groupRepository.GetOpenGroups()
                .Select(t => new GroupModel(t, _dateService))
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
