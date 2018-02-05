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
    public class CoordinacionController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GuardarCoordinacion(Coordinacion oCoordinacion)
        {
            try
            {
                CoordinacionBl oCoordinacionBl = new CoordinacionBl();
                var coordinador = oCoordinacionBl.GuardarCoordinacion(oCoordinacion);
                return Ok(new { success = true, coordinador });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpGet]
        public IHttpActionResult ConsultarCoordinacion()
        {

            try
            {
                CoordinacionBl oCoordinacionBl = new CoordinacionBl();
                var Datos = oCoordinacionBl.ConsultarCoordinacion();
                return Ok(new { success = true, datos = Datos });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ModificarCoordinacion(Coordinacion oCoordinacion)
        {
            try
            {
                CoordinacionBl oCoordinacionBl = new CoordinacionBl();
                var Coordinacion = oCoordinacionBl.ConsultarCoordinacionId(oCoordinacion.IdCoordinacion);
                return Ok(new { success = true, Coordinacion });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult GuardarModificacionCoordinacion(Coordinacion oCoordinacion)
        {
            try
            {
                CoordinacionBl oCoordinacionBl = new CoordinacionBl();
                oCoordinacionBl.ActualizarRegistro(oCoordinacion);
                return Ok(new { success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult EliminarCoordinacion(ParametrosDTO oParametrosDTO)
        {
            try
            {
                CoordinacionBl oCoordinacionBl = new CoordinacionBl();
                foreach (var item in oParametrosDTO.Parametros)
                {
                    oCoordinacionBl.EliminarCoordinacion(int.Parse(item.Parametro1));
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