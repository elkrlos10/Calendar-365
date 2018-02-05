using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using LogicaNegocio.LogicaNegocio;
using Datos.Modelo;
using ProgramacionAmbientes.Parametros;
using System.Timers;
using System.Globalization;

namespace ProgramacionAmbientes.Controllers
{
    public class FichaController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GuardarFicha(Ficha oFicha)
        {
            try
            {
                bool respuesta = false;
                FichaBl oFichaBl = new FichaBl();
                respuesta = oFichaBl.GuardarFicha(oFicha);
                return Ok(new { respuesta = respuesta, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpGet]
        public IHttpActionResult ConsultarFichas()
        {
            try
            {
                FichaBl oFichaBl = new FichaBl();
                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();
                var Datos = oFichaBl.ConsultarFichas();
                ProgramaBl oProgramaBl = new ProgramaBl();
                foreach (var item in Datos)
                {
                    ParametrosDTO oParametrosDTO = new ParametrosDTO();
                    var nombrePrograma = oProgramaBl.ConsultarProgramaId(item.IdPrograma);
                    var nombre = new CultureInfo("es-CO", false).TextInfo.ToTitleCase(nombrePrograma.NombrePrograma.ToLower());
                    oParametrosDTO.Parametro1 = item.IdFicha.ToString();
                    oParametrosDTO.Parametro2 = item.Ficha1;
                    oParametrosDTO.Parametro3 = item.NumAprendices.ToString();
                    oParametrosDTO.Parametro4 = new CultureInfo("es-CO", false).TextInfo.ToTitleCase(item.TipoFormacion.ToLower());
                    oParametrosDTO.Parametro5 = item.Estado.ToString();
                    oParametrosDTO.Parametro6 = item.FechaInicio.ToString();
                    oParametrosDTO.Parametro7 = item.FechaFin.ToString();
                    oParametrosDTO.Parametro8 = nombre;
                    oParametrosDTO.Parametro9 = item.Jornada;
                    ListaParametro.Add(oParametrosDTO);
                }

                return Ok(new { datos = ListaParametro, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }


        [HttpPost]
        public IHttpActionResult ConsultarFichasxArea(ParametrosDTO oParametrosDTO1)
        {
            try
            {
                FichaBl oFichaBl = new FichaBl();
                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();
                var Datos = oFichaBl.ConsultarFichasxArea(int.Parse(oParametrosDTO1.Parametro1));
                ProgramaBl oProgramaBl = new ProgramaBl();
                foreach (var item in Datos)
                {
                    ParametrosDTO oParametrosDTO = new ParametrosDTO();
                    var nombrePrograma = oProgramaBl.ConsultarProgramaId(item.IdPrograma);
                    var nombre = new CultureInfo("es-CO", false).TextInfo.ToTitleCase(nombrePrograma.NombrePrograma.ToLower());
                    oParametrosDTO.Parametro1 = item.IdFicha.ToString();
                    oParametrosDTO.Parametro2 = item.Ficha1;
                    oParametrosDTO.Parametro3 = item.NumAprendices.ToString();
                    oParametrosDTO.Parametro4 = new CultureInfo("es-CO", false).TextInfo.ToTitleCase(item.TipoFormacion.ToLower());
                    oParametrosDTO.Parametro5 = item.Estado.ToString();
                    oParametrosDTO.Parametro6 = item.FechaInicio.ToString();
                    oParametrosDTO.Parametro7 = item.FechaFin.ToString();
                    oParametrosDTO.Parametro8 = nombre;
                    oParametrosDTO.Parametro9 = item.Jornada;
                    ListaParametro.Add(oParametrosDTO);
                }

                return Ok(new { datos = ListaParametro, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpGet]
        public IHttpActionResult ConsultarFichasInactivas()
        {
            try
            {
                FichaBl oFichaBl = new FichaBl();
                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();
                var Datos = oFichaBl.ConsultarFichasInactivas();
                ProgramaBl oProgramaBl = new ProgramaBl();
                foreach (var item in Datos)
                {
                    ParametrosDTO oParametrosDTO = new ParametrosDTO();
                    var nombrePrograma = oProgramaBl.ConsultarProgramaId(item.IdPrograma);
                    oParametrosDTO.Parametro1 = item.IdFicha.ToString();
                    oParametrosDTO.Parametro2 = item.Ficha1;
                    oParametrosDTO.Parametro3 = item.NumAprendices.ToString();
                    oParametrosDTO.Parametro4 = item.TipoFormacion.ToString();
                    oParametrosDTO.Parametro5 = item.Estado.ToString();
                    oParametrosDTO.Parametro6 = item.FechaInicio.ToString();
                    oParametrosDTO.Parametro7 = item.FechaFin.ToString();
                    oParametrosDTO.Parametro8 = nombrePrograma.NombrePrograma;
                    oParametrosDTO.Parametro9 = item.Jornada;
                    ListaParametro.Add(oParametrosDTO);
                }

                return Ok(new { datos = ListaParametro, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult InHabilitarFicha(ParametrosDTO oParametrosDTO)
        {
            try
            {
                FichaBl oFichaBl = new FichaBl();
                foreach (var item in oParametrosDTO.Parametros)
                {
                    oFichaBl.InHabilitarFicha(int.Parse(item.Parametro1));
                }
                return Ok(new { success = true });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]

        public IHttpActionResult ModificarFicha(ParametrosDTO oParametrosDTO)
        {
            try
            {
                FichaBl oFichaBl = new FichaBl();
                var Ficha = oFichaBl.ConsultarFichaId(int.Parse(oParametrosDTO.Parametro1));

                return Ok(new { success = true, Ficha });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }
        }


        [HttpPost]

        public IHttpActionResult ConsultarAreaxPrograma(ParametrosDTO oParametrosDTO)
        {
            try
            {
                FichaBl oFichaBl = new FichaBl();
                var datos = oFichaBl.ConsultarAreaxPrograma(int.Parse(oParametrosDTO.Parametro1));


                
                    ParametrosDTO oParametrosDTO1 = new ParametrosDTO();
                    //var NombreCoordinacion = oCoordinacionBl.ConsultarNombreCoordinacion(item.IdCoordinacion);
                    oParametrosDTO1.Parametro1 = datos.IdArea.ToString();
                    oParametrosDTO1.Parametro2 = datos.Codigo.ToString();
                    //oParametrosDTO.Parametro3 = NombreCoordinacion;
                    oParametrosDTO1.Parametro4 = datos.Nombre;
                    oParametrosDTO1.Parametro5 = datos.Descripcion;
                    
                

                return Ok(new { success = true,  dato = oParametrosDTO1 });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult GuardarModificacionFicha(Ficha oFicha)
        {
            try
            {
                FichaBl oFichaBl = new FichaBl();
                oFichaBl.ActualizarRegistro(oFicha);
                return Ok(new { success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }

        }

        [HttpGet]
        public IHttpActionResult ConsultarProgramas()
        {
            try
            {
                ProgramaBl oProgramaBl = new ProgramaBl();
                var Datos = oProgramaBl.ConsultarProgramas();

                return Ok(new { datos = Datos, success = true });

            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }


        [HttpPost]
        public IHttpActionResult ReporteProgramacion(ParametrosDTO oParametrosDTO)
        {
            try
            {

                FichaBl oFichaBl = new FichaBl();
                var datos = oFichaBl.ReporteProgramacion(DateTime.Parse(oParametrosDTO.Parametro1), DateTime.Parse(oParametrosDTO.Parametro2), int.Parse(oParametrosDTO.Parametro3));
                return Ok(new { success = true, datos });

            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc.Message });
            }


        }

        [HttpPost]
        public IHttpActionResult ConsultarFichasxPrograma(ParametrosDTO oParametrosDTO)
        {
            try
            {
                FichaBl oFichaBl = new FichaBl();
                var datos = oFichaBl.ConsultarFichasxPrograma(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { success = true, datos });

            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc.Message });
            }


        }
    }
}