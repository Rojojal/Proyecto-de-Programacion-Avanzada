using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Libreria.Models
{
    public class ProductoImagenes
    {

        [Key]
        public int IdImagen { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public virtual Producto Producto { get; set; }
    }
}