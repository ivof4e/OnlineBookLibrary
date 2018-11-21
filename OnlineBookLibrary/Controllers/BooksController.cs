using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineBookLibrary.Models;
using PagedList;

namespace OnlineBookLibrary.Controllers
{
    public class BooksController : Controller
    {
        private OnlineBookLibraryContext db = new OnlineBookLibraryContext();

        // GET: Books
        public ActionResult Index(int? page, string searchTitle, string searchGenre, string searchWriter, string sortOrder)
        {
            var books = db.Books.Include(b => b.Genre).Include(b => b.Writer);

            var GenreList = new List<string>();

            var GenreQry = from d in db.Books
                           orderby d.Genre.GenreName
                           select d.Genre.GenreName;

            GenreList.AddRange(GenreQry.Distinct());
            ViewBag.searchGenre = new SelectList(GenreList);
            ViewBag.CurrentSortParm = sortOrder;
            ViewBag.TitleSortParam = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";

            if (!String.IsNullOrEmpty(searchTitle))
            {
                books = books.Where(s => s.Title.Contains(searchTitle));
            }

            if (!String.IsNullOrEmpty(searchGenre))
            {
                books = books.Where(s => s.Genre.GenreName == (searchGenre));
            }

            if (!String.IsNullOrEmpty(searchWriter))
            {
                books = books.Where(s => s.Writer.FirstName.Contains(searchWriter));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(x => x.Title);
                    break;

                default:
                    books = books.OrderBy(x => x.Title);
                    break;
            }

            int pageNumber = page ?? 1;
            int pageSize = 3;
            return View(books.ToPagedList(pageNumber, pageSize));
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "GenreName");
            ViewBag.WriterId = new SelectList(db.Writers, "Id", "FirstName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,ReleaseDate,WriterId,GenreId,Description")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GenreId = new SelectList(db.Genres, "Id", "GenreName", book.GenreId);
            ViewBag.WriterId = new SelectList(db.Writers, "Id", "FirstName", book.WriterId);
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "GenreName", book.GenreId);
            ViewBag.WriterId = new SelectList(db.Writers, "Id", "FirstName", book.WriterId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,ReleaseDate,WriterId,GenreId,Description")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "GenreName", book.GenreId);
            ViewBag.WriterId = new SelectList(db.Writers, "Id", "FirstName", book.WriterId);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
