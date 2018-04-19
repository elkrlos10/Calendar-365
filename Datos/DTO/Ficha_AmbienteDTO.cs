using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.DTO
{
    public class Ficha_AmbienteDTO
    {
        public int Id { get; set; }

        public int IdFicha { get; set; }

        public int IdAmbiente { get; set; }

        public string Ambiente { get; set; }

        public int IdInstructor { get; set; }

        public string CedulaIns { get; set; }

        public string NombreInstructor { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public TimeSpan? HoraInicio { get; set; }

        public TimeSpan? HoraFin { get; set; }

        public string Color { get; set; }

        public string Ficha { get; set; }

        public string Competencia { get; set; }

        public string CodigoCompetencia { get; set; }

        public string Resultado { get; set; }

        public string Programa { get; set; }

        public string Estado { get; set; }

        public string Jornada { get; set; }

        public int NumeroJornada { get; set; }
        public int? DiaSemana { get; set; }

        public string Dia { get; set; }

        public string CedulaInstructor { get; set; }

        public int CodigoPrograma { get; set; }

        public int CodigoResultado { get; set; }

        public double VersionPrograma { get; set; }

        public string NivelPrograma { get; set; }
        public TimeSpan TotalHoras { get; set; }

        public string Sede { get; set; }

        public string DiasProgramados { get; set; }

        public bool? Lunes { get; set; }

        public bool? Martes { get; set; }

        public bool? Miercoles { get; set; }

        public bool? Jueves { get; set; }

        public bool? Viernes { get; set; }

        public string Area { get; set; }

        public bool RecibioLLaves { get; set; }
        public bool EntregoLLaves { get; set; }

        public string Observacion { get; set; }
        public string NombreEvento { get; set; }

    }
}
