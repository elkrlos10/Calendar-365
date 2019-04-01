using Datos.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.LogicaNegocio
{
    public class CompetenciaBl
    {
        Model1 entity = new Model1();

        public IQueryable<Competencia> ConsultarListasCompetencias()
        {
            var Datos = from i in entity.Competencia
                        join p in entity.Programa on i.IdPrograma equals p.IdPrograma
                        where p.NombrePrograma != "Transversal"
                        select i;
            return Datos;
        }


        public List<Competencia> ConsultarCompetencias()
        {
            var Datos = (from i in entity.Competencia
                         join p in entity.Programa on i.IdPrograma equals p.IdPrograma
                         where p.NombrePrograma != "Transversal"
                         select i).ToList();
            return Datos;
        }

        public List<Competencia> ConsultarCompetenciasxArea(int Idcoordinacion)
        {
            var coordinacion = (from i in entity.Coordinacion
                                where i.IdCoordinacion == Idcoordinacion
                                select i).FirstOrDefault();

            var Datos = (from i in entity.Competencia
                         join p in entity.Programa on i.IdPrograma equals p.IdPrograma
                         where p.IdArea == coordinacion.IdArea
                         select i).ToList();
            return Datos;
        }

        /// <summary>
        /// Save   Listas
        /// </summary>
        /// <param name="objListas">Entity  Listas</param>
        /// <returns>Tercero</returns>
        public bool GuardarCompetencia(Competencia oCompetencia)
        {
            var Datos = (from i in entity.Competencia
                         where i.Codigo == oCompetencia.Codigo
                         && i.IdPrograma == oCompetencia.IdPrograma
                         select i).FirstOrDefault();
            if (Datos == null)
            {
                entity.Competencia.Add(oCompetencia);
                entity.SaveChanges();
                return true;
            }
            else
            {
                return false;

            }
        }

        public void EliminarCompetencia(int IdCompetencia)
        {

            var Datos = (from i in entity.Competencia
                         where i.IdCompetencia == IdCompetencia
                         select i).FirstOrDefault();
            entity.Competencia.Remove(Datos);
            entity.SaveChanges();
        }

        public List<Programa> ConsultarProgramas()
        {
            var Datos = (from i in entity.Programa
                         orderby i.NombrePrograma
                         select i).ToList();
            return Datos;
        }

        public Competencia ConsultarCompetenciaId(Int64 IdCompetencia)
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Competencia
                         where i.IdCompetencia == IdCompetencia
                         select i).FirstOrDefault();
            return Datos;

        }

        public Competencia ConsultarCompetenciaCodigo(Int64 Codigo, int IdPrograma)
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Competencia
                         join p in entity.Programa on i.IdPrograma equals p.IdPrograma
                         where i.Codigo == Codigo
                         && p.IdPrograma == IdPrograma
                         select i).FirstOrDefault();
            return Datos;

        }

        public Competencia ConsultarCompetenciaCodigo1(Int64 Codigo)
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Competencia
                         where i.Codigo == Codigo
                         select i).FirstOrDefault();
            return Datos;

        }


        public void ActualizarRegistro(Competencia oCompetencia)
        {
            Model1 entity = new Model1();

            var Item = (from i in entity.Competencia
                        where i.IdCompetencia == oCompetencia.IdCompetencia
                        select i).First();

            Item.Nombre = oCompetencia.Nombre;
            Item.Codigo = oCompetencia.Codigo;
            Item.IdPrograma = oCompetencia.IdPrograma;
            Item.Horas = oCompetencia.Horas;
            entity.SaveChanges();

        }

        public List<Competencia> ConsultarCompetenciasxPrograma(int IdPrograma)
        {
            var Item = (from i in entity.Competencia
                        where i.IdPrograma == IdPrograma
                        orderby i.Nombre
                        select i).ToList();
            return Item;
        }
    }
}
