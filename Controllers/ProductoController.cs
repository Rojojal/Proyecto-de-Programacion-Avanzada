using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Libreria.Models;

namespace Libreria.Controllers
{
   
    public class ProductoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Productos
        public ActionResult Index()
        {
            return View(db.Productos.ToList());
        }

        // GET: Productos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProducto,Nombre,Precio,Descripcion,Disponibilidad,Reseñas,Estado")] Producto producto, IEnumerable<HttpPostedFileBase> ImagenFiles)
        {
            if (ModelState.IsValid)
            {
                // Valida si hay imagenes
                if (ImagenFiles != null && ImagenFiles.Any(f => f != null && f.ContentLength > 0))
                {
                    // Crea una lista de imagenes asociadas para el producto
                    var imagenes = new List<ProductoImagenes>();

                    foreach (var file in ImagenFiles)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            // Guarda cada imagen en una carpeta del servidor
                            string nombreArchivo = Path.GetFileName(file.FileName);
                            string ruta = Path.Combine(Server.MapPath("~/Imagenes"), nombreArchivo);
                            file.SaveAs(ruta);
                            var productoImagen = new ProductoImagenes
                            {
                                ImageUrl = "/Imagenes/" + nombreArchivo
                            };

                            // Crea un nuevo objeto ProductoImagenes y lo agrega a la lista
                            imagenes.Add(productoImagen);
                            db.ProductoImagenes.Add(productoImagen);
                        }
                    }

                    // Asocia las imagenes con el producto
                    producto.Imagenes = imagenes;
                }

                db.Productos.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producto);
        }



        // GET: Productos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProducto,Nombre,Precio,Descripcion,Disponibilidad,Reseñas,Estado")] Producto producto, IEnumerable<HttpPostedFileBase> ImagenFiles)
        {
            if (ModelState.IsValid)
            {
                // Actualiza lo demás del producto
                db.Entry(producto).State = EntityState.Modified;

                // Valida si hay nuevas imágenes para agregar
                if (ImagenFiles != null && ImagenFiles.Any(f => f != null && f.ContentLength > 0))
                {
                    foreach (var file in ImagenFiles)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            // Guardar la nueva imagen en la carpeta /imagenes
                            string nombreArchivo = Path.GetFileName(file.FileName);
                            string ruta = Path.Combine(Server.MapPath("~/Imagenes"), nombreArchivo);
                            file.SaveAs(ruta);

                            // Crear un nuevo objeto de imagen y lo asocia con el proyecto
                            var productoImagen = new ProductoImagenes
                            {
                                ImageUrl = "/Imagenes/" + nombreArchivo,
                                IdProducto = producto.IdProducto // Relaciona la imagen cn el producto
                            };

                            db.ProductoImagenes.Add(productoImagen);
                        }
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Productos.Find(id);
            if (producto != null)
            {
               
                producto.Estado = false;

                // Marca la entidad como modificada para guardar el cambio
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
            }
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
