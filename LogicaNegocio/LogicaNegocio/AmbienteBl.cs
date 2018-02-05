using Datos.DTO;
using Datos.Modelo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LogicaNegocio.LogicaNegocio
{
    public class AmbienteBl
    {
        Model1 entity = new Model1();

        public List<Ambiente> ConsultarAmbientes()
        {
            var Datos = (from i in entity.Ambiente
                         orderby i.IdSede ascending
                         select i).ToList();
            return Datos;
        }

        public List<Ambiente> ConsultarAmbienteSinColegios()
        {
            var Datos = (from i in entity.Ambiente
                         join a in entity.Sede on i.IdSede equals a.IdSede
                         where a.TipoSede == 1
                         orderby i.IdSede ascending
                         select i).ToList();
            return Datos;
        }

        public List<Ambiente> ConsultarAmbientesColegios()
        {
            var Datos = (from i in entity.Ambiente
                         join a in entity.Sede on i.IdSede equals a.IdSede
                         where a.TipoSede == 3
                         orderby i.IdSede ascending
                         select i).ToList();
            return Datos;
        }

        public List<Ambiente> ConsultarAmbienteEmpresa()
        {
            var Datos = (from i in entity.Ambiente
                         join a in entity.Sede on i.IdSede equals a.IdSede
                         where a.TipoSede == 2
                         orderby i.IdSede ascending
                         select i).ToList();
            return Datos;
        }

        public List<Ambiente> ConsultarAmbienteVirtual()
        {
            var Datos = (from i in entity.Ambiente
                         join a in entity.Sede on i.IdSede equals a.IdSede
                         where a.TipoSede == 4
                         orderby i.IdSede ascending
                         select i).ToList();
            return Datos;
        }
        public List<Ambiente> OrganizarPisoAsc()
        {
            var Datos = (from i in entity.Ambiente
                         orderby i.Piso ascending
                         select i).ToList();
            return Datos;
        }

        public List<Ambiente> OrganizarPisoDesc()
        {
            var Datos = (from i in entity.Ambiente
                         orderby i.Piso descending
                         select i).ToList();
            return Datos;
        }

        public List<Ambiente> OrganizarNumeroAsc()
        {
            var Datos = (from i in entity.Ambiente
                         orderby i.Numero ascending
                         select i).ToList();
            return Datos;
        }

        public List<Ambiente> OrganizarNumeroDesc()
        {
            var Datos = (from i in entity.Ambiente
                         orderby i.Numero descending
                         select i).ToList();
            return Datos;
        }

        public List<Ambiente> ConsultarAmbientesxArea(int IdArea, int IdSede)
        {
            var Datos = (from i in entity.Ambiente
                        where i.IdArea == IdArea
                        && i.IdSede == IdSede
                        select i).ToList();
            return Datos;
        }



        public List<Ambiente> AmbientesxArea(int Idcoordinacion)
        {
            var coordinacion = (from i in entity.Coordinacion
                                where i.IdCoordinacion == Idcoordinacion
                                select i).FirstOrDefault();


            var Datos = (from i in entity.Ambiente
                        where i.IdArea == coordinacion.IdArea
                        select i).ToList();
            return Datos;
        }
        /// <summary>
        /// Save   Listas
        /// </summary>
        /// <param name="objListas">Entity  Listas</param>
        /// <returns>Tercero</returns>
        public bool GuardarAmbiente(Ambiente oAmbiente)
        {
            var Datos = (from i in entity.Ambiente
                         where i.IdSede == oAmbiente.IdSede
                         && i.Numero == oAmbiente.Numero
                         select i).FirstOrDefault();
            if (Datos == null)
            {
                entity.Ambiente.Add(oAmbiente);
                entity.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }            
        }

        public void EliminarAmbiente(int IdAmbiente)
        {

            var Datos = (from i in entity.Ambiente
                         where i.IdAmbiente == IdAmbiente
                         select i).FirstOrDefault();
            entity.Ambiente.Remove(Datos);
            entity.SaveChanges();
        }

        public Ambiente ConsultarAmbienteId(int IdAmbiente)
        {
            Model1 entity = new Model1();
            var Datos = (from i in entity.Ambiente
                         where i.IdAmbiente == IdAmbiente
                         select i).FirstOrDefault();
            return Datos;

        }

        public void ActualizarRegistro(Ambiente oAmbiente)
        {
            Model1 entity = new Model1();

            var Item = (from i in entity.Ambiente
                        where i.IdAmbiente == oAmbiente.IdAmbiente
                        select i).First();

            Item.IdArea = oAmbiente.IdArea;
            Item.Numero = oAmbiente.Numero;
            Item.NumeroEquipos = oAmbiente.NumeroEquipos;
            Item.Pantalla = oAmbiente.Pantalla;
            Item.Piso = oAmbiente.Piso;
            Item.NumeroMesas = oAmbiente.NumeroMesas;
            Item.NumeroSillas = oAmbiente.NumeroSillas;
            Item.IdSede = oAmbiente.IdSede;
            entity.SaveChanges();
        }

        public List<AmbienteDTO> ConsultarAmbientesCoordinador(int IdCoordinador)
        {
            var Lista = new List<AmbienteDTO>();
            
            var Items = (from i in entity.Coordinacion
                        join a in entity.Area on i.IdArea equals a.IdArea
                        join am in entity.Ambiente on a.IdArea equals am.IdArea
                        join s in entity.Sede on am.IdSede equals s.IdSede
                        where i.IdCoordinacion == IdCoordinador
                        orderby s.Nombre_Sede, am.Numero
                        select new {am, s.Nombre_Sede} ).ToList();

            foreach (var item in Items)
            {
                var Ambientes = new AmbienteDTO();

                Ambientes.IdAmbiente = item.am.IdAmbiente;
                Ambientes.IdSede = item.am.IdSede;
                Ambientes.Piso = item.am.Piso;
                Ambientes.IdArea = item.am.IdArea;
                Ambientes.Pantalla = item.am.Pantalla;
                Ambientes.Numero = item.am.Numero;
                Ambientes.NumeroEquipos = item.am.NumeroEquipos;
                Ambientes.NumeroSillas = item.am.NumeroSillas;
                Ambientes.NumeroMesas = item.am.NumeroMesas;
                Ambientes.NombreSede = item.Nombre_Sede;

                Lista.Add(Ambientes);
            }

            return Lista;
        }


        public List<Ambiente> ambientesDisponibles(DateTime FechaInicio, DateTime FechaFin, string HoraInicio, string HoraFin, int opc, int IdCoordinacion, bool Lunes, bool Martes, bool Miercoles, bool Jueves, bool Viernes)
        {

            var coordinador = (from i in entity.Coordinacion
                               where i.IdCoordinacion == IdCoordinacion
                               select i).FirstOrDefault();

            var DisponibilidadAmbiente = entity.Database.SqlQuery<Ambiente>("AmbientesDisponibles @fecha_ini, @fecha_Fin, @HoraInicio, @HoraFin,@IdArea, @opc, @lunes,@martes,@miercoles,@jueves,@viernes",
                                                                                 new SqlParameter("fecha_ini", FechaInicio),
                                                                                 new SqlParameter("fecha_Fin", FechaFin),
                                                                                 new SqlParameter("HoraInicio", TimeSpan.Parse(HoraInicio)),
                                                                                 new SqlParameter("HoraFin", TimeSpan.Parse(HoraFin)),
                                                                                 new SqlParameter("IdArea", coordinador.IdArea),
                                                                                 new SqlParameter("lunes", Lunes),
                                                                                 new SqlParameter("martes", Martes),
                                                                                 new SqlParameter("miercoles", Miercoles),
                                                                                 new SqlParameter("jueves", Jueves),
                                                                                 new SqlParameter("viernes", Viernes),
                                                                                 new SqlParameter("opc", opc)).ToList();




            return DisponibilidadAmbiente;


        }

    }
}
