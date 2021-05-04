using Caucedo.LoyaltyProgram.BLL;
using Caucedo.LoyaltyProgram.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caucedo.LoyaltyProgram.Controllers
{
    public class MovilController : Controller
    {
        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: Movil
        //public ActionResult Index()
        //{
        //    string root = ConfigurationManager.AppSettings["destinationFiles"];
        //    var model = new dataVM();
        //    ViewBag.root = root;

        //    return View(model);
        //}
        [HttpGet]
        public ActionResult Index(string rntt)
        {
            string root = ConfigurationManager.AppSettings["destinationFiles"];
            var model = new dataVM();
            ViewBag.root = root;
            model.chofer = model.Chofer.Where(m => m.RNTT == rntt).ToList().FirstOrDefault();
            logger.Info(new { model });
            //var chofer = Model.Chofer.Where(m => m.RNTT == "198").ToList().FirstOrDefault();
            return View(model);
        }

        public ActionResult GetChoferRntt(string rntt)
        {


            return View();
        }

        public ActionResult Contro3am()
        {
            return View();
        }
  
    }
}