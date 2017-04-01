using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBook.Controllers {
   public class HomeController : Controller {
      public ActionResult Index() {
         return View();
      }//close Index()


      public ActionResult About() {
         ViewBag.Message = "Your application description page.";

         return View();
      }//close About()


      public ActionResult Contact() {
         ViewBag.Message = "Your contact page.";

         return View();
      }//close Contact()
   }//close class HomeController
}//close namespace MvcBook.Controllers