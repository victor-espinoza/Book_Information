
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcBook.Models;

namespace MvcBook.Controllers {
   public class BookTitlesController : Controller {
      private BookTitleDBContext db = new BookTitleDBContext();

      // GET: BookTitles
      //public ActionResult Index(string searchString) {
      //    var bookTitles = from m in db.Titles
      //                 select m;

      //    if (!String.IsNullOrEmpty(searchString)) {
      //        bookTitles = bookTitles.Where(s => s.Title.Contains(searchString));
      //    }
      //    return View(bookTitles);
      //}


      public ActionResult Index(string publishYear, string searchString) {

         var pubYearLst = new List<string>();

         var pubYearQry = from d in db.Titles
                          orderby d.Copyright
                          select d.Copyright;

         pubYearLst.AddRange(pubYearQry.Distinct());
         ViewBag.publishYear = new SelectList(pubYearLst);

         var BookTitles = from m in db.Titles
                          select m;

         if (!String.IsNullOrEmpty(searchString))
            BookTitles = BookTitles.Where(s => s.Title.Contains(searchString));

         if (!string.IsNullOrEmpty(publishYear))
            BookTitles = BookTitles.Where(x => x.Copyright == publishYear);

         return View(BookTitles);
      }//close Index(...)


      // GET: BookTitles/Details/5
      public ActionResult Details(int? id) {
         if (id == null)
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

         BookTitle bookTitle = db.Titles.Find(id);
         if (bookTitle == null)
            return HttpNotFound();

         return View(bookTitle);
      }//close Detals(...)


      // GET: BookTitles/Create
      public ActionResult Create() {
         return View();
      }//close Create()


      // POST: BookTitles/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to,
      // for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "ID,ISBN,Title,EditionNumber,Copyright")] 
         BookTitle bookTitle) {
         if (ModelState.IsValid) {
            db.Titles.Add(bookTitle);
            db.SaveChanges();
            return RedirectToAction("Index");
         }//end if

         return View(bookTitle);
      }//close Create(...)


      // GET: BookTitles/Edit/5
      public ActionResult Edit(int? id) {
         if (id == null)
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

         BookTitle bookTitle = db.Titles.Find(id);
         if (bookTitle == null)
            return HttpNotFound();

         return View(bookTitle);
      }//close Edit(...)


      // POST: BookTitles/Edit/5
      // To protect from overposting attacks, please enable the specific properties you want to bind to,  
      // for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "ID,ISBN,Title,EditionNumber,Copyright")] 
         BookTitle bookTitle) {
         if (ModelState.IsValid) {
            db.Entry(bookTitle).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }//end if
         return View(bookTitle);
      }//close Edit(...)


      // GET: BookTitles/Delete/5
      public ActionResult Delete(int? id) {
         if (id == null)
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

         BookTitle bookTitle = db.Titles.Find(id);
         if (bookTitle == null)
            return HttpNotFound();

         return View(bookTitle);
      }//close Delete(...)


      // POST: BookTitles/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id) {
         BookTitle bookTitle = db.Titles.Find(id);
         db.Titles.Remove(bookTitle);
         db.SaveChanges();
         return RedirectToAction("Index");
      }//close DeleteConfirmed(...)

      protected override void Dispose(bool disposing) {
         if (disposing)
            db.Dispose();

         base.Dispose(disposing);
      }//close Dispose(...)

   }//close class BookTitlesController
}//close namespace MvcBook.Controllers
