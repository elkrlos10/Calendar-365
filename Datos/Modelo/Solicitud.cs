namespace Datos.Modelo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Solicitud")]
    public partial class Solicitud
    {
        [Key]

        public int IdSolicitud { get; set; }


        public int IdFicha_Ambiente { get; set; }


        public int IdCoordinacion { get; set; }
    }
}
