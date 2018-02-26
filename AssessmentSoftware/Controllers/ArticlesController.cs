using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssessmentSoftware.Models;

namespace AssessmentSoftware.Controllers
{
    /// <summary>
    /// Holds fuctions to create, edit and delete articles
    /// </summary>
    public class ArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articles
        public ActionResult Index(String search)
        {
            var articles = new List<Article>();

            if (search != null)
            {
                articles = db.Articles.Where(
                f => f.ArticleDescription.Contains(search)
                ).Include(f => f.Comments).ToList();
            }
            else
            {
                articles = db.Articles.Include(f => f.Comments).ToList();
            }

            return View(articles);
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var article = db.Articles.Where(b => b.ArticleID == id).Include("Comments").FirstOrDefault();
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }
        [HttpPost, ActionName("details")]
        public ActionResult Details(AssessmentSoftware.Models.Comment res)
        {
            res.CommentName = User.Identity.Name;
            res.CommentDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Comments.Add(res);
                db.SaveChanges();
                return RedirectToAction("Details", res.ArticleID);
            }
            return View();
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleID,ArticleTitle,ArticleDescription,ArticleDate,UserName")] Article article)
        {
            article.ArticleDate = DateTime.Now;
            article.UserName = User.Identity.Name;
            db.Articles.Add(article);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var article = db.Articles
            .Where(b => b.ArticleID == id)
            .Include("Comments")
            .FirstOrDefault();
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleID,ArticleTitle,ArticleDescription,ArticleDate,UserName")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("DeleteResponse")]
        public ActionResult DeleteResponse(Comment res)
        {
            var modeltodelete = db.Comments.FirstOrDefault(s => s.CommentID == res.CommentID);

            if (ModelState.IsValid)
            {
                db.Comments.Remove(modeltodelete);
                db.SaveChanges();
                return RedirectToAction("Edit", "Articles", new { @id = res.ArticleID });
            }
            return View();
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var article = db.Articles
                .Where(b => b.ArticleID == id)
                .Include("Comments")
                .FirstOrDefault();
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var article = db.Articles
                .Where(b => b.ArticleID == id)
                .Include("Comments")
                .FirstOrDefault();
            db.Articles.Remove(article);
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
