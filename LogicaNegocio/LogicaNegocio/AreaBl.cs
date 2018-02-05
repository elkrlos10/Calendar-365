using Datos.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.LogicaNegocio
{
    public class AreaBl
    {

        Model1 entity = new Model1();

        public IQueryable<Area> ConsultarListasAreas()
        {
            var Datos = from i in entity.Area
                        select i;
            return Datos;
        }

        public string ConsultarNombreArea(int id)
        {
            var Datos = (from i in entity.Area
                         where i.IdArea == id
                         select i.Nombre).FirstOrDefault();
            return Datos;
        }

        public List<Area> ConsultarAreas()
        {
            var Datos = (from i in entity.Area
                         orderby i.Nombre
                         select i).ToList();
            return Datos;
        }

        /// <summary>
        /// Save   Listas
        /// </summary>
        /// <param name="objListas">Entity  Listas</param>
        /// <returns>Tercero</returns>
        public bool GuardarArea(Area oArea)
        {
            var Datos = (from i in entity.Area
                         where i.Codigo == oArea.Codigo
                         select i).FirstOrDefault();
            if (Datos == null)
            {
                entity.Area.Add(oArea);
                entity.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }            
        }

        public bool EliminarArea(int IdArea)
        {

            var Datos = (from i in entity.Area
                         from a in entity.Ambiente
                         where a.IdArea == IdArea
                         select i).FirstOrDefault();
            if (Datos != null)
            {
                return false;
            }
            else
            {
                var areas = (from i in entity.Area
                             where i.IdArea == IdArea
                             select i).FirstOrDefault();
                entity.Area.Remove(areas);
                entity.SaveChanges();
                return true;
            }

        }


        public Area ConsultarAreaId(int IdArea)
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Area
                         where i.IdArea == IdArea
                         select i).FirstOrDefault();
            return Datos;
        }

        public Area ConsultarAreaCodigo(int Codigo)
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Area
                         where i.Codigo == Codigo
                         select i).FirstOrDefault();
            return Datos;
        }


        public void ActualizarRegistro(Area oArea)
        {
            Model1 entity = new Model1();

            var Item = (from i in entity.Area
                        where i.IdArea == oArea.IdArea
                        select i).First();

            Item.Nombre = oArea.Nombre;
            Item.Descripcion = oArea.Descripcion;
            Item.Codigo = oArea.Codigo;
            //Item.IdCoordinacion = oArea.IdCoordinacion;
            entity.SaveChanges();

        }

        public List<Coordinacion> ConsultarCoordinaciones()
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Coordinacion
                         select i).ToList();
            return Datos;
        }
        /// <summary>
        /// Actualiza un Listas
        /// </summary>
        /// <param name="objListas>">Entity  Listas</param>
        /// <returns>Listas</returns>
        //public void ActualizarListas(Listas oListas)
        //{
        //    ListasDao oListasDao = new ListasDao();
        //    oListasDao.ActualizarListas(oListas);
        //}

        ///// <summary>
        ///// Delete  Listas
        ///// </summary>
        ///// <param name="objListas>">Entity  Listas</param>
        //public void EliminarListas(string Id)
        //{
        //    ListasDao oListasDao = new ListasDao();
        //    oListasDao.EliminarListas(Id);
        //}

    }
}
