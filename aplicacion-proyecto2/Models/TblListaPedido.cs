using Microsoft.AspNetCore.Mvc.Rendering;

namespace aplicacion_proyecto2.Models
{
    public class TblListaPedido
    {
        public TblPedido? pedido { get; set; }
        public TblDetallePedido? detallePedido { get; set; }
        public TblUsuario? usuario { get; set; }

    }
}
