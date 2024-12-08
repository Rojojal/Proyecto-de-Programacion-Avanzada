namespace Libreria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarritoProductoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProducto = c.Int(nullable: false),
                        NombreProducto = c.String(nullable: false, maxLength: 50),
                        Cantidad = c.Int(nullable: false),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IdCarrito = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Carritoes", t => t.IdCarrito, cascadeDelete: true)
                .ForeignKey("dbo.Productoes", t => t.IdProducto, cascadeDelete: true)
                .Index(t => t.IdProducto)
                .Index(t => t.IdCarrito);
            
            CreateTable(
                "dbo.Carritoes",
                c => new
                    {
                        IdCarrito = c.Int(nullable: false, identity: true),
                        IdUsuario = c.String(),
                    })
                .PrimaryKey(t => t.IdCarrito);
            
            CreateTable(
                "dbo.Productoes",
                c => new
                    {
                        IdProducto = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 75),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descripcion = c.String(nullable: false, maxLength: 2000),
                        Disponibilidad = c.Boolean(nullable: false),
                        Reseñas = c.String(),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdProducto);
            
            CreateTable(
                "dbo.ProductoImagenes",
                c => new
                    {
                        IdImagen = c.Int(nullable: false, identity: true),
                        IdProducto = c.Int(nullable: false),
                        ImageUrl = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdImagen)
                .ForeignKey("dbo.Productoes", t => t.IdProducto, cascadeDelete: true)
                .Index(t => t.IdProducto);
            
            CreateTable(
                "dbo.Reseña",
                c => new
                    {
                        IdReseña = c.Int(nullable: false, identity: true),
                        IdUsuario = c.Int(nullable: false),
                        IdProducto = c.Int(nullable: false),
                        Calificacion = c.Int(nullable: false),
                        Comentario = c.String(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdReseña)
                .ForeignKey("dbo.Productoes", t => t.IdProducto, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.IdUsuario)
                .Index(t => t.IdProducto);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        IdUsuario = c.Int(nullable: false, identity: true),
                        NombreUsuario = c.String(nullable: false, maxLength: 50),
                        Contraseña = c.String(nullable: false, maxLength: 255),
                        UltimaConexion = c.DateTime(nullable: false),
                        Estado = c.Boolean(nullable: false),
                        AspNetUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IdUsuario)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUserId)
                .Index(t => t.AspNetUserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Pedidoes",
                c => new
                    {
                        IdPedido = c.Int(nullable: false, identity: true),
                        IdUsuario = c.String(nullable: false),
                        FechaPedido = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdPedido);
            
            CreateTable(
                "dbo.PedidosListas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProducto = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Cantidad = c.Int(nullable: false),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IdPedido = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pedidoes", t => t.IdPedido, cascadeDelete: true)
                .Index(t => t.IdPedido);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PedidosListas", "IdPedido", "dbo.Pedidoes");
            DropForeignKey("dbo.CarritoProductoes", "IdProducto", "dbo.Productoes");
            DropForeignKey("dbo.Reseña", "IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.Usuarios", "AspNetUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reseña", "IdProducto", "dbo.Productoes");
            DropForeignKey("dbo.ProductoImagenes", "IdProducto", "dbo.Productoes");
            DropForeignKey("dbo.CarritoProductoes", "IdCarrito", "dbo.Carritoes");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PedidosListas", new[] { "IdPedido" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Usuarios", new[] { "AspNetUserId" });
            DropIndex("dbo.Reseña", new[] { "IdProducto" });
            DropIndex("dbo.Reseña", new[] { "IdUsuario" });
            DropIndex("dbo.ProductoImagenes", new[] { "IdProducto" });
            DropIndex("dbo.CarritoProductoes", new[] { "IdCarrito" });
            DropIndex("dbo.CarritoProductoes", new[] { "IdProducto" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PedidosListas");
            DropTable("dbo.Pedidoes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Usuarios");
            DropTable("dbo.Reseña");
            DropTable("dbo.ProductoImagenes");
            DropTable("dbo.Productoes");
            DropTable("dbo.Carritoes");
            DropTable("dbo.CarritoProductoes");
        }
    }
}
