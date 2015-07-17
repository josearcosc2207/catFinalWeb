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
    public class ProductoController : Controller
    {
        private BaseDatosEntities db = new BaseDatosEntities();

        //
        // GET: /Producto/

        public ActionResult Index()
        {
            var producto = db.Producto.Include(p => p.Categorias);
            return View(producto.ToList());
        }

        //
        // GET: /Producto/Details/5

        public ActionResult Details(int id = 0)
        {
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        //
        // GET: /Producto/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.idCategoria = new SelectList(db.Categorias, "idCategoria", "nombre");
            return View();
        }

        //
        // POST: /Producto/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create(Producto producto, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    producto.ImageMimeType = image.ContentType;
                    int length = image.ContentLength;
                    byte[] buffer = new byte[length];
                    image.InputStream.Read(buffer, 0, length);
                    producto.picture = buffer;
                }

                db.Producto.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        //
        // GET: /Producto/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id = 0)
        {
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCategoria = new SelectList(db.Categorias, "idCategoria", "nombre", producto.idCategoria);
            return View(producto);
        }

        //
        // POST: /Producto/Edit/5
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCategoria = new SelectList(db.Categorias, "idCategoria", "nombre", producto.idCategoria);
            return View(producto);
        }

        //
        // GET: /Producto/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id = 0)
        {
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        //
        // POST: /Producto/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Producto.Find(id);
            db.Producto.Remove(producto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public FileContentResult GetImage(Int32 idProducto)
        {
            Producto prod = db.Producto.FirstOrDefault(c => c.idProducto == idProducto);
            if (prod != null)
            {

                string type = string.Empty;
                if (!string.IsNullOrEmpty(prod.ImageMimeType))
                {
                    type = prod.ImageMimeType;
                }
                else
                {
                    type = "image/jpeg";
                }

                return File(prod.picture, type);
            }
            else
            {
                return null;
            }

        }
    }
}

