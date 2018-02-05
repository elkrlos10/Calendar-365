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
    public class ProgramaController : ApiController
    {

        [HttpPost]
        public IHttpActionResult GuardarPrograma(Programa oPrograma)
        {
            try
            {
                ProgramaBl oProgramaBl = new ProgramaBl();
                oProgramaBl.GuardarPrograma(oPrograma);
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
                AreaBl AreaBl = new AreaBl();

                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();


                foreach (var item in Datos)
                {
                    ParametrosDTO oParametro = new ParametrosDTO();
                    var NombreArea = AreaBl.ConsultarAreaId(item.IdArea);
                    oParametro.Parametro1 = item.IdPrograma.ToString();
                    oParametro.Parametro2 = item.CodigoPrograma.ToString();
                    oParametro.Parametro3 = item.Nivel;
                    oParametro.Parametro4 = item.LineaTecnologica;
                    oParametro.Parametro5 = item.Red_Tecnologica;
                    oParametro.Parametro6 = item.Perfil_Instructor;
                    oParametro.Parametro7 = item.Version_Programa.ToString();
                    oParametro.Parametro8 = NombreArea.Nombre.ToString();
                    oParametro.Parametro9 = item.NombrePrograma;

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

        public IHttpActionResult EliminarPrograma(ParametrosDTO oParametros)
        {
            try
            {
                bool Respuesta = false;
                int contadorTrue = 0;
                int contadorFalse = 0;
                ProgramaBl oProgramaBl = new ProgramaBl();
                foreach (var item in oParametros.Parametros)
                {

                    Respuesta = oProgramaBl.EliminarPrograma(int.Parse(item.Parametro1));

                    if (Respuesta)
                    {
                        contadorTrue++;
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

        public IHttpActionResult ModificarPrograma(ParametrosDTO oParametros)
        {
            try
            {
                ProgramaBl oProgramaBl = new ProgramaBl();
                var programa = oProgramaBl.ConsultarProgramaId(int.Parse(oParametros.Parametro1));

                return Ok(new { success = true, programa = programa });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }

        }


        [HttpPost]

        public IHttpActionResult GuardarModificacionPrograma(Programa oPrograma)
        {
            try
            {
                ProgramaBl oProgramaBl = new ProgramaBl();
                oProgramaBl.ActualizarRegistro(oPrograma);

                return Ok(new { success = true });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }

        }

        [HttpGet]
        public IHttpActionResult ConsultarAreas()
        {
            try
            {
                AreaBl oAreaBl = new AreaBl();

                var Datos = oAreaBl.ConsultarAreas();

                return Ok(new { datos = Datos, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ConsultarProgramaxCoordinacion(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramaBl oProgramaBl = new ProgramaBl();
                var programa = oProgramaBl.ConsultarProgramaxCoordinacion(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { success = true, programa });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ProgramaxCoordinacion(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramaBl oProgramaBl = new ProgramaBl();
                var Datos = oProgramaBl.ConsultarProgramaxCoordinacion(int.Parse(oParametrosDTO.Parametro1));
                AreaBl AreaBl = new AreaBl();

                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();


                foreach (var item in Datos)
                {
                    ParametrosDTO oParametro = new ParametrosDTO();
                    var NombreArea = AreaBl.ConsultarAreaId(item.IdArea);
                    oParametro.Parametro1 = item.IdPrograma.ToString();
                    oParametro.Parametro2 = item.CodigoPrograma.ToString();
                    oParametro.Parametro3 = item.Nivel;
                    oParametro.Parametro4 = item.LineaTecnologica;
                    oParametro.Parametro5 = item.Red_Tecnologica;
                    oParametro.Parametro6 = item.Perfil_Instructor;
                    oParametro.Parametro7 = item.Version_Programa.ToString();
                    oParametro.Parametro8 = NombreArea.Nombre.ToString();
                    oParametro.Parametro9 = item.NombrePrograma;

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
        public IHttpActionResult ProgramaTituladosxCoordinacion(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramaBl oProgramaBl = new ProgramaBl();
                var Datos = oProgramaBl.ProgramaTituladosxCoordinacion(int.Parse(oParametrosDTO.Parametro1));
                AreaBl AreaBl = new AreaBl();

                return Ok(new { datos = Datos, success = true });

                //return Ok(new { datos = Datos, success = true });

            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ProgramaVirtualesTitulados(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramaBl oProgramaBl = new ProgramaBl();
                var Datos = oProgramaBl.ProgramaVirtualesTitulados(int.Parse(oParametrosDTO.Parametro1));
                AreaBl AreaBl = new AreaBl();

                return Ok(new { datos = Datos, success = true });

                //return Ok(new { datos = Datos, success = true });

            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ProgramasComplementariosxCoordinacion(ParametrosDTO oParametrosDTO)
        {
            try
            {
                ProgramaBl oProgramaBl = new ProgramaBl();
                var Datos = oProgramaBl.ProgramasComplementariosxCoordinacion(int.Parse(oParametrosDTO.Parametro1));
                AreaBl AreaBl = new AreaBl();

                return Ok(new { datos = Datos, success = true });

                //return Ok(new { datos = Datos, success = true });

            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ConsultarProgramaxArea(ParametrosDTO oParametrosDTO)
        {
            try
            {

                ProgramaBl oProgramaBl = new ProgramaBl();
                var programa = oProgramaBl.ConsultarProgramaxArea(int.Parse(oParametrosDTO.Parametro1));
                return Ok(new { success = true, programa });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }


        [HttpPost]
        public IHttpActionResult ConsultarPragamaTransversal()
        {
            try
            {
                ProgramaBl oProgramaBl = new ProgramaBl();
                var programa = oProgramaBl.ConsultarPragamaTransversal();
                return Ok(new { success = true, programa });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ReporteProgramas(ParametrosDTO oParametros)
        {
            try
            {
                ProgramaBl oProgramaBl = new ProgramaBl();

                var Datos = oProgramaBl.ReporteProgramas(int.Parse(oParametros.Parametro1));

                return Ok(new { success = true, Datos });
            }
            catch (Exception exc )
            {

                return Ok(new { success = false , exc.Message});
            }
        }
            
      

    }
}