using System;
using System.Collections.Generic;

namespace aplicacion_proyecto2.Models
{
    public partial class TblPromocione
    {
        /// <summary>
        /// Identificador de la promoción
        /// </summary>
        public int IdPromocion { get; set; }
        /// <summary>
        /// Identificador del producto
        /// </summary>
        public int IdProducto { get; set; }
        /// <summary>
        /// Descripcion de la promocion
        /// </summary>
        public string? Descripcion { get; set; }
        /// <summary>
        /// Descuento aplicado al producto
        /// </summary>
        public int Descuento { get; set; }
        /// <summary>
        /// Estao de la promocion. Posibles valores: A:Activa, I:Inactiva
        /// </summary>
        public string Estado { get; set; } = null!;

        public virtual TblProducto IdProductoNavigation { get; set; } = null!;
    }
}
