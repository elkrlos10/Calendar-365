using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.DTO
{
    public class InstructorDTO
    {
        public int IdInstructor { get; set; }
        
        public string Nombre { get; set; }
        
        public string Apellido { get; set; }
        
        public string Cedula { get; set; }

        public string Email { get; set; }
        
        public bool Estado { get; set; }

        public int TipoContrato { get; set; }

        public string NombreTipoContrato { get; set; }

        public string Telefono { get; set; }

        public int IdUsuario { get; set; }

        public int TipoInstructor { get; set; }

        public string NombreTipoInstructor { get; set; }

        public int IdPrograma { get; set; }

        public double? HorasRango { get; set; }

        public double? Total { get; set; }

        public int? Jornada { get; set; }

        public string NombreArea { get; set; }

        public int IdArea { get; set; }
       
    }
}
