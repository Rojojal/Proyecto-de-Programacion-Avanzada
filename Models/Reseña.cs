using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Libreria.Models
{
    public class Reseña
    {
        [Key]
        public int IdReseña { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        public int Calificacion { get; set; }

        public string Comentario { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        
        public virtual Usuario Usuario { get; set; }
        public virtual Producto Producto { get; set; }


    }
}