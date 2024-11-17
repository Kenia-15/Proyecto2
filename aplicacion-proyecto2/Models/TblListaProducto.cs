using System;
using System.Collections.Generic;

namespace aplicacion_proyecto2.Models
{
    public partial class TblListaProducto
    {
        public TblListaProducto()
        {
            
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
        /// Medida del producto
        /// </summary>
        public string Medida { get; set; }
        /// <summary>
        /// Color del producto
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// Cantidad de productos en stock
        /// </summary>
        public int Stock { get; set; }
        
        /// <summary>
        /// Nombre del productos
        /// </summary>
        public string NombreProducto { get; set; }
        /// <summary>
        /// Descripción del produto
        /// </summary>
        public string Descripcion { get; set; }

        public decimal Precio { get; set; }
    }
}
