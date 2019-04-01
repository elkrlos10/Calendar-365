using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using LogicaNegocio.LogicaNegocio;
using Datos.Modelo;
using ProgramacionAmbientes.Parametros;
using Datos.DTO;

namespace ProgramacionAmbientes.Controllers
{
    public class ProgramacionController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetFichas(ParametrosDTO oParametrosDTO)
        {
            try
            {
                FichaBl oFichaBl = new FichaBl();
                var Datos = oFichaBl.ConsultarFichasxArea(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { datos = Datos, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult GetAmbientes(ParametrosDTO oParametrosDTO)
        {
            try
            {
                AmbienteBl oAmbienteBl = new AmbienteBl();
                var Datos = oAmbienteBl.ConsultarAmbientes();
                //var Datos = oAmbienteBl.ConsultarAmbientesxArea1(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { datos = Datos, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult GetCompetencias(ParametrosDTO oParametrosDTO)
        {
            try
            {
                CompetenciaBl oCompetenciaBl = new CompetenciaBl();
                var Datos = oCompetenciaBl.ConsultarCompetenciasxArea(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { datos = Datos, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpGet]
        public IHttpActionResult GetInstructores()
        {
            try
            {
                InstructorBl oInstructorBl = new InstructorBl();
                var Datos = oInstructorBl.ConsultarInstructores();
                return Ok(new { datos = Datos, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult GuardarProgramacion(List<Ficha_Ambiente> oFicha_Ambiente)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var mensaje = oProgramacionBl.GuardarProgramacion(oFicha_Ambiente);
                return Ok(new { success = true, mensaje });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }


        [HttpPost]
        public IHttpActionResult EliminarProgramacion(Ficha_Ambiente oFicha_Ambiente)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                oProgramacionBl.EliminarProgramacion(oFicha_Ambiente);
                return Ok(new { success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult AprobarSolicitud(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                oProgramacionBl.AprobarSolicitud(int.Parse(oParametrosDTO.Parametro1), bool.Parse(oParametrosDTO.Parametro2));
                return Ok(new { success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }


        [HttpPost]
        public IHttpActionResult ConsultarAmbienteSolicitado(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var datos = oProgramacionBl.ConsultarAmbienteSolicitado(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { datos, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult GuardarProgramacionTrasversal(Ficha_Ambiente oFicha_Ambiente)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                oProgramacionBl.GuardarProgramacionTrasversal(oFicha_Ambiente);
                return Ok(new { success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ConsultarProgramacion(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var Datos = oProgramacionBl.ConsultarProgramacion(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { datos = Datos, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ConsultarProgramacionTransversales(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var Datos = oProgramacionBl.ConsultarProgramacionTransversales(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { datos = Datos, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ConsultarProgramacionSabados(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var Datos = oProgramacionBl.ConsultarProgramacionSabados(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { datos = Datos, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult FiltrarAmbiente(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var Datos = oProgramacionBl.FiltrarAmbiente(int.Parse(oParametrosDTO.Parametro1), int.Parse(oParametrosDTO.Parametro2));
                return Ok(new { programacion = Datos, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }


        [HttpPost]
        public IHttpActionResult ConsultarResultados(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ResultadoBl oResultadoBl = new ResultadoBl();
                var Datos = oResultadoBl.ConsultarResultadoXCompetencia(int.Parse(oParametrosDTO.Parametro1));

                return Ok(new { datos = Datos, success = true });

            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult Disponibilidad(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var Datos = oProgramacionBl.Disponibilidad(bool.Parse(oParametrosDTO.Parametro6.ToLower()), int.Parse(oParametrosDTO.Parametro1), DateTime.Parse(oParametrosDTO.Parametro2), DateTime.Parse(oParametrosDTO.Parametro3), oParametrosDTO.Parametro4, oParametrosDTO.Parametro5, int.Parse(oParametrosDTO.Parametro7), int.Parse(oParametrosDTO.Parametro8), bool.Parse(oParametrosDTO.Parametro9), bool.Parse(oParametrosDTO.Parametro10), bool.Parse(oParametrosDTO.Parametro11), bool.Parse(oParametrosDTO.Parametro12), bool.Parse(oParametrosDTO.Parametro13), bool.Parse(oParametrosDTO.Parametro14), bool.Parse(oParametrosDTO.Parametro15), bool.Parse(oParametrosDTO.Parametro16), bool.Parse(oParametrosDTO.Parametro17), bool.Parse(oParametrosDTO.Parametro18), bool.Parse(oParametrosDTO.Parametro19));
                return Ok(new { ambiente = Datos.Item1, instructor = Datos.Item2, ficha = Datos.Item3, AmbientesOtraArea = Datos.Item4,NoValidarFicha= Datos.Item5, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult DisponibilidadTransversalCJ(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var Datos = oProgramacionBl.DisponibilidadTransversalCJ(bool.Parse(oParametrosDTO.Parametro6.ToLower()), int.Parse(oParametrosDTO.Parametro1), DateTime.Parse(oParametrosDTO.Parametro2), DateTime.Parse(oParametrosDTO.Parametro3), oParametrosDTO.Parametro4, oParametrosDTO.Parametro5, int.Parse(oParametrosDTO.Parametro7), int.Parse(oParametrosDTO.Parametro8), bool.Parse(oParametrosDTO.Parametro9), bool.Parse(oParametrosDTO.Parametro10), bool.Parse(oParametrosDTO.Parametro11), bool.Parse(oParametrosDTO.Parametro12), bool.Parse(oParametrosDTO.Parametro13), bool.Parse(oParametrosDTO.Parametro14), bool.Parse(oParametrosDTO.Parametro15));
                return Ok(new { ambiente = Datos.Item1, instructor = Datos.Item2, ficha = Datos.Item3, AmbientesOtraArea = Datos.Item4, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult Disponibilidad1(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var Datos = oProgramacionBl.Disponibilidad1(bool.Parse(oParametrosDTO.Parametro6.ToLower()), int.Parse(oParametrosDTO.Parametro1), DateTime.Parse(oParametrosDTO.Parametro2), DateTime.Parse(oParametrosDTO.Parametro3), oParametrosDTO.Parametro4, oParametrosDTO.Parametro5, int.Parse(oParametrosDTO.Parametro7), int.Parse(oParametrosDTO.Parametro8), int.Parse(oParametrosDTO.Parametro9), int.Parse(oParametrosDTO.Parametro10));
                return Ok(new { ambiente = Datos.Item1, instructor = Datos.Item2, ficha = Datos.Item3, MensajeInstructor = Datos.Item4, MensajeFicha = Datos.Item5, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult DisponibilidadTrasversal(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var Datos = oProgramacionBl.DisponibilidadTrasversal(int.Parse(oParametrosDTO.Parametro1), DateTime.Parse(oParametrosDTO.Parametro2), DateTime.Parse(oParametrosDTO.Parametro3), oParametrosDTO.Parametro4, oParametrosDTO.Parametro5, int.Parse(oParametrosDTO.Parametro6), int.Parse(oParametrosDTO.Parametro7), bool.Parse(oParametrosDTO.Parametro8), bool.Parse(oParametrosDTO.Parametro9), bool.Parse(oParametrosDTO.Parametro10), bool.Parse(oParametrosDTO.Parametro11), bool.Parse(oParametrosDTO.Parametro12), bool.Parse(oParametrosDTO.Parametro13), bool.Parse(oParametrosDTO.Parametro14));
                return Ok(new { datos = Datos.Item1, ProgramacionPrincipal = Datos.Item2, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }
        
        [HttpPost]
        public IHttpActionResult ConsultarProgramacionxInstructor(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var Datos = oProgramacionBl.FiltrarInstrutor(int.Parse(oParametrosDTO.Parametro1), int.Parse(oParametrosDTO.Parametro2));
                return Ok(new { programacion = Datos, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ConsultarTransversales(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var Respuesta = oProgramacionBl.ConsultarTransversales(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { success = true, resp = Respuesta.Item1, dias = Respuesta.Item2, ProgramacionPrincipal = Respuesta.Item3 });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult FiltroProgramacionxCoordinador(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var programacion = oProgramacionBl.FiltroProgramacionxCoordinador(int.Parse(oParametrosDTO.Parametro2), int.Parse(oParametrosDTO.Parametro3));
                return Ok(new { success = true, programacion });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ConsultarDiaTransversal(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var respuesta = oProgramacionBl.ConsultarDiaTransversal(DateTime.Parse(oParametrosDTO.Parametro1), DateTime.Parse(oParametrosDTO.Parametro2), oParametrosDTO.Parametro3, oParametrosDTO.Parametro4, int.Parse(oParametrosDTO.Parametro5));
                var Instructor = oProgramacionBl.ConsultarInstructorTransversal(DateTime.Parse(oParametrosDTO.Parametro1), DateTime.Parse(oParametrosDTO.Parametro2), oParametrosDTO.Parametro3, oParametrosDTO.Parametro4, respuesta.Item2);
                return Ok(new { success = true, resp = respuesta.Item1, diasemana = respuesta.Item2, instructor = Instructor });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult GenerarReporte(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var datos = oProgramacionBl.GenerarReporte(DateTime.Parse(oParametrosDTO.Parametro1), DateTime.Parse(oParametrosDTO.Parametro2), int.Parse(oParametrosDTO.Parametro3));
                return Ok(new { success = true, datos });

            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult EnviarProgramacion(List<Ficha_AmbienteDTO> oFicha_AmbienteDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                oProgramacionBl.EnviarProgramacion(oFicha_AmbienteDTO);
                return Ok(new { success = true });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc.Message });
            }
        }
        
        [HttpPost]

        public IHttpActionResult ConsultarAmbientesxSede(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var sedes = oProgramacionBl.ConsultarAmbientesxSede(int.Parse(oParametrosDTO.Parametro1)).OrderBy(x => int.Parse(x.Numero)).ToList();
               
                return Ok(new { success = true, sedes });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc.Message });
            }
        }

        [HttpPost]

        public IHttpActionResult ConsultaEliminarProgramacionTransversales(Ficha_AmbienteDTO oFicha_Ambiente)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var programaciones = oProgramacionBl.ConsultaEliminarProgramacionTransversales(oFicha_Ambiente);
                return Ok(new { success = true, programaciones });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc.Message });
            }
        }

        [HttpPost]

        public IHttpActionResult ProgramasMediaTecnica(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramaBl oProgramaBl = new ProgramaBl();
                var datos = oProgramaBl.ProgramasMediaTecnica(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { success = true, datos });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ConsultarPogramacionesInstructor(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var datos = oProgramacionBl.ConsultarPogramacionesInstructor(oParametrosDTO.Parametro1);
                return Ok(new { success = true, datos });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult GuardarPrestamoLLaves(Ficha_AmbienteDTO oProgramacion)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var datos = oProgramacionBl.GuardarPrestamoLLaves(oProgramacion);
                return Ok(new { success = true, datos });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ReporteLlaves(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var datos = oProgramacionBl.ReporteLlaves(DateTime.Parse(oParametrosDTO.Parametro1), DateTime.Parse(oParametrosDTO.Parametro2), int.Parse(oParametrosDTO.Parametro3));
                return Ok(new { success = true, datos });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult AmbientesDisponibles(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var datos = oProgramacionBl.AmbientesDisponibles(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { success = true, datos });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult RegresarLlavesAmbientesDisponibles(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var datos = oProgramacionBl.RegresarLlavesAmbientesDisponibles(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { success = true, datos });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult EliminarReciboLlaves(Ficha_AmbienteDTO oProgramacion)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var datos = oProgramacionBl.EliminarReciboLlaves(oProgramacion);
                return Ok(new { success = true, datos });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult AmbientesSinProgramacion(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var datos = oProgramacionBl.AmbientesSinProgramacion(oParametrosDTO.Parametro1, oParametrosDTO.Parametro2,int.Parse(oParametrosDTO.Parametro3));
                return Ok(new { success = true, datos });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult GuardarPrestamoLLavesAmbientes(AmbienteDTO oAmbiente)
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var datos = oProgramacionBl.GuardarPrestamoLLavesAmbientes(oAmbiente);
                return Ok(new { success = true, datos });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult AmbientesEntregarLlaves()
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                var datos = oProgramacionBl.AmbientesEntregarLlaves();
                return Ok(new { success = true, datos });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult RegistrarProgramacionesPrestamoLLaves()
        {
            try
            {
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
              oProgramacionBl.RegistrarProgramacionesPrestamoLLaves();
                return Ok(new { success = true });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ConsultarLLavesEditar(ParametrosDTO oPametrosDTO)
        {
            try
            {
                var datos = new List<Ficha_AmbienteDTO>();
                ProgramacionBl oProgramacionBl = new ProgramacionBl();
                if (int.Parse(oPametrosDTO.Parametro1) == 0)
                {
                     datos = oProgramacionBl.ConsultarLLavesEditar();
                }
                else
                {
                     datos = oProgramacionBl.ConsultarLLavesEditar(int.Parse(oPametrosDTO.Parametro1));
                }
               
                
                return Ok(new { success = true, datos });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc.Message });
            }
        }
    }
}
