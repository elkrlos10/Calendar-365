using Datos.DTO;
using Datos.Modelo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaNegocio.Mail;

namespace LogicaNegocio.LogicaNegocio
{
    public class ProgramacionBl
    {
        public string GuardarProgramacion(List<Ficha_Ambiente> ListaProgramacion)
        {
            int contador = 0;
            double minutosxProgramacion = 0;
            DateTime horaInicio;
            DateTime horaFin = DateTime.Parse("00:00");

            foreach (var oFicha_Ambiente in ListaProgramacion)
            {
                if (contador == 0)
                {
                    TimeSpan TotalHoras = oFicha_Ambiente.HoraFin - oFicha_Ambiente.HoraInicio;
                    minutosxProgramacion = TotalHoras.TotalMinutes / (ListaProgramacion.Count);
                    horaInicio = Convert.ToDateTime(oFicha_Ambiente.HoraInicio.ToString());

                    horaFin = horaInicio.AddMinutes(minutosxProgramacion);
                    contador++;
                }
                else
                {
                    horaInicio = horaFin;
                    horaFin = horaInicio.AddMinutes(minutosxProgramacion);
                }


                var Hora_i = (horaInicio.ToString().Split(' ')[1]);
                var Hora_f = (horaFin.ToString().Split(' ')[1]);
                oFicha_Ambiente.HoraInicio = TimeSpan.Parse(Hora_i);
                oFicha_Ambiente.HoraFin = TimeSpan.Parse(Hora_f);


                Model1 entity = new Model1();
                Random r = new Random();
                string mensaje = "";

                if (oFicha_Ambiente.Lunes == null)
                {
                    oFicha_Ambiente.Lunes = false;
                }
                if (oFicha_Ambiente.Martes == null)
                {
                    oFicha_Ambiente.Martes = false;
                }
                if (oFicha_Ambiente.Miercoles == null)
                {
                    oFicha_Ambiente.Miercoles = false;
                }
                if (oFicha_Ambiente.Jueves == null)
                {
                    oFicha_Ambiente.Jueves = false;
                }
                if (oFicha_Ambiente.Viernes == null)
                {
                    oFicha_Ambiente.Viernes = false;
                }

                var ambiente = (from i in entity.Ambiente
                                where i.IdAmbiente == oFicha_Ambiente.IdAmbiente
                                select i).FirstOrDefault();

                var Programa = (from f in entity.Programa
                                join i in entity.Ficha on f.IdPrograma equals i.IdPrograma
                                where i.IdFicha == oFicha_Ambiente.IdFicha
                                select f).FirstOrDefault();

                var instructor = (from i in entity.Instructor
                                  where i.IdInstructor == oFicha_Ambiente.IdInstructor
                                  select i).FirstOrDefault();

                var fichaasociada = (from i in entity.Ficha
                                     where i.IdFicha == oFicha_Ambiente.IdFicha
                                     select i).FirstOrDefault();

                var resultado = (from i in entity.Resultado_Aprendizaje
                                 where i.IdResultado == oFicha_Ambiente.IdResultado
                                 select i).FirstOrDefault();

                var sede = (from i in entity.Sede
                            join a in entity.Ambiente on i.IdSede equals a.IdSede
                            where a.IdAmbiente == oFicha_Ambiente.IdAmbiente
                            select i).FirstOrDefault();


                var coordinadocion = (from i in entity.Coordinacion
                                      where i.IdArea == instructor.IdArea
                                      select i).FirstOrDefault();

                var validacion = new Ficha_Ambiente();

                if (oFicha_Ambiente.Transversal == true)
                {
                    validacion = entity.Database.SqlQuery<Ficha_Ambiente>("sp_ValidarProgramacionTransversal @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @IdAmbiente, @IdInstructor, @IdFicha, @lunes, @martes, @miercoles, @jueves, @viernes,@Opc",
                                                                                      new SqlParameter("fecha_ini", oFicha_Ambiente.FechaInicio),
                                                                                      new SqlParameter("fecha_Fin", oFicha_Ambiente.FechaFin),
                                                                                      new SqlParameter("HoraInicio", oFicha_Ambiente.HoraInicio),
                                                                                      new SqlParameter("HoraFin", oFicha_Ambiente.HoraFin),
                                                                                      new SqlParameter("IdAmbiente", oFicha_Ambiente.IdAmbiente),
                                                                                      new SqlParameter("IdInstructor", oFicha_Ambiente.IdInstructor),
                                                                                      new SqlParameter("IdFicha", oFicha_Ambiente.IdFicha),
                                                                                      new SqlParameter("lunes", oFicha_Ambiente.Lunes),
                                                                                      new SqlParameter("martes", oFicha_Ambiente.Martes),
                                                                                      new SqlParameter("miercoles", oFicha_Ambiente.Miercoles),
                                                                                      new SqlParameter("jueves", oFicha_Ambiente.Jueves),
                                                                                      new SqlParameter("viernes", oFicha_Ambiente.Viernes),
                                                                                      new SqlParameter("Opc", oFicha_Ambiente.Jornada)).FirstOrDefault();
                    if (validacion != null)
                    {

                        if (oFicha_Ambiente.IdFicha == validacion.IdFicha)
                        {

                            return mensaje = "La ficha número " + fichaasociada.Ficha1 + " ya tiene programación transversal asociada en estos horarios: ";
                        }
                    }


                }
                else
                {
                    if (sede.TipoSede == 1)
                    {


                        validacion = entity.Database.SqlQuery<Ficha_Ambiente>("sp_ValidarProgramacion @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @IdAmbiente, @IdInstructor, @IdFicha, @jornada, @lunes, @martes, @miercoles, @jueves, @viernes",
                                                                                          new SqlParameter("fecha_ini", oFicha_Ambiente.FechaInicio),
                                                                                          new SqlParameter("fecha_Fin", oFicha_Ambiente.FechaFin),
                                                                                          new SqlParameter("HoraInicio", oFicha_Ambiente.HoraInicio),
                                                                                          new SqlParameter("HoraFin", oFicha_Ambiente.HoraFin),
                                                                                          new SqlParameter("IdAmbiente", oFicha_Ambiente.IdAmbiente),
                                                                                          new SqlParameter("IdInstructor", oFicha_Ambiente.IdInstructor),
                                                                                          new SqlParameter("IdFicha", oFicha_Ambiente.IdFicha),
                                                                                          new SqlParameter("jornada", oFicha_Ambiente.Jornada),
                                                                                          new SqlParameter("lunes", oFicha_Ambiente.Lunes),
                                                                                          new SqlParameter("martes", oFicha_Ambiente.Martes),
                                                                                          new SqlParameter("miercoles", oFicha_Ambiente.Miercoles),
                                                                                          new SqlParameter("jueves", oFicha_Ambiente.Jueves),
                                                                                          new SqlParameter("viernes", oFicha_Ambiente.Viernes)).FirstOrDefault();
                    }
                }

                if (oFicha_Ambiente.Lunes == false)
                {
                    oFicha_Ambiente.Lunes = null;
                }
                if (oFicha_Ambiente.Martes == false)
                {
                    oFicha_Ambiente.Martes = null;
                }
                if (oFicha_Ambiente.Miercoles == false)
                {
                    oFicha_Ambiente.Miercoles = null;
                }
                if (oFicha_Ambiente.Jueves == false)
                {
                    oFicha_Ambiente.Jueves = null;
                }
                if (oFicha_Ambiente.Viernes == false)
                {
                    oFicha_Ambiente.Viernes = null;
                }


                if (validacion != null)
                {
                    if (oFicha_Ambiente.IdAmbiente == validacion.IdAmbiente)
                    {
                        return mensaje = "El ambiente " + ambiente.Numero + " acaba de ser programado. \n Por favor consulte de nuevo";
                    }

                    if (oFicha_Ambiente.IdInstructor == validacion.IdInstructor)
                    {
                        return mensaje = "El Instructor " + instructor.Nombre + " " + instructor.Apellido + " acaba de ser programado. \n Por favor consulte de nuevo";
                    }
                    if (coordinadocion.IdCoordinacion != 11)
                    {
                        if (oFicha_Ambiente.IdFicha == validacion.IdFicha)
                        {
                            return mensaje = "La ficha número " + fichaasociada.Ficha1 + " acaba de ser programada. \n Por favor consulte de nuevo";
                        }
                    }

                }



                if (sede.TipoSede == 1)
                {
                    //if (ambiente.IdArea == Programa.IdArea)
                    //{
                    oFicha_Ambiente.Estado = true;
                    oFicha_Ambiente.Color = "#" + r.Next(100000, 999999).ToString();
                    entity.Ficha_Ambiente.Add(oFicha_Ambiente);
                    entity.SaveChanges();
                    //var Asunto = "Programación asociada";
                    //var Cuerpo = "Instrutor " + instructor.Nombre + " " + instructor.Apellido + ", se le ha asociado la siguiente programacion"
                    //              + "<br/>Ficha:" + fichaasociada.Ficha1 + "<br>Ambiente:" + ambiente.Numero +
                    //              "<br>Resultado de aprendizaje: " + resultado.Codigo + " - " + resultado.Resultado;
                    //SendMail.SendMailMessage(Asunto, Cuerpo, instructor.Email);

                    //}
                    //else
                    //{
                    //    oFicha_Ambiente.Estado = false;
                    //    var coordinadorSolicitado = (from i in entity.Coordinacion
                    //                                 where i.IdArea == ambiente.IdArea
                    //                                 select i).FirstOrDefault();

                    //    oFicha_Ambiente.Color = "#" + r.Next(100000, 999999).ToString();
                    //    entity.Ficha_Ambiente.Add(oFicha_Ambiente);
                    //    entity.SaveChanges();

                    //    var coordinador = (from i in entity.Coordinacion
                    //                       where i.IdArea == Programa.IdArea
                    //                       select i).FirstOrDefault();


                    //    var Asunto = "Solicitud de ambiente";
                    //    var Cuerpo = "La coordinacion de " + coordinador.Nombre_Coordinacion + " agotó los ambientes asignados a su área y requiere un préstamo de uno de sus ambientes. Por favor abra el link a continuación y apruebe o rechace la solicitud";
                    //    var Link = "<br/><a href = 'http://10.3.240.88:8083/Principal.html#/Solicitud?GUID=" + oFicha_Ambiente.Id + "'>Ver </a>";
                    //    SendMail.SendMailMessage(Asunto, Cuerpo + Link, coordinadorSolicitado.Correo);



                    //    Solicitud oSolicitud = new Solicitud();
                    //    oSolicitud.IdFicha_Ambiente = oFicha_Ambiente.Id;
                    //    oSolicitud.IdCoordinacion = coordinador.IdCoordinacion;

                    //    entity.Solicitud.Add(oSolicitud);
                    //    entity.SaveChanges();


                    //}

                }
                else
                {
                    oFicha_Ambiente.Estado = true;
                    oFicha_Ambiente.Color = "#" + r.Next(100000, 999999).ToString();
                    entity.Ficha_Ambiente.Add(oFicha_Ambiente);
                    entity.SaveChanges();

                }

            }
            return null;
        }

        public string GuardarProgramacionTrasversal(Ficha_Ambiente oFicha_Ambiente)
        {

            Model1 entity = new Model1();
            Random r = new Random();
            string mensaje = "";

            if (oFicha_Ambiente.Lunes == null)
            {
                oFicha_Ambiente.Lunes = false;
            }
            if (oFicha_Ambiente.Martes == null)
            {
                oFicha_Ambiente.Martes = false;
            }
            if (oFicha_Ambiente.Miercoles == null)
            {
                oFicha_Ambiente.Miercoles = false;
            }
            if (oFicha_Ambiente.Jueves == null)
            {
                oFicha_Ambiente.Jueves = false;
            }
            if (oFicha_Ambiente.Viernes == null)
            {
                oFicha_Ambiente.Viernes = false;
            }

            var ambiente = (from i in entity.Ambiente
                            where i.IdAmbiente == oFicha_Ambiente.IdAmbiente
                            select i).FirstOrDefault();

            var Programa = (from f in entity.Programa
                            join i in entity.Ficha on f.IdPrograma equals i.IdPrograma
                            where i.IdFicha == oFicha_Ambiente.IdFicha
                            select f).FirstOrDefault();

            var instructor = (from i in entity.Instructor
                              where i.IdInstructor == oFicha_Ambiente.IdInstructor
                              select i).FirstOrDefault();

            var fichaasociada = (from i in entity.Ficha
                                 where i.IdFicha == oFicha_Ambiente.IdFicha
                                 select i).FirstOrDefault();

            var resultado = (from i in entity.Resultado_Aprendizaje
                             where i.IdResultado == oFicha_Ambiente.IdResultado
                             select i).FirstOrDefault();

            var sede = (from i in entity.Sede
                        join a in entity.Ambiente on i.IdSede equals a.IdSede
                        where a.IdAmbiente == oFicha_Ambiente.IdAmbiente
                        select i).FirstOrDefault();

            var validacion = new Ficha_Ambiente();

            if (oFicha_Ambiente.Transversal == true)
            {
                validacion = entity.Database.SqlQuery<Ficha_Ambiente>("sp_ValidarProgramacionTransversal @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @IdAmbiente, @IdInstructor, @IdFicha, @lunes, @martes, @miercoles, @jueves, @viernes,@Opc",
                                                                                  new SqlParameter("fecha_ini", oFicha_Ambiente.FechaInicio),
                                                                                  new SqlParameter("fecha_Fin", oFicha_Ambiente.FechaFin),
                                                                                  new SqlParameter("HoraInicio", oFicha_Ambiente.HoraInicio),
                                                                                  new SqlParameter("HoraFin", oFicha_Ambiente.HoraFin),
                                                                                  new SqlParameter("IdAmbiente", oFicha_Ambiente.IdAmbiente),
                                                                                  new SqlParameter("IdInstructor", oFicha_Ambiente.IdInstructor),
                                                                                  new SqlParameter("IdFicha", oFicha_Ambiente.IdFicha),
                                                                                  new SqlParameter("lunes", oFicha_Ambiente.Lunes),
                                                                                  new SqlParameter("martes", oFicha_Ambiente.Martes),
                                                                                  new SqlParameter("miercoles", oFicha_Ambiente.Miercoles),
                                                                                  new SqlParameter("jueves", oFicha_Ambiente.Jueves),
                                                                                  new SqlParameter("viernes", oFicha_Ambiente.Viernes),
                                                                                  new SqlParameter("Opc", oFicha_Ambiente.Jornada)).FirstOrDefault();
                if (validacion != null)
                {

                    if (oFicha_Ambiente.IdFicha == validacion.IdFicha)
                    {
                        return mensaje = "La ficha número " + fichaasociada.Ficha1 + " ya tiene una programación transversal asociada en ese horario. \n Por favor consulte de nuevo";
                    }
                }


            }
            else
            {
                if (sede.TipoSede == 1)
                {


                    validacion = entity.Database.SqlQuery<Ficha_Ambiente>("sp_ValidarProgramacion @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @IdAmbiente, @IdInstructor, @IdFicha, @jornada, @lunes, @martes, @miercoles, @jueves, @viernes",
                                                                                      new SqlParameter("fecha_ini", oFicha_Ambiente.FechaInicio),
                                                                                      new SqlParameter("fecha_Fin", oFicha_Ambiente.FechaFin),
                                                                                      new SqlParameter("HoraInicio", oFicha_Ambiente.HoraInicio),
                                                                                      new SqlParameter("HoraFin", oFicha_Ambiente.HoraFin),
                                                                                      new SqlParameter("IdAmbiente", oFicha_Ambiente.IdAmbiente),
                                                                                      new SqlParameter("IdInstructor", oFicha_Ambiente.IdInstructor),
                                                                                      new SqlParameter("IdFicha", oFicha_Ambiente.IdFicha),
                                                                                      new SqlParameter("jornada", oFicha_Ambiente.Jornada),
                                                                                      new SqlParameter("lunes", oFicha_Ambiente.Lunes),
                                                                                      new SqlParameter("martes", oFicha_Ambiente.Martes),
                                                                                      new SqlParameter("miercoles", oFicha_Ambiente.Miercoles),
                                                                                      new SqlParameter("jueves", oFicha_Ambiente.Jueves),
                                                                                      new SqlParameter("viernes", oFicha_Ambiente.Viernes)).FirstOrDefault();
                }
            }

            if (oFicha_Ambiente.Lunes == false)
            {
                oFicha_Ambiente.Lunes = null;
            }
            if (oFicha_Ambiente.Martes == false)
            {
                oFicha_Ambiente.Martes = null;
            }
            if (oFicha_Ambiente.Miercoles == false)
            {
                oFicha_Ambiente.Miercoles = null;
            }
            if (oFicha_Ambiente.Jueves == false)
            {
                oFicha_Ambiente.Jueves = null;
            }
            if (oFicha_Ambiente.Viernes == false)
            {
                oFicha_Ambiente.Viernes = null;
            }

            if (validacion != null)
            {
                if (oFicha_Ambiente.IdAmbiente == validacion.IdAmbiente)
                {
                    return mensaje = "El ambiente " + ambiente.Numero + " acaba de ser programado. \n Por favor consulte de nuevo";
                }

                if (oFicha_Ambiente.IdInstructor == validacion.IdInstructor)
                {
                    return mensaje = "El Instructor " + instructor.Nombre + " " + instructor.Apellido + " acaba de ser programado. \n Por favor consulte de nuevo";
                }
                if (oFicha_Ambiente.IdFicha == validacion.IdFicha)
                {
                    return mensaje = "La ficha número " + fichaasociada.Ficha1 + " acaba de ser programada. \n Por favor consulte de nuevo";
                }
            }

            if (oFicha_Ambiente.DiaSemana != null)
            {
                oFicha_Ambiente.Estado = true;
                oFicha_Ambiente.Color = "#" + r.Next(100000, 999999).ToString();
                entity.Ficha_Ambiente.Add(oFicha_Ambiente);
                entity.SaveChanges();
                return null;
            }

            if (sede.TipoSede == 1)
            {
                if (ambiente.IdArea == Programa.IdArea)
                {
                    oFicha_Ambiente.Estado = true;
                    oFicha_Ambiente.Color = "#" + r.Next(100000, 999999).ToString();
                    entity.Ficha_Ambiente.Add(oFicha_Ambiente);
                    entity.SaveChanges();
                    //var Asunto = "Programación asociada";
                    //var Cuerpo = "Instrutor " + instructor.Nombre + " " + instructor.Apellido + ", se le ha asociado la siguiente programacion"
                    //              + "<br/>Ficha:" + fichaasociada.Ficha1 + "<br>Ambiente:" + ambiente.Numero +
                    //              "<br>Resultado de aprendizaje: " + resultado.Codigo + " - " + resultado.Resultado;
                    //SendMail.SendMailMessage(Asunto, Cuerpo, instructor.Email);
                    return null;
                }
                else
                {
                    oFicha_Ambiente.Estado = false;
                    var coordinadorSolicitado = (from i in entity.Coordinacion
                                                 where i.IdArea == ambiente.IdArea
                                                 select i).FirstOrDefault();

                    oFicha_Ambiente.Color = "#" + r.Next(100000, 999999).ToString();
                    entity.Ficha_Ambiente.Add(oFicha_Ambiente);
                    entity.SaveChanges();

                    var coordinador = (from i in entity.Coordinacion
                                       where i.IdArea == Programa.IdArea
                                       select i).FirstOrDefault();


                    var Asunto = "Solicitud de ambiente";
                    var Cuerpo = "La coordinacion de " + coordinador.Nombre_Coordinacion + "agotó los ambientes asignados a su área y requiere un préstamo de uno de sus ambientes. Por favor abra el link a continuación y apruebe o rechace la solicitud";
                    var Link = "<br/><a href = 'http://localhost:61996/Principal.html#/Solicitud?GUID=" + oFicha_Ambiente.Id + "'>Ver </a>";
                    SendMail.SendMailMessage(Asunto, Cuerpo + Link, coordinadorSolicitado.Correo);



                    Solicitud oSolicitud = new Solicitud();
                    oSolicitud.IdFicha_Ambiente = oFicha_Ambiente.Id;
                    oSolicitud.IdCoordinacion = coordinador.IdCoordinacion;

                    entity.Solicitud.Add(oSolicitud);
                    entity.SaveChanges();

                    return null;
                }

            }
            else
            {
                oFicha_Ambiente.Estado = true;
                oFicha_Ambiente.Color = "#" + r.Next(100000, 999999).ToString();
                entity.Ficha_Ambiente.Add(oFicha_Ambiente);
                entity.SaveChanges();
                return null;
            }

        }

