using System;
using System.Collections.Generic;

namespace aplicacion_proyecto2.Models
{
    public partial class TblDetallePedido
    {
        /// <summary>
        /// Identificador del detalle de pedido
        /// </summary>
        public int IdDetallePedido { get; set; }
        /// <summary>
        /// Identificador del pedido
        /// </summary>
        public int IdPedido { get; set; }
        /// <summary>
        /// Identificador del detalle de producto
        /// </summary>
        public int IdDetalleProducto { get; set; }
        /// <summary>
        /// Cantidad de productos
        /// </summary>
        public int Cantidad { get; set; }
        /// <summary>
        /// Monto total del pedido
        /// </summary>
        public decimal TotalPedido { get; set; }

        public virtual TblDetalleProducto IdDetalleProductoNavigation { get; set; } = null!;
        public virtual TblPedido IdPedidoNavigation { get; set; } = null!;
    }
}
