using System;
using System.Collections.Generic;

namespace aplicacion_proyecto2.Models
{
    public partial class TblPedido
    {
        public TblPedido()
        {
            TblDetallePedidos = new HashSet<TblDetallePedido>();
        }

        /// <summary>
        /// Identificador del pedido
        /// </summary>
        public int IdPedido { get; set; }
        /// <summary>
        /// Identificador del usuario que realiza el pedido
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Monto total del pedido
        /// </summary>
        public decimal MontoTotal { get; set; }
        /// <summary>
        /// Telefono del usuario que realiza el pedido
        /// </summary>
        public int? Telefono { get; set; }
        /// <summary>
        /// Direccion del usuario que realiza el pedido
        /// </summary>
        public string Direccion { get; set; } = null!;
        /// <summary>
        /// Fecha en que se realiza el pedido
        /// </summary>
        public DateTime? Fecha { get; set; }

        public virtual TblUsuario IdUsuarioNavigation { get; set; } = null!;
        public virtual ICollection<TblDetallePedido> TblDetallePedidos { get; set; }
    }
}
