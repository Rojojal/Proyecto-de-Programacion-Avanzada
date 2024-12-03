using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Libreria.Models
{
    public class PedidosLista
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public decimal Precio { get; set; }

        public decimal Subtotal => Cantidad * Precio;

        [Required]
        public int IdPedido { get; set; }
        public virtual Pedido Pedido { get; set; }
    }
}