using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Libreria.Models;
using Microsoft.AspNet.Identity;

namespace Libreria.Controllers
{
    
    public class CarritoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Carrito
        public ActionResult Index()
        {
            return View(db.Carritos.ToList());
        }

        // POST: Carrito/AddToCart
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult AddToCart(int idProducto, int cantidad = 1)
        {
            var usuarioId = User.Identity.GetUserId(); // Obtener el ID del usuario actual

            // Verificar si el carrito ya existe para el usuario
            var carrito = db.Carritos.FirstOrDefault(c => c.IdUsuario == usuarioId);
            if (carrito == null)
            {
                carrito = new Carrito { IdUsuario = usuarioId };
                db.Carritos.Add(carrito);
                db.SaveChanges();
            }

            // Verificar si el producto ya está en el carrito
            var carritoProducto = db.CarritoProductos
                .FirstOrDefault(cp => cp.IdCarrito == carrito.IdCarrito && cp.IdProducto == idProducto);

            if (carritoProducto != null)
            {
                // Incrementar la cantidad si ya existe
                carritoProducto.Cantidad += cantidad;
            }
            else
            {
                // Añadir el producto al carrito
                var producto = db.Productos.Find(idProducto);
                if (producto != null)
                {
                    carritoProducto = new CarritoProducto
                    {
                        IdCarrito = carrito.IdCarrito,
                        IdProducto = producto.IdProducto,
                        NombreProducto = producto.Nombre,
                        Precio = producto.Precio,
                        Cantidad = cantidad
                    };
                    db.CarritoProductos.Add(carritoProducto);
                }
            }

            db.SaveChanges();
            return RedirectToAction("Index", "Producto"); // Redirigir al catálogo después de añadir
        }



        [Authorize(Roles = "User,Admin")]
        // GET: Carrito/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrito carrito = db.Carritos.Find(id);
            if (carrito == null)
            {
                return HttpNotFound();
            }
            return View(carrito);
        }


        [Authorize(Roles = "User,Admin")]
        // GET: Carrito/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carrito/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCarrito,IdUsuario")] Carrito carrito)
        {
            if (ModelState.IsValid)
            {
                db.Carritos.Add(carrito);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carrito);
        }



        // GET: Carrito/Edit/5
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrito carrito = db.Carritos.Find(id);
            if (carrito == null)
            {
                return HttpNotFound();
            }
            return View(carrito);
        }

        // POST: Carrito/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCarrito,IdUsuario")] Carrito carrito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carrito).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carrito);
        }

        // GET: Carrito/Delete/5
        [Authorize(Roles = "User,Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carrito carrito = db.Carritos.Find(id);
            if (carrito == null)
            {
                return HttpNotFound();
            }
            return View(carrito);
        }

        // POST: Carrito/Delete/5
        [Authorize(Roles = "User,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Carrito carrito = db.Carritos.Find(id);
            db.Carritos.Remove(carrito);
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
