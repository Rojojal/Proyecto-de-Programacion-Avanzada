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
        public int IdUsuario { get; set; }

        public int Calificacion { get; set; }

        public string Comentario { get; set; }

        public DateTime Fecha { get; set; }

      

    }
}