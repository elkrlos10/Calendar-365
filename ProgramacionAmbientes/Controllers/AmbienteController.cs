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
    public class AmbienteController: ApiController
    {


        [HttpPost]
        public IHttpActionResult GuardarAmbiente(Ambiente oAmbiente)
        {
            try
            {
                AmbienteBl oAmbienteBl = new AmbienteBl();
                var Ambiente = oAmbienteBl.GuardarAmbiente(oAmbiente);
                if (Ambiente ==  true)
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
        public IHttpActionResult ConsultarNombreArea(int id)
        {
            try
            {
                AreaBl oAreaBl = new AreaBl();
                var nombre = oAreaBl.ConsultarNombreArea(id);
                return Ok(new { success = true, nombre});
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }

        }

        [HttpPost]
        public IHttpActionResult ConsultarAmbiente()
        {
            try
            {
                AmbienteBl oAmbienteBl = new AmbienteBl();

                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();

                var Datos = oAmbienteBl.ConsultarAmbientes();
                AreaBl oAreaBl = new AreaBl();
                SedeBl oSedeBl = new SedeBl();

                foreach (var item in Datos)
                {
                    ParametrosDTO oParametro = new ParametrosDTO();
                    var nombreArea = oAreaBl.ConsultarNombreArea(item.IdArea);
                    var NombreSede = oSedeBl.ConsultarSedeId(item.IdSede);
                    oParametro.Parametro1 = item.IdAmbiente.ToString();
                    oParametro.Parametro2 = nombreArea;
                    oParametro.Parametro3 = item.Numero;
                    oParametro.Parametro4 = item.NumeroEquipos.ToString();
                    oParametro.Parametro5 = item.IdArea.ToString();
                    oParametro.Parametro6 = item.Pantalla.ToString();
                    oParametro.Parametro7 = item.Piso.ToString();
                    oParametro.Parametro8 = NombreSede.Nombre_Sede;
                    oParametro.Parametro9 = item.NumeroMesas.ToString();
                    oParametro.Parametro10 = item.NumeroSillas.ToString();
                    ListaParametro.Add(oParametro);
                }
                return Ok(new { datos = ListaParametro, success = true });

            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ConsultarAmbientesColegios()
        {
            try
            {
                AmbienteBl oAmbienteBl = new AmbienteBl();

                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();

                var Datos = oAmbienteBl.ConsultarAmbientesColegios();
                AreaBl oAreaBl = new AreaBl();
                SedeBl oSedeBl = new SedeBl();

                foreach (var item in Datos)
                {
                    ParametrosDTO oParametro = new ParametrosDTO();
                    var nombreArea = oAreaBl.ConsultarNombreArea(item.IdArea);
                    var NombreSede = oSedeBl.ConsultarSedeId(item.IdSede);
                    oParametro.Parametro1 = item.IdAmbiente.ToString();
                    oParametro.Parametro2 = nombreArea;
                    oParametro.Parametro3 = item.Numero;
                    oParametro.Parametro4 = item.NumeroEquipos.ToString();
                    oParametro.Parametro5 = item.IdArea.ToString();
                    oParametro.Parametro6 = item.Pantalla.ToString();
                    oParametro.Parametro7 = item.Piso.ToString();
                    oParametro.Parametro8 = NombreSede.Nombre_Sede;
                    oParametro.Parametro9 = item.NumeroMesas.ToString();
                    oParametro.Parametro10 = item.NumeroSillas.ToString();
                    ListaParametro.Add(oParametro);
                }
                return Ok(new { datos = ListaParametro, success = true });

            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ConsultarAmbienteSinColegios()
        {
            try
            {
                AmbienteBl oAmbienteBl = new AmbienteBl();

                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();

                var Datos = oAmbienteBl.ConsultarAmbienteSinColegios();
                AreaBl oAreaBl = new AreaBl();
                SedeBl oSedeBl = new SedeBl();

                foreach (var item in Datos)
                {
                    ParametrosDTO oParametro = new ParametrosDTO();
                    var nombreArea = oAreaBl.ConsultarNombreArea(item.IdArea);
                    var NombreSede = oSedeBl.ConsultarSedeId(item.IdSede);
                    oParametro.Parametro1 = item.IdAmbiente.ToString();
                    oParametro.Parametro2 = nombreArea;
                    oParametro.Parametro3 = item.Numero;
                    oParametro.Parametro4 = item.NumeroEquipos.ToString();
                    oParametro.Parametro5 = item.IdArea.ToString();
                    oParametro.Parametro6 = item.Pantalla.ToString();
                    oParametro.Parametro7 = item.Piso.ToString();
                    oParametro.Parametro8 = NombreSede.Nombre_Sede;
                    oParametro.Parametro9 = item.NumeroMesas.ToString();
                    oParametro.Parametro10 = item.NumeroSillas.ToString();
                    ListaParametro.Add(oParametro);
                }
                return Ok(new { datos = ListaParametro, success = true });

            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }


        [HttpPost]
        public IHttpActionResult AmbientesxArea(ParametrosDTO OparametrosDTO)
        {
            try
            {
                AmbienteBl oAmbienteBl = new AmbienteBl();

                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();

                var Datos = oAmbienteBl.AmbientesxArea(int.Parse(OparametrosDTO.Parametro1));
                AreaBl oAreaBl = new AreaBl();
                SedeBl oSedeBl = new SedeBl();

                foreach (var item in Datos)
                {
                    ParametrosDTO oParametro = new ParametrosDTO();
                    var nombreArea = oAreaBl.ConsultarNombreArea(item.IdArea);
                    var NombreSede = oSedeBl.ConsultarSedeId(item.IdSede);
                    oParametro.Parametro1 = item.IdAmbiente.ToString();
                    oParametro.Parametro2 = nombreArea;
                    oParametro.Parametro3 = item.Numero;
                    oParametro.Parametro4 = item.NumeroEquipos.ToString();
                    oParametro.Parametro5 = item.IdArea.ToString();
                    oParametro.Parametro6 = item.Pantalla.ToString();
                    oParametro.Parametro7 = item.Piso.ToString();
                    oParametro.Parametro8 = NombreSede.Nombre_Sede;
                    oParametro.Parametro9 = item.NumeroMesas.ToString();
                    oParametro.Parametro10 = item.NumeroSillas.ToString();
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
        public IHttpActionResult OrganizarPisoAsc()
        {
            try
            {
                AmbienteBl oAmbienteBl = new AmbienteBl();

                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();

                var Datos = oAmbienteBl.OrganizarPisoAsc();
                AreaBl oAreaBl = new AreaBl();
                SedeBl oSedeBl = new SedeBl();

                foreach (var item in Datos)
                {
                    ParametrosDTO oParametro = new ParametrosDTO();
                    var nombreArea = oAreaBl.ConsultarNombreArea(item.IdArea);
                    var NombreSede = oSedeBl.ConsultarSedeId(item.IdSede);
                    oParametro.Parametro1 = item.IdAmbiente.ToString();
                    oParametro.Parametro2 = nombreArea;
                    oParametro.Parametro3 = item.Numero;
                    oParametro.Parametro4 = item.NumeroEquipos.ToString();
                    oParametro.Parametro5 = item.IdArea.ToString();
                    oParametro.Parametro6 = item.Pantalla.ToString();
                    oParametro.Parametro7 = item.Piso.ToString();
                    oParametro.Parametro8 = NombreSede.Nombre_Sede;
                    oParametro.Parametro9 = item.NumeroMesas.ToString();
                    oParametro.Parametro10 = item.NumeroSillas.ToString();
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
        public IHttpActionResult OrganizarPisoDesc()
        {
            try
            {
                AmbienteBl oAmbienteBl = new AmbienteBl();

                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();

                var Datos = oAmbienteBl.OrganizarPisoDesc();
                AreaBl oAreaBl = new AreaBl();
                SedeBl oSedeBl = new SedeBl();

                foreach (var item in Datos)
                {
                    ParametrosDTO oParametro = new ParametrosDTO();
                    var nombreArea = oAreaBl.ConsultarNombreArea(item.IdArea);
                    var NombreSede = oSedeBl.ConsultarSedeId(item.IdSede);
                    oParametro.Parametro1 = item.IdAmbiente.ToString();
                    oParametro.Parametro2 = nombreArea;
                    oParametro.Parametro3 = item.Numero;
                    oParametro.Parametro4 = item.NumeroEquipos.ToString();
                    oParametro.Parametro5 = item.IdArea.ToString();
                    oParametro.Parametro6 = item.Pantalla.ToString();
                    oParametro.Parametro7 = item.Piso.ToString();
                    oParametro.Parametro8 = NombreSede.Nombre_Sede;
                    oParametro.Parametro9 = item.NumeroMesas.ToString();
                    oParametro.Parametro10 = item.NumeroSillas.ToString();
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
        public IHttpActionResult OrganizarNumeroAsc()
        {
            try
            {
                AmbienteBl oAmbienteBl = new AmbienteBl();

                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();

                var Datos = oAmbienteBl.OrganizarNumeroAsc();
                AreaBl oAreaBl = new AreaBl();
                SedeBl oSedeBl = new SedeBl();

                foreach (var item in Datos)
                {
                    ParametrosDTO oParametro = new ParametrosDTO();
                    var nombreArea = oAreaBl.ConsultarNombreArea(item.IdArea);
                    var NombreSede = oSedeBl.ConsultarSedeId(item.IdSede);
                    oParametro.Parametro1 = item.IdAmbiente.ToString();
                    oParametro.Parametro2 = nombreArea;
                    oParametro.Parametro3 = item.Numero;
                    oParametro.Parametro4 = item.NumeroEquipos.ToString();
                    oParametro.Parametro5 = item.IdArea.ToString();
                    oParametro.Parametro6 = item.Pantalla.ToString();
                    oParametro.Parametro7 = item.Piso.ToString();
                    oParametro.Parametro8 = NombreSede.Nombre_Sede;
                    oParametro.Parametro9 = item.NumeroMesas.ToString();
                    oParametro.Parametro10 = item.NumeroSillas.ToString();
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
        public IHttpActionResult OrganizarNumeroDesc()
        {
            try
            {
                AmbienteBl oAmbienteBl = new AmbienteBl();

                List<ParametrosDTO> ListaParametro = new List<ParametrosDTO>();

                var Datos = oAmbienteBl.OrganizarNumeroDesc();
                AreaBl oAreaBl = new AreaBl();
                SedeBl oSedeBl = new SedeBl();

                foreach (var item in Datos)
                {
                    ParametrosDTO oParametro = new ParametrosDTO();
                    var nombreArea = oAreaBl.ConsultarNombreArea(item.IdArea);
                    var NombreSede = oSedeBl.ConsultarSedeId(item.IdSede);
                    oParametro.Parametro1 = item.IdAmbiente.ToString();
                    oParametro.Parametro2 = nombreArea;
                    oParametro.Parametro3 = item.Numero;
                    oParametro.Parametro4 = item.NumeroEquipos.ToString();
                    oParametro.Parametro5 = item.IdArea.ToString();
                    oParametro.Parametro6 = item.Pantalla.ToString();
                    oParametro.Parametro7 = item.Piso.ToString();
                    oParametro.Parametro8 = NombreSede.Nombre_Sede;
                    oParametro.Parametro9 = item.NumeroMesas.ToString();
                    oParametro.Parametro10 = item.NumeroSillas.ToString();
                    ListaParametro.Add(oParametro);
                }
                return Ok(new { datos = ListaParametro, success = true });

            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]

        public IHttpActionResult EliminarAmbiente(ParametrosDTO oParametrosDTO)
        {
            try
            {
                AmbienteBl oAmbienteBl = new AmbienteBl();

                foreach (var item in oParametrosDTO.Parametros)
                {
                    oAmbienteBl.EliminarAmbiente(int.Parse(item.Parametro1));
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

        public IHttpActionResult ModificarAmbiente(ParametrosDTO oParametro)
        {
            try
            {
                AmbienteBl oAmbienteBl = new AmbienteBl();
                var Ambiente = oAmbienteBl.ConsultarAmbienteId(int.Parse(oParametro.Parametro1));

                return Ok(new { success = true, Ambiente });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc = exc.Message });
            }

        }

        [HttpPost]

        public IHttpActionResult GuardarModificacionAmbiente(Ambiente oAmbiente)
        {
            try
            {
                AmbienteBl oAmbienteBl = new AmbienteBl();

                oAmbienteBl.ActualizarRegistro(oAmbiente);

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

        [HttpGet]
        public IHttpActionResult ConsultarSedes()
        {
            try
            {
                SedeBl oSedeBl = new SedeBl();
                var Datos = oSedeBl.ConsultarSedes();
                return Ok(new { datos = Datos, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult ConsultarSedesColegios()
        {
            try
            {
                SedeBl oSedeBl = new SedeBl();
                var Datos = oSedeBl.ConsultarColegios();
                return Ok(new { datos = Datos, success = true });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false, exc = exc.Message });
            }
        }


        [HttpPost]
        public IHttpActionResult ConsultarAmbientesCoordinador(ParametrosDTO oParametrosDTO)
        {
            AmbienteBl oAmbienteBl = new AmbienteBl();
            var ambiente = oAmbienteBl.ConsultarAmbientesCoordinador(int.Parse(oParametrosDTO.Parametro1));
            return Ok(new { success = true, ambiente });
        }


        [HttpPost]
        public IHttpActionResult ambientesDisponibles(ParametrosDTO oParamaetrosDTO)
        {

            try
            {
                AmbienteBl oAmbienteBl = new AmbienteBl();
                if (oParamaetrosDTO.Parametro6 == "")
                {
                    oParamaetrosDTO.Parametro6 = "0";
                }
                var Datos = oAmbienteBl.ambientesDisponibles(DateTime.Parse(oParamaetrosDTO.Parametro1), DateTime.Parse(oParamaetrosDTO.Parametro2), oParamaetrosDTO.Parametro3, oParamaetrosDTO.Parametro4, int.Parse(oParamaetrosDTO.Parametro5), int.Parse(oParamaetrosDTO.Parametro6),bool.Parse(oParamaetrosDTO.Parametro7), bool.Parse(oParamaetrosDTO.Parametro8), bool.Parse(oParamaetrosDTO.Parametro9), bool.Parse(oParamaetrosDTO.Parametro10), bool.Parse(oParamaetrosDTO.Parametro11));
                return Ok(new { success = true, datos = Datos });
            }
            catch (Exception exc)
            {

                return Ok(new { success = false, exc.Message });
            }
        }
    }
}
