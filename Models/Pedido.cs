using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Web;

namespace Libreria.Models
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }

        [Required]
        public string IdUsuario { get; set; }

        [Required]
        public DateTime FechaPedido { get; set; }

        public virtual ICollection<PedidosLista> Pedidos { get; set; }

    }
}