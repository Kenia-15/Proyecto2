using System;
using System.Collections.Generic;

namespace aplicacion_proyecto2.Models
{
    public partial class TblDistrito
    {
        public TblDistrito()
        {
            TblPedidos = new HashSet<TblPedido>();
        }

        /// <summary>
        /// Identificador del distrito
        /// </summary>
        public int IdDistrito { get; set; }
        /// <summary>
        /// Identificador de la provincia
        /// </summary>
        public int IdProvincia { get; set; }
        /// <summary>
        /// Identificador del canton
        /// </summary>
        public int IdCanton { get; set; }
        /// <summary>
        /// Nombre del distrito
        /// </summary>
        public string Distrito { get; set; } = null!;

        public virtual TblCantone IdCantonNavigation { get; set; } = null!;
        public virtual TblProvincia IdProvinciaNavigation { get; set; } = null!;
        public virtual ICollection<TblPedido> TblPedidos { get; set; }
    }
}
