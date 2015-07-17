using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using catFinal.Models;

namespace catFinal.Controllers
{
    public class CategoriaController : Controller
    {
        private BaseDatosEntities db = new BaseDatosEntities();

        //
        // GET: /Categoria/

        public ActionResult Index()
        {
            return View(db.Categorias.ToList());
        }

        //
        // GET: /Categoria/Details/5

        public ActionResult Details(int id = 0)
        {
            Categorias categorias = db.Categorias.Find(id);
            if (categorias == null)
            {
                return HttpNotFound();
            }
            return View(categorias);
        }

        //
        // GET: /Categoria/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Categoria/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categorias categorias)
        {
            if (ModelState.IsValid)
            {
                db.Categorias.Add(categorias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categorias);
        }

        //
        // GET: /Categoria/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id = 0)
        {
            Categorias categorias = db.Categorias.Find(id);
            if (categorias == null)
            {
                return HttpNotFound();
            }
            return View(categorias);
        }

        //
        // POST: /Categoria/Edit/5
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categorias categorias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categorias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categorias);
        }

        //
        // GET: /Categoria/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id = 0)
        {
            Categorias categorias = db.Categorias.Find(id);
            if (categorias == null)
            {
                return HttpNotFound();
            }
            return View(categorias);
        }

        //
        // POST: /Categoria/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categorias categorias = db.Categorias.Find(id);
            db.Categorias.Remove(categorias);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}