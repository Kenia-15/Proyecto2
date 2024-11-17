using System;
using System.Collections.Generic;

namespace aplicacion_proyecto2.Models
{
    public partial class TblColore
    {
        public TblColore()
        {
            TblDetalleProductos = new HashSet<TblDetalleProducto>();
        }

        /// <summary>
        /// Identificador del color
        /// </summary>
        public int IdColor { get; set; }
        /// <summary>
        /// Color del producto
        /// </summary>
        public string Color { get; set; } = null!;

        public virtual ICollection<TblDetalleProducto> TblDetalleProductos { get; set; }
    }
}
