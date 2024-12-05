using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Web;

namespace Libreria.Models
{
    public class Carrito
    {
        [Key]
        public int IdCarrito { get; set; }

        public String IdUsuario { get; set; }
        public virtual ICollection<CarritoProducto> Carritos { get; set; }
    }
}