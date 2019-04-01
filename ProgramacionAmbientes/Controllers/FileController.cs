using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Globalization;
using System.Text;
using System.Net.Mail;
using LogicaNegocio.LogicaNegocio;
using Datos.Modelo;
using ProgramacionAmbientes.Controllers;
using LinqToExcel;
using System.Data.OleDb;
using System.Data;

namespace CORONA.ValidacionRef.Servicios.Controllers
{
    public class FileController : ApiController
    {
        //Funnciones para la carga de los archivos de excel

        [HttpPost]
        public IHttpActionResult UploadFile()
        {
            Model1 entity = new Model1();
            try
            {
                //                List<LogResponseDTO> lstErrores = new List<LogResponseDTO>();
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var fileSavePath = string.Empty;

                    var docfiles = new List<string>();

                    var URLArchivo = "";

                    foreach (string file in httpRequest.Files)
                    {

                        var postedFile = httpRequest.Files[file];
                        //var filePath = HttpContext.Current.Server.MapPath("/UploadedFiles/");
                        var filePath = "C:/UploadedFiles/";

                        var GUID = Guid.NewGuid().ToString();

                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        fileSavePath = Path.Combine(filePath, GUID + "." + postedFile.FileName.Split('.')[1]);



                        postedFile.SaveAs(fileSavePath);

                        docfiles.Add(filePath);

                        URLArchivo = "C:/UploadedFiles/" + GUID + "." + postedFile.FileName.Split('.')[1];


                        string e = Path.GetExtension(URLArchivo);
                        if (e != ".xlsx")
                        {
                            return Ok(new { success = false, message = "La extencion del archivo no es valida" });
                        }

                    }


                    InstructorBl instructor = new InstructorBl();

                    var book = new ExcelQueryFactory(URLArchivo);

                    var hoja = book.GetWorksheetNames();
                    var resultado = (from i in book.Worksheet(hoja.FirstOrDefault())
                                     select i).ToList();

                    foreach (var values in resultado)
                    {
                        Instructor oInstructor = new Instructor();
                        var cedula = instructor.ConsultarInstructorCedula(values[2]);
                        if (cedula == null)
                        {
                            oInstructor.Nombre = values[0];
                            oInstructor.Apellido = values[1];
                            oInstructor.Cedula = values[2];
                            oInstructor.Email = values[3];
                            oInstructor.Estado = true;
                            oInstructor.Telefono = values[4];

                            var codigo = int.Parse(values[5]);
                            var area = (from i in entity.Area
                                        where i.Codigo == codigo
                                        select i).FirstOrDefault();
                            oInstructor.IdArea = area.IdArea;
                            if (values[7].ToString().ToLower() == "transversal")
                            {
                                oInstructor.TipoInstructor = 2;
                            }
                            else
                            {
                                oInstructor.TipoInstructor = 1;
                            }

                            if (values[6].ToString().ToLower() == "contratista")
                            {
                                oInstructor.TipoContrato = 1;
                            }
                            else
                            {
                                oInstructor.TipoContrato = 2;
                            }
                            //oInstructor.IdCompetencia = int.Parse(values[4]);

                            ProgramaBl oProgramaBl = new ProgramaBl();
                            AreaBl oAreaBl = new AreaBl();

                            // var idProgrma = oProgramaBl.ConsultarProgramaCodigo(int.Parse(values[5]));
                            var idArea = oAreaBl.ConsultarAreaCodigo(int.Parse(values[5]));

                            oInstructor.IdArea = idArea.IdArea;

                            // oListaInstructor.Add(oInstructor);

                            instructor.GuardarInstructor(oInstructor);
                        }
                    }
                    return Ok(new { success = true, path = URLArchivo, });

                }
                else
                {
                    return Ok(new { success = false, message = "No File" });
                }

            }
            catch (Exception exc)
            {

                return Ok(new { success = false, message = exc.Message });
            }
        }

