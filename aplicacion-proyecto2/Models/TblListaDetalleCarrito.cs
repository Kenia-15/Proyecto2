namespace aplicacion_proyecto2.Models
{
    public class TblListaDetalleCarrito
    {
        public TblListaDetalleCarrito()
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

        public int IdCarrito { get; set; }
        /// <summary>
        /// Medida del producto
        /// </summary>
        public string Medida { get; set; } = null!;
        /// <summary>
        /// Color del producto
        /// </summary>
        public string Color { get; set; } = null!;
        /// <summary>
        /// Cantidad de productos en stock
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Nombre del productos
        /// </summary>
        public string NombreProducto { get; set; } = null!;
        /// <summary>
        /// Descripción del produto
        /// </summary>
        public string Descripcion { get; set; } = null!;

        public decimal Precio { get; set; }

        public string? RutaImagen { get; set; }

        public int Cantidad { get; set; }

    }
}
