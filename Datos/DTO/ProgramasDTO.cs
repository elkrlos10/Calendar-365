using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.DTO
{
    public class ProgramasDTO
    {
        public int CodigoPrograma { get; set; }

        public string Nivel { get; set; }

        public string LineaTecnologica { get; set; }

        public string Red_Tecnologica { get; set; }

        public double Version_Programa { get; set; }

        public String Area { get; set; }

        public string NombrePrograma { get; set; }

        public Int64 CodigoCompetencia { get; set; }

        public int Horas { get; set; }

        public string NombreCompetencia { get; set; }

        public int CodigoResultado { get; set; }

        public string Resultado { get; set; }
    }
}
