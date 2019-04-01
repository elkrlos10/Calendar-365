namespace Datos.Modelo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sede")]
    public partial class Sede
    {
        [Key]
        public int IdSede { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre_Sede { get; set; }

        [Required]
        [StringLength(50)]
        public string Direccion { get; set; }

        [Required]
        [StringLength(50)]
        public string IdMunicipio { get; set; }


        public int Codigo { get; set; }

        public int TipoSede { get; set; }

        public int? IdUsuario { get; set; }

    }
}
