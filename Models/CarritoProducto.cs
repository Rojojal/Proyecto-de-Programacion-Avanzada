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

        [Required]
        public int IdProducto { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreProducto { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public decimal Precio { get; set; }

        public decimal Subtotal => Cantidad * Precio;

       
        [Required]
        public int IdCarrito { get; set; }
        public virtual Carrito Carrito { get; set; }




    }
}