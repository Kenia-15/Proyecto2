using System;
using System.Collections.Generic;

namespace aplicacion_proyecto2.Models
{
    public partial class TblCarrito
    {
        /// <summary>
        /// Identificador del carrito
        /// </summary>
        public int IdCarrito { get; set; }
        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public int IdUsuario { get; set; }
        /// <summary>
        /// Identificador del detalle del producto
        /// </summary>
        public int IdDetalleProducto { get; set; }
        /// <summary>
        /// Cantidad de productos
        /// </summary>
        public int? Cantidad { get; set; }

        public virtual TblDetalleProducto IdDetalleProductoNavigation { get; set; } = null!;
        public virtual TblUsuario IdUsuarioNavigation { get; set; } = null!;
    }
}
