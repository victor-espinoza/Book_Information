﻿///////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MvcBook.Models;

namespace MvcBook.Controllers {
   [Authorize]
   public class ManageController : Controller {
      private ApplicationSignInManager _signInManager;
      private ApplicationUserManager _userManager;

      public ManageController() {

      }//close ManageController()


      public ManageController(ApplicationUserManager userManager, ApplicationSignInManager
       signInManager) {
         UserManager = userManager;
         SignInManager = signInManager;
      }//close ManageController(...)


      public ApplicationSignInManager SignInManager {
         get {
            return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
         }//end get
         private set {
            _signInManager = value;
         }//end set
      }//close SignInManager property


      public ApplicationUserManager UserManager {
         get {
            return _userManager ?? HttpContext.GetOwinContext().
             GetUserManager<ApplicationUserManager>();
         }//end get
         private set {
            _userManager = value;
         }//end set
      }//close UserManager property

      //
      // GET: /Manage/Index
      public async Task<ActionResult> Index(ManageMessageId? message) {
         ViewBag.StatusMessage =
             message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
             : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
             : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication " +
               "provider has been set."
             : message == ManageMessageId.Error ? "An error has occurred."
             : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
             : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
             : "";

         var userId = User.Identity.GetUserId();
         var model = new IndexViewModel {
            HasPassword = HasPassword(),
            PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
            TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
            Logins = await UserManager.GetLoginsAsync(userId),
            BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
         };
         return View(model);
      }//close Index(...)

      //
      // POST: /Manage/RemoveLogin
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey) {
         ManageMessageId? message;
         var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new
          UserLoginInfo(loginProvider, providerKey));
         if (result.Succeeded) {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
               await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            message = ManageMessageId.RemoveLoginSuccess;
         }//end if
         else
            message = ManageMessageId.Error;

         return RedirectToAction("ManageLogins", new { Message = message });
      }//close RemoveLogin(...)

      //
      // GET: /Manage/AddPhoneNumber
      public ActionResult AddPhoneNumber() {
         return View();
      }//close AddPhoneNumber()

      //
      // POST: /Manage/AddPhoneNumber
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model) {
         if (!ModelState.IsValid)
            return View(model);

         // Generate the token and send it
         var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(),
          model.Number);
         if (UserManager.SmsService != null) {
            var message = new IdentityMessage {
               Destination = model.Number,
               Body = "Your security code is: " + code
            };
            await UserManager.SmsService.SendAsync(message);
         }//end if

         return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
      }//close AddPhoneNumber(...)

      //
      // POST: /Manage/EnableTwoFactorAuthentication
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> EnableTwoFactorAuthentication() {
         await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
         var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
         if (user != null)
            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

         return RedirectToAction("Index", "Manage");
      }//close EnableTwoFactorAuthentication()

      //
      // POST: /Manage/DisableTwoFactorAuthentication
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> DisableTwoFactorAuthentication() {
         await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
         var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
         if (user != null)
            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

         return RedirectToAction("Index", "Manage");
      }//close DisableTwoFactorAuthentication()

      //
      // GET: /Manage/VerifyPhoneNumber
      public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber) {
         var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(),
          phoneNumber);
         // Send an SMS through the SMS provider to verify the phone number
         return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel {
            PhoneNumber = phoneNumber
         });
      }//close VerifyPhoneNumber(...)

      //
      // POST: /Manage/VerifyPhoneNumber
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model) {
         if (!ModelState.IsValid)
            return View(model);

         var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(),
          model.PhoneNumber, model.Code);
         if (result.Succeeded) {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
               await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
         }//end if
         // If we got this far, something failed, redisplay form
         ModelState.AddModelError("", "Failed to verify phone");
         return View(model);
      }//close VerifyPhoneNumber(...)

      //
      // GET: /Manage/RemovePhoneNumber
      public async Task<ActionResult> RemovePhoneNumber() {
         var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
         if (!result.Succeeded)
            return RedirectToAction("Index", new { Message = ManageMessageId.Error });

         var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
         if (user != null)
            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

         return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
      }//close RemovePhoneNumber()

      //
      // GET: /Manage/ChangePassword
      public ActionResult ChangePassword() {
         return View();
      }//close ChangePassword()

      //
      // POST: /Manage/ChangePassword
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model) {
         if (!ModelState.IsValid)
            return View(model);

         var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(),
          model.OldPassword, model.NewPassword);
         if (result.Succeeded) {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
               await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
         }//end if
         AddErrors(result);
         return View(model);
      }//close ChangePassword(...)

      //
      // GET: /Manage/SetPassword
      public ActionResult SetPassword() {
         return View();
      }//close SetPassword() 

      //
      // POST: /Manage/SetPassword
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> SetPassword(SetPasswordViewModel model) {
         if (ModelState.IsValid) {
            var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(),
             model.NewPassword);
            if (result.Succeeded) {
               var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
               if (user != null)
                  await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

               return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
            }//end if
            AddErrors(result);
         }//end if

         // If we got this far, something failed, redisplay form
         return View(model);
      }//close SetPassword(...)

      //
      // GET: /Manage/ManageLogins
      public async Task<ActionResult> ManageLogins(ManageMessageId? message) {
         ViewBag.StatusMessage =
             message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
             : message == ManageMessageId.Error ? "An error has occurred."
             : "";
         var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
         if (user == null)
            return View("Error");

         var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
         var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(
          auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
         ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
         return View(new ManageLoginsViewModel {
            CurrentLogins = userLogins,
            OtherLogins = otherLogins
         });
      }//close ManageLogins(...)

      //
      // POST: /Manage/LinkLogin
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult LinkLogin(string provider) {
         // Request a redirect to the external login provider to link a login for the current user
         return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback",
          "Manage"), User.Identity.GetUserId());
      }//close LinkLogin(...)

      //
      // GET: /Manage/LinkLoginCallback
      public async Task<ActionResult> LinkLoginCallback() {
         var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey,
          User.Identity.GetUserId());
         if (loginInfo == null)
            return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });

         var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
         return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins",
          new { Message = ManageMessageId.Error });
      }//close LinkLoginCallback(...)

      protected override void Dispose(bool disposing) {
         if (disposing && _userManager != null) {
            _userManager.Dispose();
            _userManager = null;
         }//end if

         base.Dispose(disposing);
      }//close Dispose(...)

      #region Helpers
      // Used for XSRF protection when adding external logins
      private const string XsrfKey = "XsrfId";

      private IAuthenticationManager AuthenticationManager {
         get {
            return HttpContext.GetOwinContext().Authentication;
         }
      }

      private void AddErrors(IdentityResult result) {
         foreach (var error in result.Errors) {
            ModelState.AddModelError("", error);
         }
      }

      private bool HasPassword() {
         var user = UserManager.FindById(User.Identity.GetUserId());
         if (user != null) {
            return user.PasswordHash != null;
         }
         return false;
      }

      private bool HasPhoneNumber() {
         var user = UserManager.FindById(User.Identity.GetUserId());
         if (user != null) {
            return user.PhoneNumber != null;
         }
         return false;
      }

      public enum ManageMessageId {
         AddPhoneSuccess,
         ChangePasswordSuccess,
         SetTwoFactorSuccess,
         SetPasswordSuccess,
         RemoveLoginSuccess,
         RemovePhoneSuccess,
         Error
      }

      #endregion
   }//close class ManageController
}//close namespace MvcBook.Controllers