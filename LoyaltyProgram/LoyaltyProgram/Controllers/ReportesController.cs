using Caucedo.LoyaltyProgram.Models;
using Caucedo.LoyaltyProgram.BLL;
using Caucedo.Base.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoyaltyProgram.Controllers
{
    public class ReportesController : Controller
    {
        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: Reportes
        public ActionResult Index()
        {
            try
            {
                //var model = new dataVM();

                return View();
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }

        public ActionResult GetRegistros(GetRegistrosParams parametros)
        {

            try
            {
                logger.Info(new { parametros });
                var registros = Bll.GetRegistrosDeIncidencias(parametros);
                var aaa = registros;
                logger.Error(new { registros });

                //var xml = Extensiones.DeserializeFromXml<List<UtilsApp.Incidencias>>(doc);

                return Json(new { data = registros }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }
    }
}