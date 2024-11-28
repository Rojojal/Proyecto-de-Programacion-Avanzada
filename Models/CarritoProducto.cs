using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Libreria.Models
{
    public class CarritoProducto
    {
        [Key]
        public int Id { get; set; }
        public string IdProducto { get; set; }

        public string NombreProducto { get; set; }

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

        public decimal Subtotal => Cantidad * Precio;

        public virtual Carrito Carrito { get; set; }




    }
}