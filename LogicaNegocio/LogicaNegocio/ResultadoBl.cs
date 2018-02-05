using Datos.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.LogicaNegocio
{
    public class ResultadoBl
    {
        Model1 entity = new Model1();

        public List<Resultado_Aprendizaje> ConsultarResultados()
        {
            var Datos = (from i in entity.Resultado_Aprendizaje
                        select i).ToList();
            return Datos;
        }

        public bool GuardarResultado(Resultado_Aprendizaje oResultado_Aprendizaje)
        {
            var Datos = (from i in entity.Resultado_Aprendizaje
                         where i.IdCompetencia == oResultado_Aprendizaje.IdCompetencia
                         && i.Codigo == oResultado_Aprendizaje.Codigo
                         select i).FirstOrDefault();

            if (Datos == null)
            {
                entity.Resultado_Aprendizaje.Add(oResultado_Aprendizaje);
                entity.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public void EliminarResultado(int IdResultado)
        {
            var Datos = (from i in entity.Resultado_Aprendizaje
                         where i.IdResultado == IdResultado
                         select i).FirstOrDefault();
            entity.Resultado_Aprendizaje.Remove(Datos);
            entity.SaveChanges();
        }

        public Resultado_Aprendizaje ConsultarResultadoId(int IdResultado)
        {
            var Datos = (from i in entity.Resultado_Aprendizaje
                         where i.IdResultado == IdResultado
                         select i).FirstOrDefault();
            return Datos;
        }

        public List<Competencia> ConsultarCompetenciaxCodigo(Int64 CodigoCompetencia)
        {
            var ListaCompetencias = (from i in entity.Competencia
                                     where i.Codigo == CodigoCompetencia
                                     select i).ToList();
            return ListaCompetencias;
        }
        

        public List<Resultado_Aprendizaje> ConsultarResultadoCodigo(int Codigo, Int64 IdCompetencia)
        {

            //var competencia = (from i in entity.Competencia
            //                   where i.Codigo == CodigoCompetencia
            //                   select i).FirstOrDefault();

            var Datos = (from i in entity.Resultado_Aprendizaje
                         join c in entity.Competencia on i.IdCompetencia equals c.IdCompetencia
                         where i.Codigo == Codigo
                         && c.IdCompetencia == IdCompetencia
                         select i).ToList();
            return Datos;
        }

        public List<Resultado_Aprendizaje> ConsultarResultadoXCompetencia(int IdCompetencia)
        {
            var Datos = (from i in entity.Resultado_Aprendizaje
                         where i.IdCompetencia == IdCompetencia
                         select i).ToList();
            return Datos;
        }

        public void ActualizarRegistro(Resultado_Aprendizaje oResultado_Aprendizaje)
        {
            var Datos = (from i in entity.Resultado_Aprendizaje
                         where i.IdResultado == oResultado_Aprendizaje.IdResultado
                         select i).FirstOrDefault();
            Datos.Codigo = oResultado_Aprendizaje.Codigo;
            Datos.Resultado = oResultado_Aprendizaje.Resultado;
            Datos.IdCompetencia = oResultado_Aprendizaje.IdCompetencia;
            entity.SaveChanges();
        }

        public List<Competencia> ConsultarCompetencias()
        {
            var Datos = (from i in entity.Competencia
                         orderby i.Nombre
                         select i).ToList();
            return Datos;
        }
    }
}
