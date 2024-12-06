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
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        public decimal Precio{ get; set; }

        [Required]
        public bool Disponibilidad { get; set; }
        public string Reseñas { get; set; }

        [Required]
        [StringLength(10)]
        public string Estado { get; set; }

        public virtual ICollection<Reseña> Reseña { get; set; }

        public virtual ICollection<ProductoImagenes> Imagenes { get; set; }
    }
}