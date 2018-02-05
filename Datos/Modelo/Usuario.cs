namespace Datos.Modelo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Usuario")]
    public partial class Usuario
    {
        [Key]

        public int IdUsuario { get; set; }

        [StringLength(50)]
        public string NombreUsuario { get; set; }


        [StringLength(40)]
        public string Password { get; set; }

        public int TipoUsuario { get; set; }

    }
}
