namespace Datos.Modelo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Coordinacion")]
    public partial class Coordinacion
    {
        [Key]
        public int IdCoordinacion { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre_Coordinacion { get; set; }

        [Required]
        [StringLength(50)]
        public string Cedula { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellido { get; set; }

        [Required]
        [StringLength(20)]
        public string Telefono { get; set; }

        [StringLength(50)]
        public string Correo { get; set; }

        public int IdUsuario { get; set; }

        public int IdArea { get; set; }
    }
}
