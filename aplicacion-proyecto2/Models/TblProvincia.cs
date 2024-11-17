using System;
using System.Collections.Generic;

namespace aplicacion_proyecto2.Models
{
    public partial class TblProvincia
    {
        public TblProvincia()
        {
            TblCantones = new HashSet<TblCantone>();
            TblDistritos = new HashSet<TblDistrito>();
        }

        /// <summary>
        /// Identificador de la provincia
        /// </summary>
        public int IdProvincia { get; set; }
        /// <summary>
        /// Nombre de la provincia
        /// </summary>
        public string Provincia { get; set; } = null!;

        public virtual ICollection<TblCantone> TblCantones { get; set; }
        public virtual ICollection<TblDistrito> TblDistritos { get; set; }
    }
}
