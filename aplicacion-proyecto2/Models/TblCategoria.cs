using System;
using System.Collections.Generic;

namespace aplicacion_proyecto2.Models
{
    public partial class TblCategoria
    {
        public TblCategoria()
        {
            TblProductos = new HashSet<TblProducto>();
        }

        /// <summary>
        /// Identificador de la categoria del producto
        /// </summary>
        public int IdCategoria { get; set; }
        /// <summary>
        /// Categoria del producto
        /// </summary>
        public string Categoria { get; set; } = null!;
        /// <summary>
        /// Estado del usuario. Posibles valores: A: Activo, I: Inactivo
        /// </summary>
        public string Estado { get; set; } = null!;

        public virtual ICollection<TblProducto> TblProductos { get; set; }
    }
}
