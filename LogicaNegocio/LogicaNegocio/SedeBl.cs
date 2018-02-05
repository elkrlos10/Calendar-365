using Datos.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.LogicaNegocio
{
    public class SedeBl
    {
        Model1 entity = new Model1();

        public bool GuardarSede(Sede oSede)
        {
            var Datos = (from i in entity.Sede
                         where i.Codigo == oSede.Codigo
                         select i).FirstOrDefault();
            if (Datos == null)
            {
                entity.Sede.Add(oSede);
                entity.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Sede> ConsultarSedes()
        {
            var Datos = (from i in entity.Sede
                        where i.TipoSede != 3
                        orderby i.Nombre_Sede
                        select i).ToList();
            return Datos;
        }

        public List<Sede> ConsultarColegios()
        {
            var Datos = (from i in entity.Sede
                         where i.TipoSede == 3
                         orderby i.Nombre_Sede
                         select i).ToList();
            return Datos;
        }

        public Sede ConsultarSede(int IdSede)
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Sede
                         where i.IdSede == IdSede
                         select i).FirstOrDefault();
            return Datos;

        }

        public Sede ConsultarSedeCodigo(int Codigo)
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Sede
                         where i.Codigo == Codigo
                         select i).FirstOrDefault();
            return Datos;

        }

        public Municipio ConsultarMunicipioxNombre(string NombreMunicipio, string NombreDepartamento )
        {


            Model1 entity = new Model1();

            var departamento = (from i in entity.Departamento
                                where i.NombreDepartamento.ToUpper() == NombreDepartamento.ToUpper()
                                select i).FirstOrDefault();

            var Datos = (from i in entity.Municipio
                         where i.NombreMunicipio.ToString().ToUpper() == NombreMunicipio && i.IdDepartamento == departamento.IdDepartamento
                         select i).FirstOrDefault();
            return Datos;

        }

        public void ActualizarRegistro(Sede oSede)
        {
            Model1 entity = new Model1();

            var Item = (from i in entity.Sede
                        where i.IdSede == oSede.IdSede
                        select i).First();

            Item.Nombre_Sede = oSede.Nombre_Sede;

            Item.Direccion = oSede.Direccion;

            Item.IdMunicipio = oSede.IdMunicipio;

            Item.Codigo = oSede.Codigo;

            Item.TipoSede = oSede.TipoSede;

            entity.SaveChanges();

        }

        public bool EliminarSede(int IdSede)
        {

            var Datos = (from i in entity.Sede
                         from f in entity.Ambiente
                         where IdSede == i.IdSede && IdSede == f.IdSede
                         select i).FirstOrDefault();
            if (Datos != null)
            {
                return false;
            }
            else
            {
                var Sedes = (from i in entity.Sede
                             where i.IdSede == IdSede
                             select i).FirstOrDefault();
                entity.Sede.Remove(Sedes);
                entity.SaveChanges();
                return true;
            }

        }

        public Sede ConsultarSedeId(int IdSede)
        {
           
            var Datos = (from i in entity.Sede
                         where i.IdSede == IdSede
                         select i).FirstOrDefault();
            return Datos;

        }

        public List<Departamento> consultarDepartamento()
        {
            var departamentos = (from i in entity.Departamento
                                 select i).ToList();

            return departamentos;

        }

        public Departamento consultarDepartamentoxMunicipio(string IdMunicipio)
        {
            var Municipio = (from i in entity.Municipio
                             where i.IdMunicipio == IdMunicipio
                             select i).FirstOrDefault();

            var departamentos = (from i in entity.Departamento
                                 where i.IdDepartamento == Municipio.IdDepartamento
                                 select i).FirstOrDefault();

            return departamentos;

        }

        public List<Municipio> consultarMunicipios(int IdDepartamento)
        {
            var departamentos = (from i in entity.Municipio
                                 where i.IdDepartamento == IdDepartamento
                                 select i).ToList();

            return departamentos;

        }

        public Municipio consultarMunicipioxId(string IdMunicipio)
        {
            var departamentos = (from i in entity.Municipio
                                 where i.IdMunicipio == IdMunicipio
                                 select i).FirstOrDefault();

            return departamentos;

        }

        public Sede CodigoSede()
        {

            var codigo = (from i in entity.Sede
                          orderby i.Codigo descending
                          select i).FirstOrDefault();

            return codigo;
        }
    }
}
