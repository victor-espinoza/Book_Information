using DataAnnotationsExtensions.ClientValidation;

[assembly: WebActivator.PreApplicationStartMethod(typeof(MvcBook.App_Start.RegisterClientValidationExtensions), "Start")]

namespace MvcBook.App_Start {
   public static class RegisterClientValidationExtensions {
      public static void Start() {
         DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();
      }
   }
}