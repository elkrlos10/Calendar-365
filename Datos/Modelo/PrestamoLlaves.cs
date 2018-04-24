using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Modelo
{
    public class PrestamoLlaves
    {
        public int Id { get; set; }
        public int IdFicha_Ambiente { get; set; }
        public bool Recibio { get; set; }
        public bool Entrego { get; set; }
        [Column(TypeName = "date")]
        public DateTime Fecha { get; set; }
        public TimeSpan? HoraRecibio { get; set; }
        public TimeSpan? HoraEntrego { get; set; }
        public string Observacion { get; set; }
        public TimeSpan? HoraRecibio2 { get; set; }
        public TimeSpan? HoraEntrego2 { get; set; }

    }
}
