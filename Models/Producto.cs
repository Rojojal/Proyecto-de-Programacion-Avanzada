using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Libreria.Models
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required]
        [StringLength(75)]
        public string Nombre { get; set; }

        [Required]
        public decimal Precio{ get; set; }

        [Required]
        [StringLength(2000)]
        public string Descripcion { get; set; }

        [Required]
        public bool Disponibilidad { get; set; }
        public string Reseñas { get; set; }

        [Required]
        public bool Estado { get; set; }

        public virtual ICollection<Reseña> Reseña { get; set; }

        public virtual ICollection<ProductoImagenes> Imagenes { get; set; }
    }
}