namespace Datos.Modelo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ficha")]
    public partial class Ficha
    {
        [Key]
        public int IdFicha { get; set; }

        public int IdPrograma { get; set; }

        [Required]
        [StringLength(15)]
        public string Ficha1 { get; set; }

        public int NumAprendices { get; set; }

        public bool? Estado { get; set; }

        public string TipoFormacion { get; set; }

        [Column(TypeName = "date")]
        public DateTime FechaInicio { get; set; }

        [Column(TypeName = "date")]
        public DateTime FechaFin { get; set; }

        public string Jornada { get; set; }

    }
}
