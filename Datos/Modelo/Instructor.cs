namespace Datos.Modelo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Instructor")]
    public partial class Instructor
    {
        [Key]
        public int IdInstructor { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellido { get; set; }

        [Required]
        [StringLength(50)]
        public string Cedula { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        public bool Estado { get; set; }

        public int TipoContrato { get; set; }

        [StringLength(50)]
        public string Telefono { get; set; }

        public int IdUsuario { get; set; }

        public int TipoInstructor { get; set; }

        public int IdArea { get; set; }

        public bool EnvioCorreo { get; set; }
    }
}
