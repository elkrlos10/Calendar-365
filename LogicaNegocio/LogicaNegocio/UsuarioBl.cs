using Datos.DTO;
using Datos.Modelo;
using LogicaNegocio.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.LogicaNegocio
{
    public class UsuarioBl
    {
        Model1 entity = new Model1();

        public PersonaDTO IniciarSesion(string Usuario, string Password)
        {
            var Encriptar = SecurityEncode.SecurityEncode.Encriptar(Password);
            PersonaDTO oPersonaDTO = new PersonaDTO();
            var Datos = (from i in entity.Usuario
                         where i.NombreUsuario == Usuario
                         && i.Password == Encriptar
                         select i).FirstOrDefault();

            if (Datos == null)
            {
                return null;
            }

            var Coordinador = (from i in entity.Coordinacion
                               where i.IdUsuario == Datos.IdUsuario
                               select i).FirstOrDefault();

            var Instructor = new Instructor();

            if (Coordinador == null)
            {
               var inst = (from i in entity.Usuario
                         join p in entity.Instructor on i.IdUsuario equals p.IdUsuario
                         where i.NombreUsuario == Usuario && p.Cedula == Usuario
                         && i.Password == Encriptar
                         select i).FirstOrDefault();

                if (inst!= null)
                {
                    Instructor = (from i in entity.Instructor
                                  where i.IdUsuario == inst.IdUsuario
                                  select i).FirstOrDefault();
                }
                else
                {
                    Instructor = null;
                }

            }
            else
            {
                Instructor = null;
            }


            if (Instructor != null)
            {
                oPersonaDTO.IdUsuario = Instructor.IdUsuario;
                oPersonaDTO.Nombre = Instructor.Nombre;
                oPersonaDTO.Apellido = Instructor.Apellido;
                oPersonaDTO.Cedula = Instructor.Cedula;
                oPersonaDTO.TipoUsuario = Datos.TipoUsuario;
                oPersonaDTO.IdPersona = Instructor.IdInstructor;
            }
            if (Coordinador != null)
            {
                oPersonaDTO.IdUsuario = Coordinador.IdUsuario;
                oPersonaDTO.Nombre = Coordinador.Nombre;
                oPersonaDTO.Apellido = Coordinador.Apellido;
                oPersonaDTO.Cedula = Coordinador.Cedula;
                oPersonaDTO.TipoUsuario = Datos.TipoUsuario;
                oPersonaDTO.IdPersona = Coordinador.IdCoordinacion;
            }
            if (Instructor == null && Coordinador == null)
            {
                oPersonaDTO.IdUsuario = 0;
                oPersonaDTO.Nombre = Datos.NombreUsuario;
                oPersonaDTO.Apellido = "";
                oPersonaDTO.Cedula = "";
                oPersonaDTO.TipoUsuario = Datos.TipoUsuario;
                oPersonaDTO.IdUsuario = Datos.IdUsuario;
            }
            return oPersonaDTO;
        }

        public string RecuperarPassword(string Email, string Cedula)
        {
            var Contraseña = "";
            PersonaDTO oPersonaDTO = new PersonaDTO();
            var Instructor = (from i in entity.Instructor
                              where i.Email == Email
                              && i.Cedula == Cedula
                              select i).FirstOrDefault();

            var Coordinador = (from i in entity.Coordinacion
                               where i.Correo == Email
                               && i.Cedula == Cedula
                               select i).FirstOrDefault();

            if (Instructor != null)
            {
                var Usuario = (from i in entity.Usuario
                               where i.IdUsuario == Instructor.IdUsuario
                               select i).FirstOrDefault();

                oPersonaDTO.IdUsuario = Instructor.IdUsuario;
                oPersonaDTO.Nombre = Instructor.Nombre;
                oPersonaDTO.Apellido = Instructor.Apellido;
                oPersonaDTO.Cedula = Instructor.Cedula;
                Contraseña = SecurityEncode.SecurityEncode.Desencriptar(Usuario.Password);
            }
            if (Coordinador != null)
            {
                var Usuario = (from i in entity.Usuario
                               where i.IdUsuario == Coordinador.IdUsuario
                               select i).FirstOrDefault();

                oPersonaDTO.IdUsuario = Coordinador.IdUsuario;
                oPersonaDTO.Nombre = Coordinador.Nombre;
                oPersonaDTO.Apellido = Coordinador.Apellido;
                oPersonaDTO.Cedula = Coordinador.Cedula;
                Contraseña = SecurityEncode.SecurityEncode.Desencriptar(Usuario.Password);
            }
            if (Instructor != null || Coordinador != null)
            {
                var Asunto = "Recupetación de Contraseña";
                var Plantilla = "Sr(a) " + oPersonaDTO.Nombre + " " + oPersonaDTO.Apellido + ".<br/> La Contraseña de su cuenta de usuario es " + Contraseña;
                SendMail.SendMailMessage1(Asunto, Plantilla, Email);
                return Email;
            }
            else
            {
                return null;
            }

        }

        public void CambiarContrasena(string Password, string newPassword, int usuario)
        {
            var Encriptar = SecurityEncode.SecurityEncode.Encriptar(Password);
            PersonaDTO oPersonaDTO = new PersonaDTO();
            var Datos = (from i in entity.Usuario
                         where i.Password == Encriptar && i.IdUsuario == usuario
                         select i).FirstOrDefault();

            if (Datos != null)
            {
                Datos.Password = SecurityEncode.SecurityEncode.Encriptar(newPassword);
                entity.SaveChanges();
            }
        }
    }
}
