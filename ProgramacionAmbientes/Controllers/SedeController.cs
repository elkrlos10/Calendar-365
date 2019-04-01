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
    public class SedeController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GuardarSede(Sede oSede)
        {
            try
            {
                SedeBl oSedeBl = new SedeBl();
                var Sede = oSedeBl.GuardarSede(oSede);
                if (Sede == true)
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

        [HttpGet]
        public IHttpActionResult ConsultarSedes()
        {
            try
            {
                SedeBl oSedeBl = new SedeBl();
                var Datos = oSedeBl.ConsultarSedes();


                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();


                foreach (var item in Datos)
                {
                    ParametrosDTO oParametro = new ParametrosDTO();
                    var nombreMunicipio = oSedeBl.consultarMunicipioxId(item.IdMunicipio);
                    oParametro.Parametro1 = item.IdSede.ToString();
                    oParametro.Parametro2 = nombreMunicipio.NombreMunicipio;
                    oParametro.Parametro3 = item.Nombre_Sede;
                    oParametro.Parametro4 = item.Direccion;
                    oParametro.Parametro5 = item.Codigo.ToString();
                    ListaParametro.Add(oParametro);
                }



                return Ok(new { datos = ListaParametro, success = true });

                //return Ok(new { datos = Datos, success = true });

            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult EliminarSede(ParametrosDTO oParametros)
        {
            try
            {
                bool Respuesta = false;
                int contadorTrue = 0;
                int contadorFalse = 0;
                SedeBl oSedeBl = new SedeBl();
                foreach (var item in oParametros.Parametros)
                {

                    Respuesta = oSedeBl.EliminarSede(int.Parse(item.Parametro1));

                    if (Respuesta)
                    {
                        contadorTrue++;
                    }
                    else
                    {
                        contadorFalse++;
                    }

                }
                return Ok(new { respuesta = Respuesta, contadorTrue = contadorTrue, contadorFalse = contadorFalse, success = true, });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }

        }

        [HttpPost]

        public IHttpActionResult ModificarSede(ParametrosDTO oParametros)
        {
            try
            {
                SedeBl oSedeBl = new SedeBl();
                var Sede = oSedeBl.ConsultarSedeId(int.Parse(oParametros.Parametro1));

                return Ok(new { success = true, sede = Sede });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }

        }


        [HttpPost]

        public IHttpActionResult GuardarModificacionSede(Sede oSede)
        {
            try
            {
                SedeBl oSedeBl = new SedeBl();
                oSedeBl.ActualizarRegistro(oSede);

                return Ok(new { success = true });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }

        }

        [HttpPost]
        public IHttpActionResult ConsultarDepartamentos()
        {
            try
            {
                SedeBl oSedeBl = new SedeBl();
                var Datos = oSedeBl.consultarDepartamento();
                return Ok(new { datos = Datos, success = true });

            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }


        [HttpPost]
        public IHttpActionResult consultarDepartamentoxMunicipio(ParametrosDTO oParametrosDTO)
        {
            try
            {
                SedeBl oSedeBl = new SedeBl();
                var Datos = oSedeBl.consultarDepartamentoxMunicipio(oParametrosDTO.Parametro1);
                return Ok(new { datos = Datos, success = true });

            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ConsultarMunicipios(ParametrosDTO oParametrosDTO)
        {
            try
            {
                SedeBl oSedeBl = new SedeBl();
                var Datos = oSedeBl.consultarMunicipios(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { datos = Datos, success = true });

            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult CodigoSede()
        {
            try
            {
                SedeBl oSedeBl = new SedeBl();
                var Datos = oSedeBl.CodigoSede();
                return Ok(new { datos = Datos, success = true });

            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ConsultarColegios()
        {
            try
            {
                SedeBl oSedeBl = new SedeBl();
                var Datos = oSedeBl.ConsultarColegios();

                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();


                foreach (var item in Datos)
                {
                    ParametrosDTO oParametro = new ParametrosDTO();
                    var nombreMunicipio = oSedeBl.consultarMunicipioxId(item.IdMunicipio);
                    oParametro.Parametro1 = item.IdSede.ToString();
                    oParametro.Parametro2 = nombreMunicipio.NombreMunicipio;
                    oParametro.Parametro3 = item.Nombre_Sede;
                    oParametro.Parametro4 = item.Direccion;
                    oParametro.Parametro5 = item.Codigo.ToString();
                    ListaParametro.Add(oParametro);
                }

                return Ok(new { datos = ListaParametro, colegios = Datos, success = true });

            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]

        public IHttpActionResult ConsultarSedexId(ParametrosDTO oParametros)
        {
            try
            {
                SedeBl oSedeBl = new SedeBl();
                var Sede = oSedeBl.ConsultarSedeId(int.Parse(oParametros.Parametro1));

                return Ok(new { success = true, sede = Sede });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }

        }

        [HttpPost]

        public IHttpActionResult ConsultarUsuarioSede()
        {
            try
            {
                SedeBl oSedeBl = new SedeBl();
                var Sede = oSedeBl.ConsultarUsuarioSede();

                return Ok(new { success = true, datos = Sede });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }

        }

        [HttpPost]

        public IHttpActionResult ModificarUsuario(Usuario oUsuario)
        {
            try
            {
                SedeBl oSedeBl = new SedeBl();
                var users = oSedeBl.ModificarUsuario(oUsuario);

                return Ok(new { success = true, datos = users });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }

        }


    }
}