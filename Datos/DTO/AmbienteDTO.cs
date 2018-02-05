using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.DTO
{
    public class AmbienteDTO
    {
        
        public int IdAmbiente { get; set; }

        public int IdSede { get; set; }

        public int Piso { get; set; }

        public string Numero { get; set; }

        public int IdArea { get; set; }

        public bool Pantalla { get; set; }

        public int? NumeroEquipos { get; set; }

        public int? NumeroSillas { get; set; }

        public int? NumeroMesas { get; set; }

        public string NombreSede { get; set; }
    }
}
