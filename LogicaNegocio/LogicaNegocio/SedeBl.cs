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
            var oUsuario = new Usuario();

            if (Datos == null)
            {
                if (oSede.TipoSede==1)
                {
                    var NombreSede = oSede.Nombre_Sede.Replace(" ", "");
                    oUsuario.NombreUsuario = oSede.Nombre_Sede;
                    var Encriptar = SecurityEncode.SecurityEncode.Encriptar(NombreSede);
                    oUsuario.Password = Encriptar;
                    oUsuario.TipoUsuario = 4;
                    entity.Usuario.Add(oUsuario);
                    entity.SaveChanges();
                }
               
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
                         orderby i.Nombre_Sede
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
                                 orderby i.NombreDepartamento
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
                                 orderby i.NombreMunicipio
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

        public List<Usuario> ConsultarUsuarioSede()
        {
            var Users = (from i in entity.Usuario
                         where i.TipoUsuario == 4
                         orderby i.NombreUsuario
                         select i).ToList();

            foreach (var item in Users.Select((value, i) => new { i, value }))
            {
                var pass = SecurityEncode.SecurityEncode.Desencriptar(item.value.Password);
                Users[item.i].Password = pass;
            }

            return Users;
        }

        public List<Usuario> ModificarUsuario(Usuario oUsuario)
        {
            var User = (from i in entity.Usuario
                         where i.IdUsuario == oUsuario.IdUsuario
                         select i).First();

            User.NombreUsuario = oUsuario.NombreUsuario;
            User.Password = SecurityEncode.SecurityEncode.Encriptar(oUsuario.Password);
            entity.SaveChanges();

            var Users= ConsultarUsuarioSede();


            return Users;
        }
    }
}
