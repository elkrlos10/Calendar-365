﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Datos.Modelo
{
    [Table("PrestamoAmbiente")]
    public class PrestamoAmbiente
    {
        
        public int Id { get; set; }
        public int IdAmbiente { get; set; }
        public bool Recibio { get; set; }
        public bool Entrego { get; set; }
        [Column(TypeName = "date")]
        public DateTime Fecha { get; set; }
        public TimeSpan? HoraRecibio { get; set; }
        public TimeSpan? HoraEntrego { get; set; }
        public string Observacion { get; set; }
    }
}
