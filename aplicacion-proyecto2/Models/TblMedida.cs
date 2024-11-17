using System;
using System.Collections.Generic;

namespace aplicacion_proyecto2.Models
{
    public partial class TblMedida
    {
        public TblMedida()
        {
            TblDetalleProductos = new HashSet<TblDetalleProducto>();
        }

        /// <summary>
        /// Identificador de la medida
        /// </summary>
        public int IdMedida { get; set; }
        /// <summary>
        /// Medidad del producto, puede ser en cm, pies, pulgadas, entre otras
        /// </summary>
        public string Medida { get; set; } = null!;

        public virtual ICollection<TblDetalleProducto> TblDetalleProductos { get; set; }
    }
}
