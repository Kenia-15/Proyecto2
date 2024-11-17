using System;
using System.Collections.Generic;

namespace aplicacion_proyecto2.Models
{
    public partial class TblUsuario
    {
        public TblUsuario()
        {
            TblCarritos = new HashSet<TblCarrito>();
            TblPedidos = new HashSet<TblPedido>();
        }

        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public int IdUsuario { get; set; }
        /// <summary>
        /// Identificador de la persona
        /// </summary>
        public int IdPersona { get; set; }
        /// <summary>
        /// Direccion de correo electronico del usuario
        /// </summary>
        public string Email { get; set; } = null!;
        /// <summary>
        /// Contraseña del usuario
        /// </summary>
        public string Contrasena { get; set; } = null!;
        /// <summary>
        /// Estado del usuario. Posibles valores: A: Activo, I: Inactivo
        /// </summary>
        public string Estado { get; set; } = null!;

        public virtual TblPersona IdPersonaNavigation { get; set; } = null!;
        public virtual ICollection<TblCarrito> TblCarritos { get; set; }
        public virtual ICollection<TblPedido> TblPedidos { get; set; }
    }
}
