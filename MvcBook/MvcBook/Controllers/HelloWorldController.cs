using System.Web;
using System.Web.Mvc;

namespace MvcBook.Controllers {
   public class HelloWorldController : Controller {
      // 
      // GET: /HelloWorld/ 

      public ActionResult Index() {
         return View();
      }//close Index()

      // 
      // GET: /HelloWorld/Welcome/ 

      public ActionResult Welcome(string name, int numTimes = 1) {
         ViewBag.Message = "Hello " + name;
         ViewBag.NumTimes = numTimes;

         return View();
      }//close Welcome(...)
   }//close class HelloWorldController
}//close namespace MvcBook.Controllers