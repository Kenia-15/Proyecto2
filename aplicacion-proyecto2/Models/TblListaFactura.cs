namespace aplicacion_proyecto2.Models
{
    public class TblListaFactura
    {
        public TblListaFactura() { }

        public TblPedido? pedido { get; set; }
        public List<TblListaDetallePedido>? detalle { get; set; } 
        public TblPersona? persona { get; set; }    
        public TblProducto? producto { get; set; }  
        public TblUsuario? usuario { get; set; }
        public TblMedida? medida { get; set; }
        public TblColore? color { get; set; }
    }
}
