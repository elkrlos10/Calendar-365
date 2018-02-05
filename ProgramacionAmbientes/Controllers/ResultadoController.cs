using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using LogicaNegocio.LogicaNegocio;
using Datos.Modelo;
using ProgramacionAmbientes.Parametros;

namespace ProgramacionAmbientes.Controllers
{
    public class ResultadoController : ApiController
    {
        [HttpGet]
        public IHttpActionResult ConsultarCompetencias()
        {
            try
            {
                ResultadoBl oResultadoBl = new ResultadoBl();
                var Datos = oResultadoBl.ConsultarCompetencias();
                return Ok(new { datos = Datos, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult GuardarResultado(Resultado_Aprendizaje oResultado_Aprendizaje)
        {
            try
            {
                ResultadoBl oResultadoBl = new ResultadoBl();
                var Resultado = oResultadoBl.GuardarResultado(oResultado_Aprendizaje);
                if (Resultado == true)
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

        [HttpPost]
        public IHttpActionResult ConsultarResultados()
        {
            try
            {
                ResultadoBl oResultadoBl = new ResultadoBl();
                var Datos = oResultadoBl.ConsultarResultados();
                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();
                CompetenciaBl oCompetenciaBl = new CompetenciaBl();
                foreach (var item in Datos)
                {
                    ParametrosDTO oParametro = new ParametrosDTO();
                    var NombreCompetencia = oCompetenciaBl.ConsultarCompetenciaId(item.IdCompetencia);
                    oParametro.Parametro1 = item.IdResultado.ToString();
                    oParametro.Parametro2 = NombreCompetencia.Nombre;
                    oParametro.Parametro3 = item.Codigo.ToString();
                    oParametro.Parametro4 = item.Resultado;
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
        public IHttpActionResult ConsultarResultados1()
        {
            try
            {
                ResultadoBl oResultadoBl = new ResultadoBl();
                var Datos = oResultadoBl.ConsultarResultados();
                
                return Ok(new { datos = Datos, success = true });

            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ModificarResultado(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ResultadoBl oResultadoBl = new ResultadoBl();
                var Resultado = oResultadoBl.ConsultarResultadoId(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { success = true, Resultado });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]

        public IHttpActionResult GuardarModificacionResultado(Resultado_Aprendizaje oResultado_Aprendizaje)
        {
            try
            {
                ResultadoBl oResultadoBl = new ResultadoBl();

                oResultadoBl.ActualizarRegistro(oResultado_Aprendizaje);

                return Ok(new { success = true });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }

        }

        [HttpPost]
        public IHttpActionResult EliminarResultado(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ResultadoBl oResultadoBl = new ResultadoBl();

                foreach (var item in oParametrosDTO.Parametros)
                {
                    oResultadoBl.EliminarResultado(int.Parse(item.Parametro1));
                }
                return Ok(new { success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });

            }

        }
    }
}
