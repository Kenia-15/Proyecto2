using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace aplicacion_proyecto2.Models
{
    public partial class db_carritoContext : DbContext
    {
        public db_carritoContext()
        {
        }

        public db_carritoContext(DbContextOptions<db_carritoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblCarrito> TblCarritos { get; set; } = null!;
        public virtual DbSet<TblCategoria> TblCategorias { get; set; } = null!;
        public virtual DbSet<TblColore> TblColores { get; set; } = null!;
        public virtual DbSet<TblDetallePedido> TblDetallePedidos { get; set; } = null!;
        public virtual DbSet<TblDetalleProducto> TblDetalleProductos { get; set; } = null!;
        public virtual DbSet<TblMedida> TblMedidas { get; set; } = null!;
        public virtual DbSet<TblMetodosPago> TblMetodosPagos { get; set; } = null!;
        public virtual DbSet<TblPedido> TblPedidos { get; set; } = null!;
        public virtual DbSet<TblPersona> TblPersonas { get; set; } = null!;
        public virtual DbSet<TblProducto> TblProductos { get; set; } = null!;
        public virtual DbSet<TblPromocione> TblPromociones { get; set; } = null!;
        public virtual DbSet<TblUsuario> TblUsuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
/*#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-G3CHLPA\\SQLEXPRESS;Database=db_carrito;Trusted_Connection=True;");*/
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblCarrito>(entity =>
            {
                entity.HasKey(e => e.IdCarrito)
                    .HasName("PK__tbl_carr__83A2AD9CF1566FA8");

                entity.ToTable("tbl_carrito");

                entity.Property(e => e.IdCarrito)
                    .HasColumnName("id_carrito")
                    .HasComment("Identificador del carrito");

                entity.Property(e => e.Cantidad)
                    .HasColumnName("cantidad")
                    .HasDefaultValueSql("((0))")
                    .HasComment("Cantidad de productos");

                entity.Property(e => e.IdDetalleProducto)
                    .HasColumnName("id_detalle_producto")
                    .HasComment("Identificador del detalle del producto");

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("id_usuario")
                    .HasComment("Identificador del usuario");

                entity.HasOne(d => d.IdDetalleProductoNavigation)
                    .WithMany(p => p.TblCarritos)
                    .HasForeignKey(d => d.IdDetalleProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tbl_carri__id_de__01142BA1");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.TblCarritos)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tbl_carri__id_us__00200768");
            });

            modelBuilder.Entity<TblCategoria>(entity =>
            {
                entity.HasKey(e => e.IdCategoria)
                    .HasName("PK__tbl_cate__CD54BC5AE091E323");

                entity.ToTable("tbl_categorias");

                entity.Property(e => e.IdCategoria)
                    .HasColumnName("id_categoria")
                    .HasComment("Identificador de la categoria del producto");

                entity.Property(e => e.Categoria)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("categoria")
                    .HasComment("Categoria del producto");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("('A')")
                    .HasComment("Estado del usuario. Posibles valores: A: Activo, I: Inactivo");
            });

            modelBuilder.Entity<TblColore>(entity =>
            {
                entity.HasKey(e => e.IdColor)
                    .HasName("PK__tbl_colo__7CF2AF0309B10B76");

                entity.ToTable("tbl_colores");

                entity.Property(e => e.IdColor)
                    .HasColumnName("id_color")
                    .HasComment("Identificador del color");

                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("color")
                    .HasComment("Color del producto");
            });

            modelBuilder.Entity<TblDetallePedido>(entity =>
            {
                entity.HasKey(e => e.IdDetallePedido)
                    .HasName("PK__tbl_deta__C08768CFA877373F");

                entity.ToTable("tbl_detalle_pedido");

                entity.Property(e => e.IdDetallePedido)
                    .HasColumnName("id_detalle_pedido")
                    .HasComment("Identificador del detalle de pedido");

                entity.Property(e => e.Cantidad)
                    .HasColumnName("cantidad")
                    .HasComment("Cantidad de productos");

                entity.Property(e => e.IdDetalleProducto)
                    .HasColumnName("id_detalle_producto")
                    .HasComment("Identificador del detalle de producto");

                entity.Property(e => e.IdPedido)
                    .HasColumnName("id_pedido")
                    .HasComment("Identificador del pedido");

                entity.Property(e => e.TotalPedido)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("total_pedido")
                    .HasComment("Monto total del pedido");

                entity.HasOne(d => d.IdDetalleProductoNavigation)
                    .WithMany(p => p.TblDetallePedidos)
                    .HasForeignKey(d => d.IdDetalleProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tbl_detal__id_de__0A9D95DB");

                entity.HasOne(d => d.IdPedidoNavigation)
                    .WithMany(p => p.TblDetallePedidos)
                    .HasForeignKey(d => d.IdPedido)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tbl_detal__id_pe__09A971A2");
            });

            modelBuilder.Entity<TblDetalleProducto>(entity =>
            {
                entity.HasKey(e => e.IdDetalleProducto)
                    .HasName("PK__tbl_deta__C88208253CF3001C");

                entity.ToTable("tbl_detalle_producto");

                entity.Property(e => e.IdDetalleProducto)
                    .HasColumnName("id_detalle_producto")
                    .HasComment("Identificador del detalle de producto");

                entity.Property(e => e.IdColor)
                    .HasColumnName("id_color")
                    .HasComment("Identificador del color del producto");

                entity.Property(e => e.IdMedida)
                    .HasColumnName("id_medida")
                    .HasComment("Identificador de la medida del producto");

                entity.Property(e => e.IdProducto)
                    .HasColumnName("id_producto")
                    .HasComment("Identificador del producto");

                entity.Property(e => e.NombreImagen)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre_imagen")
                    .HasComment("Nombre de la imagen del produto");

                entity.Property(e => e.RutaImagen)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ruta_imagen")
                    .HasComment("Ruta de la imagen del producto");

                entity.Property(e => e.Stock)
                    .HasColumnName("stock")
                    .HasComment("Cantidad de productos en stock");

                entity.HasOne(d => d.IdColorNavigation)
                    .WithMany(p => p.TblDetalleProductos)
                    .HasForeignKey(d => d.IdColor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tbl_detal__id_co__7C4F7684");

                entity.HasOne(d => d.IdMedidaNavigation)
                    .WithMany(p => p.TblDetalleProductos)
                    .HasForeignKey(d => d.IdMedida)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tbl_detal__id_me__7B5B524B");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.TblDetalleProductos)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tbl_detal__id_pr__7A672E12");
            });

            modelBuilder.Entity<TblMedida>(entity =>
            {
                entity.HasKey(e => e.IdMedida)
                    .HasName("PK__tbl_medi__E038E090F6DE10F4");

                entity.ToTable("tbl_medidas");

                entity.Property(e => e.IdMedida)
                    .HasColumnName("id_medida")
                    .HasComment("Identificador de la medida");

                entity.Property(e => e.Medida)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("medida")
                    .HasComment("Medidad del producto, puede ser en cm, pies, pulgadas, entre otras");
            });

            modelBuilder.Entity<TblMetodosPago>(entity =>
            {
                entity.HasKey(e => e.IdMetodoPago)
                    .HasName("PK__tbl_meto__85BE0EBC75A3D397");

                entity.ToTable("tbl_metodos_pago");

                entity.Property(e => e.IdMetodoPago)
                    .HasColumnName("id_metodo_pago")
                    .HasComment("Identificador del metodo de pago");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("('A')")
                    .HasComment("Estado del metodo de pago. Posibles valores: A: Activo, I: Inactivo");

                entity.Property(e => e.MetodoPago)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("metodo_pago")
                    .HasComment("Metodo de pago");
            });

            modelBuilder.Entity<TblPedido>(entity =>
            {
                entity.HasKey(e => e.IdPedido)
                    .HasName("PK__tbl_pedi__6FF01489E4A2C11D");

                entity.ToTable("tbl_pedidos");

                entity.Property(e => e.IdPedido)
                    .HasColumnName("id_pedido")
                    .HasComment("Identificador del pedido");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("direccion")
                    .HasComment("Direccion del usuario que realiza el pedido");

                entity.Property(e => e.Fecha)
                    .HasColumnType("date")
                    .HasColumnName("fecha")
                    .HasComment("Fecha en que se realiza el pedido");

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("id_usuario")
                    .HasComment("Identificador del usuario que realiza el pedido");

                entity.Property(e => e.MontoTotal)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("monto_total")
                    .HasComment("Monto total del pedido");

                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasComment("Telefono del usuario que realiza el pedido");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.TblPedidos)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tbl_pedid__id_us__04E4BC85");

                entity.Property(e => e.Estado)
                   .HasMaxLength(1)
                   .IsUnicode(false)
                   .HasColumnName("estado")
                   .HasDefaultValueSql("('A')")
                   .HasComment("Estao del pedido. Posibles valores: A:Activa, I:Inactiva");
            });

            modelBuilder.Entity<TblPersona>(entity =>
            {
                entity.HasKey(e => e.IdPersona)
                    .HasName("PK__tbl_pers__228148B01823568D");

                entity.ToTable("tbl_personas");

                entity.Property(e => e.IdPersona)
                    .HasColumnName("id_persona")
                    .HasComment("Identificador de la persona");

                entity.Property(e => e.IdMetodoPago)
                    .HasColumnName("id_metodo_pago")
                    .HasComment("Identificador del metodo de pago");

                entity.Property(e => e.NumeroIdentificacion)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("numero_identificacion")
                    .HasComment("Numero de identificacion de la persona");

                entity.Property(e => e.PrimerApellido)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("primer_apellido")
                    .HasComment("Primer apellido de la persona");

                entity.Property(e => e.PrimerNombre)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("primer_nombre")
                    .HasComment("Primer nombre de la persona");

                entity.Property(e => e.SegundoApellido)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("segundo_apellido")
                    .HasComment("Segundo apellido de la persona");

                entity.Property(e => e.SegundoNombre)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("segundo_nombre")
                    .HasComment("Segundo nombre de la persona");

                entity.HasOne(d => d.IdMetodoPagoNavigation)
                    .WithMany(p => p.TblPersonas)
                    .HasForeignKey(d => d.IdMetodoPago)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tbl_perso__id_me__5070F446");
            });

            modelBuilder.Entity<TblProducto>(entity =>
            {
                entity.HasKey(e => e.IdProducto)
                    .HasName("PK__tbl_prod__FF341C0D5FFF4C58");

                entity.ToTable("tbl_productos");

                entity.Property(e => e.IdProducto)
                    .HasColumnName("id_producto")
                    .HasComment("Identificador del producto");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("descripcion")
                    .HasComment("Descripcion del producto");

                entity.Property(e => e.IdCategoria)
                    .HasColumnName("id_categoria")
                    .HasComment("Identificador de la categoria del producto");

                entity.Property(e => e.NombreProducto)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre_producto")
                    .HasComment("Nombre del producto");

                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(14, 2)")
                    .HasColumnName("precio")
                    .HasDefaultValueSql("((0))")
                    .HasComment("Precio del producto");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.TblProductos)
                    .HasForeignKey(d => d.IdCategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tbl_produ__id_ca__5FB337D6");
            });

            modelBuilder.Entity<TblPromocione>(entity =>
            {
                entity.HasKey(e => e.IdPromocion)
                    .HasName("PK__tbl_prom__F89308E02CE21C57");

                entity.ToTable("tbl_promociones");

                entity.Property(e => e.IdPromocion)
                    .HasColumnName("id_promocion")
                    .HasComment("Identificador de la promoción");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("descripcion")
                    .HasComment("Descripcion de la promocion");

                entity.Property(e => e.Descuento)
                    .HasColumnName("descuento")
                    .HasComment("Descuento aplicado al producto");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("('A')")
                    .HasComment("Estao de la promocion. Posibles valores: A:Activa, I:Inactiva");

                entity.Property(e => e.IdProducto)
                    .HasColumnName("id_producto")
                    .HasComment("Identificador del producto");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.TblPromociones)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tbl_promo__id_pr__0F624AF8");
            });

            modelBuilder.Entity<TblUsuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__tbl_usua__4E3E04AD2230BF72");

                entity.ToTable("tbl_usuarios");

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("id_usuario")
                    .HasComment("Identificador del usuario");

                entity.Property(e => e.Contrasena)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("contrasena")
                    .HasComment("Contraseña del usuario");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email")
                    .HasComment("Direccion de correo electronico del usuario");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("('A')")
                    .HasComment("Estado del usuario. Posibles valores: A: Activo, I: Inactivo");

                entity.Property(e => e.IdPersona)
                    .HasColumnName("id_persona")
                    .HasComment("Identificador de la persona");

                entity.HasOne(d => d.IdPersonaNavigation)
                    .WithMany(p => p.TblUsuarios)
                    .HasForeignKey(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tbl_usuar__id_pe__571DF1D5");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
