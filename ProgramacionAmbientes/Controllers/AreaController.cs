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

    public class AreaController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GuardarArea(Area oArea)
        {
            try
            {
                AreaBl oAreaBl = new AreaBl();
                var Area = oAreaBl.GuardarArea(oArea);
                if (Area == true)
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

        public IHttpActionResult EliminarArea(ParametrosDTO oParametrosDTO)
        {
            try
            {
                bool Respuesta = false;
                int contadorTrue = 0;
                int contadorFalse = 0;
                AreaBl oAreaBl = new AreaBl();
                foreach (var item in oParametrosDTO.Parametros)
                {

                 Respuesta = oAreaBl.EliminarArea(int.Parse(item.Parametro1));

                    if (Respuesta)
                    {
                        contadorTrue++;
                    }
                    else
                    {
                        contadorFalse++;
                    }
                
                }
                return Ok(new { respuesta = Respuesta, contadorTrue = contadorTrue, contadorFalse= contadorFalse, success = true,  });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }

        }

        [HttpPost]

        public IHttpActionResult ModificarArea(ParametrosDTO oParametrosDTO)
        {
            try
            {
                AreaBl oAreaBl = new AreaBl();
                var Area = oAreaBl.ConsultarAreaId(int.Parse(oParametrosDTO.Parametro1));

                return Ok(new { success = true, Area });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }

        }


        [HttpPost]

        public IHttpActionResult GuardarModificacionArea(Area oArea)
        {
            try
            {
                AreaBl oAreaBl = new AreaBl();
                oAreaBl.ActualizarRegistro(oArea);

                return Ok(new { success = true });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }

        }

        [HttpPost]
        public IHttpActionResult ConsultarCoordinaciones()
        {
            try
            {
                AreaBl oAreaBl = new AreaBl();
                var Coordinacion = oAreaBl.ConsultarCoordinaciones();
                return Ok(new { success = true, coordinacion = Coordinacion });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        //[HttpGet]
        //public IHttpActionResult ConsultarAreas()
        //{
        //    try
        //    {
        //        AreaBl oAreaBl = new AreaBl();
        //        var Datos = oAreaBl.ConsultarAreas();
        //        return Ok(new { datos = Datos, success = true });

        //    }
        //    catch (Exception exc)
        //    {
        //        return Ok(new { success = false, exc = exc.Message });
        //    }
        //}
        [HttpGet]
        public IHttpActionResult ConsultarAreas()
        {
            try
            {
                AreaBl oAreaBl = new AreaBl();
                List<ParametrosDTO> ListaParametros = new List<ParametrosDTO>();
                var Datos = oAreaBl.ConsultarAreas();
                CoordinacionBl oCoordinacionBl = new CoordinacionBl();

                foreach (var item in Datos)
                {
                    ParametrosDTO oParametrosDTO = new ParametrosDTO();
                    //var NombreCoordinacion = oCoordinacionBl.ConsultarNombreCoordinacion(item.IdCoordinacion);
                    oParametrosDTO.Parametro1 = item.IdArea.ToString();
                    oParametrosDTO.Parametro2 = item.Codigo.ToString();
                    //oParametrosDTO.Parametro3 = NombreCoordinacion;
                    oParametrosDTO.Parametro4 = item.Nombre;
                    oParametrosDTO.Parametro5 = item.Descripcion;
                    ListaParametros.Add(oParametrosDTO);
                }
                return Ok(new { datos = ListaParametros, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }
    }
}