namespace Datos.Modelo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Competencia")]
    public partial class Competencia
    {
        [Key]
        public Int64 IdCompetencia { get; set; }

        public Int64 Codigo { get; set; }

        public int Horas { get; set; }

        public int IdPrograma { get; set; }

        [Required]
        [StringLength(1000)]
        public string Nombre { get; set; }

    }
}
