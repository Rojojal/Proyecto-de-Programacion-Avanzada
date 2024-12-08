using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Libreria.Models;
using Microsoft.AspNet.Identity;

namespace Libreria.Controllers
{
    public class ReseñaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reseña
        public ActionResult Index()
        {
            var reseña = db.Reseña.Include(r => r.Producto).Include(r => r.Usuario);
            return View(reseña.ToList());
        }

        // GET: Reseña/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reseña reseña = db.Reseña.Find(id);
            if (reseña == null)
            {
                return HttpNotFound();
            }
            return View(reseña);
        }


        // GET: Reseña/Create
        public ActionResult Create(int idProducto)
        {
            var producto = db.Productos.Find(idProducto);
            if (producto == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProductoNombre = producto.Nombre;
            ViewBag.IdProducto = idProducto;
            return View();
        }

        // POST: Reseña/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Create(int idProducto, [Bind(Include = "Calificacion,Comentario")] Reseña reseña)
        {
            if (ModelState.IsValid)
            {
                // Obtiene el ID del usuario autenticado
                string usuarioId = User.Identity.GetUserId();

                // Asocia la reseña al usuario registrado y al producto
                var usuario = db.Usuarios.FirstOrDefault(u => u.AspNetUserId == usuarioId);
                if (usuario == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Usuario no encontrado.");
                }

                reseña.IdUsuario = usuario.IdUsuario;
                reseña.IdProducto = idProducto;
                reseña.Fecha = DateTime.Now;

               
                db.Reseña.Add(reseña);
                db.SaveChanges();

                
                return RedirectToAction("Details", "Producto", new { id = idProducto });
            }

            // Obtiene información del producto para la vista en caso de que el modelo no sea valido
            var producto = db.Productos.Find(idProducto);
            ViewBag.ProductoNombre = producto?.Nombre;
            return View(reseña);
        }


        // GET: Reseña/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reseña reseña = db.Reseña.Find(id);
            if (reseña == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "Nombre", reseña.IdProducto);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "NombreUsuario", reseña.IdUsuario);
            return View(reseña);
        }

        // POST: Reseña/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdReseña,IdUsuario,IdProducto,Calificacion,Comentario,Fecha")] Reseña reseña)
        {
            if (ModelState.IsValid)
            {
                reseña.Fecha = DateTime.Now;

                db.Entry(reseña).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Producto", new { id = reseña.IdProducto });
            }
            ViewBag.IdProducto = new SelectList(db.Productos, "IdProducto", "Nombre", reseña.IdProducto);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "NombreUsuario", reseña.IdUsuario);
            return View(reseña);
        }

        // GET: Reseña/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reseña reseña = db.Reseña.Find(id);
            db.Reseña.Remove(reseña);
            db.SaveChanges();
            return RedirectToAction("Details", "Producto", new { id = reseña.IdProducto });
            
        }

        // POST: Reseña/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reseña reseña = db.Reseña.Find(id);
            db.Reseña.Remove(reseña);
            db.SaveChanges();
            return RedirectToAction("Details", "Producto", new { id = reseña.IdProducto });
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
