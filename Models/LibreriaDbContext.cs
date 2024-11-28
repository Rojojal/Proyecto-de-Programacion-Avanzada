using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;

namespace Libreria.Models
{
    public class LibreriaDbContext : DbContext
    {
        public LibreriaDbContext() : base("DefaultConnection") { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Carrito> Carritos { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }

    }
}