        public Tuple<List<Ambiente>, List<Instructor>, List<Ficha>, bool, bool> Disponibilidad(bool Transversal, int IdCoordinacion, DateTime FechaInicio, DateTime FechaFin, string HoraInicio, string HoraFin, int IdSede, int IdPrograma, bool Lunes, bool Martes, bool Miercoles, bool Jueves, bool Viernes, bool Sabado, bool Domingo, bool colegio, bool Manana, bool Tarde, bool Noche)
        {
            Model1 entity = new Model1();
            InstructorBl oInstructorBl = new InstructorBl();
            AmbienteBl oAmbienteBl = new AmbienteBl();
            FichaBl oFichaBl = new FichaBl();
            List<Instructor> DisponibilidadInstructor = new List<Instructor>();
            List<Instructor> Instructor = new List<Instructor>();
            var DisponibilidadAmbiente = new List<Ambiente>();
            var DisponibilidadFicha = new List<Ficha>();
            bool AmbientesOtraArea = false;
            bool NoValidarFicha = false;


            var Coordinacion = (from i in entity.Coordinacion
                                where i.IdCoordinacion == IdCoordinacion
                                select i).FirstOrDefault();
            var IdArea = (from i in entity.Programa
                          where i.IdPrograma == i.IdPrograma
                          select i.IdArea).FirstOrDefault();


            var TipoSede = (from s in entity.Sede
                            where s.IdSede == IdSede && (s.TipoSede == 2 || s.TipoSede == 4)
                            select s).FirstOrDefault();

            //var empresa = 0;

            //if (SedeEmpresa == null)
            //{
            //    empresa = 1;
            //}


            if (Sabado == true || Domingo == true)
            {
                int jornada = 0;
                if (Sabado == true)
                {
                    jornada = 2;
                }
                if (Domingo == true)
                {
                    jornada = 3;
                }
                DisponibilidadAmbiente = entity.Database.SqlQuery<Ambiente>("sp_disponibilidadAmbienteSabado @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin,@jornada",
                                                                                                 new SqlParameter("fecha_ini", FechaInicio),
                                                                                                 new SqlParameter("fecha_Fin", FechaFin),
                                                                                                 new SqlParameter("HoraInicio", HoraInicio),
                                                                                                 new SqlParameter("HoraFin", HoraFin),
                                                                                                 new SqlParameter("jornada", jornada)).ToList();


                if (Transversal == true)
                {
                    //DisponibilidadInstructor = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructorTransversalSabado @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin",
                    //                                                                 new SqlParameter("fecha_ini", FechaInicio),
                    //                                                                 new SqlParameter("fecha_Fin", FechaFin),
                    //                                                                 new SqlParameter("HoraInicio", HoraInicio),
                    //                                                                 new SqlParameter("HoraFin", HoraFin)).ToList();
                    //Instructor = (from i in entity.Instructor
                    //              where i.Estado == true
                    //              && i.TipoInstructor == 2
                    //              select i).ToList();
                }
                else
                {
                    DisponibilidadInstructor = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructorSabado @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @idarea, @jornada",
                                                                                     new SqlParameter("fecha_ini", FechaInicio),
                                                                                     new SqlParameter("fecha_Fin", FechaFin),
                                                                                     new SqlParameter("HoraInicio", HoraInicio),
                                                                                     new SqlParameter("HoraFin", HoraFin),
                                                                                     new SqlParameter("idarea", Coordinacion.IdArea),
                                                                                     new SqlParameter("jornada", jornada)).ToList();
                    Instructor = (from i in entity.Instructor
                                  where i.Estado == true && i.IdArea == Coordinacion.IdArea
                                  && i.TipoInstructor == 1
                                  orderby i.Nombre
                                  select i).ToList();
                }

                if (Coordinacion.Nombre != "PROGRAMACIÓN EVENTOS")
                {
                    DisponibilidadFicha = entity.Database.SqlQuery<Ficha>("sp_disponibilidadFichaSabado @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @idarea, @jornada",
                                                                                     new SqlParameter("fecha_ini", FechaInicio),
                                                                                     new SqlParameter("fecha_Fin", FechaFin),
                                                                                     new SqlParameter("HoraInicio", HoraInicio),
                                                                                     new SqlParameter("HoraFin", HoraFin),
                                                                                     new SqlParameter("idarea", Coordinacion.IdArea),
                                                                                     new SqlParameter("jornada", jornada)).ToList();
                    NoValidarFicha = true;
                }

            }
            else
            {

                if (colegio == false && TipoSede == null)
                {
                    DisponibilidadAmbiente = entity.Database.SqlQuery<Ambiente>("sp_disponibilidadAmbiente @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @idarea, @idsede, @lunes, @martes, @miercoles, @jueves, @viernes",
                                                                                    new SqlParameter("fecha_ini", FechaInicio),
                                                                                    new SqlParameter("fecha_Fin", FechaFin),
                                                                                    new SqlParameter("HoraInicio", HoraInicio),
                                                                                    new SqlParameter("HoraFin", HoraFin),
                                                                                    new SqlParameter("idarea", Coordinacion.IdArea),
                                                                                    new SqlParameter("idsede", IdSede),
                                                                                    new SqlParameter("lunes", Lunes),
                                                                                    new SqlParameter("martes", Martes),
                                                                                    new SqlParameter("miercoles", Miercoles),
                                                                                    new SqlParameter("jueves", Jueves),
                                                                                    new SqlParameter("viernes", Viernes)).ToList();

                }



                if (Transversal == true)
                {

                    //DisponibilidadInstructor = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructorTransversal @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin",
                    //                                                              new SqlParameter("fecha_ini", FechaInicio),
                    //                                                              new SqlParameter("fecha_Fin", FechaFin),
                    //                                                              new SqlParameter("HoraInicio", HoraInicio),
                    //                                                              new SqlParameter("HoraFin", HoraFin)).ToList();
                    //Instructor = (from i in entity.Instructor
                    //              where i.Estado == true
                    //              && i.TipoInstructor == 2
                    //              select i).ToList();

                }
                else
                {
                    DisponibilidadInstructor = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructor @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin,@idarea, @lunes, @martes, @miercoles, @jueves, @viernes",
                                                                                     new SqlParameter("fecha_ini", FechaInicio),
                                                                                     new SqlParameter("fecha_Fin", FechaFin),
                                                                                     new SqlParameter("HoraInicio", HoraInicio),
                                                                                     new SqlParameter("HoraFin", HoraFin),
                                                                                     new SqlParameter("idarea", Coordinacion.IdArea),
                                                                                     new SqlParameter("lunes", Lunes),
                                                                                     new SqlParameter("martes", Martes),
                                                                                     new SqlParameter("miercoles", Miercoles),
                                                                                     new SqlParameter("jueves", Jueves),
                                                                                     new SqlParameter("viernes", Viernes)).ToList();

                    Instructor = (from i in entity.Instructor
                                  where i.Estado == true && i.IdArea == Coordinacion.IdArea
                                  && i.TipoInstructor == 1
                                  orderby i.Nombre
                                  select i).ToList();
                }

                if (Coordinacion.Nombre_Coordinacion != "PROGRAMACIÓN EVENTOS")
                {
                    DisponibilidadFicha = entity.Database.SqlQuery<Ficha>("sp_disponibilidadFicha @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @idarea,  @idprograma, @lunes, @martes, @miercoles, @jueves, @viernes",
                                                                                      new SqlParameter("fecha_ini", FechaInicio),
                                                                                      new SqlParameter("fecha_Fin", FechaFin),
                                                                                      new SqlParameter("HoraInicio", HoraInicio),
                                                                                      new SqlParameter("HoraFin", HoraFin),
                                                                                      new SqlParameter("idarea", Coordinacion.IdArea),
                                                                                      new SqlParameter("idprograma", IdPrograma),
                                                                                      new SqlParameter("lunes", Lunes),
                                                                                      new SqlParameter("martes", Martes),
                                                                                      new SqlParameter("miercoles", Miercoles),
                                                                                      new SqlParameter("jueves", Jueves),
                                                                                      new SqlParameter("viernes", Viernes)).ToList();

                    NoValidarFicha = true;
                }
            }


            var Ambiente = new List<Ambiente>();
            if (colegio == true)
            {
                Ambiente = oAmbienteBl.ConsultarAmbientesColegios();
            }
            else if (TipoSede != null)
            {
                if (TipoSede.TipoSede == 2)
                {
                    Ambiente = oAmbienteBl.ConsultarAmbienteEmpresa();
                }
                if (TipoSede.TipoSede == 4)
                {
                    Ambiente = oAmbienteBl.ConsultarAmbienteVirtual();
                }
            }
            else
            {
                //Ambiente = oAmbienteBl.ConsultarAmbientesxArea(Coordinacion.IdArea, IdSede);
                Ambiente = oAmbienteBl.ConsultarAmbientes();
                Ambiente = (from i in Ambiente
                            where i.IdSede == IdSede
                            select i).ToList();
            }


            var Ficha = new List<Ficha>();
            if (Transversal == true)
            {
                Ficha = oFichaBl.ConsultarFichasxArea(IdCoordinacion);
            }
            else
            {
                if (Manana == true)
                {
                    Ficha = oFichaBl.ConsultarFichasxPrograma(IdPrograma);
                    Ficha = (from i in Ficha
                             where i.Jornada == "MAÑANA"
                             select i).ToList();
                }
                else if (Tarde == true)
                {
                    Ficha = oFichaBl.ConsultarFichasxPrograma(IdPrograma);
                    Ficha = (from i in Ficha
                             where i.Jornada == "TARDE"
                             select i).ToList();
                }
                else if (Noche == true)
                {
                    Ficha = oFichaBl.ConsultarFichasxPrograma(IdPrograma);
                    Ficha = (from i in Ficha
                             where i.Jornada == "NOCTURNA"
                             select i).ToList();
                }
                else
                {
                    Ficha = oFichaBl.ConsultarFichasxPrograma(IdPrograma);
                }


            }

            List<Ambiente> oListaAmbiente = new List<Ambiente>();
            List<Ficha> oListaFicha = new List<Ficha>();
            List<Instructor> oListaInstructor = new List<Instructor>();

            if (DisponibilidadAmbiente.Count == 0)
            {
                oListaAmbiente.AddRange(Ambiente);
            }
            else
            {
                var Amb = (from i in Ambiente
                           where !(from o in DisponibilidadAmbiente
                                   select o.IdAmbiente)
                                   .Contains(i.IdAmbiente)
                           select i).ToList();

                oListaAmbiente = Amb;
            }

            if (DisponibilidadFicha.Count == 0)
            {
                oListaFicha.AddRange(Ficha.Where(i => i.FechaInicio <= FechaInicio && i.FechaFin >= FechaFin));
            }
            else
            {

                var fic = (from i in Ficha
                           where !(from o in DisponibilidadFicha
                                   select o.IdFicha)
                                   .Contains(i.IdFicha)
                           && (i.FechaInicio <= FechaInicio && i.FechaFin >= FechaFin)
                           select i).ToList();

                oListaFicha = fic;

            }

            if (DisponibilidadInstructor.Count == 0)
            {
                oListaInstructor.AddRange(Instructor);
            }
            else
            {
                var Ins = (from i in Instructor
                           where !(from o in DisponibilidadInstructor
                                   select o.IdInstructor)
                                   .Contains(i.IdInstructor)
                           select i).ToList();
                oListaInstructor = Ins;
            }

            //DESCOMENTAR SI SE VA A REGRESAR A LOS AMBIENTES ASIGNADOS A CADA COORDINACION Y CAMBIAR LA VARIABLE "AmbientesOtraArea" A TRUE
            //if (oListaAmbiente.Count == 0)
            //{
            if (Sabado == false && Domingo == false)
            {
                var ambientes = entity.Database.SqlQuery<Ambiente>("sp_disponibilidadAmbienteAreasAlternas @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @lunes, @martes, @miercoles, @jueves, @viernes,@IdSede",
                                                                    new SqlParameter("fecha_ini", FechaInicio),
                                                                    new SqlParameter("fecha_Fin", FechaFin),
                                                                    new SqlParameter("HoraInicio", HoraInicio),
                                                                    new SqlParameter("HoraFin", HoraFin),
                                                                    new SqlParameter("lunes", Lunes),
                                                                    new SqlParameter("martes", Martes),
                                                                    new SqlParameter("miercoles", Miercoles),
                                                                    new SqlParameter("jueves", Jueves),
                                                                    new SqlParameter("viernes", Viernes),
                                                                    new SqlParameter("IdSede", IdSede)).ToList();
                oListaAmbiente = ambientes.OrderBy(x => int.Parse(x.Numero)).ToList();

            }

            //foreach (var item in ambientes)
            //{
            //    oListaAmbiente.Add(item);
            //}


            AmbientesOtraArea = false;

            //}



