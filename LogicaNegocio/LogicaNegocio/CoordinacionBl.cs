using Datos.Modelo;
using LogicaNegocio.Enumeraciones;
using LogicaNegocio.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.LogicaNegocio
{
    public class CoordinacionBl
    {
        Model1 entity = new Model1();

        public IQueryable<Coordinacion> ConsultarCoordinacion()
        {
            var Datos = from i in entity.Coordinacion
                        orderby i.Nombre_Coordinacion
                        select i;
            return Datos;
        }

        public int GuardarCoordinacion(Coordinacion oCoordinacion)
        {
            Model1 entity = new Model1();

            var coordinador = (from i in entity.Coordinacion
                               where i.Cedula == oCoordinacion.Cedula 
                               select i).FirstOrDefault();

            var coordinacion = (from i in entity.Coordinacion
                                where i.IdArea == oCoordinacion.IdArea
                                select i).FirstOrDefault();

            if (coordinacion != null)
            {
                return 3;
            }

            if (coordinador == null)
            {
                Usuario oUsuario = new Usuario();
                oUsuario.NombreUsuario = oCoordinacion.Cedula;
                var Encriptar = SecurityEncode.SecurityEncode.Encriptar(oCoordinacion.Cedula);
                oUsuario.Password = Encriptar;
                oUsuario.TipoUsuario = (int)TipoUsuario.Coordinador;
                entity.Usuario.Add(oUsuario);
                entity.SaveChanges();
                oCoordinacion.IdUsuario = oUsuario.IdUsuario;
                entity.Coordinacion.Add(oCoordinacion);
                entity.SaveChanges();
                var Asunto = "Nuevo Usuario";
                var Plantilla = "Usuario : " + oUsuario.NombreUsuario + "<br/> Contraseña: " + oUsuario.NombreUsuario;
                SendMail.SendMailMessage(Asunto, Plantilla, oCoordinacion.Correo);
                return 1;
            }
            else
            {
                return 2;
            }

            
        }

        public void EliminarCoordinacion(int IdCoordinacion)
        {
            
            var Datos = (from i in entity.Coordinacion
                         where i.IdCoordinacion == IdCoordinacion
                         select i).FirstOrDefault();
            entity.Coordinacion.Remove(Datos);
            entity.SaveChanges();

            var Usuario = (from i in entity.Usuario
                           where i.NombreUsuario == Datos.Cedula
                           select i).FirstOrDefault();
            entity.Usuario.Remove(Usuario);
            entity.SaveChanges();
        }

        public Coordinacion ConsultarCoordinacionId(int IdCoordinacion)
        {
            var Datos = (from i in entity.Coordinacion
                         where i.IdCoordinacion == IdCoordinacion
                         select i).FirstOrDefault();
            return Datos;
        }

        public void ActualizarRegistro(Coordinacion oCoordinacion)
        {
            var Item = (from i in entity.Coordinacion
                        where i.IdCoordinacion == oCoordinacion.IdCoordinacion
                        select i).FirstOrDefault();
            Item.Nombre_Coordinacion = oCoordinacion.Nombre_Coordinacion;
            Item.Cedula = oCoordinacion.Cedula;
            Item.Nombre = oCoordinacion.Nombre;
            Item.Apellido = oCoordinacion.Apellido;
            Item.Telefono = oCoordinacion.Telefono;
            Item.Correo = oCoordinacion.Correo;
            entity.SaveChanges();
        }

        public string ConsultarNombreCoordinacion(int Id)
        {
            var Datos = (from i in entity.Coordinacion
                         where i.IdCoordinacion == Id
                         select i.Nombre_Coordinacion).FirstOrDefault();
            return Datos;
        }
    }
}
