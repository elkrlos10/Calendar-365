using Datos.DTO;
using Datos.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.LogicaNegocio
{
    public class ProgramaBl
    {
        Model1 entity = new Model1();

        public void GuardarPrograma(Programa oPrograma)
        {
            entity.Programa.Add(oPrograma);
            entity.SaveChanges();
        }

        public List<Programa> ConsultarProgramas()
        {
            var Datos = (from i in entity.Programa
                         where i.NombrePrograma != "Transversal" || i.NombrePrograma != "TRANSVERSAL"
                         select i).ToList();
            return Datos;
        }


        public void ActualizarRegistro(Programa oPrograma)
        {
            Model1 entity = new Model1();

            var Item = (from i in entity.Programa
                        where i.IdPrograma == oPrograma.IdPrograma
                        select i).FirstOrDefault();

            Item.CodigoPrograma = oPrograma.CodigoPrograma;
            Item.Nivel = oPrograma.Nivel;
            Item.LineaTecnologica = oPrograma.LineaTecnologica;
            Item.Red_Tecnologica = oPrograma.Red_Tecnologica;
            Item.Perfil_Instructor = oPrograma.Perfil_Instructor;
            Item.Version_Programa = oPrograma.Version_Programa;
            Item.IdArea = oPrograma.IdArea;
            Item.NombrePrograma = oPrograma.NombrePrograma;
            entity.SaveChanges();

        }

        public bool EliminarPrograma(int IdProgrma)
        {


            var programas = (from i in entity.Programa
                             where i.IdPrograma == IdProgrma
                             select i).FirstOrDefault();
            entity.Programa.Remove(programas);
            entity.SaveChanges();
            return true;


        }

        public Programa ConsultarProgramaId(int IdPrograma)
        {

            var Datos = (from i in entity.Programa
                         where i.IdPrograma == IdPrograma
                         select i).FirstOrDefault();
            return Datos;

        }

        public Programa ConsultarProgramaCodigo(int CodigoPrograma)
        {

            var Datos = (from i in entity.Programa
                         where i.CodigoPrograma == CodigoPrograma
                         select i).FirstOrDefault();
            return Datos;

        }

        public List<Programa> ConsultarProgramaxCoordinacion(int IdCoordinador)
        {
            var programa = (from p in entity.Programa
                            join a in entity.Area on p.IdArea equals a.IdArea
                            join c in entity.Coordinacion on a.IdArea equals c.IdArea
                            where c.IdCoordinacion == IdCoordinador && (p.NombrePrograma != "Transversal" || p.NombrePrograma != "TRANSVERSAL")
                            && p.Nivel != "MEDIA TÉCNICA"
                            //&& p.Nivel != "CURSO ESPECIAL"
                            orderby p.NombrePrograma
                            select p).ToList();
            return programa;
        }

        public List<Programa> ProgramaTituladosxCoordinacion(int IdCoordinador)
        {

            var coordinador = (from i in entity.Coordinacion
                               where i.IdCoordinacion == IdCoordinador
                               select i).FirstOrDefault();


            var area = (from i in entity.Area
                        where i.IdArea == coordinador.IdArea
                        select i).FirstOrDefault();

            var Datos = (from i in entity.Programa
                         where i.IdArea == area.IdArea
                         && i.Nivel != "MEDIA TÉCNICA"
                         && i.Nivel != "CURSO ESPECIAL"
                          && i.Nivel != "CURSO VIRTUAL"
                         && i.Nivel != "ESPECIALIZACIÓN TECNOLÓGICA VIRTUAL"
                          && i.Nivel != "TECNÓLOGO VIRTUAL"
                         && (i.NombrePrograma != "Transversal" || i.NombrePrograma != "TRANSVERSAL")
                         orderby i.NombrePrograma
                         select i).ToList();

            return Datos;
        }

        public List<Programa> ProgramaVirtualesTitulados(int IdCoordinador)
        {

            var coordinador = (from i in entity.Coordinacion
                               where i.IdCoordinacion == IdCoordinador
                               select i).FirstOrDefault();


            var area = (from i in entity.Area
                        where i.IdArea == coordinador.IdArea
                        select i).FirstOrDefault();

            var Datos = (from i in entity.Programa
                         where i.IdArea == area.IdArea
                         && (i.Nivel == "TECNÓLOGO VIRTUAL"
                         || i.Nivel == "ESPECIALIZACIÓN TECNOLÓGICA VIRTUAL" || i.Nivel == "CURSO VIRTUAL")

                         orderby i.NombrePrograma
                         select i).ToList();

            return Datos;
        }

        public List<Programa> ProgramasComplementariosxCoordinacion(int IdCoordinador)
        {

            var coordinador = (from i in entity.Coordinacion
                               where i.IdCoordinacion == IdCoordinador
                               select i).FirstOrDefault();


            var area = (from i in entity.Area
                        where i.IdArea == coordinador.IdArea
                        select i).FirstOrDefault();

            var Datos = (from i in entity.Programa
                         where i.IdArea == area.IdArea && i.Nivel == "CURSO ESPECIAL"
                         orderby i.NombrePrograma
                         select i).ToList();

            return Datos;
        }


        public List<Programa> ConsultarProgramaxArea(int IdArea)
        {
            var programa = (from p in entity.Programa
                            where p.IdArea == IdArea && (p.NombrePrograma != "Transversal" || p.NombrePrograma != "TRANSVERSAL")
                            orderby p.NombrePrograma
                            select p).ToList();
            return programa;
        }

        public List<Programa> ConsultarPragamaTransversal()
        {
            var programa = (from p in entity.Programa
                            where p.NombrePrograma == "Transversal" || p.NombrePrograma == "TRANSVERSAL"
                            select p).ToList();

            return programa;
        }

        public List<Programa> ProgramasMediaTecnica(int IdCoordinador)
        {
            var coordinador = (from i in entity.Coordinacion
                               where i.IdCoordinacion == IdCoordinador
                               select i).FirstOrDefault();


            var area = (from i in entity.Area
                        where i.IdArea == coordinador.IdArea
                        select i).FirstOrDefault();

            var Datos = (from i in entity.Programa
                         where i.Nivel == "MEDIA TÉCNICA" && i.IdArea == area.IdArea
                         select i).ToList();

            return Datos;

        }

        public List<Programa> ProgramasxEmpresa(int IdCoordinador)
        {
            var coordinador = (from i in entity.Coordinacion
                               where i.IdCoordinacion == IdCoordinador
                               select i).FirstOrDefault();


            var area = (from i in entity.Area
                        where i.IdArea == coordinador.IdArea
                        select i).FirstOrDefault();

            var Datos = (from i in entity.Programa
                         where i.Nivel == "MEDIA TÉCNICA" && i.IdArea == area.IdArea
                         select i).ToList();

            return Datos;

        }
        public List<Programa> ProgramasVirtualesTitulados(int IdCoordinador)
        {
            var coordinador = (from i in entity.Coordinacion
                               where i.IdCoordinacion == IdCoordinador
                               select i).FirstOrDefault();


            var area = (from i in entity.Area
                        where i.IdArea == coordinador.IdArea
                        select i).FirstOrDefault();

            var Datos = (from i in entity.Programa
                         where (i.Nivel == "TECNÓLOGO VIRTUAL" || i.Nivel == "ESPECIALIZACIÓN TECNOLÓGICA VIRTUAL" || i.Nivel == "CURSO VIRTUAL") && i.IdArea == area.IdArea
                         select i).ToList();

            return Datos;

        }


        public List<Programa> ProgramasCursosVirtuales(int IdCoordinador)
        {
            var coordinador = (from i in entity.Coordinacion
                               where i.IdCoordinacion == IdCoordinador
                               select i).FirstOrDefault();


            var area = (from i in entity.Area
                        where i.IdArea == coordinador.IdArea
                        select i).FirstOrDefault();

            var Datos = (from i in entity.Programa
                         where (i.Nivel == "TECNÓLOGO VIRTUAL" || i.Nivel == "ESPECIALIZACIÓN TECNOLÓGICA VIRTUAL" || i.Nivel == "CURSO VIRTUAL") && i.IdArea == area.IdArea
                         select i).ToList();

            return Datos;

        }

        public List<ProgramasDTO> ReporteProgramas(int IdCoordinacion)
        {
            var Programas = new List<ProgramasDTO>();
            if (IdCoordinacion == 0)
            {
                Programas = (from r in entity.Resultado_Aprendizaje
                             join c in entity.Competencia on r.IdCompetencia equals c.IdCompetencia
                             join p in entity.Programa on c.IdPrograma equals p.IdPrograma
                             join a in entity.Area on p.IdArea equals a.IdArea
                             orderby a.Nombre, p.NombrePrograma
                             select new ProgramasDTO
                             {
                                 CodigoPrograma = p.CodigoPrograma,
                                 NombrePrograma = p.NombrePrograma,
                                 Nivel = p.Nivel,
                                 LineaTecnologica = p.LineaTecnologica,
                                 Red_Tecnologica = p.Red_Tecnologica,
                                 Version_Programa = p.Version_Programa,
                                 Area = a.Nombre,
                                 CodigoCompetencia = c.Codigo,
                                 NombreCompetencia = c.Nombre,
                                 Horas = c.Horas,
                                 CodigoResultado = r.Codigo,
                                 Resultado = r.Resultado

                             }).ToList();
            }
            else
            {
                Programas = (from r in entity.Resultado_Aprendizaje
                             join c in entity.Competencia on r.IdCompetencia equals c.IdCompetencia
                             join p in entity.Programa on c.IdPrograma equals p.IdPrograma
                             join a in entity.Area on p.IdArea equals a.IdArea
                             join co in entity.Coordinacion on a.IdArea equals co.IdArea
                             where co.IdCoordinacion == IdCoordinacion && p.NombrePrograma != "TRANSVERSAL"
                             select new ProgramasDTO
                             {
                                 CodigoPrograma = p.CodigoPrograma,
                                 NombrePrograma = p.NombrePrograma,
                                 Nivel = p.Nivel,
                                 LineaTecnologica = p.LineaTecnologica,
                                 Red_Tecnologica = p.Red_Tecnologica,
                                 Version_Programa = p.Version_Programa,
                                 Area = a.Nombre,
                                 CodigoCompetencia = c.Codigo,
                                 NombreCompetencia = c.Nombre,
                                 Horas = c.Horas,
                                 CodigoResultado = r.Codigo,
                                 Resultado = r.Resultado

                             }).ToList();
            }



            for (int i = 0; i < Programas.Count; i++)
            {
                if (Programas[i].LineaTecnologica == "TRANSVERSAL")
                {
                    Programas[i].Area = "TRANSVERSAL";
                }

            }

            return Programas;
        }
    }
}
