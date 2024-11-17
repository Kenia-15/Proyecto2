using System;
using System.Collections.Generic;

namespace aplicacion_proyecto2.Models
{
    public partial class TblProducto
    {
        public TblProducto()
        {
            TblDetalleProductos = new HashSet<TblDetalleProducto>();
            TblPromociones = new HashSet<TblPromocione>();
        }

        /// <summary>
        /// Identificador del producto
        /// </summary>
        public int IdProducto { get; set; }
        /// <summary>
        /// Identificador de la categoria del producto
        /// </summary>
        public int IdCategoria { get; set; }
        /// <summary>
        /// Nombre del producto
        /// </summary>
        public string NombreProducto { get; set; } = null!;
        /// <summary>
        /// Descripcion del producto
        /// </summary>
        public string? Descripcion { get; set; }
        /// <summary>
        /// Precio del producto
        /// </summary>
        public decimal? Precio { get; set; }

        public virtual TblCategoria IdCategoriaNavigation { get; set; } = null!;
        public virtual ICollection<TblDetalleProducto> TblDetalleProductos { get; set; }
        public virtual ICollection<TblPromocione> TblPromociones { get; set; }
    }
}
