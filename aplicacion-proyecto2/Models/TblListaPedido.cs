namespace aplicacion_proyecto2.Models
{
    public class TblListaPedido
    {
        public TblPedido? pedido { get; set; }
        public TblDetallePedido? detallePedido { get; set; }
        public TblUsuario? usuario { get; set; }
        public TblProvincia? provincia { get; set; }
        public TblCantone? cantone { get; set; }
        public TblDistrito? distrito { get; set; } 

    }
}
