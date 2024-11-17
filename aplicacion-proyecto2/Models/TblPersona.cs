using System;
using System.Collections.Generic;

namespace aplicacion_proyecto2.Models
{
    public partial class TblPersona
    {
        public TblPersona()
        {
            TblUsuarios = new HashSet<TblUsuario>();
        }

        /// <summary>
        /// Identificador de la persona
        /// </summary>
        public int IdPersona { get; set; }
        /// <summary>
        /// Identificador del metodo de pago
        /// </summary>
        public int IdMetodoPago { get; set; }
        /// <summary>
        /// Numero de identificacion de la persona
        /// </summary>
        public string NumeroIdentificacion { get; set; } = null!;
        /// <summary>
        /// Primer nombre de la persona
        /// </summary>
        public string PrimerNombre { get; set; } = null!;
        /// <summary>
        /// Segundo nombre de la persona
        /// </summary>
        public string? SegundoNombre { get; set; }
        /// <summary>
        /// Primer apellido de la persona
        /// </summary>
        public string PrimerApellido { get; set; } = null!;
        /// <summary>
        /// Segundo apellido de la persona
        /// </summary>
        public string? SegundoApellido { get; set; }

        public virtual TblMetodosPago IdMetodoPagoNavigation { get; set; } = null!;
        public virtual ICollection<TblUsuario> TblUsuarios { get; set; }
    }
}
