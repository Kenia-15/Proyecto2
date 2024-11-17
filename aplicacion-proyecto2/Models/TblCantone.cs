using System;
using System.Collections.Generic;

namespace aplicacion_proyecto2.Models
{
    public partial class TblCantone
    {
        public TblCantone()
        {
            TblDistritos = new HashSet<TblDistrito>();
        }

        /// <summary>
        /// Identificador del canton
        /// </summary>
        public int IdCanton { get; set; }
        /// <summary>
        /// Identificador de la provincia
        /// </summary>
        public int IdProvincia { get; set; }
        /// <summary>
        /// Nombre del canton
        /// </summary>
        public string Canton { get; set; } = null!;

        public virtual TblProvincia IdProvinciaNavigation { get; set; } = null!;
        public virtual ICollection<TblDistrito> TblDistritos { get; set; }
    }
}
