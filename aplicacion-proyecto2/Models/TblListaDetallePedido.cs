namespace aplicacion_proyecto2.Models
{
    public class TblListaDetallePedido
    {
     public TblListaDetallePedido()
     {
     }
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
        public int IdUsuario { get; set; }
        public int Cantidad { get; set; }

        public string Medida { get; set; } = null!;
        /// <summary>
        /// Color del producto
        /// </summary>
        public string Color { get; set; } = null!;
        /// <summary>
        /// Nombre del productos
        /// </summary>
        public string NombreProducto { get; set; } = null!;
        /// <summary>
        /// Descripción del produto
        /// </summary>
        public decimal Precio { get; set; }

    }
}
