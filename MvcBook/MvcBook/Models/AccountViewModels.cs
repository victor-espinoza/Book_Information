///////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcBook.Models {
   public class ExternalLoginConfirmationViewModel {
      [Required]
      [Display(Name = "Email")]
      public string Email { get; set; }
   }//close class ExternalLoginConfirmationViewModel


   public class ExternalLoginListViewModel {
      public string ReturnUrl { get; set; }
   }//close class ExternalLoginListViewModel


   public class SendCodeViewModel {
      public string SelectedProvider { get; set; }
      public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
      public string ReturnUrl { get; set; }
      public bool RememberMe { get; set; }
   }//close class SendCodeViewModel 


   public class VerifyCodeViewModel {
      [Required]
      public string Provider { get; set; }

      [Required]
      [Display(Name = "Code")]
      public string Code { get; set; }
      public string ReturnUrl { get; set; }

      [Display(Name = "Remember this browser?")]
      public bool RememberBrowser { get; set; }

      public bool RememberMe { get; set; }
   }//close class VerifyCodeViewModel


   public class ForgotViewModel {
      [Required]
      [Display(Name = "Email")]
      public string Email { get; set; }
   }//close class ForgotViewModel


   public class LoginViewModel {
      [Required]
      [Display(Name = "Email")]
      [EmailAddress]
      public string Email { get; set; }

      [Required]
      [DataType(DataType.Password)]
      [Display(Name = "Password")]
      public string Password { get; set; }

      [Display(Name = "Remember me?")]
      public bool RememberMe { get; set; }
   }//close class LoginViewModel


   public class RegisterViewModel {
      [Required]
      [EmailAddress]
      [Display(Name = "Email")]
      public string Email { get; set; }

      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.",
       MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "Password")]
      public string Password { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Confirm password")]
      [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }
   }//close class RegisterViewModel


   public class ResetPasswordViewModel {
      [Required]
      [EmailAddress]
      [Display(Name = "Email")]
      public string Email { get; set; }

      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.",
       MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "Password")]
      public string Password { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Confirm password")]
      [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }

      public string Code { get; set; }
   }//close class ResetPasswordViewModel


   public class ForgotPasswordViewModel {
      [Required]
      [EmailAddress]
      [Display(Name = "Email")]
      public string Email { get; set; }
   }//close class ForgotPasswordViewModel 
} //close namespace MvcBook.Models
