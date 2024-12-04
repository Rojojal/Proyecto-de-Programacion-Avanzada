using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Libreria.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreUsuario { get; set; }

        [Required]
        [StringLength(255)]
        public string Contraseña { get; set; } 
        public DateTime UltimaConexion { get; set; }

        [Required]  
        public bool Estado { get; set; }

        [ForeignKey("AspNetUser")]
        public string AspNetUserId { get; set; }

        public virtual ApplicationUser AspNetUser { get; set; }

    }
}