using backend.vitaaid.com.Model;
using System;
using System.Linq;
using System.Web.Mvc;

namespace backend.vitaaid.com.Controllers
{
    public class AccountController : BaseController
    {
        // GET: /Account/SignIn
        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new SignInViewModel() { RememberMe = true });
        }

        // POST: /Account/SignIn
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(SignInViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // DXCOMMENT: You Authentication logic
            if (AuthHelper.SignIn(model.UserName, model.Password))
                return RedirectToAction("Index", "Member");
            else
            {
                SetErrorText("Invalid login attempt.");
                ModelState.AddModelError(string.Empty, ViewBag.GeneralError);
                return View(model);
            }
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // DXCOMMENT: Your Registration logic 
            }

            return View(model);
        }

        // POST: /Account/SignOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            AuthHelper.SignOut(); // DXCOMMENT: Your Signing out logic
            return RedirectToAction("SignIn", "Account");
        }

        public ActionResult UserMenuItemPartial()
        {
            return PartialView("UserMenuItemPartial", AuthHelper.GetLoggedInUserInfo());
        }
    }
}