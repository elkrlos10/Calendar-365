using ClosedXML.Excel;
using Datos.DTO;
using Datos.Modelo;
using LogicaNegocio.Enumeraciones;
using LogicaNegocio.Mail;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace LogicaNegocio.LogicaNegocio
{
    public class InstructorBl
    {


        public bool GuardarInstructor(Instructor oInstructor)
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Instructor
                         where i.Cedula == oInstructor.Cedula
                         select i).FirstOrDefault();
            if (Datos == null)
            {
                Usuario oUsuario = new Usuario();
                oUsuario.NombreUsuario = oInstructor.Cedula;
                var Encriptar = SecurityEncode.SecurityEncode.Encriptar(oInstructor.Cedula);
                oUsuario.Password = Encriptar;
                oUsuario.TipoUsuario = (int)TipoUsuario.Instructor;
                entity.Usuario.Add(oUsuario);
                entity.SaveChanges();
                oInstructor.Estado = true;
                oInstructor.IdUsuario = oUsuario.IdUsuario;
                oInstructor.EnvioCorreo = false;
                entity.Instructor.Add(oInstructor);
                entity.SaveChanges();
                //var Asunto = "Nuevo Usuario";
                //var Plantilla = "Usuario : " + oUsuario.NombreUsuario + "<br/> Contraseña: " + oUsuario.NombreUsuario;
                //SendMail.SendMailMessage(Asunto, Plantilla, oInstructor.Email);
                return true;
            }
            else
            {
                return false;
            }

        }

        public void EnviarCorreoInstructor()
        {
            Model1 entity = new Model1();
            var Instructor = (from i in entity.Instructor
                              where i.EnvioCorreo == false
                              select i).FirstOrDefault();


            var Usuario = (from i in entity.Usuario
                           where i.IdUsuario == Instructor.IdUsuario
                           select i).FirstOrDefault();
            Instructor.EnvioCorreo = true;
            entity.SaveChanges();
            var Desencriptar = SecurityEncode.SecurityEncode.Desencriptar(Usuario.Password);


            var Asunto = "Nuevo Usuario";
            var Plantilla = "Usuario : " + Usuario.NombreUsuario + "<br/> Contraseña: " + Desencriptar;
            SendMail.SendMailMessage(Asunto, Plantilla, Instructor.Email);
        }

        public void EnviarCorreoLista(List<Instructor> oInstrutor)
        {
            foreach (var item in oInstrutor)
            {
                var Asunto = "Nuevo Usuario";
                var Plantilla = "Usuario : " + item.Cedula + "<br/> Contraseña: " + item.Cedula;
                SendMail.SendMailMessage1(Asunto, Plantilla, item.Email);
            }
        }

        public List<InstructorDTO> ConsultarInstructores()
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Instructor
                         where i.Estado == true
                         orderby i.Nombre
                         select i).ToList();

            List<InstructorDTO> ListaInstructor = new List<InstructorDTO>();
            foreach (var item in Datos)
            {
                InstructorDTO oInstructorDTO = new InstructorDTO();
                oInstructorDTO.IdInstructor = item.IdInstructor;
                oInstructorDTO.Nombre = item.Nombre;
                oInstructorDTO.Apellido = item.Apellido;
                oInstructorDTO.Cedula = item.Cedula;
                oInstructorDTO.Telefono = item.Telefono;
                oInstructorDTO.Email = item.Email;
                oInstructorDTO.Estado = item.Estado;
                oInstructorDTO.IdUsuario = item.IdUsuario;
                oInstructorDTO.TipoContrato = item.TipoContrato;
                oInstructorDTO.TipoInstructor = item.TipoInstructor;
                if (item.TipoInstructor == 1)
                {
                    oInstructorDTO.NombreTipoInstructor = "TÉCNICO";
                }
                else
                {
                    oInstructorDTO.NombreTipoInstructor = "TRANSVERSAL";
                }
                if (oInstructorDTO.TipoContrato == 1)
                {
                    oInstructorDTO.NombreTipoContrato = "CONTRATISTA";
                }
                else
                {
                    oInstructorDTO.NombreTipoContrato = "PLANTA";
                }
                ListaInstructor.Add(oInstructorDTO);
            }
            return ListaInstructor;
        }

        public List<InstructorDTO> ConsultarInstructoresxArea(int Idcoordinador)
        {
            Model1 entity = new Model1();

            var coordinador = (from i in entity.Coordinacion
                               where i.IdCoordinacion == Idcoordinador
                               select i).FirstOrDefault();

            var Datos = (from i in entity.Instructor
                         where i.Estado == true && i.IdArea == coordinador.IdArea
                         orderby i.Nombre
                         select i).ToList();

            List<InstructorDTO> ListaInstructor = new List<InstructorDTO>();
            foreach (var item in Datos)
            {
                InstructorDTO oInstructorDTO = new InstructorDTO();
                oInstructorDTO.IdInstructor = item.IdInstructor;
                oInstructorDTO.Nombre = item.Nombre;
                oInstructorDTO.Apellido = item.Apellido;
                oInstructorDTO.Cedula = item.Cedula;
                oInstructorDTO.Email = item.Email;
                oInstructorDTO.Telefono = item.Telefono;
                oInstructorDTO.Estado = item.Estado;
                oInstructorDTO.IdUsuario = item.IdUsuario;
                oInstructorDTO.TipoContrato = item.TipoContrato;
                oInstructorDTO.TipoInstructor = item.TipoInstructor;
                if (item.TipoInstructor == 1)
                {
                    oInstructorDTO.NombreTipoInstructor = "TÉCNICO";
                }
                else
                {
                    oInstructorDTO.NombreTipoInstructor = "TRANSVERSAL";
                }
                if (oInstructorDTO.TipoContrato == 1)
                {
                    oInstructorDTO.NombreTipoContrato = "CONTRATISTA";
                }
                else
                {
                    oInstructorDTO.NombreTipoContrato = "PLANTA";
                }
                ListaInstructor.Add(oInstructorDTO);
            }
            return ListaInstructor;
        }

        public List<InstructorDTO> ConsultarInhabilitados()
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Instructor
                         where i.Estado == false
                         orderby i.Nombre
                         select i).ToList();

            List<InstructorDTO> ListaInstructor = new List<InstructorDTO>();
            foreach (var item in Datos)
            {
                InstructorDTO oInstructorDTO = new InstructorDTO();
                oInstructorDTO.IdInstructor = item.IdInstructor;
                oInstructorDTO.Nombre = item.Nombre;
                oInstructorDTO.Apellido = item.Apellido;
                oInstructorDTO.Cedula = item.Cedula;
                oInstructorDTO.Email = item.Email;
                oInstructorDTO.Telefono = item.Telefono;
                oInstructorDTO.Estado = item.Estado;
                oInstructorDTO.IdUsuario = item.IdUsuario;
                oInstructorDTO.TipoContrato = item.TipoContrato;
                oInstructorDTO.TipoInstructor = item.TipoInstructor;
                if (item.TipoInstructor == 1)
                {
                    oInstructorDTO.NombreTipoInstructor = "TÉCNICO";
                }
                else
                {
                    oInstructorDTO.NombreTipoInstructor = "TRANSVERSAL";
                }
                if (oInstructorDTO.TipoContrato == 1)
                {
                    oInstructorDTO.NombreTipoContrato = "CONTRATISTA";
                }
                else
                {
                    oInstructorDTO.NombreTipoContrato = "PLANTA";
                }
                ListaInstructor.Add(oInstructorDTO);
            }
            return ListaInstructor;
        }

        public List<InstructorDTO> ConsultarInhabilitadosxArea(int IdCoordinacion)
        {


            Model1 entity = new Model1();
            var coordinacion = (from i in entity.Coordinacion
                                where i.IdCoordinacion == IdCoordinacion
                                select i).FirstOrDefault();

            var Datos = (from i in entity.Instructor
                         where i.Estado == false && i.IdArea == coordinacion.IdArea
                         orderby i.Nombre
                         select i).ToList();

            List<InstructorDTO> ListaInstructor = new List<InstructorDTO>();
            foreach (var item in Datos)
            {
                InstructorDTO oInstructorDTO = new InstructorDTO();
                oInstructorDTO.IdInstructor = item.IdInstructor;
                oInstructorDTO.Nombre = item.Nombre;
                oInstructorDTO.Apellido = item.Apellido;
                oInstructorDTO.Cedula = item.Cedula;
                oInstructorDTO.Email = item.Email;
                oInstructorDTO.Estado = item.Estado;
                oInstructorDTO.IdUsuario = item.IdUsuario;
                oInstructorDTO.Telefono = item.Telefono;
                oInstructorDTO.TipoContrato = item.TipoContrato;
                oInstructorDTO.TipoInstructor = item.TipoInstructor;
                if (item.TipoInstructor == 1)
                {
                    oInstructorDTO.NombreTipoInstructor = "TÉCNICO";
                }
                else
                {
                    oInstructorDTO.NombreTipoInstructor = "TRANSVERSAL";
                }
                if (oInstructorDTO.TipoContrato == 1)
                {
                    oInstructorDTO.NombreTipoContrato = "CONTRATISTA";
                }
                else
                {
                    oInstructorDTO.NombreTipoContrato = "PLANTA";
                }
                ListaInstructor.Add(oInstructorDTO);
            }
            return ListaInstructor;
        }

        public void CambiarEstado(Instructor oInstructor)
        {
            Model1 entity = new Model1();
            var Item = (from i in entity.Instructor
                        where i.IdInstructor == oInstructor.IdInstructor
                        select i).First();
            if (Item.Estado == true)
            {
                Item.Estado = false;
                entity.SaveChanges();
            }
            else
            {
                Item.Estado = true;
                entity.SaveChanges();
            }
        }

        public String ConsultarInstructorCedula(string cedula)
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Instructor
                         where i.Cedula == cedula
                         select i.Cedula).FirstOrDefault();
            return Datos;
        }

        public Instructor ConsultarInstructorCedula1(string Cedula)
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Instructor
                         where i.Cedula == Cedula
                         select i).FirstOrDefault();
            return Datos;

        }

        public void ActualizarRegistro(Instructor oInstructor)
        {
            Model1 entity = new Model1();

            var Item = (from i in entity.Instructor
                        where i.IdInstructor == oInstructor.IdInstructor
                        select i).First();

            Item.Nombre = oInstructor.Nombre;
            Item.Apellido = oInstructor.Apellido;
            Item.Cedula = oInstructor.Cedula;
            Item.Email = oInstructor.Email;
            Item.Estado = oInstructor.Estado;
            Item.TipoContrato = oInstructor.TipoContrato;
            Item.Telefono = oInstructor.Telefono;
            Item.TipoInstructor = oInstructor.TipoInstructor;
            Item.IdArea = oInstructor.IdArea;
            Item.Estado = true;
            entity.SaveChanges();

        }

        public List<Ficha_AmbienteDTO> ReporteDeProgramacionInstructores(int IdInstructor, DateTime FechaInicio, DateTime FechaFin)
        {
            Model1 entity = new Model1();

            DateTime FechaTemporal = DateTime.Today;


            var Programacion = (from i in entity.Ficha_Ambiente
                         where i.IdInstructor == IdInstructor &&
                         ((i.FechaInicio >= FechaInicio && i.FechaInicio <= FechaFin) ||
                          (i.FechaFin <= FechaFin && i.FechaFin >= FechaInicio) ||
                          (i.FechaInicio <= FechaInicio && i.FechaFin >= FechaFin))
                         orderby i.Jornada ascending
                         select i).ToList();

            var instructor = (from i in entity.Instructor
                              where i.IdInstructor == IdInstructor
                              select i).FirstOrDefault();



            List<Ficha_AmbienteDTO> Datos = new List<Ficha_AmbienteDTO>();

            foreach (var item in Programacion)
            {

                var DatosFicha = (from i in entity.Ficha
                                  where i.IdFicha == item.IdFicha
                                  select i).FirstOrDefault();
                var DatosResultado = (from i in entity.Resultado_Aprendizaje
                                      where i.IdResultado == item.IdResultado
                                      select i).FirstOrDefault();
                var DatosAmbiente = (from i in entity.Ambiente
                                     where i.IdAmbiente == item.IdAmbiente
                                     select i).FirstOrDefault();


                Ficha_AmbienteDTO Ficha = new Ficha_AmbienteDTO();
                Ficha.Id = item.Id;
                Ficha.Ambiente = DatosAmbiente.Numero;
                Ficha.Ficha = DatosFicha.Ficha1;
                Ficha.Resultado = DatosResultado.Resultado;
                Ficha.NombreInstructor = instructor.Nombre +" "+ instructor.Apellido;
                Ficha.FechaInicio = item.FechaInicio;
                Ficha.FechaFin = item.FechaFin;
                Ficha.HoraInicio = item.HoraInicio;
                Ficha.HoraFin = item.HoraFin;
                Ficha.Lunes = item.Lunes;
                Ficha.Martes = item.Martes;
                Ficha.Miercoles = item.Miercoles;
                Ficha.Jueves = item.Jueves;
                Ficha.Viernes = item.Viernes;
                Ficha.NumeroJornada = item.Jornada;

                Datos.Add(Ficha);
            }

            return Datos;
        }

        public List<InstructorDTO> ReporteHorasInstructores(int IdInstructor, DateTime FechaInicio, DateTime FechaFin)
        {
            Model1 entity = new Model1();


            var Instructor = entity.Database.SqlQuery<InstructorDTO>("sp_HorasInstructor @fecha_ini, @fecha_Fin, @idinstructor",
                                                                            new SqlParameter("fecha_ini", FechaInicio),
                                                                            new SqlParameter("fecha_Fin", FechaFin),
                                                                            new SqlParameter("idinstructor", IdInstructor)).ToList();


            return Instructor;
        }

        public List<Ficha_AmbienteDTO> EnviarProgramacionInstructor(DateTime FechaInicio, DateTime FechaFin, int IdInstructor)
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Ficha_Ambiente
                         where i.IdInstructor == IdInstructor && (i.FechaInicio >= FechaInicio
                                                              && i.FechaInicio <= FechaFin
                                                              && i.FechaFin >= FechaInicio
                                                              && i.FechaFin <= FechaFin)
                         select i).ToList();

            List<Ficha_AmbienteDTO> Programacion = new List<Ficha_AmbienteDTO>();

            foreach (var item in Datos)
            {

                Ficha_AmbienteDTO Ficha = new Ficha_AmbienteDTO();
                var DatosFicha = (from i in entity.Ficha
                                  where i.IdFicha == item.IdFicha
                                  select i).FirstOrDefault();
                var DatosResultado = (from i in entity.Resultado_Aprendizaje
                                      where i.IdResultado == item.IdResultado
                                      select i).FirstOrDefault();
                var DatosAmbiente = (from i in entity.Ambiente
                                     where i.IdAmbiente == item.IdAmbiente
                                     select i).FirstOrDefault();

                Ficha.Id = item.Id;
                Ficha.Ambiente = DatosAmbiente.Numero;
                Ficha.Ficha = DatosFicha.Ficha1;
                Ficha.Resultado = DatosResultado.Resultado;
                Ficha.IdInstructor = item.IdInstructor;
                Ficha.FechaInicio = item.FechaInicio;
                Ficha.FechaFin = item.FechaFin;
                Ficha.HoraInicio = item.HoraInicio;
                Ficha.HoraFin = item.HoraFin;
                Ficha.Color = item.Color;
                Programacion.Add(Ficha);
            }
            return Programacion;
        }

        public List<InstructorDTO> RepoorteHorasInstructores(DateTime FechaInicio, DateTime FechaFin, int Idcoordinador)
        {
            var Instructor = new List<InstructorDTO>();
            Model1 entity = new Model1();
            if (Idcoordinador == 0)
            {
                 Instructor = entity.Database.SqlQuery<InstructorDTO>("HorasInstructorTodos @fecha_ini, @fecha_Fin",
                                                                            new SqlParameter("fecha_ini", FechaInicio),
                                                                            new SqlParameter("fecha_Fin", FechaFin)).ToList();
            }
            else
            {
                var coordinador = (from i in entity.Coordinacion
                                   where i.IdCoordinacion == Idcoordinador
                                   select i).FirstOrDefault();

                 Instructor = entity.Database.SqlQuery<InstructorDTO>("HorasInstructorCoordinacion @fecha_ini, @fecha_Fin, @IdArea",
                                                                            new SqlParameter("fecha_ini", FechaInicio),
                                                                            new SqlParameter("fecha_Fin", FechaFin),
                                                                            new SqlParameter("IdArea", coordinador.IdArea)).ToList();

                foreach (var item in Instructor.Select((value, i) => new { i, value }))
                {
                    Instructor[item.i].NombreArea = (from i in entity.Area where i.IdArea== item.value.IdArea select i.Nombre).FirstOrDefault();
                }

            }

            foreach (var item in Instructor.Select((value, i) => new { i, value }))
            {
                Instructor[item.i].NombreArea = (from i in entity.Area where i.IdArea == item.value.IdArea select i.Nombre).FirstOrDefault();
                Instructor[item.i].NombreTipoInstructor = (item.value.NombreTipoInstructor = (item.value.TipoInstructor == 1) ? "TÉCNICO" : "TRANSVERSAL");
                Instructor[item.i].NombreTipoContrato = (item.value.NombreTipoContrato = (item.value.TipoContrato == 1) ? "CONTRATISTA" : "PLANTA");
                Instructor[item.i].Nombre = item.value.Nombre + " " + item.value.Apellido;
            }


            return Instructor;
        }
    }
}
