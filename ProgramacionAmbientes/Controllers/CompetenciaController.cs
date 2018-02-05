using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using LogicaNegocio.LogicaNegocio;
using Datos.Modelo;
using ProgramacionAmbientes.Parametros;

namespace ProgramacionAmbientes.Controllers
{

    public class CompetenciaController : ApiController
    {
        //Función del servicio para Guardar las competencias
        [HttpPost]
        public IHttpActionResult GuardarCompetencia(Competencia oCompetencia)
        {
            try
            {
                CompetenciaBl oCompetenciaBl = new CompetenciaBl();
                var Competencia = oCompetenciaBl.GuardarCompetencia(oCompetencia);
                if (Competencia == true)
                {
                    return Ok(new { success = true });
                }
                else
                {
                    return Ok(new { success = false });
                }
                
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }

        }


        //Función del servicio para eliminar una competencia
        [HttpPost]

        public IHttpActionResult EliminarCompetencia(ParametrosDTO oParametrosDTO)
        {
            try
            {
                CompetenciaBl oCompetenciaBl = new CompetenciaBl();

                foreach (var item in oParametrosDTO.Parametros)
                {
                    oCompetenciaBl.EliminarCompetencia(int.Parse(item.Parametro1));
                }
                return Ok(new { success = true });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }

        }

        //Función del servicio para modificar una competencia
        [HttpPost]

        public IHttpActionResult ModificarCompetencia(ParametrosDTO oParametro)
        {
            try
            {
                CompetenciaBl oCompetenciaBl = new CompetenciaBl();
                var Competencia = oCompetenciaBl.ConsultarCompetenciaId(int.Parse(oParametro.Parametro1));

                return Ok(new { success = true, Competencia });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }

        }

        //Función 2 del servicio para modificar una competencia
        [HttpPost]

        public IHttpActionResult GuardarModificacionCompetencia(Competencia oCompetencia)
        {
            try
            {



                CompetenciaBl oCompetenciaBl = new CompetenciaBl();

                oCompetenciaBl.ActualizarRegistro(oCompetencia);

                return Ok(new { success = true });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }

        }

        //Función del servicio para consultar una competencia
       [HttpGet]
        public IHttpActionResult ConsultarCompetencia()
        {
            try
            {
                CompetenciaBl oCompetenciaBl = new CompetenciaBl();

                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();

                var Datos = oCompetenciaBl.ConsultarCompetencias();
                ProgramaBl oProgramaBl = new ProgramaBl();


                foreach (var item in Datos)
                {
                    ParametrosDTO oParametro = new ParametrosDTO();
                    var nombrePrograma = oProgramaBl.ConsultarProgramaId(item.IdPrograma);
                    oParametro.Parametro1 = item.IdCompetencia.ToString();
                    oParametro.Parametro2 = nombrePrograma.NombrePrograma;
                    oParametro.Parametro3 = item.Nombre;
                    oParametro.Parametro4 = item.Horas.ToString();
                    oParametro.Parametro5 = item.Codigo.ToString();
                    ListaParametro.Add(oParametro);
                }



                return Ok(new { datos = ListaParametro, success = true });

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
                CompetenciaBl oCompetenciaBl = new CompetenciaBl();
                var Datos = oCompetenciaBl.ConsultarProgramas();
                return Ok(new { datos = Datos, success = true });
            }
            catch (Exception exc)
            {

                return Ok(new { exc, success = false });
            }
        }

        [HttpGet]
        public IHttpActionResult ConsultarCompetencias1()
        {
            try
            {
                CompetenciaBl oCompetenciaBl = new CompetenciaBl();
                var Competencias = oCompetenciaBl.ConsultarCompetencias();
                return Ok(new { datos = Competencias, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ConsultarCompetenciasxPrograma(ParametrosDTO oParametrosDTO)
        {
            try
            {
                CompetenciaBl oCompetenciaBl = new CompetenciaBl();
                var competencia = oCompetenciaBl.ConsultarCompetenciasxPrograma(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { success = true, competencia });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }
    }
}
