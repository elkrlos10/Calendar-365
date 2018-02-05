namespace Datos.Modelo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ficha_Ambiente
    {
        public int Id { get; set; }

        public int IdFicha { get; set; }

        public int IdAmbiente { get; set; }

        public int IdInstructor { get; set; }

        [Column(TypeName = "date")]
        public DateTime FechaInicio { get; set; }

        [Column(TypeName = "date")]
        public DateTime FechaFin { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        [Required]
        [StringLength(10)]
        public string Color { get; set; }

        public int IdResultado { get; set; }

        public bool Estado { get; set; }

        public int Jornada { get; set; }

        public int? DiaSemana { get; set; }

        public int? ProgramacionPrincipal { get; set; }

        public bool? Lunes { get; set; }

        public bool? Martes { get; set; }

        public bool? Miercoles { get; set; }

        public bool? Jueves { get; set; }

        public bool? Viernes { get; set; }

        public bool? Transversal { get; set; }

        public string NombreEmpresa { get; set; }

    }
}
