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
        [StringLength(50)]
        public string Disponibilidad { get; set; }
        public string Reseñas { get; set; }
        public List<byte[]> Imagenes { get; set; }

        [Required]
        public string Estado { get; set; }

        public List<Reseña> Reseña { get; set; }


    }
}