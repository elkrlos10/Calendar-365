using Datos.Modelo;
using LogicaNegocio.LogicaNegocio;
using ProgramacionAmbientes.Parametros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ProgramacionAmbientes.Controllers
{
    public class UsuarioController : ApiController
    {
        [HttpPost]
        public IHttpActionResult IniciarSesion(Usuario oUsuario)
        {
            try
            {
                UsuarioBl oUsuarioBl = new UsuarioBl();
                var usuario = oUsuarioBl.IniciarSesion(oUsuario.NombreUsuario, oUsuario.Password);
                if (usuario == null)
                {
                    return Ok(new { success = true, usuario, resp = 1 });
                }
                return Ok(new { success = true, usuario });
            }
            catch (Exception exc)
            {
                return Ok(new { success = false });
            }
        }

        [HttpPost]
        public IHttpActionResult RecuperarPassword(ParametrosDTO oParametrosDTO)
        {
            try
            {
                UsuarioBl oUsuarioBl = new UsuarioBl();
                var Email = oUsuarioBl.RecuperarPassword(oParametrosDTO.Parametro1, oParametrosDTO.Parametro2);
                if (Email != null)
                {
                    return Ok(new { success = true, Email });
                }
                else
                {
                    return Ok(new { success = false });
                }

            }
            catch (Exception exc)
            {
                return Ok(new { success = false });
            }

        }

        [HttpPost]
        public IHttpActionResult CambiarPassword(ParametrosDTO oParametrosDTO)
        {
            try
            {
                UsuarioBl oUsuarioBl = new UsuarioBl();
                oUsuarioBl.CambiarContrasena(oParametrosDTO.Parametro1, oParametrosDTO.Parametro2,int.Parse(oParametrosDTO.Parametro3));
                return Ok(new { success = true });
            }
            catch (Exception exc )
            {

                return Ok(new { success = false, exc.Message });
            }
           
                 

        }
    }
}