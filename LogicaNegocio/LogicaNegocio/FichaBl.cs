using Datos.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LogicaNegocio.LogicaNegocio
{
    public class FichaBl
    {
        Model1 entity = new Model1();
        Timer aTimer = new Timer();

        public bool GuardarFicha(Ficha oFicha)
        {
            var Datos = (from i in entity.Ficha
                         where i.Ficha1 == oFicha.Ficha1
                         select i).FirstOrDefault();

            if (Datos == null)
            {
                oFicha.Estado = true;
                entity.Ficha.Add(oFicha);
                entity.SaveChanges();               
                return true;
            }
            else
            {
                return false;
            }

        }

        public List<Ficha> ConsultarFichas()
        {
            
            var Datos = (from i in entity.Ficha
                         where i.Estado == true
                         select i).ToList();


            foreach (var item in Datos)
            {
                var fecha = item.FechaFin;

                if (DateTime.Now >= fecha)
                {
                    item.Estado = false;
                    entity.SaveChanges();
                }
            }


            var Datos1 = (from i in entity.Ficha
                         where i.Estado == true
                         select i).ToList();

            return Datos1;
        }


        public List<Ficha> ConsultarFichasxArea(int Idcoordinador)
        {
            var coordinador = (from i in entity.Coordinacion
                               where i.IdCoordinacion == Idcoordinador
                               select i).FirstOrDefault();

            var Datos = (from i in entity.Ficha
                         where i.Estado == true && i.Jornada != "VIRTUAL"
                         select i).ToList();


            foreach (var item in Datos)
            {
                var fecha = item.FechaFin;

                if (DateTime.Now >= fecha)
                {
                    item.Estado = false;
                    entity.SaveChanges();
                }
            }


            var Datos1 = (from i in entity.Ficha
                          join p in entity.Programa on i.IdPrograma equals p.IdPrograma
                          join a in entity.Area on p.IdArea equals a.IdArea
                          join c in entity.Coordinacion on a.IdArea equals c.IdArea
                          where i.Estado == true && c.IdCoordinacion == coordinador.IdCoordinacion
                          orderby p.IdPrograma
                          select i).ToList();

            return Datos1;
        }


        public List<Ficha> ConsultarFichasxPrograma(int IdPrograma)
        {
            var Datos = (from i in entity.Ficha
                         where i.Estado == true && i.Jornada != "VIRTUAL"
                         select i).ToList();


            foreach (var item in Datos)
            {
                var fecha = item.FechaFin;

                if (DateTime.Now >= fecha)
                {
                    item.Estado = false;
                    entity.SaveChanges();
                }
            }

            var Datos1 = (from i in entity.Ficha
                          join p in entity.Programa on i.IdPrograma equals p.IdPrograma
                          where i.Estado == true && p.IdPrograma == IdPrograma
                          select i).ToList();

            return Datos1;
        }

        public Area ConsultarAreaxPrograma(int IdPrograma)
        {

            var Datos = (from a in entity.Area
                         join p in entity.Programa on a.IdArea equals p.IdArea
                         where p.IdPrograma == IdPrograma
                         select a).FirstOrDefault();

            return Datos;
        }

        public List<Ficha> ConsultarFichasInactivas()
        {

            var Datos = (from i in entity.Ficha
                         where i.Estado == false
                         select i).ToList();
            return Datos;
        }


        public void InHabilitarFicha(int IdFicha)
        {

            var Datos = (from i in entity.Ficha
                         where i.IdFicha == IdFicha
                         select i).FirstOrDefault();

            Datos.Estado = false;
            entity.SaveChanges();
        }

        public Ficha ConsultarFichaId(int IdFicha)
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Ficha
                         where i.IdFicha == IdFicha
                         select i).FirstOrDefault();
            return Datos;
        }

        public void ActualizarRegistro(Ficha oFicha)
        {
            Model1 entity = new Model1();

            var Item = (from i in entity.Ficha
                        where i.IdFicha == oFicha.IdFicha
                        select i).First();
            //Item.IdArea = oFicha.IdArea;
            Item.Ficha1 = oFicha.Ficha1;
            Item.Estado = true;
            Item.NumAprendices = oFicha.NumAprendices;
            Item.TipoFormacion = oFicha.TipoFormacion;
            Item.FechaInicio = oFicha.FechaInicio;
            Item.FechaFin = oFicha.FechaFin;
            Item.IdPrograma = oFicha.IdPrograma;
            Item.Jornada = oFicha.Jornada;
            entity.SaveChanges();

        }


        // Specify what you want to happen when the Elapsed event is raised.
        public static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Model1 entity = new Model1();

            var Datos = (from i in entity.Ficha
                         where i.Estado == true
                         select i).ToList();

            foreach (var item in Datos)
            {
                var fecha = item.FechaFin.AddMonths(6);

                if (DateTime.Now >= fecha)
                {
                    item.Estado = false;
                    entity.SaveChanges();
                }
            }


        }


        public List<Ficha_Ambiente> ReporteProgramacion(DateTime FechaInicio, DateTime FechaFin, int IdFicha)
        {
           
            Model1 entity = new Model1();

            var Programacion = (from i in entity.Ficha_Ambiente
                                where i.IdFicha == IdFicha && 
                                ((i.FechaInicio >= FechaInicio && i.FechaInicio <= FechaFin) ||
                                 (i.FechaFin <= FechaFin && i.FechaFin >= FechaInicio) ||
                                 (i.FechaInicio<= FechaInicio && i.FechaFin >= FechaFin )) 
                                select i).ToList();


            return Programacion;

        }

        
    }
}
