using System;
using System.Collections.Generic;

namespace aplicacion_proyecto2.Models
{
    public partial class TblMetodosPago
    {
        public TblMetodosPago()
        {
            TblPersonas = new HashSet<TblPersona>();
        }

        /// <summary>
        /// Identificador del metodo de pago
        /// </summary>
        public int IdMetodoPago { get; set; }
        /// <summary>
        /// Metodo de pago
        /// </summary>
        public string MetodoPago { get; set; } = null!;
        /// <summary>
        /// Estado del metodo de pago. Posibles valores: A: Activo, I: Inactivo
        /// </summary>
        public string Estado { get; set; } = null!;

        public virtual ICollection<TblPersona> TblPersonas { get; set; }
    }
}
