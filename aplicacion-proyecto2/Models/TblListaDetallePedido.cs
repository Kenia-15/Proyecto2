namespace aplicacion_proyecto2.Models
{
    public class TblListaDetallePedido
    {
     public TblListaDetallePedido()
     {
     }
        public int IdDetallePedido { get; set; }
        public int IdPedido { get; set; }
        public int IdDetalleProducto { get; set; }
        public int IdUsuario { get; set; }
        public int Cantidad { get; set; }
        public string? NombreProducto { get; set; }
        public decimal? Precio { get; set; }
        public string? Medida { get; set; }
        public string? Color { get; set; }

    }
}
