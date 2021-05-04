using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Caucedo.LoyaltyProgram.BLL;
using Caucedo.LoyaltyProgram.Models;
using System.Xml;
using Caucedo.Base.Common;
using System.IO;

namespace LoyaltyProgram.Controllers
{

    public class RegistroIncidenciasController : Controller
    {
        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //public ActionResult Index()
        //{
        //    try
        //    {
        //        var model = new ReglasYEventos();
        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(new { ex });
        //        throw;
        //    }
        //}
        public ActionResult Index(int? varDate)
        {
            try
            {
                var model = new ReglasYEventos();
                DateTime date = DateTime.Today;

                GetRegistrosParams pa = new GetRegistrosParams();
                pa.DATE_START = date; pa.DATE_END = date.DayEnd();

                switch (varDate)
                {
                    case 2:
                        pa.DATE_START = date.AddDays(-1); pa.DATE_END = date.DayEnd().AddDays(-1);
                        model.Incidencias = Bll.GetRegistrosDeIncidencias(pa);
                        ViewBag.Encabezado = "Registros de ayer";
                        break;                //AYER
                    case 3:
                        pa.DATE_START = date.DayEnd().AddDays(-7); pa.DATE_END = date.DayEnd();
                        model.Incidencias = Bll.GetRegistrosDeIncidencias(pa);
                        ViewBag.Encabezado = "Registros de los últimos 7 días";
                        break;           //ULTIMOS 7 DIAS
                    default:
                        model.Incidencias = Bll.GetRegistrosDeIncidencias(pa);
                        ViewBag.Encabezado = "Registros de hoy";
                        break;                           //HOY
                }               
                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }
        }

        //[HttpGet]
        //public ActionResult RegistroTabla(int varDate)
        //{
        //    try
        //    {
        //        var model = new ReglasYEventos();
        //        DateTime date = DateTime.Today;

        //        GetRegistrosParams pa = new GetRegistrosParams();
        //        pa.DATE_START = date; pa.DATE_END = date.DayEnd();

        //        switch (varDate)
        //        {
        //            case 2:
        //                pa.DATE_START = date.AddDays(-1); pa.DATE_END = date.DayEnd().AddDays(-1);
        //                model.Incidencias = Bll.GetRegistrosDeIncidencias(pa);
        //                break;                //AYER
        //            case 3:
        //                pa.DATE_START = date.DayEnd().AddDays(-7); pa.DATE_END = date.DayEnd();
        //                model.Incidencias = Bll.GetRegistrosDeIncidencias(pa);
        //                break;           //ULTIMOS 7 DIAS
        //            default:
        //                model.Incidencias = Bll.GetRegistrosDeIncidencias(pa);
        //                break;                           //HOY
        //        }
        //        return PartialView("RegistroTabla", model);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(new { ex });
        //        throw;
        //    }         
        //}

        public ActionResult Crear_Registro_Incidencia(RegIncidenciaParams parametros)
        {
            try
            {
                parametros.CREATED_BY = HttpContext.User.Identity.Name;
                logger.Info(new { parametros });
                var rpt = Bll.InsertarRegistroIncidencia(parametros);
                logger.Info(new { rpt });
                return Json(new { data = rpt }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }

        public ActionResult GetChoferes(string parametro)
        {
            try
            {
                logger.Info(new { parametro });
                var chofer = Bll.GetChoferes(parametro);
                var choferT = chofer[0].ToString();
                logger.Debug(new { choferT });
                return Json(new { data = chofer }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }

        public ActionResult UpdateRegistro(UpdateRegistroParams cambios)
        {
            try
            {
                cambios.USER = HttpContext.User.Identity.Name;
                var list = Bll.GetRegistrosDeIncidenciasForID(cambios.ID_REGISTRO).FirstOrDefault();
                var RegistroOld = new List<string>() { list.ROTULO, list.FECHA.ToString(), list.DESCRIPCION, cambios.USER };
                cambios.CambiosList = RegistroOld;
                
                logger.Info(new { cambios });
                var update_registro = Bll.UpdateRegistroIncidencia(cambios);
                logger.Info(new { update_registro });
                return Json(new { data = update_registro }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }
        }

        public ActionResult GetRegistroForId(int id)
        {
            try
            {
                logger.Info(new { id });
                var registro = Bll.GetRegistrosDeIncidenciasForID(id).FirstOrDefault();

                logger.Info(new { registro });
                return Json(new { data = registro }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }
        }

    }
}