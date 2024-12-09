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
    [Authorize(Roles = "User,Admin")]
    public class PedidoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pedidos
        public ActionResult Index()
        {
            
            var userId = User.Identity.GetUserId(); //Obtiene la id del usuario actual

            // Filtra los pedidos para la id del usuario actual
            var pedidos = db.Pedidos
                            .Where(p => p.IdUsuario == userId) 
                            .Include(p => p.Pedidos) 
                            .ToList();

            return View(pedidos);
        }


        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult FinalizarCompra()
        {
            var usuarioId = User.Identity.GetUserId();
            var carrito = db.Carritos.FirstOrDefault(c => c.IdUsuario == usuarioId);

            if (carrito == null)
            {
                // Manejar si no hay carrito
                TempData["Error"] = "No tienes productos en el carrito.";
                return RedirectToAction("Index");
            }

            var productosEnCarrito = db.CarritoProductos.Where(cp => cp.IdCarrito == carrito.IdCarrito).ToList();

            // Crea un nuevo pedido
            var pedido = new Pedido
            {
                IdUsuario = usuarioId,
                FechaPedido = DateTime.Now
            };
            db.Pedidos.Add(pedido);
            db.SaveChanges();

            // Transferir productos del carrito a PedidosLista
            foreach (var producto in productosEnCarrito)
            {
                var pedidoDetalle = new PedidosLista
                {
                    IdPedido = pedido.IdPedido,
                    IdProducto = producto.IdProducto,
                    Nombre = producto.NombreProducto,
                    Cantidad = producto.Cantidad,
                    Precio = producto.Precio
                };

                db.PedidosLista.Add(pedidoDetalle);
                db.CarritoProductos.Remove(producto); // Elimina el producto del carrito
            }

            db.SaveChanges();

            TempData["Success"] = "Compra realizada exitosamente.";
            return RedirectToAction("Index", "Pedido");
        }



        // GET: Pedidos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // GET: Pedidos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pedidos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPedido,IdUsuario,FechaPedido")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Pedidos.Add(pedido);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pedido);
        }

        // GET: Pedidos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPedido,IdUsuario,FechaPedido")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedidos.Find(id);
            db.Pedidos.Remove(pedido);
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
