using Caucedo.Base.SqlSvr.DAL;
using Caucedo.Base.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Web;

namespace Caucedo.LoyaltyProgram.BLL
{

    public static class Bll
    {
        public static List<Reglas> GetReglas()
        {
            return Database.DataServer.ExecReaderSelSP<Reglas>("SP_GetReglas");
        }

        public static List<Eventos> GetEventos()
        {
            return Database.DataServer.ExecReaderSelSP<Eventos>("SP_GetEventos");
        }

        public static List<Recompensas> GetRecompensas()
        {
            return Database.DataServer.ExecReaderSelSP<Recompensas>("SP_GetRecompensas");
        }

        public static List<Campanias> GetCampanias()
        {
            return Database.DataServer.ExecReaderSelSP<Campanias>("SP_GetCampanias");
        }
        public static List<Recompensas> GetRecompensas(string id_recompensa)
        {
            var parametro = SearchRec.ToSqlParams(new
            {
                ID_RECOMPENSA = Convert.ToInt32(id_recompensa)
            });
            return Database.DataServer.ExecReaderSelSP<Recompensas>("SP_GetRecompensas", parametro);
        }
        public static List<Choferes> GetChoferes(string rntt)
        {
            var parametro = SearchRec.ToSqlParams(new
            {
                RNTT = rntt
            });
            return Database.DataServer.ExecReaderSelSP<Choferes>("SP_GetChoferes", parametro);

        }

        public static List<Choferes> GetChoferes()
        {
            return Database.DataServer.ExecReaderSelSP<Choferes>("SP_GetChoferes");
        }

        public static List<GetRegistroDeIncidenciasParams> GetRegistrosDeIncidencias(GetRegistrosParams p)
        {

            var parametro = SearchRec.ToSqlParams(new
            {
                FECHA_START = p.DATE_START,
                FECHA_END = p.DATE_END,
                RNTT = p.RNTT,
                ROTULO = p.ROTULO
            });
            return Database.DataServer.ExecReaderSelSP<GetRegistroDeIncidenciasParams>("SP_GetRegistrosIncidencias", parametro);
        }

        public static List<GetRegistroDeIncidenciasParams> GetRegistrosDeIncidenciasForID(int id_registro)
        {

            var parametro = SearchRec.ToSqlParams(new
            {
                ID_REGISTRO = id_registro
            });
            return Database.DataServer.ExecReaderSelSP<GetRegistroDeIncidenciasParams>("SP_GetRegistrosIncidenciasID", parametro);
        }

        public static List<GetRegisDeIncidenciasChoferParams> GetRegistrosDeIncidenciasChofer(string rntt)
        {
            var parametro = SearchRec.ToSqlParams(new
            {
                RNTT = rntt
            });
            return Database.DataServer.ExecReaderSelSP<GetRegisDeIncidenciasChoferParams>("SP_GetRegistrosIncidenciasChofer", parametro);
        }

        public static List<GetRegisDeIncidenciasChoferParams> GetRegistrosRecompensasChofer(string rntt)
        {
            var parametro = SearchRec.ToSqlParams(new
            {
                RNTT = rntt
            });
            return Database.DataServer.ExecReaderSelSP<GetRegisDeIncidenciasChoferParams>("SP_GetRegistrosRecompensasChofer", parametro);
        }

        public static List<ListadoEntregaRecompensas> GetListadoEntregaRecompensas(string campania)
        {
            var parametro = SearchRec.ToSqlParams(new
            {
                CAMPANIA = campania
            });
            return Database.DataServer.ExecReaderSelSP<ListadoEntregaRecompensas>("SP_HistorialEntregaRecompensas", parametro);
        }

        public static List<string> GetCampanasDeCanjes()
        {          
            return Database.DataServer.ExecReaderSelSP<string>("SP_GetCampaniasDeCanjes", null);
        }

        public static List<string> GetIncidentes()
        {
            return Database.DataServer.ExecReaderSelSP<string>("SP_GetIncidencias");
        }


        public static Recompensas InsertarRecompensa(InsertRecompensaParams iR)
        {
            var parametros = SearchRec.ToSqlParams(new
            {
                ID_RECOMPENSA = iR.ID_RECOMPENSA,
                NOMBRE = iR.NOMBRE,
                IMAGEN = iR.IMAGEN,
                DESCRIPCION = iR.DESCRIPCION,
                VALOR_PUNTOS = iR.VALOR_PUNTOS,
                STOCK = iR.STOCK,
                CREATED_DATE = DateTime.Now,
                CREATED_BY = iR.CREATED_BY,
                ACTIVO = iR.ACTIVO
            });
            var result = Database.DataServer.ExecReaderSelSP<Recompensas>("SP_InsertarRecompensa", parametros);
            return result.FirstOrDefault();
        }
        public static Reglas InsertarRegla(InsertReglasParams iR)
        {
            var parametros = SearchRec.ToSqlParams(new
            {
                ID_REGLA = iR.ID_REGLA,
                REGLA = iR.REGLA,
                ICON = iR.ICON,
                DESCRIPCION = iR.DESCRIPCION,
                CREATED_DATE = DateTime.Now,
                CREATED_BY = iR.CREATED_BY,
                ACTIVO = iR.ACTIVO
            });
            var result = Database.DataServer.ExecReaderSelSP<Reglas>("SP_InsertarRegla", parametros);
            return result.FirstOrDefault();
        }
        public static Eventos InsertarEvento(InsertEventoParams iR)
        {
            var parametros = SearchRec.ToSqlParams(new
            {
                ID_EVENTO = iR.ID_EVENTO,
                EVENTO = iR.EVENTO,
                PUNTAJE = iR.PUNTAJE,
                CREATED_DATE = DateTime.Now,
                CREATED_BY = iR.CREATED_BY,
                ACTIVO = iR.ACTIVO
            });
            var result = Database.DataServer.ExecReaderSelSP<Eventos>("SP_InsertarEvento", parametros);
            return result.FirstOrDefault();
        }

