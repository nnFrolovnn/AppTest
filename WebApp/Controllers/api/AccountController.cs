using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using Microsoft.Owin;
using WebApp.Helpers;
using System.Threading.Tasks;
using WebApp.Models;
using System.Security.Claims;

namespace WebApp.Controllers.api
{
    [Authorize]
    
    public class AccountController : ApiController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(loginViewModel.Email, loginViewModel.Password);
                if (user != null)
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                                DefaultAuthenticationTypes.ApplicationCookie);

                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true,
                    }, claim);


                    return Ok();
                }
                else
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = ConverterVM.ToUser(registerViewModel);

                IdentityResult result = await UserManager.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "user");

                    //todo confirmation
                    //var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code },
                    //           protocol: Request.Url.Scheme);

                    //await UserManager.SendEmailAsync(user.Id, "Подтверждение email",
                    //           "Для завершения регистрации перейдите по ссылке:: <a href=\""
                    //                                           + callbackUrl + "\">завершить регистрацию</a>");
                    return Ok();
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return BadRequest(ModelState);
        }


        [AllowAnonymous]
        public async Task<IHttpActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest("userId or code is empty");
            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if(result.Succeeded)
            {
                return Ok();
            }

            return BadRequest("smth wrong");      
        }


        [HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IHttpActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Ok();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return Request.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        #endregion
    }
}
