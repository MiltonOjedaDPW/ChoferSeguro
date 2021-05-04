using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Caucedo.LoyaltyProgram.BLL;

namespace LoyaltyProgram.Controllers
{
    public class HomeController : Controller
    {
        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index()
        {
            try
            {
                //var a = UtilsApp.GenerarEstadisticasReglas();
                return View();
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }
            
        }
    }
}