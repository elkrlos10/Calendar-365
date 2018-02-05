namespace Datos.Modelo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Resultado_Aprendizaje
    {
        [Key]
        public int IdResultado { get; set; }

        public int Codigo { get; set; }

        [Required]
        [StringLength(1000)]
        public string Resultado { get; set; }

        public Int64 IdCompetencia { get; set; }
    }
}
