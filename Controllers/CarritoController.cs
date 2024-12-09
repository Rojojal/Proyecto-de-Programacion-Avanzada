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
            // Llama el método para obtener el carrito, si no existe crea uno
            var carrito = ObtenerCarrito();

            // Obtiene los productos en el carrito, si los hay
            var productosEnCarrito = db.CarritoProductos.Where(cp => cp.IdCarrito == carrito.IdCarrito).ToList();

            return View(productosEnCarrito);
        }

        // Método para añadir productos al carrito
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult AgregarProductoCarrito(int idProducto, int cantidad)
        {
            // Llama el método para verificar si el carrito es el correcto
            var carrito = ObtenerCarrito();

            // Busca si ya existe el producto en el carrito
            var carritoProducto = db.CarritoProductos.FirstOrDefault(cp =>
                cp.IdCarrito == carrito.IdCarrito && cp.IdProducto == idProducto);

            if (carritoProducto == null)
            {
                // Si no existe lo agrega
                var producto = db.Productos.Find(idProducto);
                carritoProducto = new CarritoProducto
                {
                    IdCarrito = carrito.IdCarrito,
                    IdProducto = idProducto,
                    NombreProducto = producto.Nombre,
                    Cantidad = cantidad,
                    Precio = producto.Precio
                };
                db.CarritoProductos.Add(carritoProducto);
            }
            else
            {
                // Actualiza la cantidad
                carritoProducto.Cantidad += cantidad;
                db.Entry(carritoProducto).State = EntityState.Modified;
            }

            db.SaveChanges();
            return RedirectToAction("Index", "Producto");
        }


        private Carrito ObtenerCarrito()
        {
            // Saca la ID del usuario con la sesion activa
            string usuarioId = User.Identity.GetUserId();

            // Busca el carrito del usuario actual
            var carrito = db.Carritos.FirstOrDefault(c => c.IdUsuario == usuarioId);

            if (carrito == null)
            {
                // Si no hay carrito hace uno nuevo
                carrito = new Carrito
                {
                    IdUsuario = usuarioId
                };
                db.Carritos.Add(carrito);
                db.SaveChanges();
            }

            return carrito;
        }

        // Elimina el producto el carrito 
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult EliminarProductoCarrito(int idProducto)
        {
            var usuarioId = User.Identity.GetUserId();
            var carrito = db.Carritos.FirstOrDefault(c => c.IdUsuario == usuarioId);

            if (carrito != null)
            {
                // Buscar el producto específico en el carrito
                var carritoProducto = db.CarritoProductos.FirstOrDefault(cp => cp.IdCarrito == carrito.IdCarrito && cp.IdProducto == idProducto);
                if (carritoProducto != null)
                {
                    // Eliminar el producto del carrito
                    db.CarritoProductos.Remove(carritoProducto);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
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

            Carrito carrito = db.Carritos.Include(c => c.Carritos).FirstOrDefault(c => c.IdCarrito == id);
            if (carrito == null)
            {
                return HttpNotFound();
            }

            // Pasa la lista de CarritoProducto a la vista
            return View(carrito.Carritos.ToList());
        }

        // POST: Carrito/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IEnumerable<CarritoProducto> productos)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in productos)
                {
                    var carritoProducto = db.CarritoProductos.Find(item.Id);
                    if (carritoProducto != null)
                    {
                        carritoProducto.Cantidad = item.Cantidad;
                        db.Entry(carritoProducto).State = EntityState.Modified;
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productos);
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