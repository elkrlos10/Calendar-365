namespace Datos.Modelo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Municipio")]
    public partial class Municipio
    {
        [Key]
        [StringLength(50)]
        public string IdMunicipio { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreMunicipio { get; set; }

        public int IdDepartamento { get; set; }
    }
}
