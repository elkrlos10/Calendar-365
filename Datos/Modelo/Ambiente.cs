namespace Datos.Modelo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ambiente")]
    public partial class Ambiente
    {

        [Key]
        public int IdAmbiente { get; set; }

        public int IdSede { get; set; }

        public int Piso { get; set; }

        [Required]
        [StringLength(10)]
        public string Numero { get; set; }

        public int IdArea { get; set; }

        public bool Pantalla { get; set; }

        public int? NumeroEquipos { get; set; }

        public int? NumeroSillas { get; set; }

        public int? NumeroMesas { get; set; }


    }
}