            return new Tuple<List<Ambiente>, List<Instructor>, List<Ficha>, bool, bool>(oListaAmbiente, oListaInstructor.OrderBy(x => x.Nombre).ToList(), oListaFicha.OrderBy(x => x.Ficha1).ToList(), AmbientesOtraArea, NoValidarFicha);
        }

        public Tuple<List<Ambiente>, List<Instructor>, List<Ficha>, bool> DisponibilidadTransversalCJ(bool Transversal, int IdCoordinacion, DateTime FechaInicio, DateTime FechaFin, string HoraInicio, string HoraFin, int IdSede, int IdPrograma, bool Lunes, bool Martes, bool Miercoles, bool Jueves, bool Viernes, bool Sabado, bool Domingo)
        {
            Model1 entity = new Model1();
            InstructorBl oInstructorBl = new InstructorBl();
            AmbienteBl oAmbienteBl = new AmbienteBl();
            FichaBl oFichaBl = new FichaBl();
            List<Instructor> DisponibilidadInstructor = new List<Instructor>();
            List<Instructor> Instructor = new List<Instructor>();
            var DisponibilidadAmbiente = new List<Ambiente>();
            var DisponibilidadFicha = new List<Ficha>();
            bool AmbientesOtraArea = false;


            var Coordinacion = (from i in entity.Coordinacion
                                where i.IdCoordinacion == IdCoordinacion
                                select i).FirstOrDefault();




            if (Sabado == true || Domingo == true)
            {
                int jornada = 0;
                if (Sabado == true)
                {
                    jornada = 2;
                }
                if (Domingo == true)
                {
                    jornada = 3;
                }
                DisponibilidadAmbiente = entity.Database.SqlQuery<Ambiente>("sp_disponibilidadAmbienteSabado @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin,@jornada",
                                                                                                 new SqlParameter("fecha_ini", FechaInicio),
                                                                                                 new SqlParameter("fecha_Fin", FechaFin),
                                                                                                 new SqlParameter("HoraInicio", HoraInicio),
                                                                                                 new SqlParameter("HoraFin", HoraFin),
                                                                                                 new SqlParameter("jornada", jornada)).ToList();


                DisponibilidadFicha = entity.Database.SqlQuery<Ficha>("sp_disponibilidadFichaSabado @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @idarea, @jornada",
                                                                                      new SqlParameter("fecha_ini", FechaInicio),
                                                                                      new SqlParameter("fecha_Fin", FechaFin),
                                                                                      new SqlParameter("HoraInicio", HoraInicio),
                                                                                      new SqlParameter("HoraFin", HoraFin),
                                                                                      new SqlParameter("idarea", Coordinacion.IdArea),
                                                                                      new SqlParameter("jornada", jornada)).ToList();

                if (Sabado == true)
                {

                    DisponibilidadInstructor = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructorTransversalSabado @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin",
                                                                                       new SqlParameter("fecha_ini", FechaInicio),
                                                                                       new SqlParameter("fecha_Fin", FechaFin),
                                                                                       new SqlParameter("HoraInicio", HoraInicio),
                                                                                       new SqlParameter("HoraFin", HoraFin)).ToList();

                }

                if (Domingo == true)
                {
                    DisponibilidadInstructor = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructorTransversalDomingo @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin",
                                                                                       new SqlParameter("fecha_ini", FechaInicio),
                                                                                       new SqlParameter("fecha_Fin", FechaFin),
                                                                                       new SqlParameter("HoraInicio", HoraInicio),
                                                                                       new SqlParameter("HoraFin", HoraFin)).ToList();
                }
                Instructor = (from i in entity.Instructor
                              where i.Estado == true
                              && i.TipoInstructor == 2
                              select i).ToList();
            }
            else
            {
                var sede = (from i in entity.Sede
                            where i.IdSede == IdSede
                            select i).FirstOrDefault();

                if (sede.TipoSede != 2)
                {
                    DisponibilidadAmbiente = entity.Database.SqlQuery<Ambiente>("sp_disponibilidadAmbiente @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @idarea, @idsede, @lunes, @martes, @miercoles, @jueves, @viernes",
                                                                                    new SqlParameter("fecha_ini", FechaInicio),
                                                                                    new SqlParameter("fecha_Fin", FechaFin),
                                                                                    new SqlParameter("HoraInicio", HoraInicio),
                                                                                    new SqlParameter("HoraFin", HoraFin),
                                                                                    new SqlParameter("idarea", Coordinacion.IdArea),
                                                                                    new SqlParameter("idsede", IdSede),
                                                                                    new SqlParameter("lunes", Lunes),
                                                                                    new SqlParameter("martes", Martes),
                                                                                    new SqlParameter("miercoles", Miercoles),
                                                                                    new SqlParameter("jueves", Jueves),
                                                                                    new SqlParameter("viernes", Viernes)).ToList();
                }





                if (Transversal == true)
                {

                    if (Sabado == true)
                    {

                        DisponibilidadInstructor = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructorTransversalSabado @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin",
                                                                                           new SqlParameter("fecha_ini", FechaInicio),
                                                                                           new SqlParameter("fecha_Fin", FechaFin),
                                                                                           new SqlParameter("HoraInicio", HoraInicio),
                                                                                           new SqlParameter("HoraFin", HoraFin)).ToList();

                    }

                    if (Domingo == true)
                    {
                        DisponibilidadInstructor = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructorTransversalDomingo @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin",
                                                                                           new SqlParameter("fecha_ini", FechaInicio),
                                                                                           new SqlParameter("fecha_Fin", FechaFin),
                                                                                           new SqlParameter("HoraInicio", HoraInicio),
                                                                                           new SqlParameter("HoraFin", HoraFin)).ToList();
                    }

                    if (Lunes == true || Martes == true || Miercoles == true || Jueves == true || Viernes == true)
                    {
                        DisponibilidadInstructor = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructorTransversalDias @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @lunes, @martes, @miercoles, @jueves, @viernes",
                                                                                       new SqlParameter("fecha_ini", FechaInicio),
                                                                                       new SqlParameter("fecha_Fin", FechaFin),
                                                                                       new SqlParameter("HoraInicio", HoraInicio),
                                                                                       new SqlParameter("HoraFin", HoraFin),
                                                                                       new SqlParameter("lunes", Lunes),
                                                                                       new SqlParameter("martes", Martes),
                                                                                       new SqlParameter("miercoles", Miercoles),
                                                                                       new SqlParameter("jueves", Jueves),
                                                                                       new SqlParameter("viernes", Viernes)).ToList();
                    }

                    Instructor = (from i in entity.Instructor
                                  where i.Estado == true
                                  && i.TipoInstructor == 2 && i.IdArea == Coordinacion.IdArea
                                  select i).ToList();

                }
                else
                {
                    DisponibilidadInstructor = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructor @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin,@idarea, @lunes, @martes, @miercoles, @jueves, @viernes",
                                                                                     new SqlParameter("fecha_ini", FechaInicio),
                                                                                     new SqlParameter("fecha_Fin", FechaFin),
                                                                                     new SqlParameter("HoraInicio", HoraInicio),
                                                                                     new SqlParameter("HoraFin", HoraFin),
                                                                                     new SqlParameter("idarea", Coordinacion.IdArea),
                                                                                     new SqlParameter("lunes", Lunes),
                                                                                     new SqlParameter("martes", Martes),
                                                                                     new SqlParameter("miercoles", Miercoles),
                                                                                     new SqlParameter("jueves", Jueves),
                                                                                     new SqlParameter("viernes", Viernes)).ToList();

                    Instructor = (from i in entity.Instructor
                                  where i.Estado == true && i.IdArea == Coordinacion.IdArea
                                  && i.TipoInstructor == 1
                                  select i).ToList();
                }

                DisponibilidadFicha = entity.Database.SqlQuery<Ficha>("sp_disponibilidadFicha @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @idarea,  @idprograma, @lunes, @martes, @miercoles, @jueves, @viernes",
                                                                                      new SqlParameter("fecha_ini", FechaInicio),
                                                                                      new SqlParameter("fecha_Fin", FechaFin),
                                                                                      new SqlParameter("HoraInicio", HoraInicio),
                                                                                      new SqlParameter("HoraFin", HoraFin),
                                                                                      new SqlParameter("idarea", Coordinacion.IdArea),
                                                                                      new SqlParameter("idprograma", IdPrograma),
                                                                                      new SqlParameter("lunes", Lunes),
                                                                                      new SqlParameter("martes", Martes),
                                                                                      new SqlParameter("miercoles", Miercoles),
                                                                                      new SqlParameter("jueves", Jueves),
                                                                                      new SqlParameter("viernes", Viernes)).ToList();

            }
            //var Ambiente = oAmbienteBl.ConsultarAmbientesxArea(Coordinacion.IdArea, IdSede);

            var Ambiente = oAmbienteBl.ConsultarAmbientes();
            Ambiente = (from i in Ambiente
                        where i.IdSede == IdSede
                        select i).ToList();

            var Ficha = new List<Ficha>();
            //if (Transversal == true)
            //{
            //    Ficha = oFichaBl.ConsultarFichasxArea(IdCoordinacion);
            //}
            //else
            //{
            //    Ficha = oFichaBl.ConsultarFichasxPrograma(IdPrograma);
            //}
            Ficha = oFichaBl.ConsultarFichasxPrograma(IdPrograma);

            List<Ambiente> oListaAmbiente = new List<Ambiente>();
            List<Ficha> oListaFicha = new List<Ficha>();
            List<Instructor> oListaInstructor = new List<Instructor>();

            if (DisponibilidadAmbiente.Count == 0)
            {
                oListaAmbiente.AddRange(Ambiente);
            }
            else
            {
                var Amb = (from i in Ambiente
                           where !(from o in DisponibilidadAmbiente
                                   select o.IdAmbiente)
                                   .Contains(i.IdAmbiente)
                           select i).ToList();

                oListaAmbiente = Amb;
            }

            if (DisponibilidadFicha.Count == 0)
            {
                oListaFicha.AddRange(Ficha);
            }
            else
            {

                var fic = (from i in Ficha
                           where !(from o in DisponibilidadFicha
                                   select o.IdFicha)
                                   .Contains(i.IdFicha)
                           select i).ToList();

                oListaFicha = fic;

            }

            if (DisponibilidadInstructor.Count == 0)
            {
                oListaInstructor.AddRange(Instructor);
            }
            else
            {
                var Ins = (from i in Instructor
                           where !(from o in DisponibilidadInstructor
                                   select o.IdInstructor)
                                   .Contains(i.IdInstructor)
                           select i).ToList();
                oListaInstructor = Ins;
            }

            //if (oListaAmbiente.Count == 0)
            //{
            if (Sabado == false && Domingo == false)
            {
                var ambientes = entity.Database.SqlQuery<Ambiente>("sp_disponibilidadAmbienteAreasAlternas @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @lunes, @martes, @miercoles, @jueves, @viernes, @IdSede",
                                                                       new SqlParameter("fecha_ini", FechaInicio),
                                                                       new SqlParameter("fecha_Fin", FechaFin),
                                                                       new SqlParameter("HoraInicio", HoraInicio),
                                                                       new SqlParameter("HoraFin", HoraFin),
                                                                       new SqlParameter("lunes", Lunes),
                                                                       new SqlParameter("martes", Martes),
                                                                       new SqlParameter("miercoles", Miercoles),
                                                                       new SqlParameter("jueves", Jueves),
                                                                       new SqlParameter("viernes", Viernes),
                                                                       new SqlParameter("IdSede", IdSede)).ToList();
            }



            //foreach (var item in ambientes)
            //{
            //    oListaAmbiente.Add(item);
            //}

            AmbientesOtraArea = false;

            //}



            return new Tuple<List<Ambiente>, List<Instructor>, List<Ficha>, bool>(oListaAmbiente, oListaInstructor.OrderBy(x => x.Nombre).ToList(), oListaFicha.OrderBy(x => x.Ficha1).ToList(), AmbientesOtraArea);
        }

        public Tuple<List<Ambiente>, List<Instructor>, List<Ficha>, string, string> Disponibilidad1(bool Transversal, int IdCoordinacion, DateTime FechaInicio, DateTime FechaFin, string HoraInicio, string HoraFin, int jornada, int IdInstructor, int IdFicha, int IdAmbiente)
        {
            Model1 entity = new Model1();
            InstructorBl oInstructorBl = new InstructorBl();
            AmbienteBl oAmbienteBl = new AmbienteBl();
            FichaBl oFichaBl = new FichaBl();
            List<Instructor> DisponibilidadInstructor = new List<Instructor>();
            List<Instructor> Instructor = new List<Instructor>();
            var MensajeInstructor = "";
            var MensajeFicha = "";
            var Dia = "";

            var Coordinacion = (from i in entity.Coordinacion
                                where i.IdCoordinacion == IdCoordinacion
                                select i).FirstOrDefault();

            var DisponibilidadAmbiente = entity.Database.SqlQuery<Ambiente>("sp_disponibilidadAmbienteSabado @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin,@jornada",
                                                                                  new SqlParameter("fecha_ini", FechaInicio),
                                                                                  new SqlParameter("fecha_Fin", FechaFin),
                                                                                  new SqlParameter("HoraInicio", HoraInicio),
                                                                                  new SqlParameter("HoraFin", HoraFin),
                                                                                  new SqlParameter("jornada", jornada)).ToList();


            if (Transversal == true)
            {
                DisponibilidadInstructor = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructorTransversalSabado @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin",
                                                                                 new SqlParameter("fecha_ini", FechaInicio),
                                                                                 new SqlParameter("fecha_Fin", FechaFin),
                                                                                 new SqlParameter("HoraInicio", HoraInicio),
                                                                                 new SqlParameter("HoraFin", HoraFin)).ToList();
                Instructor = (from i in entity.Instructor
                              where i.Estado == true
                              && i.TipoInstructor == 2 && i.IdArea == Coordinacion.IdArea
                              select i).ToList();
            }
            else
            {
                DisponibilidadInstructor = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructorSabado @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @idarea, @jornada",
                                                                                 new SqlParameter("fecha_ini", FechaInicio),
                                                                                 new SqlParameter("fecha_Fin", FechaFin),
                                                                                 new SqlParameter("HoraInicio", HoraInicio),
                                                                                 new SqlParameter("HoraFin", HoraFin),
                                                                                 new SqlParameter("idarea", Coordinacion.IdArea),
                                                                                 new SqlParameter("jornada", jornada)).ToList();
                Instructor = (from i in entity.Instructor
                              where i.Estado == true
                              && i.TipoInstructor == 1 && i.IdArea == Coordinacion.IdArea
                              select i).ToList();
            }

            var DisponibilidadFicha = entity.Database.SqlQuery<Ficha>("sp_disponibilidadFichaSabado @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @idarea, @jornada",
                                                                                  new SqlParameter("fecha_ini", FechaInicio),
                                                                                  new SqlParameter("fecha_Fin", FechaFin),
                                                                                  new SqlParameter("HoraInicio", HoraInicio),
                                                                                  new SqlParameter("HoraFin", HoraFin),
                                                                                  new SqlParameter("idarea", Coordinacion.IdArea),
                                                                                  new SqlParameter("jornada", jornada)).ToList();

            if (jornada == 2)
            {
                Dia = "Sábado";
            }
            if (jornada == 3)
            {
                Dia = "Domingo";
            }

            var ValidarInstructor = (from i in DisponibilidadInstructor
                                     where i.IdInstructor == IdInstructor
                                     select i).FirstOrDefault();

            if (ValidarInstructor != null)
            {
                MensajeInstructor = "El instructor " + ValidarInstructor.Nombre + " " + ValidarInstructor.Apellido + " no se encuentra disponible en ese horario el día " + Dia;
            }

            var ValidarFicha = (from i in DisponibilidadFicha
                                where i.IdFicha == IdFicha
                                select i).FirstOrDefault();


            if (ValidarFicha != null)
            {
                MensajeFicha = "La ficha " + ValidarFicha.Ficha1 + " no se encuentra disponible en ese horario el día " + Dia;
            }
            //Consulta de ambientes Translape
            var sede = oAmbienteBl.ConsultarAmbienteId(IdAmbiente);
            var Ambiente = oAmbienteBl.ConsultarAmbientes().Where(x => x.IdSede == sede.IdSede);
            var Ficha = oFichaBl.ConsultarFichasxArea(Coordinacion.IdCoordinacion);




            List<Ambiente> oListaAmbiente = new List<Ambiente>();
            List<Ficha> oListaFicha = new List<Ficha>();
            List<Instructor> oListaInstructor = new List<Instructor>();

            if (DisponibilidadAmbiente.Count == 0)
            {
                oListaAmbiente.AddRange(Ambiente);
            }
            else
            {
                var Amb = (from i in Ambiente
                           where !(from o in DisponibilidadAmbiente
                                   select o.IdAmbiente)
                                   .Contains(i.IdAmbiente)
                           select i).ToList();

                oListaAmbiente = Amb;
            }

            if (DisponibilidadFicha.Count == 0)
            {
                oListaFicha.AddRange(Ficha);
            }
            else
            {
                var fic = (from i in Ficha
                           where !(from o in DisponibilidadFicha
                                   select o.IdFicha)
                                   .Contains(i.IdFicha)
                           select i).ToList();

                oListaFicha = fic;
            }

            if (DisponibilidadInstructor.Count == 0)
            {
                oListaInstructor.AddRange(Instructor);
            }
            else
            {
                var Ins = (from i in Instructor
                           where !(from o in DisponibilidadInstructor
                                   select o.IdInstructor)
                                   .Contains(i.IdInstructor)
                           select i).ToList();
                oListaInstructor = Ins;
            }
            return new Tuple<List<Ambiente>, List<Instructor>, List<Ficha>, string, string>(oListaAmbiente.OrderBy(x => int.Parse(x.Numero)).ToList(), oListaInstructor.OrderBy(x => x.Nombre).ToList(), oListaFicha.OrderBy(x => x.Ficha1).ToList(), MensajeInstructor, MensajeFicha);
        }

        public Tuple<List<Instructor>, Ficha_Ambiente> DisponibilidadTrasversal(int IdCoordinacion, DateTime FechaInicio, DateTime FechaFin, string HoraInicio, string HoraFin, int IdProgramacionPrincipal, int DiaSemana, bool Lunes, bool Martes, bool Miercoles, bool Jueves, bool Viernes, bool Sabado, bool Domingo)
        {

            Model1 entity = new Model1();
            InstructorBl oInstructorBl = new InstructorBl();
            AmbienteBl oAmbienteBl = new AmbienteBl();
            FichaBl oFichaBl = new FichaBl();
            List<Instructor> DisponibilidadTrasversal = new List<Instructor>();
            List<Instructor> Instructor = new List<Instructor>();

            if (Sabado)
            {

                DisponibilidadTrasversal = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructorTransversalSabado @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin",
                                                                                   new SqlParameter("fecha_ini", FechaInicio),
                                                                                   new SqlParameter("fecha_Fin", FechaFin),
                                                                                   new SqlParameter("HoraInicio", HoraInicio),
                                                                                   new SqlParameter("HoraFin", HoraFin)).ToList();

            }

            if (Domingo)
            {
                DisponibilidadTrasversal = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructorTransversalDomingo @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin",
                                                                                   new SqlParameter("fecha_ini", FechaInicio),
                                                                                   new SqlParameter("fecha_Fin", FechaFin),
                                                                                   new SqlParameter("HoraInicio", HoraInicio),
                                                                                   new SqlParameter("HoraFin", HoraFin)).ToList();
            }

            if (!Domingo && !Sabado)
            {
                //DisponibilidadTrasversal = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructorTransversal @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @diasemana",
                //                                                               new SqlParameter("fecha_ini", FechaInicio),
                //                                                               new SqlParameter("fecha_Fin", FechaFin),
                //                                                               new SqlParameter("HoraInicio", HoraInicio),
                //                                                               new SqlParameter("HoraFin", HoraFin),
                //                                                               new SqlParameter("diasemana", DiaSemana)).ToList();
                DisponibilidadTrasversal = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructorTransversalDias @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @lunes, @martes, @miercoles, @jueves, @viernes",
                                                                                       new SqlParameter("fecha_ini", FechaInicio),
                                                                                       new SqlParameter("fecha_Fin", FechaFin),
                                                                                       new SqlParameter("HoraInicio", HoraInicio),
                                                                                       new SqlParameter("HoraFin", HoraFin),
                                                                                       new SqlParameter("lunes", Lunes),
                                                                                       new SqlParameter("martes", Martes),
                                                                                       new SqlParameter("miercoles", Miercoles),
                                                                                       new SqlParameter("jueves", Jueves),
                                                                                       new SqlParameter("viernes", Viernes)).ToList();
            }

            var coordinacion = (from c in entity.Coordinacion
                                where c.IdCoordinacion == IdCoordinacion
                                select c).FirstOrDefault();

            Instructor = (from i in entity.Instructor
                          where i.Estado == true
                          && i.TipoInstructor == 2 && i.IdArea == coordinacion.IdArea
                          select i).ToList();

            var ProgramacionPrincipal = (from i in entity.Ficha_Ambiente
                                         where i.Id == IdProgramacionPrincipal
                                         select i).FirstOrDefault();
            var Jornada = 0;
            if (Sabado)
            {
                Jornada = 2;
            }
            if (Domingo)
            {
                Jornada = 3;
            }
            var ProgramacionTransversal1 = (from p in entity.Ficha_Ambiente
                                            where p.ProgramacionPrincipal == IdProgramacionPrincipal &&
                                            (p.Lunes == Lunes || p.Martes == Martes || p.Miercoles == Miercoles || p.Jueves == Jueves || p.Viernes == Viernes || p.Jornada == Jornada)
                                            select p).ToList();


            List<Instructor> oListaInstructor = new List<Instructor>();

            if (DisponibilidadTrasversal.Count == 0)
            {
                oListaInstructor.AddRange(Instructor);
            }
            else
            {
                Instructor oInstructor = new Instructor();

                var InstructorTrasversal = (from i in Instructor
                                            where !(from o in DisponibilidadTrasversal
                                                    select o.IdInstructor).Contains(i.IdInstructor)
                                            select i).ToList();


                oListaInstructor.AddRange(InstructorTrasversal);


            }
            return new Tuple<List<Instructor>, Ficha_Ambiente>(oListaInstructor.OrderBy(x => x.Nombre).ToList(), ProgramacionPrincipal);
        }

        //Cambiar resultados por competencias para regresar a programar con los resultado nuevamente
        public List<Ficha_AmbienteDTO> ConsultarProgramacion(int IdCoordinador)
        {
            Model1 entity = new Model1();
            var fecha = DateTime.Parse((DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()).ToString());
            var Datos = (from i in entity.Ficha_Ambiente
                         join a in entity.Instructor on i.IdInstructor equals a.IdInstructor
                         join p in entity.Area on a.IdArea equals p.IdArea
                         join c in entity.Coordinacion on a.IdArea equals c.IdArea
                         where c.IdCoordinacion == IdCoordinador && i.Transversal != true
                         orderby i.Id descending
                         select i).ToList();

            var fechaMayor = (from i in Datos
                     where i.FechaInicio > fecha 
                     orderby i.FechaInicio
                     select i).ToList();

            Datos = (from i in Datos
                     where  (i.FechaInicio >= fecha && i.FechaInicio <= fecha)
                             || (i.FechaFin <= fecha && i.FechaFin >= fecha)
                             || (i.FechaInicio <= fecha && i.FechaFin >= fecha)
                     orderby i.FechaInicio
                     select i).ToList();

            Datos.AddRange(fechaMayor);

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
                var DatosCompetencia = (from i in entity.Competencia
                                        join r in entity.Resultado_Aprendizaje on i.IdCompetencia equals r.IdCompetencia
                                        where r.IdResultado == item.IdResultado
                                        select i).FirstOrDefault();

                var DatosAmbiente = (from i in entity.Ambiente
                                     where i.IdAmbiente == item.IdAmbiente
                                     select i).FirstOrDefault();
                var DatosPrograma = (from i in entity.Programa
                                     join c in entity.Competencia on i.IdPrograma equals c.IdPrograma
                                     join r in entity.Resultado_Aprendizaje on c.IdCompetencia equals r.IdCompetencia
                                     where r.IdResultado == item.IdResultado
                                     select i).FirstOrDefault();
                var DatosInstructor = (from i in entity.Instructor
                                       where i.IdInstructor == item.IdInstructor
                                       select i).FirstOrDefault();

                var Sede = (from i in entity.Sede
                            where i.IdSede == DatosAmbiente.IdSede
                            select i).FirstOrDefault();


                if (item.Lunes == true)
                {
                    Ficha.DiasProgramados += "Lunes -";
                }
                if (item.Martes == true)
                {
                    Ficha.DiasProgramados += "Martes -";
                }
                if (item.Miercoles == true)
                {
                    Ficha.DiasProgramados += "Miercoles -";
                }
                if (item.Jueves == true)
                {
                    Ficha.DiasProgramados += "Jueves -";
                }
                if (item.Viernes == true)
                {
                    Ficha.DiasProgramados += "Viernes -";

                }
                if (item.Jornada == 2)
                {
                    Ficha.DiasProgramados += "Sábado -";
                }
                if (item.Jornada == 3)
                {
                    Ficha.DiasProgramados += "Domingo -";
                }
                Ficha.NombreInstructor = DatosInstructor.Nombre + " " + DatosInstructor.Apellido;
                Ficha.Programa = DatosPrograma.NombrePrograma;
                Ficha.Id = item.Id;
                Ficha.Ambiente = DatosAmbiente.Numero;
                Ficha.Ficha = DatosFicha.Ficha1;
                Ficha.CodigoResultado = DatosResultado.Codigo;
                Ficha.Resultado = DatosResultado.Resultado;
                Ficha.CodigoCompetencia = DatosCompetencia.Codigo.ToString();
                Ficha.Competencia = DatosCompetencia.Nombre;
                Ficha.IdInstructor = item.IdInstructor;
                Ficha.FechaInicio = item.FechaInicio;
                Ficha.FechaFin = item.FechaFin;
                Ficha.HoraInicio = item.HoraInicio;
                Ficha.HoraFin = item.HoraFin;
                Ficha.Color = item.Color;
                Ficha.IdAmbiente = DatosAmbiente.IdAmbiente;
                Ficha.IdFicha = DatosFicha.IdFicha;
                Ficha.Sede = Sede.Nombre_Sede;
                Programacion.Add(Ficha);

            }

            return Programacion;
            //return Datos;
        }

        public List<Ficha_AmbienteDTO> ConsultarProgramacionSabados(int IdCoordinador)
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Ficha_Ambiente
                         join a in entity.Ambiente on i.IdAmbiente equals a.IdAmbiente
                         join p in entity.Area on a.IdArea equals p.IdArea
                         join c in entity.Coordinacion on a.IdArea equals c.IdArea
                         where c.IdCoordinacion == IdCoordinador && i.Jornada != 1 && i.DiaSemana == null
                         orderby i.FechaFin descending
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
                var DatosCompetencia = (from i in entity.Competencia
                                        join r in entity.Resultado_Aprendizaje on i.IdCompetencia equals r.IdCompetencia
                                        where r.IdResultado == item.IdResultado
                                        select i).FirstOrDefault();

                var DatosAmbiente = (from i in entity.Ambiente
                                     where i.IdAmbiente == item.IdAmbiente
                                     select i).FirstOrDefault();
                var DatosPrograma = (from i in entity.Programa
                                     join c in entity.Competencia on i.IdPrograma equals c.IdPrograma
                                     join r in entity.Resultado_Aprendizaje on c.IdCompetencia equals r.IdCompetencia
                                     where r.IdResultado == item.IdResultado
                                     select i).FirstOrDefault();
                var DatosInstructor = (from i in entity.Instructor
                                       where i.IdInstructor == item.IdInstructor
                                       select i).FirstOrDefault();


                var DatosSede = (from s in entity.Sede
                                 where s.IdSede == DatosAmbiente.IdSede
                                 select s).FirstOrDefault();


                Ficha.Sede = DatosSede.Nombre_Sede;
                Ficha.NombreInstructor = DatosInstructor.Nombre + " " + DatosInstructor.Apellido;
                Ficha.Programa = DatosPrograma.NombrePrograma;
                Ficha.Id = item.Id;
                Ficha.Ambiente = DatosAmbiente.Numero;
                Ficha.Ficha = DatosFicha.Ficha1;
                Ficha.CodigoResultado = DatosResultado.Codigo;
                Ficha.Resultado = DatosResultado.Resultado;
                Ficha.CodigoCompetencia = DatosCompetencia.Codigo.ToString();
                Ficha.Competencia = DatosCompetencia.Nombre;
                Ficha.IdInstructor = item.IdInstructor;
                Ficha.FechaInicio = item.FechaInicio;
                Ficha.FechaFin = item.FechaFin;
                Ficha.HoraInicio = item.HoraInicio;
                Ficha.HoraFin = item.HoraFin;
                Ficha.Color = item.Color;


                if (item.Jornada == 2)
                {
                    Ficha.Jornada = "Sábado";
                }
                if (item.Jornada == 3)
                {
                    Ficha.Jornada = "Domingo";
                }

                Programacion.Add(Ficha);

            }

            return Programacion;
            //return Datos;
        }

        public List<Ficha_AmbienteDTO> ConsultarProgramacionTransversales(int IdCoordinador)
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Ficha_Ambiente
                         join t in entity.Instructor on i.IdInstructor equals t.IdInstructor
                         //join a in entity.Ambiente on i.IdAmbiente equals a.IdAmbiente 
                         join p in entity.Area on t.IdArea equals p.IdArea
                         join c in entity.Coordinacion on p.IdArea equals c.IdArea
                         where t.TipoInstructor == 2 && (i.Transversal == true) && c.IdCoordinacion == IdCoordinador && i.Estado == true
                         orderby i.Id descending
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

                var DatosCompetencia = (from i in entity.Competencia
                                        where i.IdCompetencia == DatosResultado.IdCompetencia
                                        select i).FirstOrDefault();

                var DatosAmbiente = (from i in entity.Ambiente
                                     where i.IdAmbiente == item.IdAmbiente
                                     select i).FirstOrDefault();
                var DatosPrograma = (from i in entity.Programa
                                     join c in entity.Competencia on i.IdPrograma equals c.IdPrograma
                                     join r in entity.Resultado_Aprendizaje on c.IdCompetencia equals r.IdCompetencia
                                     where r.IdResultado == item.IdResultado
                                     select i).FirstOrDefault();
                var DatosInstructor = (from i in entity.Instructor
                                       where i.IdInstructor == item.IdInstructor
                                       select i).FirstOrDefault();

                var DatosSede = (from s in entity.Sede
                                 where s.IdSede == DatosAmbiente.IdSede
                                 select s).FirstOrDefault();


                if (item.Transversal == true)
                {
                    if (item.Lunes != null)
                    {
                        Ficha.DiasProgramados += "Lunes -";
                    }
                    if (item.Martes != null)
                    {
                        Ficha.DiasProgramados += "Martes -";
                    }
                    if (item.Miercoles != null)
                    {
                        Ficha.DiasProgramados += "Miércoles -";
                    }
                    if (item.Jueves != null)
                    {
                        Ficha.DiasProgramados += "Jueves -";
                    }
                    if (item.Viernes != null)
                    {
                        Ficha.DiasProgramados += "Viernes -";
                    }
                    if (item.Jornada == 2)
                    {
                        Ficha.DiasProgramados += "Sábado -";
                    }
                    if (item.Jornada == 3)
                    {
                        Ficha.DiasProgramados += "Domingo -";
                    }
                }
                Ficha.Sede = DatosSede.Nombre_Sede;
                Ficha.NombreInstructor = DatosInstructor.Nombre + " " + DatosInstructor.Apellido;
                Ficha.Programa = DatosPrograma.NombrePrograma;
                Ficha.Id = item.Id;
                Ficha.Ambiente = DatosAmbiente.Numero;
                Ficha.Ficha = DatosFicha.Ficha1;
                Ficha.CodigoResultado = DatosResultado.Codigo;
                Ficha.Resultado = DatosResultado.Resultado;
                Ficha.Competencia = DatosCompetencia.Nombre;
                Ficha.CodigoCompetencia = DatosCompetencia.Codigo.ToString();
                Ficha.IdInstructor = item.IdInstructor;
                Ficha.FechaInicio = item.FechaInicio;
                Ficha.FechaFin = item.FechaFin;
                Ficha.HoraInicio = item.HoraInicio;
                Ficha.HoraFin = item.HoraFin;
                Ficha.Color = item.Color;
                Ficha.DiaSemana = item.DiaSemana;

                Programacion.Add(Ficha);

            }

            return Programacion;
            //return Datos;
        }

        public List<Ficha_AmbienteDTO> FiltrarAmbiente(int IdAmbiente, int jornada)
        {
            TimeSpan horaInicio = TimeSpan.Parse("00:00");
            TimeSpan horaFin = TimeSpan.Parse("00:00");

            if (jornada == 1)
            {
                horaInicio = TimeSpan.Parse("06:00");
                horaFin = TimeSpan.Parse("11:59");

            }
            if (jornada == 2)
            {
                horaInicio = TimeSpan.Parse("12:00");
                horaFin = TimeSpan.Parse("17:59");
            }

            if (jornada == 3)
            {
                horaInicio = TimeSpan.Parse("18:00");
                horaFin = TimeSpan.Parse("22:00");
            }


            Model1 entity = new Model1();
            var Datos = (from i in entity.Ficha_Ambiente
                         where i.IdAmbiente == IdAmbiente && (i.HoraInicio >= horaInicio && i.HoraInicio <= horaFin) && i.Estado == true
                         select i).ToList();
            List<Ficha_AmbienteDTO> Programacion = new List<Ficha_AmbienteDTO>();
            string diaIngles = "";

            foreach (var item in Datos)
            {
                for (DateTime date = item.FechaInicio; date <= item.FechaFin; date = date.AddDays(1))
                {

                    DateTime dateValue = new DateTime(date.Year, date.Month, date.Day);

                    if (item.Jornada == 1)
                    {
                        if (dateValue.DayOfWeek.ToString() == "Saturday" || dateValue.DayOfWeek.ToString() == "Sunday")
                        {

                        }
                        else
                        {
                            var Lunes = "";
                            var Martes = "";
                            var Miercoles = "";
                            var Jueves = "";
                            var Viernes = "";

                            if (item.Lunes == true)
                            {
                                Lunes = "Monday";
                            }
                            if (item.Martes == true)
                            {
                                Martes = "Tuesday";
                            }
                            if (item.Miercoles == true)
                            {
                                Miercoles = "Wednesday";
                            }
                            if (item.Jueves == true)
                            {
                                Jueves = "Thursday";
                            }
                            if (item.Viernes == true)
                            {
                                Viernes = "Friday";
                            }

                            if (dateValue.DayOfWeek.ToString() == Lunes || dateValue.DayOfWeek.ToString() == Martes || dateValue.DayOfWeek.ToString() == Miercoles || dateValue.DayOfWeek.ToString() == Jueves || dateValue.DayOfWeek.ToString() == Viernes)
                            {
                                Ficha_AmbienteDTO Ficha = new Ficha_AmbienteDTO();
                                var DatosFicha = (from i in entity.Ficha
                                                  where i.IdFicha == item.IdFicha
                                                  select i).FirstOrDefault();
                                var DatosCompetencia = (from i in entity.Resultado_Aprendizaje
                                                        where i.IdResultado == item.IdResultado
                                                        select i).FirstOrDefault();
                                var DatosPrograma = (from i in entity.Programa
                                                     join c in entity.Competencia on i.IdPrograma equals c.IdPrograma
                                                     join r in entity.Resultado_Aprendizaje on c.IdCompetencia equals r.IdCompetencia
                                                     where r.IdResultado == item.IdResultado
                                                     select i).FirstOrDefault();
                                var DatosInstructor = (from i in entity.Instructor
                                                       where i.IdInstructor == item.IdInstructor
                                                       select i).FirstOrDefault();

                                Ficha.NombreInstructor = DatosInstructor.Nombre + " " + DatosInstructor.Apellido;
                                Ficha.Programa = DatosPrograma.NombrePrograma;
                                Ficha.Id = item.Id;
                                Ficha.IdAmbiente = item.IdAmbiente;
                                Ficha.Ficha = DatosFicha.Ficha1;
                                Ficha.Competencia = DatosCompetencia.Resultado;
                                Ficha.IdInstructor = item.IdInstructor;
                                Ficha.FechaInicio = date;
                                Ficha.FechaFin = date;
                                Ficha.HoraInicio = item.HoraInicio;
                                Ficha.HoraFin = item.HoraFin;
                                Ficha.Color = item.Color;
                                Programacion.Add(Ficha);
                            }
                        }
                    }

                    //if (item.Jornada == 1 && (item.DiaSemana != null && item.DiaSemana != 6))
                    //{
                    //    switch (item.DiaSemana)
                    //    {
                    //        case 1:
                    //            {
                    //                diaIngles = "Monday";

                    //            }
                    //            break;
                    //        case 2:
                    //            {
                    //                diaIngles = "Tuesday";

                    //            }
                    //            break;
                    //        case 3:
                    //            {
                    //                diaIngles = "Wednesday";

                    //            }
                    //            break;
                    //        case 4:
                    //            {
                    //                diaIngles = "Thursday";

                    //            }
                    //            break;
                    //        case 5:
                    //            {
                    //                diaIngles = "Friday";

                    //            }
                    //            break;

                    //    }

                    //    if (dateValue.DayOfWeek.ToString() == "Saturday" || dateValue.DayOfWeek.ToString() == "Sunday")
                    //    {

                    //    }
                    //    else
                    //    {
                    //        if (dateValue.DayOfWeek.ToString() == (diaIngles).ToString())
                    //        {

                    //            Ficha_AmbienteDTO Ficha = new Ficha_AmbienteDTO();
                    //            var DatosFicha = (from i in entity.Ficha
                    //                              where i.IdFicha == item.IdFicha
                    //                              select i).FirstOrDefault();
                    //            var DatosCompetencia = (from i in entity.Resultado_Aprendizaje
                    //                                    where i.IdResultado == item.IdResultado
                    //                                    select i).FirstOrDefault();
                    //            var DatosPrograma = (from i in entity.Programa
                    //                                 join c in entity.Competencia on i.IdPrograma equals c.IdPrograma
                    //                                 join r in entity.Resultado_Aprendizaje on c.IdCompetencia equals r.IdCompetencia
                    //                                 where r.IdResultado == item.IdResultado
                    //                                 select i).FirstOrDefault();
                    //            var DatosInstructor = (from i in entity.Instructor
                    //                                   where i.IdInstructor == item.IdInstructor
                    //                                   select i).FirstOrDefault();

                    //            Ficha.NombreInstructor = DatosInstructor.Nombre + " " + DatosInstructor.Apellido;
                    //            Ficha.Programa = DatosPrograma.NombrePrograma;
                    //            Ficha.Id = item.Id;
                    //            Ficha.IdAmbiente = item.IdAmbiente;
                    //            Ficha.Ficha = DatosFicha.Ficha1;
                    //            Ficha.Competencia = DatosCompetencia.Resultado;
                    //            Ficha.IdInstructor = item.IdInstructor;
                    //            Ficha.FechaInicio = date;
                    //            Ficha.FechaFin = date;
                    //            Ficha.HoraInicio = item.HoraInicio;
                    //            Ficha.HoraFin = item.HoraFin;
                    //            Ficha.Color = item.Color;
                    //            Programacion.Add(Ficha);
                    //        }

                    //    }
                    //}

                    if (item.Jornada == 2)
                    {
                        if (dateValue.DayOfWeek.ToString() == "Saturday")
                        {

                            Ficha_AmbienteDTO Ficha = new Ficha_AmbienteDTO();
                            var DatosFicha = (from i in entity.Ficha
                                              where i.IdFicha == item.IdFicha
                                              select i).FirstOrDefault();
                            var DatosCompetencia = (from i in entity.Resultado_Aprendizaje
                                                    where i.IdResultado == item.IdResultado
                                                    select i).FirstOrDefault();
                            var DatosPrograma = (from i in entity.Programa
                                                 join c in entity.Competencia on i.IdPrograma equals c.IdPrograma
                                                 join r in entity.Resultado_Aprendizaje on c.IdCompetencia equals r.IdCompetencia
                                                 where r.IdResultado == item.IdResultado
                                                 select i).FirstOrDefault();
                            var DatosInstructor = (from i in entity.Instructor
                                                   where i.IdInstructor == item.IdInstructor
                                                   select i).FirstOrDefault();

                            Ficha.NombreInstructor = DatosInstructor.Nombre + " " + DatosInstructor.Apellido;
                            Ficha.Programa = DatosPrograma.NombrePrograma;
                            Ficha.Id = item.Id;
                            Ficha.IdAmbiente = item.IdAmbiente;
                            Ficha.Ficha = DatosFicha.Ficha1;
                            Ficha.Competencia = DatosCompetencia.Resultado;
                            Ficha.IdInstructor = item.IdInstructor;
                            Ficha.FechaInicio = date;
                            Ficha.FechaFin = date;
                            Ficha.HoraInicio = item.HoraInicio;
                            Ficha.HoraFin = item.HoraFin;
                            Ficha.Color = item.Color;
                            Programacion.Add(Ficha);

                        }

                    }

                    //if (item.Jornada == 2 && item.DiaSemana == 6)
                    //{
                    //    if (dateValue.DayOfWeek.ToString() == "Saturday")
                    //    {

                    //        Ficha_AmbienteDTO Ficha = new Ficha_AmbienteDTO();
                    //        var DatosFicha = (from i in entity.Ficha
                    //                          where i.IdFicha == item.IdFicha
                    //                          select i).FirstOrDefault();
                    //        var DatosCompetencia = (from i in entity.Resultado_Aprendizaje
                    //                                where i.IdResultado == item.IdResultado
                    //                                select i).FirstOrDefault();
                    //        var DatosPrograma = (from i in entity.Programa
                    //                             join c in entity.Competencia on i.IdPrograma equals c.IdPrograma
                    //                             join r in entity.Resultado_Aprendizaje on c.IdCompetencia equals r.IdCompetencia
                    //                             where r.IdResultado == item.IdResultado
                    //                             select i).FirstOrDefault();
                    //        var DatosInstructor = (from i in entity.Instructor
                    //                               where i.IdInstructor == item.IdInstructor
                    //                               select i).FirstOrDefault();

                    //        Ficha.NombreInstructor = DatosInstructor.Nombre + " " + DatosInstructor.Apellido;
                    //        Ficha.Programa = DatosPrograma.NombrePrograma;
                    //        Ficha.Id = item.Id;
                    //        Ficha.IdAmbiente = item.IdAmbiente;
                    //        Ficha.Ficha = DatosFicha.Ficha1;
                    //        Ficha.Competencia = DatosCompetencia.Resultado;
                    //        Ficha.IdInstructor = item.IdInstructor;
                    //        Ficha.FechaInicio = date;
                    //        Ficha.FechaFin = date;
                    //        Ficha.HoraInicio = item.HoraInicio;
                    //        Ficha.HoraFin = item.HoraFin;
                    //        Ficha.Color = item.Color;
                    //        Programacion.Add(Ficha);

                    //    }
                    //}



                    if (item.Jornada == 3)
                    {
                        if (dateValue.DayOfWeek.ToString() == "Sunday")
                        {

                            Ficha_AmbienteDTO Ficha = new Ficha_AmbienteDTO();
                            var DatosFicha = (from i in entity.Ficha
                                              where i.IdFicha == item.IdFicha
                                              select i).FirstOrDefault();
                            var DatosCompetencia = (from i in entity.Resultado_Aprendizaje
                                                    where i.IdResultado == item.IdResultado
                                                    select i).FirstOrDefault();
                            var DatosPrograma = (from i in entity.Programa
                                                 join c in entity.Competencia on i.IdPrograma equals c.IdPrograma
                                                 join r in entity.Resultado_Aprendizaje on c.IdCompetencia equals r.IdCompetencia
                                                 where r.IdResultado == item.IdResultado
                                                 select i).FirstOrDefault();
                            var DatosInstructor = (from i in entity.Instructor
                                                   where i.IdInstructor == item.IdInstructor
                                                   select i).FirstOrDefault();

                            Ficha.NombreInstructor = DatosInstructor.Nombre + " " + DatosInstructor.Apellido;
                            Ficha.Programa = DatosPrograma.NombrePrograma;
                            Ficha.Id = item.Id;
                            Ficha.IdAmbiente = item.IdAmbiente;
                            Ficha.Ficha = DatosFicha.Ficha1;
                            Ficha.Competencia = DatosCompetencia.Resultado;
                            Ficha.FechaInicio = date;
                            Ficha.FechaFin = date;
                            Ficha.HoraInicio = item.HoraInicio;
                            Ficha.HoraFin = item.HoraFin;
                            Ficha.Color = item.Color;
                            Programacion.Add(Ficha);

                        }

                    }

                    //if (item.Jornada == 3 && item.DiaSemana == 6)
                    //{
                    //    if (dateValue.DayOfWeek.ToString() == "Sunday")
                    //    {

                    //        Ficha_AmbienteDTO Ficha = new Ficha_AmbienteDTO();
                    //        var DatosFicha = (from i in entity.Ficha
                    //                          where i.IdFicha == item.IdFicha
                    //                          select i).FirstOrDefault();
                    //        var DatosCompetencia = (from i in entity.Resultado_Aprendizaje
                    //                                where i.IdResultado == item.IdResultado
                    //                                select i).FirstOrDefault();
                    //        var DatosPrograma = (from i in entity.Programa
                    //                             join c in entity.Competencia on i.IdPrograma equals c.IdPrograma
                    //                             join r in entity.Resultado_Aprendizaje on c.IdCompetencia equals r.IdCompetencia
                    //                             where r.IdResultado == item.IdResultado
                    //                             select i).FirstOrDefault();
                    //        var DatosInstructor = (from i in entity.Instructor
                    //                               where i.IdInstructor == item.IdInstructor
                    //                               select i).FirstOrDefault();
                    //        Ficha.NombreInstructor = DatosInstructor.Nombre + " " + DatosInstructor.Apellido;
                    //        Ficha.Id = item.Id;
                    //        Ficha.IdAmbiente = item.IdAmbiente;
                    //        Ficha.Ficha = DatosFicha.Ficha1;
                    //        Ficha.Competencia = DatosCompetencia.Resultado;
                    //        Ficha.IdInstructor = item.IdInstructor;
                    //        Ficha.FechaInicio = date;
                    //        Ficha.FechaFin = date;
                    //        Ficha.HoraInicio = item.HoraInicio;
                    //        Ficha.HoraFin = item.HoraFin;
                    //        Ficha.Color = item.Color;
                    //        Programacion.Add(Ficha);

                    //    }
                    //}

                }
            }

            return Programacion;
            //return Datos;
        }

        public List<Ficha_AmbienteDTO> FiltrarInstrutor(int IdInstructor, int jornada)
        {

            TimeSpan horaInicio = TimeSpan.Parse("00:00");
            TimeSpan horaFin = TimeSpan.Parse("00:00");

            if (jornada == 1)
            {
                horaInicio = TimeSpan.Parse("06:00");
                horaFin = TimeSpan.Parse("12:00");

            }
            if (jornada == 2)
            {
                horaInicio = TimeSpan.Parse("12:00");
                horaFin = TimeSpan.Parse("18:00");
            }

            if (jornada == 3)
            {
                horaInicio = TimeSpan.Parse("18:00");
                horaFin = TimeSpan.Parse("22:00");
            }

            Model1 entity = new Model1();
            var instructor = (from i in entity.Instructor
                              where i.IdInstructor == IdInstructor
                              select i).FirstOrDefault();



            var Datos = (from i in entity.Ficha_Ambiente
                         where i.IdInstructor == IdInstructor && (i.HoraInicio >= horaInicio && i.HoraInicio <= horaFin)
                         select i).ToList();
            List<Ficha_AmbienteDTO> Programacion = new List<Ficha_AmbienteDTO>();

            foreach (var item in Datos)
            {
                for (DateTime date = item.FechaInicio; date <= item.FechaFin; date = date.AddDays(1))
                {

                    DateTime dateValue = new DateTime(date.Year, date.Month, date.Day);

                    if (item.Jornada == 1)
                    {
                        if (dateValue.DayOfWeek.ToString() == "Saturday" || dateValue.DayOfWeek.ToString() == "Sunday")
                        {


                        }
                        else
                        {
                            var Lunes = "";
                            var Martes = "";
                            var Miercoles = "";
                            var Jueves = "";
                            var Viernes = "";

                            if (item.Lunes == true)
                            {
                                Lunes = "Monday";
                            }
                            if (item.Martes == true)
                            {
                                Martes = "Tuesday";
                            }
                            if (item.Miercoles == true)
                            {
                                Miercoles = "Wednesday";
                            }
                            if (item.Jueves == true)
                            {
                                Jueves = "Thursday";
                            }
                            if (item.Viernes == true)
                            {
                                Viernes = "Friday";
                            }
                            if (dateValue.DayOfWeek.ToString() == Lunes || dateValue.DayOfWeek.ToString() == Martes || dateValue.DayOfWeek.ToString() == Miercoles || dateValue.DayOfWeek.ToString() == Jueves || dateValue.DayOfWeek.ToString() == Viernes)
                            {
                                Ficha_AmbienteDTO Ficha = new Ficha_AmbienteDTO();
                                var DatosFicha = (from i in entity.Ficha
                                                  where i.IdFicha == item.IdFicha
                                                  select i).FirstOrDefault();

                                var DatosCompetencia = (from i in entity.Resultado_Aprendizaje
                                                        where i.IdResultado == item.IdResultado
                                                        select i).FirstOrDefault();

                                var DatosPrograma = (from i in entity.Programa
                                                     join c in entity.Competencia on i.IdPrograma equals c.IdPrograma
                                                     join r in entity.Resultado_Aprendizaje on c.IdCompetencia equals r.IdCompetencia
                                                     where r.IdResultado == item.IdResultado
                                                     select i).FirstOrDefault();

                                var DatosInstructor = (from i in entity.Instructor
                                                       where i.IdInstructor == item.IdInstructor
                                                       select i).FirstOrDefault();

                                var DatosAmbiente = (from i in entity.Ambiente
                                                     where i.IdAmbiente == item.IdAmbiente
                                                     select i).FirstOrDefault();


                                Ficha.NombreInstructor = DatosInstructor.Nombre + " " + DatosInstructor.Apellido;
                                Ficha.Programa = DatosPrograma.NombrePrograma;
                                Ficha.Id = item.Id;
                                Ficha.IdAmbiente = item.IdAmbiente;
                                Ficha.Ficha = DatosFicha.Ficha1;
                                Ficha.Competencia = DatosCompetencia.Resultado;
                                Ficha.IdInstructor = item.IdInstructor;
                                Ficha.FechaInicio = date;
                                Ficha.FechaFin = date;
                                Ficha.HoraInicio = item.HoraInicio;
                                Ficha.HoraFin = item.HoraFin;
                                Ficha.Color = item.Color;
                                Ficha.Ambiente = DatosAmbiente.Numero;
                                Programacion.Add(Ficha);
                            }
                        }
                    }
                    if (item.Jornada == 2)
                    {
                        if (dateValue.DayOfWeek.ToString() == "Saturday")
                        {

                            Ficha_AmbienteDTO Ficha = new Ficha_AmbienteDTO();
                            var DatosFicha = (from i in entity.Ficha
                                              where i.IdFicha == item.IdFicha
                                              select i).FirstOrDefault();
                            var DatosCompetencia = (from i in entity.Resultado_Aprendizaje
                                                    where i.IdResultado == item.IdResultado
                                                    select i).FirstOrDefault();
                            var DatosPrograma = (from i in entity.Programa
                                                 join c in entity.Competencia on i.IdPrograma equals c.IdPrograma
                                                 join r in entity.Resultado_Aprendizaje on c.IdCompetencia equals r.IdCompetencia
                                                 where r.IdResultado == item.IdResultado
                                                 select i).FirstOrDefault();
                            var DatosInstructor = (from i in entity.Instructor
                                                   where i.IdInstructor == item.IdInstructor
                                                   select i).FirstOrDefault();

                            var DatosAmbiente = (from i in entity.Ambiente
                                                 where i.IdAmbiente == item.IdAmbiente
                                                 select i).FirstOrDefault();

                            Ficha.Ambiente = DatosAmbiente.Numero;
                            Ficha.NombreInstructor = DatosInstructor.Nombre + " " + DatosInstructor.Apellido;
                            Ficha.Programa = DatosPrograma.NombrePrograma;
                            Ficha.Id = item.Id;
                            Ficha.IdAmbiente = item.IdAmbiente;
                            Ficha.Ficha = DatosFicha.Ficha1;
                            Ficha.Competencia = DatosCompetencia.Resultado;
                            Ficha.IdInstructor = item.IdInstructor;
                            Ficha.FechaInicio = date;
                            Ficha.FechaFin = date;
                            Ficha.HoraInicio = item.HoraInicio;
                            Ficha.HoraFin = item.HoraFin;
                            Ficha.Color = item.Color;
                            Programacion.Add(Ficha);

                        }


                    }

                    if (item.Jornada == 3)
                    {

                        if (dateValue.DayOfWeek.ToString() == "Sunday")
                        {

                            Ficha_AmbienteDTO Ficha = new Ficha_AmbienteDTO();
                            var DatosFicha = (from i in entity.Ficha
                                              where i.IdFicha == item.IdFicha
                                              select i).FirstOrDefault();
                            var DatosCompetencia = (from i in entity.Resultado_Aprendizaje
                                                    where i.IdResultado == item.IdResultado
                                                    select i).FirstOrDefault();
                            var DatosPrograma = (from i in entity.Programa
                                                 join c in entity.Competencia on i.IdPrograma equals c.IdPrograma
                                                 join r in entity.Resultado_Aprendizaje on c.IdCompetencia equals r.IdCompetencia
                                                 where r.IdResultado == item.IdResultado
                                                 select i).FirstOrDefault();
                            var DatosInstructor = (from i in entity.Instructor
                                                   where i.IdInstructor == item.IdInstructor
                                                   select i).FirstOrDefault();

                            var DatosAmbiente = (from i in entity.Ambiente
                                                 where i.IdAmbiente == item.IdAmbiente
                                                 select i).FirstOrDefault();

                            Ficha.Ambiente = DatosAmbiente.Numero;
                            Ficha.NombreInstructor = DatosInstructor.Nombre + " " + DatosInstructor.Apellido;
                            Ficha.Programa = DatosPrograma.NombrePrograma;
                            Ficha.Id = item.Id;
                            Ficha.IdAmbiente = item.IdAmbiente;
                            Ficha.Ficha = DatosFicha.Ficha1;
                            Ficha.Competencia = DatosCompetencia.Resultado;
                            Ficha.IdInstructor = item.IdInstructor;
                            Ficha.FechaInicio = date;
                            Ficha.FechaFin = date;
                            Ficha.HoraInicio = item.HoraInicio;
                            Ficha.HoraFin = item.HoraFin;
                            Ficha.Color = item.Color;
                            Programacion.Add(Ficha);
                        }

                    }

                }
            }

            return Programacion;
            //return Datos;
        }

        public List<Ficha_AmbienteDTO> FiltroProgramacionxCoordinador(int jornada, int Ambiente)
        {
            TimeSpan horaInicio = TimeSpan.Parse("00:00");
            TimeSpan horaFin = TimeSpan.Parse("00:00");

            if (jornada == 1)
            {
                horaInicio = TimeSpan.Parse("06:00");
                horaFin = TimeSpan.Parse("12:00");

            }
            if (jornada == 2)
            {
                horaInicio = TimeSpan.Parse("12:01");
                horaFin = TimeSpan.Parse("18:00");
            }

            if (jornada == 3)
            {
                horaInicio = TimeSpan.Parse("18:01");
                horaFin = TimeSpan.Parse("22:00");
            }
            Model1 entity = new Model1();
            var Datos = (from f in entity.Ficha_Ambiente
                             //join r in entity.Resultado_Aprendizaje on f.IdResultado equals r.IdResultado
                             //join c in entity.Competencia on r.IdCompetencia equals c.IdCompetencia
                             //join p in entity.Programa on c.IdPrograma equals p.IdPrograma
                             //join a in entity.Area on p.IdArea equals a.IdArea
                             //join i in entity.Coordinacion on a.IdArea equals i.IdArea

                         where f.IdAmbiente == Ambiente
                          && (f.HoraInicio >= horaInicio && f.HoraInicio <= horaFin
                         || f.HoraFin >= horaInicio && f.HoraFin <= horaFin
                         || f.HoraInicio <= horaInicio && f.HoraFin >= horaFin)
                         select f).ToList();


            List<Ficha_AmbienteDTO> Programacion = new List<Ficha_AmbienteDTO>();

            foreach (var item in Datos)
            {
                for (DateTime date = item.FechaInicio; date <= item.FechaFin; date = date.AddDays(1))
                {

                    DateTime dateValue = new DateTime(date.Year, date.Month, date.Day);

                    if (item.Jornada == 1)
                    {
                        if (dateValue.DayOfWeek.ToString() == "Saturday" || dateValue.DayOfWeek.ToString() == "Sunday")
                        {

                        }
                        else
                        {
                            var Lunes = "";
                            var Martes = "";
                            var Miercoles = "";
                            var Jueves = "";
                            var Viernes = "";

                            if (item.Lunes == true)
                            {
                                Lunes = "Monday";
                            }
                            if (item.Martes == true)
                            {
                                Martes = "Tuesday";
                            }
                            if (item.Miercoles == true)
                            {
                                Miercoles = "Wednesday";
                            }
                            if (item.Jueves == true)
                            {
                                Jueves = "Thursday";
                            }
                            if (item.Viernes == true)
                            {
                                Viernes = "Friday";
                            }

                            if (dateValue.DayOfWeek.ToString() == Lunes || dateValue.DayOfWeek.ToString() == Martes || dateValue.DayOfWeek.ToString() == Miercoles || dateValue.DayOfWeek.ToString() == Jueves || dateValue.DayOfWeek.ToString() == Viernes)
                            {
                                Ficha_AmbienteDTO Ficha = new Ficha_AmbienteDTO();
                                var DatosFicha = (from i in entity.Ficha
                                                  where i.IdFicha == item.IdFicha
                                                  select i).FirstOrDefault();
                                var DatosCompetencia = (from i in entity.Resultado_Aprendizaje
                                                        where i.IdResultado == item.IdResultado
                                                        select i).FirstOrDefault();

                                var DatosAmbiente = (from i in entity.Ambiente
                                                     where i.IdAmbiente == item.IdAmbiente
                                                     select i).FirstOrDefault();
                                var DatosPrograma = (from i in entity.Programa
                                                     join c in entity.Competencia on i.IdPrograma equals c.IdPrograma
                                                     join r in entity.Resultado_Aprendizaje on c.IdCompetencia equals r.IdCompetencia
                                                     where r.IdResultado == item.IdResultado
                                                     select i).FirstOrDefault();
                                var DatosInstructor = (from i in entity.Instructor
                                                       where i.IdInstructor == item.IdInstructor
                                                       select i).FirstOrDefault();

                                Ficha.NombreInstructor = DatosInstructor.Nombre + " " + DatosInstructor.Apellido;
                                Ficha.Programa = DatosPrograma.NombrePrograma;
                                Ficha.Ambiente = DatosAmbiente.Numero;
                                Ficha.Id = item.Id;
                                Ficha.IdAmbiente = item.IdAmbiente;
                                Ficha.Ficha = DatosFicha.Ficha1;
                                Ficha.Competencia = DatosCompetencia.Resultado;
                                Ficha.IdInstructor = item.IdInstructor;
                                Ficha.FechaInicio = date;
                                Ficha.FechaFin = date;
                                Ficha.HoraInicio = item.HoraInicio;
                                Ficha.HoraFin = item.HoraFin;
                                Ficha.Color = item.Color;
                                Programacion.Add(Ficha);
                            }
                        }
                    }
                    if (item.Jornada == 2)
                    {
                        if (dateValue.DayOfWeek.ToString() == "Saturday")
                        {

                            Ficha_AmbienteDTO Ficha = new Ficha_AmbienteDTO();
                            var DatosFicha = (from i in entity.Ficha
                                              where i.IdFicha == item.IdFicha
                                              select i).FirstOrDefault();
                            var DatosCompetencia = (from i in entity.Resultado_Aprendizaje
                                                    where i.IdResultado == item.IdResultado
                                                    select i).FirstOrDefault();

                            var DatosAmbiente = (from i in entity.Ambiente
                                                 where i.IdAmbiente == item.IdAmbiente
                                                 select i).FirstOrDefault();
                            var DatosPrograma = (from i in entity.Programa
                                                 join c in entity.Competencia on i.IdPrograma equals c.IdPrograma
                                                 join r in entity.Resultado_Aprendizaje on c.IdCompetencia equals r.IdCompetencia
                                                 where r.IdResultado == item.IdResultado
                                                 select i).FirstOrDefault();
                            var DatosInstructor = (from i in entity.Instructor
                                                   where i.IdInstructor == item.IdInstructor
                                                   select i).FirstOrDefault();

                            Ficha.NombreInstructor = DatosInstructor.Nombre + " " + DatosInstructor.Apellido;
                            Ficha.Programa = DatosPrograma.NombrePrograma;
                            Ficha.Ambiente = DatosAmbiente.Numero;
                            Ficha.Id = item.Id;
                            Ficha.IdAmbiente = item.IdAmbiente;
                            Ficha.Ficha = DatosFicha.Ficha1;
                            Ficha.Competencia = DatosCompetencia.Resultado;
                            Ficha.IdInstructor = item.IdInstructor;
                            Ficha.FechaInicio = date;
                            Ficha.FechaFin = date;
                            Ficha.HoraInicio = item.HoraInicio;
                            Ficha.HoraFin = item.HoraFin;
                            Ficha.Color = item.Color;
                            Programacion.Add(Ficha);

                        }


                    }
                    if (item.Jornada == 3)
                    {
                        if (dateValue.DayOfWeek.ToString() == "Saturday")
                        {

                            Ficha_AmbienteDTO Ficha = new Ficha_AmbienteDTO();
                            var DatosFicha = (from i in entity.Ficha
                                              where i.IdFicha == item.IdFicha
                                              select i).FirstOrDefault();
                            var DatosCompetencia = (from i in entity.Resultado_Aprendizaje
                                                    where i.IdResultado == item.IdResultado
                                                    select i).FirstOrDefault();

                            var DatosAmbiente = (from i in entity.Ambiente
                                                 where i.IdAmbiente == item.IdAmbiente
                                                 select i).FirstOrDefault();
                            var DatosPrograma = (from i in entity.Programa
                                                 join c in entity.Competencia on i.IdPrograma equals c.IdPrograma
                                                 join r in entity.Resultado_Aprendizaje on c.IdCompetencia equals r.IdCompetencia
                                                 where r.IdResultado == item.IdResultado
                                                 select i).FirstOrDefault();
                            var DatosInstructor = (from i in entity.Instructor
                                                   where i.IdInstructor == item.IdInstructor
                                                   select i).FirstOrDefault();

                            Ficha.NombreInstructor = DatosInstructor.Nombre + " " + DatosInstructor.Apellido;
                            Ficha.Programa = DatosPrograma.NombrePrograma;
                            Ficha.Ambiente = DatosAmbiente.Numero;
                            Ficha.Id = item.Id;
                            Ficha.IdAmbiente = item.IdAmbiente;
                            Ficha.Ficha = DatosFicha.Ficha1;
                            Ficha.Competencia = DatosCompetencia.Resultado;
                            Ficha.IdInstructor = item.IdInstructor;
                            Ficha.FechaInicio = date;
                            Ficha.FechaFin = date;
                            Ficha.HoraInicio = item.HoraInicio;
                            Ficha.HoraFin = item.HoraFin;
                            Ficha.Color = item.Color;
                            Programacion.Add(Ficha);

                        }


                    }
                }
            }

            return Programacion;
        }

        public void GuardarProgramacionTransversal(Ficha_AmbienteDTO oFicha_AmbienteDTO)
        {
            Model1 entity = new Model1();
            Random r = new Random();
            Ficha_Ambiente oFicha_Ambiente = new Ficha_Ambiente();
            oFicha_Ambiente.Color = "#" + r.Next(000000, 999999).ToString();
            entity.Ficha_Ambiente.Add(oFicha_Ambiente);
            entity.SaveChanges();

        }

        public void EliminarProgramacion(Ficha_Ambiente oFicha_Ambiente)
        {
            Model1 entity = new Model1();


            var Datos = (from i in entity.Ficha_Ambiente
                         where i.Id == oFicha_Ambiente.Id
                         select i).FirstOrDefault();

            var DatosTrasversales = (from i in entity.Ficha_Ambiente
                                     where i.ProgramacionPrincipal == oFicha_Ambiente.Id
                                     select i).ToList();

            if (DatosTrasversales.Count != 0)
            {
                foreach (var item in DatosTrasversales)
                {
                    entity.Ficha_Ambiente.Remove(item);
                    entity.SaveChanges();
                }
            }

            entity.Ficha_Ambiente.Remove(Datos);
            entity.SaveChanges();


        }

        public List<Ficha_Ambiente> ConsultaEliminarProgramacionTransversales(Ficha_AmbienteDTO oFicha_Ambiente)
        {
            Model1 entity = new Model1();

            var DatosTrasversales = (from i in entity.Ficha_Ambiente
                                     where i.ProgramacionPrincipal == oFicha_Ambiente.Id
                                     select i).ToList();



            return DatosTrasversales;
        }

        public void AprobarSolicitud(int IdFIcha_Ambiente, bool estado)
        {
            Model1 entity = new Model1();

            var Item = (from i in entity.Ficha_Ambiente
                        where i.Id == IdFIcha_Ambiente
                        select i).First();
            //Item.IdArea = oFicha.IdArea;
            Item.Estado = estado;

            entity.SaveChanges();

            var solicitud = (from i in entity.Solicitud
                             where i.IdFicha_Ambiente == IdFIcha_Ambiente
                             select i).FirstOrDefault();
            var coordinador = (from i in entity.Coordinacion
                               where i.IdCoordinacion == solicitud.IdCoordinacion
                               select i).FirstOrDefault();

            var datos = (from i in entity.Ficha_Ambiente
                         where i.Id == IdFIcha_Ambiente
                         select i).FirstOrDefault();

            var ambiente = (from i in entity.Ambiente
                            where i.IdAmbiente == datos.IdAmbiente
                            select i).FirstOrDefault();
            var sede = (from i in entity.Sede
                        where i.IdSede == ambiente.IdSede
                        select i).FirstOrDefault();

            var Asunto = "Respuesta Solicitud de ambiente";
            var Cuerpo = "";
            if (estado == true)
            {
                Cuerpo = "Su solicitud de prestamo del ambiente N° " + ambiente.Numero + " de la sede " + sede.Nombre_Sede + " fue aprobada";
            }
            else
            {
                Cuerpo = "Su solicitud de prestamo del ambiente N° " + ambiente.Numero + " de la sede " + sede.Nombre_Sede + " no fue aprobada";
            }


            var ficha = (from f in entity.Programa
                         join i in entity.Ficha on f.IdPrograma equals i.IdPrograma
                         where i.IdFicha == Item.IdFicha
                         select f).FirstOrDefault();

            var instructor = (from i in entity.Instructor
                              where i.IdInstructor == Item.IdInstructor
                              select i).FirstOrDefault();

            var fichaasociada = (from i in entity.Ficha
                                 where i.IdFicha == Item.IdFicha
                                 select i).FirstOrDefault();

            var resultado = (from i in entity.Resultado_Aprendizaje
                             where i.IdResultado == Item.IdResultado
                             select i).FirstOrDefault();

            SendMail.SendMailMessage(Asunto, Cuerpo, coordinador.Correo);
            var Asunto1 = "Programación asociada";
            var Cuerpo1 = "Instrutor " + instructor.Nombre + " " + instructor.Apellido + ", se le ha asociado la siguiente programacion"
                          + "<br/>Ficha:" + fichaasociada.Ficha1 + "<br>Ambiente:" + ambiente.Numero +
                          "<br>Resultado de aprendizaje: " + resultado.Codigo + " - " + resultado.Resultado;
            SendMail.SendMailMessage(Asunto1, Cuerpo1, instructor.Email);

            if (estado == false)
            {
                entity.Ficha_Ambiente.Remove(Item);
                entity.SaveChanges();
            }

        }

        public Ficha_AmbienteDTO ConsultarAmbienteSolicitado(int IdFIcha_Ambiente)
        {
            Model1 entity = new Model1();

            var programacion = (from p in entity.Ficha_Ambiente
                                where p.Id == IdFIcha_Ambiente
                                select p).FirstOrDefault();

            var ambiente = (from a in entity.Ambiente
                            where a.IdAmbiente == programacion.IdAmbiente
                            select a).FirstOrDefault();

            var sede = (from s in entity.Sede
                        where s.IdSede == ambiente.IdSede
                        select s).FirstOrDefault();

            Ficha_AmbienteDTO oFicha_AmbienteDTO = new Ficha_AmbienteDTO();

            oFicha_AmbienteDTO.Ambiente = ambiente.Numero;
            oFicha_AmbienteDTO.Sede = sede.Nombre_Sede;
            oFicha_AmbienteDTO.FechaInicio = programacion.FechaInicio;
            oFicha_AmbienteDTO.FechaFin = programacion.FechaFin;
            oFicha_AmbienteDTO.HoraInicio = programacion.HoraInicio;
            oFicha_AmbienteDTO.HoraFin = programacion.HoraFin;

            return oFicha_AmbienteDTO;
        }

        public string ValidarDias(string Dias, string horaIni, string horaFIn)
        {
            string DiasOcupado = "";
            var verificar = DiasOcupado.Contains(Dias);
            if (verificar == false)
            {
                DiasOcupado += " " + Dias + " de: \r" + horaIni.Substring(0, 5) + " - " + horaFIn.Substring(0, 5) + "\r. ";
            }
            else
            {
                DiasOcupado += " , " + horaIni.Substring(0, 5) + "-" + horaIni.Substring(0, 5);
            }

            return DiasOcupado;
        }

        public Tuple<bool, string, Ficha_Ambiente> ConsultarTransversales(int IdProgramacion)
        {
            Model1 entity = new Model1();

            var Datos = (from i in entity.Ficha_Ambiente
                         where i.ProgramacionPrincipal == IdProgramacion
                         orderby i.Lunes, i.Martes, i.Miercoles, i.Jueves, i.Viernes, i.Jornada, i.HoraInicio ascending
                         select i).ToList();

            var Programacion = (from i in entity.Ficha_Ambiente
                                where i.Id == IdProgramacion
                                select i).FirstOrDefault();

            string Dias = "";
            string DiasOcupado = "";

            foreach (var item in Datos)
            {

                if (item.Transversal == true)
                {
                    if (item.Lunes == true)
                    {
                        Dias = "Lunes";

                        DiasOcupado += ValidarDias(Dias, item.HoraInicio.ToString(), item.HoraFin.ToString());
                    }
                    if (item.Martes == true)
                    {
                        Dias = "Martes";
                        DiasOcupado += ValidarDias(Dias, item.HoraInicio.ToString(), item.HoraFin.ToString());
                    }
                    if (item.Miercoles == true)
                    {
                        Dias = "Miércoles";
                        DiasOcupado += ValidarDias(Dias, item.HoraInicio.ToString(), item.HoraFin.ToString());
                    }
                    if (item.Jueves == true)
                    {
                        Dias = "Jueves";
                        DiasOcupado += ValidarDias(Dias, item.HoraInicio.ToString(), item.HoraFin.ToString());
                    }
                    if (item.Viernes == true)
                    {
                        Dias = "Viernes";
                        DiasOcupado += ValidarDias(Dias, item.HoraInicio.ToString(), item.HoraFin.ToString());
                    }

                    if (item.Jornada == 2)
                    {
                        Dias = "Sábado";
                        DiasOcupado += ValidarDias(Dias, item.HoraInicio.ToString(), item.HoraFin.ToString());
                    }
                    if (item.Jornada == 3)
                    {
                        Dias = "Domingo";
                        DiasOcupado += ValidarDias(Dias, item.HoraInicio.ToString(), item.HoraFin.ToString());
                    }
                }


            }

            if (Datos.Count > 0)
            {
                return new Tuple<bool, string, Ficha_Ambiente>(true, DiasOcupado, Programacion);
            }
            else
            {
                return new Tuple<bool, string, Ficha_Ambiente>(false, null, Programacion);
            }
        }

        public Tuple<int, int?> ConsultarDiaTransversal(DateTime FechaInicio, DateTime FechaFin, string HoraInicio, string HoraFin, int ficha)
        {
            Model1 entity = new Model1();
            int respuesta = 0;

            var Programacion = entity.Database.SqlQuery<Ficha_Ambiente>("sp_ConsultarDiaTransversalFicha @fecha_ini, @fecha_Fin, @ficha",
                                                             new SqlParameter("fecha_ini", FechaInicio),
                                                             new SqlParameter("fecha_Fin", FechaFin),
                                                             new SqlParameter("ficha", ficha)).ToList();

            var DiaSemana = Programacion.Where(x => x.DiaSemana != null).FirstOrDefault();

            if (DiaSemana == null)
            {
                respuesta = 1;
                return new Tuple<int, int?>(respuesta, null);
            }
            respuesta = 2;
            return new Tuple<int, int?>(respuesta, DiaSemana.DiaSemana);
        }

        public List<Instructor> ConsultarInstructorTransversal(DateTime FechaInicio, DateTime FechaFin, string HoraInicio, string HoraFin, int? DiaSemana)
        {
            Model1 entity = new Model1();
            List<Instructor> DisponibilidadInstructor = new List<Instructor>();
            List<Instructor> Instructor = new List<Instructor>();

            if (DiaSemana != null)
            {
                DisponibilidadInstructor = entity.Database.SqlQuery<Instructor>("sp_disponibilidadInstructorTransversalSinTranslape @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @diasemana",
                                                                                            new SqlParameter("fecha_ini", FechaInicio),
                                                                                            new SqlParameter("fecha_Fin", FechaFin),
                                                                                            new SqlParameter("HoraInicio", HoraInicio),
                                                                                            new SqlParameter("HoraFin", HoraFin),
                                                                                            new SqlParameter("diasemana", DiaSemana)).ToList();
            }


            Instructor = (from i in entity.Instructor
                          where i.Estado == true
                          && i.TipoInstructor == 2
                          select i).ToList();

            List<Instructor> oListaInstructor = new List<Instructor>();

            if (DisponibilidadInstructor.Count == 0)
            {
                oListaInstructor.AddRange(Instructor);
            }
            else
            {
                var Ins = (from i in Instructor
                           where !(from o in DisponibilidadInstructor
                                   select o.IdInstructor)
                                   .Contains(i.IdInstructor)
                           select i).ToList();
                oListaInstructor = Ins;
            }

            return oListaInstructor;
        }

        public List<Ficha_AmbienteDTO> GenerarReporte(DateTime fechaInicio, DateTime fechaFin, int IdCoordinador)
        {
            Model1 entity = new Model1();


            List<Ficha_Ambiente> Datos = new List<Ficha_Ambiente>();

            var coordinador = (from i in entity.Coordinacion
                               where i.IdCoordinacion == IdCoordinador
                               select i).FirstOrDefault();

            //if (IdCoordinador != 0) Esta consulta es para filtrar el reporte para los coordinadores
            //{
            //Datos = (from i in entity.Ficha_Ambiente
            //         join a in entity.Ambiente on i.IdAmbiente equals a.IdAmbiente
            //         join r in entity.Resultado_Aprendizaje on i.IdResultado equals r.IdResultado
            //         join c in entity.Competencia on r.IdCompetencia equals c.IdCompetencia
            //         join p in entity.Programa on c.IdPrograma equals p.IdPrograma
            //         where p.IdArea == coordinador.IdArea && i.IdAmbiente != 192 &&
            //         ((i.FechaInicio >= fechaInicio && i.FechaInicio <= fechaFin) ||
            //         (i.FechaFin <= fechaFin && i.FechaFin >= fechaInicio)
            //         || (i.FechaInicio <= fechaInicio && i.FechaFin >= fechaFin))
            //         select i).ToList();
            //}
            //else
            //{
            Datos = (from i in entity.Ficha_Ambiente
                     where (i.FechaInicio >= fechaInicio && i.FechaInicio <= fechaFin) ||
                     (i.FechaFin <= fechaFin && i.FechaFin >= fechaInicio) ||
                     (i.FechaInicio <= fechaInicio && i.FechaFin >= fechaFin)
                     select i).ToList();
            //}


            List<Ficha_AmbienteDTO> Programacion = new List<Ficha_AmbienteDTO>();

            var Dias = "";
            foreach (var item in Datos)
            {
                Ficha_AmbienteDTO Ficha = new Ficha_AmbienteDTO();
                var DatosFicha = (from i in entity.Ficha
                                  where i.IdFicha == item.IdFicha
                                  select i).FirstOrDefault();

                var Competencia = (from i in entity.Competencia
                                   join r in entity.Resultado_Aprendizaje on i.IdCompetencia equals r.IdCompetencia
                                   where r.IdResultado == item.IdResultado
                                   select i).FirstOrDefault();

                var DatosCompetencia = (from i in entity.Resultado_Aprendizaje
                                        where i.IdResultado == item.IdResultado
                                        select i).FirstOrDefault();

                var DatosAmbiente = (from i in entity.Ambiente
                                     where i.IdAmbiente == item.IdAmbiente
                                     select i).FirstOrDefault();

                var DatosPrograma = (from i in entity.Programa
                                     join c in entity.Competencia on i.IdPrograma equals c.IdPrograma
                                     join r in entity.Resultado_Aprendizaje on c.IdCompetencia equals r.IdCompetencia
                                     where r.IdResultado == item.IdResultado
                                     select i).FirstOrDefault();

                var DatosInstructor = (from i in entity.Instructor
                                       where i.IdInstructor == item.IdInstructor
                                       select i).FirstOrDefault();

                var DatosSede = (from s in entity.Sede
                                 where s.IdSede == DatosAmbiente.IdSede
                                 select s).FirstOrDefault();

                var SedeEmpresa = (from s in entity.Sede
                                   where s.TipoSede == 2
                                   select s).FirstOrDefault();

                var area = (from a in entity.Area
                            join p in entity.Programa on a.IdArea equals p.IdArea
                            where p.IdPrograma == DatosFicha.IdPrograma
                            //&& a.IdArea == p.IdArea
                            select a).FirstOrDefault();



                Ficha.NombreInstructor = DatosInstructor.Nombre + " " + DatosInstructor.Apellido;
                Ficha.CedulaInstructor = DatosInstructor.Cedula;
                Ficha.Programa = DatosPrograma.NombrePrograma;
                Ficha.CodigoPrograma = DatosPrograma.CodigoPrograma;
                Ficha.VersionPrograma = DatosPrograma.Version_Programa;
                Ficha.NivelPrograma = DatosPrograma.Nivel;
                Ficha.Ambiente = DatosAmbiente.Numero;
                Ficha.Id = item.Id;
                Ficha.IdAmbiente = int.Parse(DatosAmbiente.Numero);
                Ficha.Ficha = DatosFicha.Ficha1;
                Ficha.Resultado = DatosCompetencia.Resultado;
                Ficha.IdInstructor = item.IdInstructor;
                Ficha.FechaInicio = item.FechaInicio;
                Ficha.FechaFin = item.FechaFin;
                Ficha.HoraInicio = item.HoraInicio;
                Ficha.HoraFin = item.HoraFin;
                Ficha.Color = item.Color;
                Ficha.Competencia = Competencia.Nombre;
                Ficha.TotalHoras = item.HoraFin - item.HoraInicio;
                Ficha.Area = area.Nombre;
                Ficha.Sede = DatosSede.Nombre_Sede;
                Ficha.NombreEvento = "";
                if (item.NombreEmpresa != "")
                {
                    if (DatosSede.IdSede == SedeEmpresa.IdSede)
                    {
                        Ficha.Sede = DatosSede.Nombre_Sede + " " + item.NombreEmpresa.ToUpper();
                    }
                    else
                    {
                        Ficha.NombreEvento = item.NombreEmpresa;
                    }
                }



                if (item.Lunes != null)
                {
                    Ficha.Jornada += "Lunes -";
                }
                if (item.Martes != null)
                {
                    Ficha.Jornada += "Martes -";
                }
                if (item.Miercoles != null)
                {
                    Ficha.Jornada += "Miércoles -";
                }
                if (item.Jueves != null)
                {
                    Ficha.Jornada += "Jueves -";
                }
                if (item.Viernes != null)
                {
                    Ficha.Jornada += "Viernes -";
                }

                if (item.Jornada == 2)
                {
                    Ficha.Jornada = "Sabado";
                };

                if (item.Jornada == 3)
                {
                    Ficha.Jornada = "Domingo";
                };


                Programacion.Add(Ficha);
            }

            return Programacion;
        }

        public void EnviarProgramacion(List<Ficha_AmbienteDTO> oFicha_Ambiente)
        {
            Model1 entity = new Model1();

            foreach (var item in oFicha_Ambiente)
            {
                var instructor = (from i in entity.Instructor
                                  where i.IdInstructor == item.IdInstructor
                                  select i).FirstOrDefault();

                var Asunto = "Nueva Programación";
                var Plantilla = "Fecha Inicio: " + item.FechaInicio + "<br/> Fecha Fin: " + item.FechaFin + "<br/> Hora Inicio: " + item.HoraInicio
                                + "<br/> Hora Fin: " + item.HoraFin + "<br/> Ambiente Número: " + item.Ambiente + "<br/> Ficha: " + item.Ficha
                                + "<br/> Resultado de aprendizaje: " + item.Resultado;
                SendMail.SendMailMessage(Asunto, Plantilla, instructor.Email);


            }


        }

        public List<Ambiente> ConsultarAmbientesxSede(int IdSede)
        {

            Model1 entity = new Model1();
            var Datos = (from i in entity.Ambiente
                         where i.IdSede == IdSede
                         select i).ToList();

            return Datos;
        }

        public List<Ficha_AmbienteDTO> ConsultarPogramacionesInstructor(string CedulaInstructor)
        {
            Model1 entity = new Model1();
            var FechaActual = DateTime.Now;
            var Lunes = false;
            var Martes = false;
            var Miercoles = false;
            var Jueves = false;
            var Viernes = false;
            var Sabado = 0;
            var Domingo = 0;

            var DiaSemana = FechaActual.DayOfWeek;

            switch (DiaSemana.ToString())
            {
                case "Monday":
                    Lunes = true;
                    break;

                case "Tuesday":
                    Martes = true;
                    break;

                case "Wednesday":
                    Miercoles = true;
                    break;
                case "Thursday":
                    Jueves = true;
                    break;
                case "Friday":
                    Viernes = true;
                    break;
                case "Saturday":
                    Sabado = 2;
                    break;
                case "Sunday":
                    Domingo = 3;
                    break;
            }

            var fecha = DateTime.Parse((DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()).ToString());
            var instructor = (from i in entity.Instructor
                              where i.Cedula == CedulaInstructor
                              select i).FirstOrDefault();



            //var Programciones1 = (from i in entity.Ficha_Ambiente
            //                      join p in entity.PrestamoLlaves on i.Id equals p.IdFicha_Ambiente
            //                      where p.Fecha != fecha && i.IdInstructor == instructor.IdInstructor
            //                      select i).ToList();

            var Programaciones = (from i in entity.Ficha_Ambiente
                                      //join p in entity.PrestamoLlaves on i.Id equals p.IdFicha_Ambiente
                                  join t in entity.Instructor on i.IdInstructor equals t.IdInstructor
                                  join f in entity.Ficha on i.IdFicha equals f.IdFicha
                                  join a in entity.Ambiente on i.IdAmbiente equals a.IdAmbiente
                                  join r in entity.Resultado_Aprendizaje on i.IdResultado equals r.IdResultado
                                  join c in entity.Competencia on r.IdCompetencia equals c.IdCompetencia
                                  where i.IdInstructor == instructor.IdInstructor
                                  && (i.FechaInicio.Year == FechaActual.Year && i.FechaFin.Year <= FechaActual.Year)
                                  && (i.Lunes == Lunes || i.Martes == Martes || i.Miercoles == Miercoles || i.Jueves == Jueves || i.Viernes == Viernes || i.Jornada == Sabado || i.Jornada == Domingo)
                                  select new Ficha_AmbienteDTO
                                  {
                                      Id = i.Id,
                                      IdFicha = i.IdFicha,
                                      Ficha = f.Ficha1,
                                      IdAmbiente = i.IdAmbiente,
                                      Ambiente = a.Numero,
                                      IdInstructor = i.IdInstructor,
                                      CedulaIns = t.Cedula,
                                      NombreInstructor = t.Nombre + " " + t.Apellido,
                                      Resultado = r.Resultado,
                                      CodigoResultado = r.Codigo,
                                      Competencia = c.Nombre,
                                      CodigoCompetencia = c.Codigo.ToString(),
                                      FechaInicio = i.FechaInicio,
                                      FechaFin = i.FechaFin,
                                      HoraInicio = i.HoraInicio,
                                      HoraFin = i.HoraFin,
                                      Color = i.Color,
                                      Lunes = i.Lunes,
                                      Martes = i.Martes,
                                      Miercoles = i.Miercoles,
                                      Jueves = i.Jueves,
                                      Viernes = i.Viernes

                                  }).ToList();

            //Clonar lista
            List<Ficha_AmbienteDTO> newList = Programaciones.GetRange(0, Programaciones.Count);

            TimeSpan Hora = TimeSpan.FromHours(FechaActual.Hour) + TimeSpan.FromMinutes(FechaActual.Minute);
            string fromTimeString = Hora.ToString("hh':'mm");

            //foreach (var item in newList)
            //{
            //     //|| !(Hora >= item.HoraInicio && Hora <= item.HoraFin)
            //    if (!(FechaActual >= item.FechaInicio && FechaActual <= item.FechaFin))
            //    {
            //        Programaciones.Remove(item);
            //    }

            //    //var ambiente = (from i in entity.PrestamoLlaves
            //    //                join f in entity.Ficha_Ambiente on i.IdFicha_Ambiente equals f.Id
            //    //                where f.Id== item.Id && f.IdAmbiente == item.IdAmbiente && i.Fecha == fecha
            //    //                && (Hora >= f.HoraInicio && Hora <= f.HoraFin)
            //    //                select f.IdAmbiente).FirstOrDefault();

            //    //if (ambiente > 0)
            //    //{
            //    //    Programaciones.Remove(item);
            //    //}

            //}
            foreach (var item in Programaciones.Select((value, i) => new { i, value }))
            {
                var FechaPrestamo = (from i in entity.PrestamoLlaves
                                     where i.IdFicha_Ambiente == item.value.Id && i.Fecha == fecha
                                     select i).FirstOrDefault();
                if (FechaPrestamo != null)
                {
                    Programaciones[item.i].EntregoLLaves = FechaPrestamo.Entrego;
                    Programaciones[item.i].RecibioLLaves = FechaPrestamo.Recibio;
                }
                else
                {
                    Programaciones[item.i].EntregoLLaves = false;
                    Programaciones[item.i].RecibioLLaves = false;
                }


                if (item.value.Lunes == true)
                {
                    Programaciones[item.i].DiasProgramados += "Lunes -";
                }
                if (item.value.Martes == true)
                {
                    Programaciones[item.i].DiasProgramados += "Martes -";
                }
                if (item.value.Miercoles == true)
                {
                    Programaciones[item.i].DiasProgramados += "Miercoles -";
                }
                if (item.value.Jueves == true)
                {
                    Programaciones[item.i].DiasProgramados += "Jueves -";
                }
                if (item.value.Viernes == true)
                {
                    Programaciones[item.i].DiasProgramados += "Viernes -";
                }
                if (item.value.Jornada == "2")
                {
                    Programaciones[item.i].DiasProgramados += "Sábado -";
                }
                if (item.value.Jornada == "3")
                {
                    Programaciones[item.i].DiasProgramados += "Domingo -";
                }
            }
            return Programaciones;
        }

        public List<Ficha_AmbienteDTO> GuardarPrestamoLLaves(Ficha_AmbienteDTO oProgramacion)
        {
            Model1 entity = new Model1();
            var oPrestamo = new PrestamoLlaves();
            var FechaActual = DateTime.Now;
            var fecha = DateTime.Parse((DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()).ToString());
            TimeSpan Hora = TimeSpan.FromHours(FechaActual.Hour) + TimeSpan.FromMinutes(FechaActual.Minute);
            string fromTimeString = Hora.ToString("hh':'mm");



            var prestamo = (from i in entity.PrestamoLlaves
                            where i.IdFicha_Ambiente == oProgramacion.Id && i.Fecha == fecha
                            select i).FirstOrDefault();

            if (Hora < TimeSpan.Parse("22:59"))
            {
                if (oProgramacion.opc == 1)
                {
                    var ambiente = (from p in entity.PrestamoLlaves
                                    join f in entity.Ficha_Ambiente on p.IdFicha_Ambiente equals f.Id
                                    where f.IdAmbiente == oProgramacion.IdAmbiente && p.Fecha == fecha && p.Entrego == false
                                    select p).ToList();
                    if (ambiente.Count > 0)
                    {
                        var IdMayor = ambiente.Max(x => x.Id);
                        var ambienteactualizar = entity.PrestamoLlaves.Where(x => x.Id == IdMayor).FirstOrDefault();
                        ambienteactualizar.Entrego = true;
                        ambienteactualizar.HoraEntrego = Hora;
                        entity.SaveChanges();
                    }
                }

                if (prestamo == null)
                {
                    if (oProgramacion.RecibioLLaves)
                    {
                        oPrestamo.Recibio = true;
                        oPrestamo.Entrego = false;
                        oPrestamo.HoraRecibio = Hora;
                        oPrestamo.HoraEntrego = TimeSpan.Parse("00:00");
                        oPrestamo.Observacion = oProgramacion.Observacion;
                        oPrestamo.IdFicha_Ambiente = oProgramacion.Id;
                        oPrestamo.Fecha = DateTime.Now;

                        entity.PrestamoLlaves.Add(oPrestamo);
                        entity.SaveChanges();
                    }

                }
                else
                {
                    if (prestamo.Recibio == true && prestamo.Entrego == true)
                    {
                        if (prestamo.HoraRecibio2 == null)
                        {
                            prestamo.HoraRecibio2 = Hora;
                            prestamo.HoraEntrego2 = TimeSpan.Parse("00:00");
                            prestamo.Observacion = oProgramacion.Observacion;
                            entity.SaveChanges();
                            return ConsultarLLavesEditar();
                        }
                        else
                        {
                            prestamo.HoraEntrego2 = Hora;
                            entity.SaveChanges();

                            return ConsultarLLavesEditar();
                        }

                    }
                    else
                    {
                        prestamo.Entrego = true;
                        prestamo.HoraEntrego = Hora;
                        entity.SaveChanges();
                    }


                }
            }
            else
            {
                //var prestamos = AmbientesDisponibles();
                //    oPrestamo.Recibio = false;
                //    oPrestamo.Entrego = false;
                //    oPrestamo.HoraRecibio = TimeSpan.Parse("00:00");
                //    oPrestamo.HoraEntrego = TimeSpan.Parse("00:00");
                //    oPrestamo.Observacion = oProgramacion.Observacion;
                //    oPrestamo.IdFicha_Ambiente = item.Id;
                //    oPrestamo.Fecha = DateTime.Now;

                //    entity.PrestamoLlaves.Add(oPrestamo);
                //    entity.SaveChanges();

            }


            var user = (from i in entity.Ficha_Ambiente
                        join a in entity.Ambiente on i.IdAmbiente equals a.IdAmbiente
                        join s in entity.Sede on a.IdSede equals s.IdSede
                        join u in entity.Usuario on s.IdUsuario equals u.IdUsuario
                        where i.Id == oProgramacion.Id
                        select u).FirstOrDefault();

            var Programaciones = RegresarLlavesAmbientesDisponibles(user.IdUsuario);

            if (oProgramacion.CedulaIns != "" && oProgramacion.CedulaIns != null)
            {
                var inst = (from i in entity.Instructor
                            where i.IdInstructor == oProgramacion.IdInstructor
                            select i).FirstOrDefault();

                Programaciones = ConsultarPogramacionesInstructor(inst.Cedula);
            }


            return Programaciones;

        }

        public List<Ficha_AmbienteDTO> EliminarReciboLlaves(Ficha_AmbienteDTO oProgramcion)
        {
            Model1 entity = new Model1();
            var fecha = DateTime.Parse((DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()).ToString());
            var programacion = (from i in entity.PrestamoLlaves
                                where i.IdFicha_Ambiente == oProgramcion.Id && i.Fecha == fecha
                                select i).FirstOrDefault();

            var user = (from i in entity.Ficha_Ambiente
                            join a in entity.Ambiente on i.IdAmbiente equals a.IdAmbiente
                            join s in entity.Sede on a.IdSede equals s.IdSede
                            join u in entity.Usuario on s.IdUsuario equals u.IdUsuario
                            where i.Id == programacion.IdFicha_Ambiente
                            select u).FirstOrDefault();
            if (programacion != null)
            {
                entity.PrestamoLlaves.Remove(programacion);
                entity.SaveChanges();
            }
       
            return RegresarLlavesAmbientesDisponibles(user.IdUsuario);
        }

        public List<Ficha_AmbienteDTO> AmbientesDisponibles(int IdUser)
        {
            Model1 entity = new Model1();
            var FechaActual = DateTime.Now;
            var Lunes = false;
            var Martes = false;
            var Miercoles = false;
            var Jueves = false;
            var Viernes = false;
            var Sabado = 0;
            var Domingo = 0;

            var DiaSemana = FechaActual.DayOfWeek;

            switch (DiaSemana.ToString())
            {
                case "Monday":
                    Lunes = true;
                    break;

                case "Tuesday":
                    Martes = true;
                    break;

                case "Wednesday":
                    Miercoles = true;
                    break;
                case "Thursday":
                    Jueves = true;
                    break;
                case "Friday":
                    Viernes = true;
                    break;
                case "Saturday":
                    Sabado = 2;
                    break;
                case "Sunday":
                    Domingo = 3;
                    break;
            }

            var fecha = DateTime.Parse((DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()).ToString());
            var hora = (TimeSpan.Parse("00:00"));

            var sede = (from i in entity.Sede
                            where i.IdUsuario== IdUser
                            select i).FirstOrDefault();

            var Programaciones = (from i in entity.Ficha_Ambiente
                                      //join p in entity.PrestamoLlaves on i.Id equals p.IdFicha_Ambiente
                                  join t in entity.Instructor on i.IdInstructor equals t.IdInstructor
                                  join f in entity.Ficha on i.IdFicha equals f.IdFicha
                                  join a in entity.Ambiente on i.IdAmbiente equals a.IdAmbiente
                                  join r in entity.Resultado_Aprendizaje on i.IdResultado equals r.IdResultado
                                  join c in entity.Competencia on r.IdCompetencia equals c.IdCompetencia
                                  where a.IdSede== sede.IdSede && (i.FechaInicio.Year == FechaActual.Year && i.FechaFin.Year <= FechaActual.Year)
                                  && (i.Lunes == Lunes || i.Martes == Martes || i.Miercoles == Miercoles || i.Jueves == Jueves || i.Viernes == Viernes || i.Jornada == Sabado || i.Jornada == Domingo)
                                  && !(from o in entity.PrestamoLlaves
                                       where o.Fecha == fecha
                                       select o.IdFicha_Ambiente).Contains(i.Id)
                                  select new Ficha_AmbienteDTO
                                  {
                                      Id = i.Id,
                                      IdFicha = i.IdFicha,
                                      Ficha = f.Ficha1,
                                      IdAmbiente = i.IdAmbiente,
                                      Ambiente = a.Numero,
                                      IdInstructor = i.IdInstructor,
                                      CedulaIns = t.Cedula,
                                      NombreInstructor = t.Nombre + " " + t.Apellido,
                                      Resultado = r.Resultado,
                                      CodigoResultado = r.Codigo,
                                      Competencia = c.Nombre,
                                      CodigoCompetencia = c.Codigo.ToString(),
                                      FechaInicio = i.FechaInicio,
                                      FechaFin = i.FechaFin,
                                      HoraInicio = i.HoraInicio,
                                      HoraFin = i.HoraFin,
                                      Color = i.Color,
                                      Lunes = i.Lunes,
                                      Martes = i.Martes,
                                      Miercoles = i.Miercoles,
                                      Jueves = i.Jueves,
                                      Viernes = i.Viernes,
                                      Jornada = i.Jornada.ToString()
                                  }).ToList();

            //Clonar lista
            List<Ficha_AmbienteDTO> newList = Programaciones.GetRange(0, Programaciones.Count);

            Programaciones = (from i in Programaciones
                              where
                               (i.FechaInicio >= fecha && i.FechaInicio <= fecha)
                              || (i.FechaFin <= fecha && i.FechaFin >= fecha)
                              || (i.FechaInicio <= fecha && i.FechaFin >= fecha)
                              select i).ToList();

            //(f.FechaInicio between @vardate and @vardate ) or
            //(f.FechaFin between @vardate and @vardate) or
            //(f.FechaInicio <= @vardate and f.FechaFin >= @vardate))

            //Programaciones = (from i in Programaciones
            //                  where !(from o in Programaciones1

            //                          select o.Id).Contains(i.Id)
            //                  select i).ToList();

            TimeSpan Hora = TimeSpan.FromHours(FechaActual.Hour) + TimeSpan.FromMinutes(FechaActual.Minute);
            string fromTimeString = Hora.ToString("hh':'mm");


            foreach (var item in Programaciones.Select((value, i) => new { i, value }))
            {
                var FechaPrestamo = (from i in entity.PrestamoLlaves
                                     where i.IdFicha_Ambiente == item.value.Id && i.Fecha == fecha
                                     select i).FirstOrDefault();
                if (FechaPrestamo != null)
                {
                    Programaciones[item.i].EntregoLLaves = FechaPrestamo.Entrego;
                    Programaciones[item.i].RecibioLLaves = FechaPrestamo.Recibio;
                }
                else
                {
                    Programaciones[item.i].EntregoLLaves = false;
                    Programaciones[item.i].RecibioLLaves = false;
                }

                if (item.value.Lunes == true)
                {
                    Programaciones[item.i].DiasProgramados += "Lunes -";
                }
                if (item.value.Martes == true)
                {
                    Programaciones[item.i].DiasProgramados += "Martes -";
                }
                if (item.value.Miercoles == true)
                {
                    Programaciones[item.i].DiasProgramados += "Miercoles -";
                }
                if (item.value.Jueves == true)
                {
                    Programaciones[item.i].DiasProgramados += "Jueves -";
                }
                if (item.value.Viernes == true)
                {
                    Programaciones[item.i].DiasProgramados += "Viernes -";
                }
                if (item.value.Jornada == "2")
                {
                    Programaciones[item.i].DiasProgramados += "Sábado -";
                }
                if (item.value.Jornada == "3")
                {
                    Programaciones[item.i].DiasProgramados += "Domingo -";
                }
            }
            //return Programaciones.OrderBy(x => x.FechaFin).ToList();
            return Programaciones.OrderBy(x => int.Parse(x.Ambiente)).ToList().OrderBy((x => x.HoraInicio)).ToList();
        }

        public List<Ficha_AmbienteDTO> AmbientesDisponibles()
        {
            Model1 entity = new Model1();
            var FechaActual = DateTime.Now;
            var Lunes = false;
            var Martes = false;
            var Miercoles = false;
            var Jueves = false;
            var Viernes = false;
            var Sabado = 0;
            var Domingo = 0;

            var DiaSemana = FechaActual.DayOfWeek;

            switch (DiaSemana.ToString())
            {
                case "Monday":
                    Lunes = true;
                    break;

                case "Tuesday":
                    Martes = true;
                    break;

                case "Wednesday":
                    Miercoles = true;
                    break;
                case "Thursday":
                    Jueves = true;
                    break;
                case "Friday":
                    Viernes = true;
                    break;
                case "Saturday":
                    Sabado = 2;
                    break;
                case "Sunday":
                    Domingo = 3;
                    break;
            }

            var fecha = DateTime.Parse((DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()).ToString());
            var hora = (TimeSpan.Parse("00:00"));

            //var sede = (from i in entity.Sede
            //            where i.IdUsuario == IdUser
            //            select i).FirstOrDefault();

            var Programaciones = (from i in entity.Ficha_Ambiente
                                      //join p in entity.PrestamoLlaves on i.Id equals p.IdFicha_Ambiente
                                  join t in entity.Instructor on i.IdInstructor equals t.IdInstructor
                                  join f in entity.Ficha on i.IdFicha equals f.IdFicha
                                  join a in entity.Ambiente on i.IdAmbiente equals a.IdAmbiente
                                  join r in entity.Resultado_Aprendizaje on i.IdResultado equals r.IdResultado
                                  join c in entity.Competencia on r.IdCompetencia equals c.IdCompetencia
                                  where (i.FechaInicio.Year == FechaActual.Year && i.FechaFin.Year <= FechaActual.Year)
                                  && (i.Lunes == Lunes || i.Martes == Martes || i.Miercoles == Miercoles || i.Jueves == Jueves || i.Viernes == Viernes || i.Jornada == Sabado || i.Jornada == Domingo)
                                  && !(from o in entity.PrestamoLlaves
                                       where o.Fecha == fecha
                                       select o.IdFicha_Ambiente).Contains(i.Id)
                                  select new Ficha_AmbienteDTO
                                  {
                                      Id = i.Id,
                                      IdFicha = i.IdFicha,
                                      Ficha = f.Ficha1,
                                      IdAmbiente = i.IdAmbiente,
                                      Ambiente = a.Numero,
                                      IdInstructor = i.IdInstructor,
                                      CedulaIns = t.Cedula,
                                      NombreInstructor = t.Nombre + " " + t.Apellido,
                                      Resultado = r.Resultado,
                                      CodigoResultado = r.Codigo,
                                      Competencia = c.Nombre,
                                      CodigoCompetencia = c.Codigo.ToString(),
                                      FechaInicio = i.FechaInicio,
                                      FechaFin = i.FechaFin,
                                      HoraInicio = i.HoraInicio,
                                      HoraFin = i.HoraFin,
                                      Color = i.Color,
                                      Lunes = i.Lunes,
                                      Martes = i.Martes,
                                      Miercoles = i.Miercoles,
                                      Jueves = i.Jueves,
                                      Viernes = i.Viernes,
                                      Jornada = i.Jornada.ToString()
                                  }).ToList();

            //Clonar lista
            List<Ficha_AmbienteDTO> newList = Programaciones.GetRange(0, Programaciones.Count);

            Programaciones = (from i in Programaciones
                              where
                               (i.FechaInicio >= fecha && i.FechaInicio <= fecha)
                              || (i.FechaFin <= fecha && i.FechaFin >= fecha)
                              || (i.FechaInicio <= fecha && i.FechaFin >= fecha)
                              select i).ToList();

            //(f.FechaInicio between @vardate and @vardate ) or
            //(f.FechaFin between @vardate and @vardate) or
            //(f.FechaInicio <= @vardate and f.FechaFin >= @vardate))

            //Programaciones = (from i in Programaciones
            //                  where !(from o in Programaciones1

            //                          select o.Id).Contains(i.Id)
            //                  select i).ToList();

            TimeSpan Hora = TimeSpan.FromHours(FechaActual.Hour) + TimeSpan.FromMinutes(FechaActual.Minute);
            string fromTimeString = Hora.ToString("hh':'mm");


            foreach (var item in Programaciones.Select((value, i) => new { i, value }))
            {
                var FechaPrestamo = (from i in entity.PrestamoLlaves
                                     where i.IdFicha_Ambiente == item.value.Id && i.Fecha == fecha
                                     select i).FirstOrDefault();
                if (FechaPrestamo != null)
                {
                    Programaciones[item.i].EntregoLLaves = FechaPrestamo.Entrego;
                    Programaciones[item.i].RecibioLLaves = FechaPrestamo.Recibio;
                }
                else
                {
                    Programaciones[item.i].EntregoLLaves = false;
                    Programaciones[item.i].RecibioLLaves = false;
                }

                if (item.value.Lunes == true)
                {
                    Programaciones[item.i].DiasProgramados += "Lunes -";
                }
                if (item.value.Martes == true)
                {
                    Programaciones[item.i].DiasProgramados += "Martes -";
                }
                if (item.value.Miercoles == true)
                {
                    Programaciones[item.i].DiasProgramados += "Miercoles -";
                }
                if (item.value.Jueves == true)
                {
                    Programaciones[item.i].DiasProgramados += "Jueves -";
                }
                if (item.value.Viernes == true)
                {
                    Programaciones[item.i].DiasProgramados += "Viernes -";
                }
                if (item.value.Jornada == "2")
                {
                    Programaciones[item.i].DiasProgramados += "Sábado -";
                }
                if (item.value.Jornada == "3")
                {
                    Programaciones[item.i].DiasProgramados += "Domingo -";
                }
            }
            //return Programaciones.OrderBy(x => x.FechaFin).ToList();
            return Programaciones.OrderBy(x => int.Parse(x.Ambiente)).ToList().OrderBy((x => x.HoraInicio)).ToList();
        }

        public List<Ficha_AmbienteDTO> RegresarLlavesAmbientesDisponibles(int IdUser)
        {
            Model1 entity = new Model1();
            var FechaActual = DateTime.Now;
            var Lunes = false;
            var Martes = false;
            var Miercoles = false;
            var Jueves = false;
            var Viernes = false;
            var Sabado = 0;
            var Domingo = 0;

            var DiaSemana = FechaActual.DayOfWeek;

            switch (DiaSemana.ToString())
            {
                case "Monday":
                    Lunes = true;
                    break;

                case "Tuesday":
                    Martes = true;
                    break;

                case "Wednesday":
                    Miercoles = true;
                    break;
                case "Thursday":
                    Jueves = true;
                    break;
                case "Friday":
                    Viernes = true;
                    break;
                case "Saturday":
                    Sabado = 2;
                    break;
                case "Sunday":
                    Domingo = 3;
                    break;
            }

            var fecha = DateTime.Parse((DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()).ToString());
            var hora = (TimeSpan.Parse("00:00"));


            var sede = (from i in entity.Sede
                        where i.IdUsuario == IdUser
                        select i).FirstOrDefault();

            var Programaciones = (from i in entity.Ficha_Ambiente
                                  join p in entity.PrestamoLlaves on i.Id equals p.IdFicha_Ambiente
                                  join t in entity.Instructor on i.IdInstructor equals t.IdInstructor
                                  join f in entity.Ficha on i.IdFicha equals f.IdFicha
                                  join a in entity.Ambiente on i.IdAmbiente equals a.IdAmbiente
                                  join r in entity.Resultado_Aprendizaje on i.IdResultado equals r.IdResultado
                                  join c in entity.Competencia on r.IdCompetencia equals c.IdCompetencia
                                  where a.IdSede== sede.IdSede && (i.FechaInicio.Year == FechaActual.Year && i.FechaFin.Year <= FechaActual.Year)
                                  && (i.Lunes == Lunes || i.Martes == Martes || i.Miercoles == Miercoles || i.Jueves == Jueves || i.Viernes == Viernes || i.Jornada == Sabado || i.Jornada == Domingo)
                                  && p.Recibio == true && p.Entrego == false && p.Fecha == fecha

                                  select new Ficha_AmbienteDTO
                                  {
                                      Id = i.Id,
                                      IdFicha = i.IdFicha,
                                      Ficha = f.Ficha1,
                                      IdAmbiente = i.IdAmbiente,
                                      Ambiente = a.Numero,
                                      IdInstructor = i.IdInstructor,
                                      CedulaIns = t.Cedula,
                                      NombreInstructor = t.Nombre + " " + t.Apellido,
                                      Resultado = r.Resultado,
                                      CodigoResultado = r.Codigo,
                                      Competencia = c.Nombre,
                                      CodigoCompetencia = c.Codigo.ToString(),
                                      FechaInicio = i.FechaInicio,
                                      FechaFin = i.FechaFin,
                                      HoraInicio = i.HoraInicio,
                                      HoraFin = i.HoraFin,
                                      Color = i.Color,
                                      Lunes = i.Lunes,
                                      Martes = i.Martes,
                                      Miercoles = i.Miercoles,
                                      Jueves = i.Jueves,
                                      Viernes = i.Viernes,
                                      EntregoLLaves = p.Entrego,
                                      RecibioLLaves = p.Recibio,
                                      Jornada = i.Jornada.ToString()
                                  }).ToList();

            //Clonar lista
            List<Ficha_AmbienteDTO> newList = Programaciones.GetRange(0, Programaciones.Count);


            TimeSpan Hora = TimeSpan.FromHours(FechaActual.Hour) + TimeSpan.FromMinutes(FechaActual.Minute);
            string fromTimeString = Hora.ToString("hh':'mm");



            foreach (var item in Programaciones.Select((value, i) => new { i, value }))
            {
                if (item.value.Lunes == true)
                {
                    Programaciones[item.i].DiasProgramados += "Lunes -";
                }
                if (item.value.Martes == true)
                {
                    Programaciones[item.i].DiasProgramados += "Martes -";
                }
                if (item.value.Miercoles == true)
                {
                    Programaciones[item.i].DiasProgramados += "Miercoles -";
                }
                if (item.value.Jueves == true)
                {
                    Programaciones[item.i].DiasProgramados += "Jueves -";
                }
                if (item.value.Viernes == true)
                {
                    Programaciones[item.i].DiasProgramados += "Viernes -";
                }
                if (item.value.Jornada == "2")
                {
                    Programaciones[item.i].DiasProgramados += "Sábado -";
                }
                if (item.value.Jornada == "3")
                {
                    Programaciones[item.i].DiasProgramados += "Domingo -";
                }
            }
            return Programaciones;
        }

        public List<AmbienteDTO> AmbientesSinProgramacion(string horaInicio, string horafin, int IdUsuario)
        {
            Model1 entity = new Model1();


            var FechaActual = DateTime.Now;
            var Lunes = false;
            var Martes = false;
            var Miercoles = false;
            var Jueves = false;
            var Viernes = false;
            var jornada = 0;

            var DiaSemana = FechaActual.DayOfWeek;

            switch (DiaSemana.ToString())
            {
                case "Monday":
                    Lunes = true;
                    break;

                case "Tuesday":
                    Martes = true;
                    break;

                case "Wednesday":
                    Miercoles = true;
                    break;
                case "Thursday":
                    Jueves = true;
                    break;
                case "Friday":
                    Viernes = true;
                    break;
                case "Saturday":
                    jornada = 2;
                    break;
                case "Sunday":
                    jornada = 3;
                    break;
            }

            var sede = (from i in entity.Sede
                        where i.IdUsuario == IdUsuario
                        select i).FirstOrDefault();
            var fecha = DateTime.Parse((DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()).ToString());
            var horaI = TimeSpan.Parse(horafin);
            var ambientes = entity.Database.SqlQuery<AmbienteDTO>("AmbientesLibres @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin, @lunes, @martes, @miercoles, @jueves, @viernes, @jornada,@IdSede",
                                                                                    new SqlParameter("fecha_ini", fecha),
                                                                                    new SqlParameter("fecha_Fin", fecha),
                                                                                    new SqlParameter("HoraInicio", TimeSpan.Parse(horaInicio)),
                                                                                    new SqlParameter("HoraFin", TimeSpan.Parse(horafin)),
                                                                                    new SqlParameter("lunes", Lunes),
                                                                                    new SqlParameter("martes", Martes),
                                                                                    new SqlParameter("miercoles", Miercoles),
                                                                                    new SqlParameter("jueves", Jueves),
                                                                                    new SqlParameter("viernes", Viernes),
                                                                                    new SqlParameter("jornada", jornada),
                                                                                    new SqlParameter("IdSede", sede.IdSede)).ToList();

            var AmbientesDisponibles = (from i in ambientes
                                        where !(from p in entity.PrestamoAmbiente
                                                where p.Fecha == fecha && p.HoraEntrego <= horaI
                                                select p.IdAmbiente).Contains(i.IdAmbiente)
                                        select i).ToList();



            return AmbientesDisponibles.OrderBy(x => x.Piso).ToList().OrderBy(x => int.Parse(x.Numero)).ToList();
        }

        public List<AmbienteDTO> GuardarPrestamoLLavesAmbientes(AmbienteDTO oAmbiente)
        {
            Model1 entity = new Model1();
            var oPrestamo = new PrestamoAmbiente();

            var fecha = DateTime.Parse((DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()).ToString());
            var prestamo = (from i in entity.PrestamoAmbiente
                            where i.IdAmbiente == oAmbiente.IdAmbiente && i.Fecha == fecha
                            select i).FirstOrDefault();

            var FechaActual = DateTime.Now;
            TimeSpan Hora = TimeSpan.FromHours(FechaActual.Hour) + TimeSpan.FromMinutes(FechaActual.Minute);
            string fromTimeString = Hora.ToString("hh':'mm");
            if (prestamo == null)
            {
                if (oAmbiente.Recibio)
                {
                    oPrestamo.Recibio = true;
                    oPrestamo.Entrego = false;
                    oPrestamo.HoraRecibio = Hora;
                    oPrestamo.HoraEntrego = TimeSpan.Parse("00:00");
                    oPrestamo.Observacion = oAmbiente.Observacion;
                    oPrestamo.IdAmbiente = oAmbiente.IdAmbiente;
                    oPrestamo.Fecha = DateTime.Now;
                }


                entity.PrestamoAmbiente.Add(oPrestamo);
                entity.SaveChanges();
            }
            else
            {
                prestamo.Entrego = true;
                prestamo.HoraEntrego = Hora;
                entity.SaveChanges();
            }

            //var Ambientes = AmbientesSinProgramacion();
            return AmbientesEntregarLlaves();

        }

        public List<AmbienteDTO> AmbientesEntregarLlaves()
        {
            Model1 entity = new Model1();
            var fecha = DateTime.Parse((DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()).ToString());
            var ambientes = (from i in entity.Ambiente
                             join p in entity.PrestamoAmbiente on i.IdAmbiente equals p.IdAmbiente
                             join s in entity.Sede on i.IdSede equals s.IdSede
                             join a in entity.Area on i.IdArea equals a.IdArea
                             where p.Recibio == true && p.Entrego == false && p.Fecha == fecha
                             select new AmbienteDTO
                             {
                                 IdAmbiente = i.IdAmbiente,
                                 IdSede = i.IdSede,
                                 Piso = i.Piso,
                                 Numero = i.Numero,
                                 IdArea = i.IdArea,
                                 Pantalla = i.Pantalla,
                                 NumeroEquipos = i.NumeroEquipos,
                                 NumeroSillas = i.NumeroSillas,
                                 NumeroMesas = i.NumeroMesas,
                                 Ambiente = i.Numero,
                                 NombreSede = s.Nombre_Sede,
                                 Area = a.Nombre,
                                 Recibio = p.Recibio,
                                 Entrego = p.Entrego,
                                 Observacion = p.Observacion
                             }).ToList();

            return ambientes;
        }

        public List<Ficha_AmbienteDTO> ReporteLlaves(DateTime fechaInicio, DateTime fechaFin, int IdUser)
        {
            Model1 entity = new Model1();

            var Reporte = (from i in entity.PrestamoLlaves
                           where i.Fecha >= fechaInicio && i.Fecha <= fechaFin
                           select i).ToList();


            var ProgramacionLista = (from i in entity.Ficha_Ambiente
                                     join pr in entity.PrestamoLlaves on i.Id equals pr.IdFicha_Ambiente
                                     join t in entity.Instructor on i.IdInstructor equals t.IdInstructor
                                     join f in entity.Ficha on i.IdFicha equals f.IdFicha
                                     join a in entity.Ambiente on i.IdAmbiente equals a.IdAmbiente
                                     join s in entity.Sede on a.IdSede equals s.IdSede
                                     join r in entity.Resultado_Aprendizaje on i.IdResultado equals r.IdResultado
                                     join c in entity.Competencia on r.IdCompetencia equals c.IdCompetencia
                                     where ((i.FechaInicio >= fechaInicio && i.FechaInicio <= fechaFin) ||
                                           (i.FechaFin <= fechaFin && i.FechaFin >= fechaInicio)
                                           || (i.FechaInicio <= fechaInicio && i.FechaFin >= fechaFin)) && pr.Fecha >= fechaInicio && pr.Fecha <= fechaFin
                                     orderby s.Nombre_Sede

                                     select new Ficha_AmbienteDTO
                                     {
                                         Id = i.Id,
                                         IdFicha = i.IdFicha,
                                         Ficha = f.Ficha1,
                                         IdAmbiente = i.IdAmbiente,
                                         Ambiente = a.Numero,
                                         IdInstructor = i.IdInstructor,
                                         CedulaIns = t.Cedula,
                                         NombreInstructor = t.Nombre + " " + t.Apellido,
                                         Resultado = r.Resultado,
                                         CodigoResultado = r.Codigo,
                                         Competencia = c.Nombre,
                                         CodigoCompetencia = c.Codigo.ToString(),
                                         Color = i.Color,
                                         Lunes = i.Lunes,
                                         Martes = i.Martes,
                                         Miercoles = i.Miercoles,
                                         Jueves = i.Jueves,
                                         Jornada = i.Jornada.ToString(),
                                         HoraInicio = pr.HoraRecibio,
                                         HoraFin = i.HoraFin,
                                         HoraEntrego = pr.HoraEntrego,
                                         Observacion = pr.Observacion,
                                         FechaInicio = pr.Fecha,
                                         HoraRecibio2 = pr.HoraRecibio2,
                                         HoraEntrego2 = pr.HoraEntrego2,
                                         Sede= s.Nombre_Sede,

                                     }).ToList();


            var UserSede = (from i in entity.Sede
                        where i.IdUsuario == IdUser
                        select i).FirstOrDefault();

            if (UserSede != null)
            {
                //var coordinador = (from i in entity.Coordinacion
                //                   where i.IdCoordinacion == IdUser
                //                   select i).FirstOrDefault();

                //ProgramacionLista = (from p in ProgramacionLista
                //                     join i in entity.Instructor on p.IdInstructor equals i.IdInstructor
                //                     join a in entity.Area on i.IdArea equals a.IdArea
                //                     //where a.IdArea == coordinador.IdArea
                //                     select p).ToList();

                ProgramacionLista = (from p in ProgramacionLista
                                     join a in entity.Ambiente on p.IdAmbiente equals a.IdAmbiente
                                     where a.IdSede == UserSede.IdSede
                                     select p).ToList();
            }
        
            foreach (var item in ProgramacionLista.Select((value, i) => new { i, value }))
            {
                ProgramacionLista[item.i].DiferenciaHoras = TimeSpan.Parse(item.value.HoraFin.ToString()).Subtract(TimeSpan.Parse(item.value.HoraEntrego.ToString()));

            }

            var reporteAmbientes = (from i in entity.PrestamoAmbiente
                                    join a in entity.Ambiente on i.IdAmbiente equals a.IdAmbiente
                                    where i.Fecha >= fechaInicio && i.Fecha <= fechaFin
                                    select new AmbienteDTO
                                    {
                                        IdAmbiente = a.IdAmbiente,
                                        IdSede = a.IdSede,
                                        Piso = a.Piso,
                                        Numero = a.Numero,
                                        IdArea = a.IdArea,
                                        Pantalla = a.Pantalla,
                                        NumeroEquipos = a.NumeroEquipos,
                                        NumeroSillas = a.NumeroSillas,
                                        NumeroMesas = a.NumeroMesas,
                                        HoraInicio = i.HoraRecibio,
                                        HoraFin = i.HoraEntrego,
                                        Observacion = i.Observacion,
                                        Ambiente = a.Numero,
                                        Fecha = i.Fecha
                                    }).ToList();

            foreach (var item in reporteAmbientes)
            {
                var Programacion = new Ficha_AmbienteDTO();
                if (UserSede != null)
                {
                    Programacion.NombreInstructor = "Prestamo Especial";
                    Programacion.CedulaIns = "Prestamo Especial";
                    Programacion.Ficha = "0000";
                    Programacion.Ambiente = item.Numero;
                    Programacion.FechaInicio = item.Fecha;
                    Programacion.HoraInicio = item.HoraInicio;
                    Programacion.HoraFin = item.HoraFin;
                    Programacion.Competencia = "Prestamo Especial";
                    Programacion.Observacion = item.Observacion;
                    Programacion.Sede = UserSede.Nombre_Sede;
                    Programacion.HoraEntrego = item.HoraFin;
                    Programacion.DiferenciaHoras = TimeSpan.Parse("00:00");
                    ProgramacionLista.Add(Programacion);
                }
                else
                {
                    var sede = (from i in entity.Sede
                                join a in entity.Ambiente on i.IdSede equals a.IdSede
                                where a.IdAmbiente == item.IdAmbiente
                                select i).First();

                    Programacion.NombreInstructor = "Prestamo Especial";
                    Programacion.CedulaIns = "Prestamo Especial";
                    Programacion.Ficha = "0000";
                    Programacion.Ambiente = item.Numero;
                    Programacion.FechaInicio = item.Fecha;
                    Programacion.HoraInicio = item.HoraInicio;
                    Programacion.HoraFin = item.HoraFin;
                    Programacion.Competencia = "Prestamo Especial";
                    Programacion.Observacion = item.Observacion;
                    Programacion.Sede = sede.Nombre_Sede;
                    Programacion.HoraEntrego = item.HoraFin;
                    Programacion.DiferenciaHoras = TimeSpan.Parse("00:00");
                    ProgramacionLista.Add(Programacion);
                }
          
            }

            return ProgramacionLista;
        }

        public void RegistrarProgramacionesPrestamoLLaves()
        {
            Model1 entity = new Model1();
            var registros = AmbientesDisponibles();
            var oPrestamo = new PrestamoLlaves();
            var FechaActual = DateTime.Now;
            var fecha = DateTime.Parse((DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()).ToString());
            TimeSpan Hora = TimeSpan.FromHours(FechaActual.Hour) + TimeSpan.FromMinutes(FechaActual.Minute);
            if (Hora >= TimeSpan.Parse("22:59"))
            {
                foreach (var item in registros)
                {
                    oPrestamo.Recibio = false;
                    oPrestamo.Entrego = false;
                    oPrestamo.HoraRecibio = TimeSpan.Parse("00:00");
                    oPrestamo.HoraEntrego = TimeSpan.Parse("00:00");
                    oPrestamo.Observacion = "";
                    oPrestamo.IdFicha_Ambiente = item.Id;
                    oPrestamo.Fecha = DateTime.Now;

                    entity.PrestamoLlaves.Add(oPrestamo);
                    entity.SaveChanges();
                }
            }

        }

        public List<Ficha_AmbienteDTO> ConsultarLLavesEditar()
        {
            Model1 entity = new Model1();
            var fecha = DateTime.Parse((DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()).ToString());
            var prestamos = (from i in entity.PrestamoLlaves
                             where i.Recibio == true && i.Entrego == true && i.Fecha == fecha
                             select i).ToList();
            var listaProgramacion = new List<Ficha_AmbienteDTO>();
            var FechaActual = DateTime.Now;
            TimeSpan Hora = TimeSpan.FromHours(FechaActual.Hour) + TimeSpan.FromMinutes(FechaActual.Minute);

          

            foreach (var item in prestamos)
            {
                var programacion = (from i in entity.Ficha_Ambiente
                                    join t in entity.Instructor on i.IdInstructor equals t.IdInstructor
                                    join f in entity.Ficha on i.IdFicha equals f.IdFicha
                                    join a in entity.Ambiente on i.IdAmbiente equals a.IdAmbiente
                                    join r in entity.Resultado_Aprendizaje on i.IdResultado equals r.IdResultado
                                    join c in entity.Competencia on r.IdCompetencia equals c.IdCompetencia
                                    where i.Id == item.IdFicha_Ambiente
                                    select new Ficha_AmbienteDTO
                                    {
                                        Id = i.Id,
                                        IdFicha = i.IdFicha,
                                        Ficha = f.Ficha1,
                                        IdAmbiente = i.IdAmbiente,
                                        Ambiente = a.Numero,
                                        IdInstructor = i.IdInstructor,
                                        CedulaIns = t.Cedula,
                                        NombreInstructor = t.Nombre + " " + t.Apellido,
                                        Resultado = r.Resultado,
                                        CodigoResultado = r.Codigo,
                                        Competencia = c.Nombre,
                                        CodigoCompetencia = c.Codigo.ToString(),
                                        Color = i.Color,
                                        Lunes = i.Lunes,
                                        Martes = i.Martes,
                                        Miercoles = i.Miercoles,
                                        Jueves = i.Jueves,
                                        Viernes = i.Viernes,
                                        Jornada = i.Jornada.ToString(),
                                        HoraInicio = i.HoraInicio,
                                        HoraFin = i.HoraFin,
                                        Observacion = item.Observacion,
                                        FechaInicio = i.FechaInicio,
                                        FechaFin = i.FechaFin,
                                        RecibioLLaves = item.Recibio,
                                        EntregoLLaves = item.Entrego
                                    }).FirstOrDefault();
                if (programacion != null)
                {


                    if (programacion.Lunes == true)
                    {
                        programacion.DiasProgramados += "Lunes -";
                    }
                    if (programacion.Martes == true)
                    {
                        programacion.DiasProgramados += "Martes -";
                    }
                    if (programacion.Miercoles == true)
                    {
                        programacion.DiasProgramados += "Miercoles -";
                    }
                    if (programacion.Jueves == true)
                    {
                        programacion.DiasProgramados += "Jueves -";
                    }
                    if (programacion.Viernes == true)
                    {
                        programacion.DiasProgramados += "Viernes -";
                    }
                    if (programacion.Jornada == "2")
                    {
                        programacion.DiasProgramados += "Sábado -";
                    }
                    if (programacion.Jornada == "3")
                    {
                        programacion.DiasProgramados += "Domingo -";
                    }

                    if (programacion.HoraFin > item.HoraEntrego)
                    {
                        if (programacion.HoraFin >= Hora)
                        {
                            if (item.HoraRecibio2 == null)
                            {
                                programacion.RecibioLLaves = false;
                            }
                            if (item.HoraEntrego2 == TimeSpan.Parse("00:00") || item.HoraEntrego2 == null)
                            {
                                programacion.EntregoLLaves = false;
                                listaProgramacion.Add(programacion);
                            }

                        }

                    }
                }

            }


            return listaProgramacion;
        }

        public List<Ficha_AmbienteDTO> ConsultarLLavesEditar(int IdUser)
        {
            Model1 entity = new Model1();
            var fecha = DateTime.Parse((DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()).ToString());
            var prestamos = (from i in entity.PrestamoLlaves
                             where i.Recibio == true && i.Entrego == true && i.Fecha == fecha
                             select i).ToList();
            var listaProgramacion = new List<Ficha_AmbienteDTO>();
            var FechaActual = DateTime.Now;
            TimeSpan Hora = TimeSpan.FromHours(FechaActual.Hour) + TimeSpan.FromMinutes(FechaActual.Minute);

            var UserSede = (from i in entity.Sede
                            where i.IdUsuario == IdUser
                            select i).FirstOrDefault();

            foreach (var item in prestamos)
            {
                var programacion = (from i in entity.Ficha_Ambiente
                                    join t in entity.Instructor on i.IdInstructor equals t.IdInstructor
                                    join f in entity.Ficha on i.IdFicha equals f.IdFicha
                                    join a in entity.Ambiente on i.IdAmbiente equals a.IdAmbiente
                                    join r in entity.Resultado_Aprendizaje on i.IdResultado equals r.IdResultado
                                    join c in entity.Competencia on r.IdCompetencia equals c.IdCompetencia
                                    where i.Id == item.IdFicha_Ambiente && a.IdSede == UserSede.IdSede
                                    select new Ficha_AmbienteDTO
                                    {
                                        Id = i.Id,
                                        IdFicha = i.IdFicha,
                                        Ficha = f.Ficha1,
                                        IdAmbiente = i.IdAmbiente,
                                        Ambiente = a.Numero,
                                        IdInstructor = i.IdInstructor,
                                        CedulaIns = t.Cedula,
                                        NombreInstructor = t.Nombre + " " + t.Apellido,
                                        Resultado = r.Resultado,
                                        CodigoResultado = r.Codigo,
                                        Competencia = c.Nombre,
                                        CodigoCompetencia = c.Codigo.ToString(),
                                        Color = i.Color,
                                        Lunes = i.Lunes,
                                        Martes = i.Martes,
                                        Miercoles = i.Miercoles,
                                        Jueves = i.Jueves,
                                        Viernes = i.Viernes,
                                        Jornada = i.Jornada.ToString(),
                                        HoraInicio = i.HoraInicio,
                                        HoraFin = i.HoraFin,
                                        Observacion = item.Observacion,
                                        FechaInicio = i.FechaInicio,
                                        FechaFin = i.FechaFin,
                                        RecibioLLaves = item.Recibio,
                                        EntregoLLaves = item.Entrego
                                    }).FirstOrDefault();
                if (programacion != null)
                {


                    if (programacion.Lunes == true)
                    {
                        programacion.DiasProgramados += "Lunes -";
                    }
                    if (programacion.Martes == true)
                    {
                        programacion.DiasProgramados += "Martes -";
                    }
                    if (programacion.Miercoles == true)
                    {
                        programacion.DiasProgramados += "Miercoles -";
                    }
                    if (programacion.Jueves == true)
                    {
                        programacion.DiasProgramados += "Jueves -";
                    }
                    if (programacion.Viernes == true)
                    {
                        programacion.DiasProgramados += "Viernes -";
                    }
                    if (programacion.Jornada == "2")
                    {
                        programacion.DiasProgramados += "Sábado -";
                    }
                    if (programacion.Jornada == "3")
                    {
                        programacion.DiasProgramados += "Domingo -";
                    }

                    if (programacion.HoraFin > item.HoraEntrego)
                    {
                        if (programacion.HoraFin >= Hora)
                        {
                            if (item.HoraRecibio2 == null)
                            {
                                programacion.RecibioLLaves = false;
                            }
                            if (item.HoraEntrego2 == TimeSpan.Parse("00:00") || item.HoraEntrego2 == null)
                            {
                                programacion.EntregoLLaves = false;
                                listaProgramacion.Add(programacion);
                            }

                        }

                    }
                }

            }


            return listaProgramacion;
        }

    }
}
