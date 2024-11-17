using System;
using System.Collections.Generic;

namespace aplicacion_proyecto2.Models
{
    public partial class TblDetalleProducto
    {
        public TblDetalleProducto()
        {
            TblCarritos = new HashSet<TblCarrito>();
            TblDetallePedidos = new HashSet<TblDetallePedido>();
        }

        /// <summary>
        /// Identificador del detalle de producto
        /// </summary>
        public int IdDetalleProducto { get; set; }
        /// <summary>
        /// Identificador del producto
        /// </summary>
        public int IdProducto { get; set; }
        /// <summary>
        /// Identificador de la medida del producto
        /// </summary>
        public int IdMedida { get; set; }
        /// <summary>
        /// Identificador del color del producto
        /// </summary>
        public int IdColor { get; set; }
        /// <summary>
        /// Cantidad de productos en stock
        /// </summary>
        public int Stock { get; set; }
        /// <summary>
        /// Nombre de la imagen del produto
        /// </summary>
        public string? NombreImagen { get; set; }
        /// <summary>
        /// Ruta de la imagen del producto
        /// </summary>
        public string? RutaImagen { get; set; }

        public virtual TblColore IdColorNavigation { get; set; } = null!;
        public virtual TblMedida IdMedidaNavigation { get; set; } = null!;
        public virtual TblProducto IdProductoNavigation { get; set; } = null!;
        public virtual ICollection<TblCarrito> TblCarritos { get; set; }
        public virtual ICollection<TblDetallePedido> TblDetallePedidos { get; set; }
    }
}
