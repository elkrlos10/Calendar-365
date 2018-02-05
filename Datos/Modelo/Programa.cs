namespace Datos.Modelo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Programa")]
    public partial class Programa
    {
        [Key]
        public int IdPrograma { get; set; }

        public int CodigoPrograma { get; set; }


        [StringLength(50)]
        public string Nivel { get; set; }

        [StringLength(200)]
        public string LineaTecnologica { get; set; }

        [StringLength(200)]
        public string Red_Tecnologica { get; set; }


        [StringLength(2500)]
        public string Perfil_Instructor { get; set; }

        public double Version_Programa { get; set; }

        public int IdArea { get; set; }

        [Required]
        [StringLength(400)]
        public string NombrePrograma { get; set; }
    }
}