        public static Campanias CrearCampania(CrearCampaniaParams cc)
        {
            var parametros = SearchRec.ToSqlParams(new
            {
                ID_CAMPANIA = cc.ID_CAMPANIA,
                CAMPANIA = cc.CAMPANIA,
                DATE_START = cc.DATE_START,
                DATE_END = cc.DATE_END,
                FECHA_CANJE = cc.FECHA_CANJE,
                CREATED_BY = cc.CREATED_BY,
                ACTIVO = cc.ACTIVO
            });
            var result = Database.DataServer.ExecReaderSelSP<Campanias>("SP_CrearCampania", parametros);
            return result.FirstOrDefault();
        }

        public static Campanias UpdateCampania(UpdateCampaniaParams cc)
        {
            var parametros = SearchRec.ToSqlParams(new
            {
                ID_CAMPANIA = cc.ID_CAMPANIA,
                CAMPANIA = cc.CAMPANIA,
                FECHA_CANJE = cc.FECHA_CANJE,
                ACTIVO = cc.ACTIVO
            });
            var result = Database.DataServer.ExecReaderSelSP<Campanias>("SP_UpdateCampania", parametros);
            return result.FirstOrDefault();
        }

        public static CambiosRegistroUpdate UpdateRegistroIncidencia(UpdateRegistroParams cc)
        {
            var cambios = UtilsApp.GenerarXMLCambiosRegistro(cc.CambiosList);
            var xmlCambios = Extensiones.SerializeToXml<CambiosRegistroUpdate>(cambios);

            var parametros = SearchRec.ToSqlParams(new
            {
                cc.ID_REGISTRO,
                cc.ROTULO,
                cc.FECHA_INCIDENTE,
                cc.DESCRIPCION,
                CAMBIOS = xmlCambios.ToString(),
                cc.USER
            });
            var result = Database.DataServer.ExecReaderSelSP<CambiosRegistroUpdate>("SP_UpdateRegistro", parametros);
            return result.FirstOrDefault();
        }

        public static Recompensas EntregarRecompensa(EntregarRecompensaParams e)
        {
            var recompensas = UtilsApp.GenerarXMLRecompensas(e.RECOMPENSAS);
            var xmlRecompesas = Extensiones.SerializeToXml<UtilsApp.Recompensas>(recompensas);

            var parametros = SearchRec.ToSqlParams(new
            {
                RNTT = e.RNTT,
                RECOMPENSAS = xmlRecompesas.ToString(),
                PUNTOS_RECOMPENSAS_T = e.PUNTOS_TOTALES,
                CREATED_DATE = DateTime.Now,
                CREATED_BY = e.CREATED_BY
            });
            var result = Database.DataServer.ExecReaderSelSP<Recompensas>("SP_EntregarRecompensa", parametros);
            return result.FirstOrDefault();
        }

        public static RegistroDeIncidencias InsertarRegistroIncidencia(RegIncidenciaParams Params)
        {

            var incidencias = UtilsApp.GenerarXML(Params.REGLAS, Params.INFRACCIONES);
            var xmlIncidencias = Extensiones.SerializeToXml<UtilsApp.Incidencias>(incidencias);
            var puntosDescontados = UtilsApp.GenerarPuntos(Params.REGLAS, Params.INFRACCIONES);

            var parametros = SearchRec.ToSqlParams(new
            {
                ROTULO = Params.ROTULO,
                RNTT = Params.RNTT,
                FULL_NAME = Params.FULL_NAME,
                CEDULA = Params.CEDULA,
                EMPRESA = Params.EMPRESA,
                FECHA = Params.FECHA,
                INCIDENTES = xmlIncidencias.ToString(),
                DESCRIPCION = Params.DESCRIPCION,
                CREATED_BY = Params.CREATED_BY,
                CREATED_DATE = DateTime.Now,
                PUNTOS_DESCONTADOS = puntosDescontados
            });

            var result = Database.DataServer.ExecReaderSelSP<RegistroDeIncidencias>("SP_InsertarRegistroIncidente", parametros);
            return result.FirstOrDefault();
        }


        public static RegistroManual InsertarRegistroManual(RegistroManualParams Params)
        {

            var parametros = SearchRec.ToSqlParams(new
            {
                RNTT = Params.RNTT,
                NOMBRE = Params.NOMBRE,
                CEDULA = Params.CEDULA,
                DESCRIPCION = Params.DESCRIPCION,
                TIPO_EVENTO = Params.TIPO_EVENTO,
                PUNTOS = Params.PUNTOS,
                CREATED_BY = Params.CREATED_BY,
                CREATED_DATE = DateTime.Now,
            });

            var result = Database.DataServer.ExecReaderSelSP<RegistroManual>("SP_InsertarRegistroManual", parametros);
            return result.FirstOrDefault();
        }

        public static int InsertPointAutomation(List<ChoferesToSQL> cho)
        {
            return Database.DataServer.ExecSPNonQuery("SP_InsertPoint", SearchRec.ToSqlParams(new { data = cho.ToDataTable() }));
        }

        public static Choferes GetRegistroChofer(string RNTT)
        {
            return Database.DataServer.ExecReaderSelSP<Choferes>("SP_GetChoferes",
                SearchRec.ToSqlParams(new
                {
                    RNTT
                })
                ).FirstOrDefault();
        }

        public static int ResetPoint()
        {
            return Database.DataServer.ExecSPNonQuery("SP_ResetPoints", null);
        }

    }
}