        public IHttpActionResult UploadFileCompetencia()
        {
            try
            {
                //                List<LogResponseDTO> lstErrores = new List<LogResponseDTO>();
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var fileSavePath = string.Empty;

                    var docfiles = new List<string>();

                    var URLArchivo = "";

                    foreach (string file in httpRequest.Files)
                    {

                        var postedFile = httpRequest.Files[file];
                        //var filePath = HttpContext.Current.Server.MapPath("~/UploadedFiles/");
                        var filePath = "C:/UploadedFiles/";

                        var GUID = Guid.NewGuid().ToString();

                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        fileSavePath = Path.Combine(filePath, GUID + "." + postedFile.FileName.Split('.')[1]);



                        postedFile.SaveAs(fileSavePath);

                        docfiles.Add(filePath);

                        //URLArchivo = "~/UploadedFiles/" + GUID + "." + postedFile.FileName.Split('.')[1];
                        URLArchivo = "C:/UploadedFiles/" + GUID + "." + postedFile.FileName.Split('.')[1];



                        string e = Path.GetExtension(URLArchivo);
                        if (e != ".xlsx")
                        {
                            return Ok(new { success = false, message = "La extencion del archivo no es valida" });
                        }

                    }

                    string FilePath = URLArchivo.Split('/')[2];

                    //  var reader = new StreamReader(File.OpenRead(HttpContext.Current.Server.MapPath("~/UploadedFiles/") + FilePath), Encoding.GetEncoding(1252));
                    //var reader = new StreamReader(File.OpenRead(("C:/UploadedFiles/") + FilePath), Encoding.GetEncoding(1252));


                    Competencia oCompetencia = new Competencia();
                    CompetenciaBl oCompetenciaBl = new CompetenciaBl();

                    ProgramaBl oProgramaBl = new ProgramaBl();

                    Area oArea = new Area();
                    AreaBl oAreaBl = new AreaBl();
                    List<string> CompetenciaNoRegistro = new List<string>();

                    var book = new ExcelQueryFactory(URLArchivo);
                    var hoja = book.GetWorksheetNames();
                    var resultado = (from i in book.Worksheet(hoja.FirstOrDefault())
                                     select i).ToList();
                    var cont = 0;
                    foreach (var values in resultado)
                    {
                        if (values[0] == "")
                        {
                            break;
                        }
                        var Programa = oProgramaBl.ConsultarProgramaCodigo(int.Parse(values[3]));
                        if (Programa == null)
                        {
                            CompetenciaNoRegistro.Add(values[3]);
                            continue;
                        }
                        var codigo = oCompetenciaBl.ConsultarCompetenciaCodigo(Convert.ToInt64(values[0]), oProgramaBl.ConsultarProgramaCodigo(int.Parse(values[3])).IdPrograma);
                        if (codigo == null)
                        {
                            oCompetencia.Codigo = Convert.ToInt64(values[0]);
                            oCompetencia.Nombre = values[1];
                            oCompetencia.Horas = int.Parse(values[2]);
                            oCompetencia.IdPrograma = oProgramaBl.ConsultarProgramaCodigo(int.Parse(values[3])).IdPrograma;


                            oCompetenciaBl.GuardarCompetencia(oCompetencia);
                            cont++;
                        }

                    }
                    var no = CompetenciaNoRegistro;
                    return Ok(new { success = true, path = URLArchivo });


                }
                else
                {
                    return Ok(new { success = false, message = "No File" });
                }

            }
            catch (Exception exc)
            {

                return Ok(new { success = false, message = exc.Message });
            }
        }

        public IHttpActionResult UploadFileArea()
        {
            try
            {
                //                List<LogResponseDTO> lstErrores = new List<LogResponseDTO>();
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var fileSavePath = string.Empty;

                    var docfiles = new List<string>();

                    var URLArchivo = "";

                    foreach (string file in httpRequest.Files)
                    {

                        var postedFile = httpRequest.Files[file];
                        //  var filePath = HttpContext.Current.Server.MapPath("C:/UploadedFiles/");
                        var filePath = "C:/UploadedFiles/";

                        var GUID = Guid.NewGuid().ToString();

                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        fileSavePath = Path.Combine(filePath, GUID + "." + postedFile.FileName.Split('.')[1]);



                        postedFile.SaveAs(fileSavePath);

                        docfiles.Add(filePath);

                        // URLArchivo = "~/UploadedFiles/" + GUID + "." + postedFile.FileName.Split('.')[1];
                        URLArchivo = "C:/UploadedFiles/" + GUID + "." + postedFile.FileName.Split('.')[1];


                        string e = Path.GetExtension(URLArchivo);
                        if (e != ".xlsx")
                        {
                            return Ok(new { success = false, message = "La extencion del archivo no es valida" });
                        }

                    }

                    string FilePath = URLArchivo.Split('/')[2];

                    // var reader = new StreamReader(File.OpenRead(HttpContext.Current.Server.MapPath("~/UploadedFiles/") + FilePath), Encoding.GetEncoding(1252));
                    //var reader = new StreamReader(File.OpenRead(("C:/UploadedFiles/") + FilePath), Encoding.GetEncoding(1252));


                    Area oArea = new Area();
                    AreaBl oAreaBl = new AreaBl();

                    var book = new ExcelQueryFactory(URLArchivo);

                    var hoja = book.GetWorksheetNames();
                    var resultado = (from i in book.Worksheet(hoja.FirstOrDefault())
                                     select i).ToList();

                    foreach (var values in resultado)
                    {
                        var codigo = oAreaBl.ConsultarAreaCodigo(int.Parse(values[0]));
                        if (codigo == null)
                        {
                            oArea.Codigo = int.Parse(values[0]);
                            oArea.Nombre = values[1];
                            oArea.Descripcion = values[2];
                            //oArea.IdCoordinacion = int.Parse(values[3]);


                            oAreaBl.GuardarArea(oArea);

                        }
                    }
                    return Ok(new { success = true, path = URLArchivo });
                }
                else
                {
                    return Ok(new { success = false, message = "No File" });
                }

            }
            catch (Exception exc)
            {

                return Ok(new { success = false, message = exc.Message });
            }
        }

        public IHttpActionResult UploadFilePrograma()
        {
            try
            {
                //                List<LogResponseDTO> lstErrores = new List<LogResponseDTO>();
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var fileSavePath = string.Empty;

                    var docfiles = new List<string>();

                    var URLArchivo = "";

                    foreach (string file in httpRequest.Files)
                    {

                        var postedFile = httpRequest.Files[file];
                        //var filePath = HttpContext.Current.Server.MapPath("~/UploadedFiles/");
                        var filePath = "C:/UploadedFiles/";

                        var GUID = Guid.NewGuid().ToString();

                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        fileSavePath = Path.Combine(filePath, GUID + "." + postedFile.FileName.Split('.')[1]);



                        postedFile.SaveAs(fileSavePath);

                        docfiles.Add(filePath);

                        // URLArchivo = "~/UploadedFiles/" + GUID + "." + postedFile.FileName.Split('.')[1];
                        URLArchivo = "C:/UploadedFiles/" + GUID + "." + postedFile.FileName.Split('.')[1];


                        string e = Path.GetExtension(URLArchivo);
                        if (e != ".xlsx")
                        {
                            return Ok(new { success = false, message = "La extencion del archivo no es valida" });
                        }

                    }

                    string FilePath = URLArchivo.Split('/')[2];

                    //var reader = new StreamReader(File.OpenRead(HttpContext.Current.Server.MapPath("~/UploadedFiles/") + FilePath), Encoding.GetEncoding(1252));
                    //var reader = new StreamReader(File.OpenRead(("C:/UploadedFiles/") + FilePath), Encoding.GetEncoding(1252));

                    Programa oPrograma = new Programa();
                    ProgramaBl oProgramaBl = new ProgramaBl();
                    List<string> ProgramaNoRegistro = new List<string>();

                    Area oArea = new Area();
                    AreaBl oAreaBl = new AreaBl();

                    var book = new ExcelQueryFactory(URLArchivo);

                    var hoja = book.GetWorksheetNames();
                    var resultado = (from i in book.Worksheet(hoja.FirstOrDefault())
                                     select i).ToList();

                    var contador = 0; 


                    foreach (var values in resultado)
                    {
                        if (values[0] == "")
                        {
                            break;
                        }
                        var codigo = oProgramaBl.ConsultarProgramaCodigo(int.Parse(values[0].ToString()));
                        if (codigo != null)
                        {
                            ProgramaNoRegistro.Add(values[1]);
                            continue;
                        }
                        if (codigo == null)
                        {
                            oPrograma.CodigoPrograma = int.Parse(values[0].ToString());
                            oPrograma.NombrePrograma = values[1];
                            oPrograma.Nivel = values[2];
                            oPrograma.LineaTecnologica = values[3];
                            oPrograma.Red_Tecnologica = values[4];
                            oPrograma.Perfil_Instructor = values[5];
                            oPrograma.Version_Programa = double.Parse(values[6]);

                            oPrograma.IdArea = oAreaBl.ConsultarAreaCodigo(int.Parse(values[7])).IdArea;

                            oProgramaBl.GuardarPrograma(oPrograma);
                            contador++;

                        }


                    }

                    var no = ProgramaNoRegistro;
                    return Ok(new { success = true, path = URLArchivo });


                }
                else
                {
                    return Ok(new { success = false, message = "No File" });
                }

            }
            catch (Exception exc)
            {

                return Ok(new { success = false, message = exc.Message });
            }
        }

        public IHttpActionResult UploadFileSede()
        {
            try
            {
                //                List<LogResponseDTO> lstErrores = new List<LogResponseDTO>();
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var fileSavePath = string.Empty;

                    var docfiles = new List<string>();

                    var URLArchivo = "";

                    foreach (string file in httpRequest.Files)
                    {

                        var postedFile = httpRequest.Files[file];
                        //var filePath = HttpContext.Current.Server.MapPath("~/UploadedFiles/");
                        var filePath = "C:/UploadedFiles/";

                        var GUID = Guid.NewGuid().ToString();

                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        fileSavePath = Path.Combine(filePath, GUID + "." + postedFile.FileName.Split('.')[1]);



                        postedFile.SaveAs(fileSavePath);

                        docfiles.Add(filePath);

                        // URLArchivo = "~/UploadedFiles/" + GUID + "." + postedFile.FileName.Split('.')[1];
                        URLArchivo = "C:/UploadedFiles/" + GUID + "." + postedFile.FileName.Split('.')[1];


                        string e = Path.GetExtension(URLArchivo);
                        if (e != ".xlsx")
                        {
                            return Ok(new { success = false, message = "La extencion del archivo no es valida" });
                        }

                    }

                    string FilePath = URLArchivo.Split('/')[2];

                    //var reader = new StreamReader(File.OpenRead(HttpContext.Current.Server.MapPath("~/UploadedFiles/") + FilePath), Encoding.GetEncoding(1252));
                    //var reader = new StreamReader(File.OpenRead(("C:/UploadedFiles/") + FilePath), Encoding.GetEncoding(1252));

                    Sede oSede = new Sede();
                    SedeBl oSedeBl = new SedeBl();

                    Area oArea = new Area();
                    AreaBl oAreaBl = new AreaBl();

                    var book = new ExcelQueryFactory(URLArchivo);

                    var hoja = book.GetWorksheetNames();

                    var resultado = (from i in book.Worksheet(hoja.FirstOrDefault())
                                     select i).ToList();

                    foreach (var values in resultado)
                    {
                        if (values[0] == "")
                        {
                            break;
                        }
                        var codigo = oSedeBl.ConsultarSedeCodigo(int.Parse(values[0]));
                        if (codigo == null)
                        {
                            oSede.Codigo = int.Parse(values[0]);
                            oSede.Nombre_Sede = values[1];
                            oSede.Direccion = values[2];

                            var municipio = oSedeBl.ConsultarMunicipioxNombre(values[4].ToString().ToUpper(), values[3]);
                            if (municipio != null)
                            {
                                oSede.IdMunicipio = municipio.IdMunicipio;
                            }
                            else
                            {
                                continue;
                            }

                            if (values[5].ToString().ToUpper() == "Normal")
                            {
                                oSede.TipoSede = 1;
                            }
                            else if (values[5].ToString().ToUpper() == "Empresa")
                            {
                                oSede.TipoSede = 2;
                            }
                            else if (values[5].ToString().ToUpper() == "Colegio")
                            {
                                oSede.TipoSede = 3;
                            }
                            else if (values[5].ToString().ToUpper() == "Virtual")
                            {
                                oSede.TipoSede = 4;
                            }



                            oSedeBl.GuardarSede(oSede);
                        }
                    }


                    return Ok(new { success = true, path = URLArchivo });


                }
                else
                {
                    return Ok(new { success = false, message = "No File" });
                }

            }
            catch (Exception exc)
            {

                return Ok(new { success = false, message = exc.Message });
            }
        }

        public IHttpActionResult UploadFileResultado()
        {
            try
            {
                //                List<LogResponseDTO> lstErrores = new List<LogResponseDTO>();
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var fileSavePath = string.Empty;

                    var docfiles = new List<string>();

                    var URLArchivo = "";

                    foreach (string file in httpRequest.Files)
                    {

                        var postedFile = httpRequest.Files[file];
                        //var filePath = HttpContext.Current.Server.MapPath("~/UploadedFiles/");
                        var filePath = "C:/UploadedFiles/";

                        var GUID = Guid.NewGuid().ToString();

                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        fileSavePath = Path.Combine(filePath, GUID + "." + postedFile.FileName.Split('.')[1]);

                        postedFile.SaveAs(fileSavePath);

                        docfiles.Add(filePath);

                        //URLArchivo = "~/UploadedFiles/" + GUID + "." + postedFile.FileName.Split('.')[1];
                        URLArchivo = "C:/UploadedFiles/" + GUID + "." + postedFile.FileName.Split('.')[1];



                        string e = Path.GetExtension(URLArchivo);
                        if (e != ".xlsx")
                        {
                            return Ok(new { success = false, message = "La extencion del archivo no es valida" });
                        }

                    }

                    string FilePath = URLArchivo.Split('/')[2];

                    //var reader = new StreamReader(File.OpenRead(HttpContext.Current.Server.MapPath("~/UploadedFiles/") + FilePath), Encoding.GetEncoding(1252));

                    //var reader = new StreamReader(File.OpenRead(("C:/UploadedFiles/") + FilePath), Encoding.GetEncoding(1252));

                    CompetenciaBl oCompetenciaBl = new CompetenciaBl();
                    ResultadoBl oResultadoBl = new ResultadoBl();
                    Resultado_Aprendizaje oResultado = new Resultado_Aprendizaje();
                    List<string> ResultadoNoRegistro = new List<string>();

                    var book = new ExcelQueryFactory(URLArchivo);
                    var hoja = book.GetWorksheetNames();
                    var resultado = (from i in book.Worksheet(hoja.FirstOrDefault())
                                     select i).ToList();

             
                    foreach (var values in resultado)
                    {

                        if (values[0] == "")
                        {
                            break;
                        }


                        var ListaCompetencias = oResultadoBl.ConsultarCompetenciaxCodigo(Convert.ToInt64(values[2]));

                        foreach (var item in ListaCompetencias)
                        {
                            var codigo = oResultadoBl.ConsultarResultadoCodigo(int.Parse(values[0]), item.IdCompetencia);
                            if (codigo == null || codigo.Count == 0)
                            {
                                oResultado.Codigo = int.Parse(values[0]);
                                var Competencia = oCompetenciaBl.ConsultarCompetenciaId(item.IdCompetencia);

                                oResultado.Resultado = values[1];
                                if (Competencia == null)
                                {
                                    ResultadoNoRegistro.Add(values[1]);
                                    continue;
                                }
                                //oResultado.IdCompetencia = oCompetenciaBl.ConsultarCompetenciaId(item.IdCompetencia).IdCompetencia;

                                oResultado.IdCompetencia = Competencia.IdCompetencia;

                                oResultadoBl.GuardarResultado(oResultado);

                            }

                        }


                    }
                    var no = ResultadoNoRegistro;

                    return Ok(new { success = true, path = URLArchivo });
                }
                else
                {
                    return Ok(new { success = false, message = "No File" });
                }

            }
            catch (Exception exc)
            {

                return Ok(new { success = false, message = exc.Message });
            }
        }

        public IHttpActionResult UploadFileAmbiente()
        {
            try
            {
                //                List<LogResponseDTO> lstErrores = new List<LogResponseDTO>();
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var fileSavePath = string.Empty;

                    var docfiles = new List<string>();

                    var URLArchivo = "";

                    foreach (string file in httpRequest.Files)
                    {

                        var postedFile = httpRequest.Files[file];
                        //var filePath = HttpContext.Current.Server.MapPath("~/UploadedFiles/");
                        var filePath = "C:/UploadedFiles/";

                        var GUID = Guid.NewGuid().ToString();

                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        fileSavePath = Path.Combine(filePath, GUID + "." + postedFile.FileName.Split('.')[1]);

                        postedFile.SaveAs(fileSavePath);

                        docfiles.Add(filePath);

                        //URLArchivo = "~/UploadedFiles/" + GUID + "." + postedFile.FileName.Split('.')[1];
                        URLArchivo = "C:/UploadedFiles/" + GUID + "." + postedFile.FileName.Split('.')[1];


                        string e = Path.GetExtension(URLArchivo);
                        if (e != ".xlsx")
                        {
                            return Ok(new { success = false, message = "La extencion del archivo no es valida" });
                        }

                    }

                    string FilePath = URLArchivo.Split('/')[2];

                    //var reader = new StreamReader(File.OpenRead(HttpContext.Current.Server.MapPath("~/UploadedFiles/") + FilePath), Encoding.GetEncoding(1252));
                    //var reader = new StreamReader(File.OpenRead(("C:/UploadedFiles/") + FilePath), Encoding.GetEncoding(1252));

                    Ambiente oAmbiente = new Ambiente();
                    AmbienteBl oAmbienteBl = new AmbienteBl();

                    SedeBl oSedeBl = new SedeBl();
                    AreaBl oAreaBl = new AreaBl();

                    var book = new ExcelQueryFactory(URLArchivo);
                    var hoja = book.GetWorksheetNames();
                    var resultado = (from i in book.Worksheet(hoja.FirstOrDefault())
                                     select i).ToList();

                    foreach (var values in resultado)
                    {
                        oAmbiente.IdSede = oSedeBl.ConsultarSedeCodigo(int.Parse(values[0])).IdSede;
                        oAmbiente.IdArea = oAreaBl.ConsultarAreaCodigo(int.Parse(values[1])).IdArea;
                        oAmbiente.Piso = int.Parse(values[2]);
                        oAmbiente.Numero = values[3];
                        oAmbiente.NumeroEquipos = int.Parse(values[4]);
                        oAmbiente.NumeroMesas = int.Parse(values[5]);
                        oAmbiente.NumeroSillas = int.Parse(values[6]);

                        if (values[7].ToString().ToLower() == "si" || values[7].ToString().ToLower() == "SI")
                        {
                            oAmbiente.Pantalla = true;
                        }
                        else
                        {
                            oAmbiente.Pantalla = false;
                        }


                        oAmbienteBl.GuardarAmbiente(oAmbiente);
                    }
                   
                    return Ok(new { success = true, path = URLArchivo });


                }
                else
                {
                    return Ok(new { success = false, message = "No File" });
                }

            }
            catch (Exception exc)
            {

                return Ok(new { success = false, message = exc.Message });
            }
        }

        public IHttpActionResult UploadFileFicha()
        {
            try
            {
                //                List<LogResponseDTO> lstErrores = new List<LogResponseDTO>();
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var fileSavePath = string.Empty;

                    var docfiles = new List<string>();

                    var URLArchivo = "";

                    foreach (string file in httpRequest.Files)
                    {

                        var postedFile = httpRequest.Files[file];
                        //var filePath = HttpContext.Current.Server.MapPath("~/UploadedFiles/");
                        var filePath = "C:/UploadedFiles/";

                        var GUID = Guid.NewGuid().ToString();

                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        fileSavePath = Path.Combine(filePath, GUID + "." + postedFile.FileName.Split('.')[1]);

                        postedFile.SaveAs(fileSavePath);

                        docfiles.Add(filePath);

                        //URLArchivo = "~/UploadedFiles/" + GUID + "." + postedFile.FileName.Split('.')[1];
                        URLArchivo = "C:/UploadedFiles/" + GUID + "." + postedFile.FileName.Split('.')[1];

                        string e = Path.GetExtension(URLArchivo);
                        if (e != ".xlsx")
                        {
                            return Ok(new { success = false, message = "La extencion del archivo no es valida" });
                        }

                    }

                    string FilePath = URLArchivo.Split('/')[2];

                    //var reader = new StreamReader(File.OpenRead(HttpContext.Current.Server.MapPath("~/UploadedFiles/") + FilePath), Encoding.GetEncoding(1252));
                    //var reader = new StreamReader(File.OpenRead(("C:/UploadedFiles/") + FilePath), Encoding.GetEncoding(1252));

                    Ficha oFicha = new Ficha();
                    FichaBl oFichaBl = new FichaBl();
                    List<string> idProgramasNoRegistro = new List<string>();

                    ProgramaBl oProgramaBl = new ProgramaBl();

                    var book = new ExcelQueryFactory(URLArchivo);
                    var hoja = book.GetWorksheetNames();
                    var resultado = (from i in book.Worksheet(hoja.FirstOrDefault())
                                     select i).ToList();

                    foreach (var values in resultado)
                    {
                        if (values[0] == "")
                        {
                            break;
                        }
                        var Programa = oProgramaBl.ConsultarProgramaCodigo(int.Parse(values[0]));

                        if (Programa == null)
                        {
                           
                            idProgramasNoRegistro.Add(values[0]);
                            continue;
                        }
                        oFicha.IdPrograma = oProgramaBl.ConsultarProgramaCodigo(int.Parse(values[0])).IdPrograma;
                        oFicha.Ficha1 = values[1];
                        oFicha.NumAprendices = int.Parse(values[2]);
                        oFicha.TipoFormacion = values[3].ToString();
                        oFicha.Jornada = values[6].ToString();

                        if (DateTime.Parse(values[4]) <= DateTime.Parse(values[5]))
                        {
                            oFicha.FechaInicio = DateTime.Parse(values[4]);
                            oFicha.FechaFin = DateTime.Parse(values[5]);
                        }

                        oFicha.Estado = true;
                        oFichaBl.GuardarFicha(oFicha);
                       
                    }
                    var no = idProgramasNoRegistro;

                    return Ok(new { success = true, path = URLArchivo });


                }
                else
                {
                    return Ok(new { success = false, message = "No File" });
                }

            }
            catch (Exception exc)
            {

                return Ok(new { success = false, message = exc.Message });
            }
        }
    }
}
