using Caucedo.LoyaltyMovil.Models;
using Caucedo.LoyaltyProgram.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace LoyaltyMovil.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string rntt)

        {
            string root = ConfigurationManager.AppSettings["destinationFiles"];
            string link = ConfigurationManager.AppSettings["linkBackHome"];
            var model = new dataVM();

            ViewBag.root = root;
            ViewBag.linkBack = link;

            model.chofer = model.Chofer.Where(m => m.RNTT == rntt).ToList().FirstOrDefault();
            var infracciones = Bll.GetRegistrosDeIncidenciasChofer(rntt);
            var canjes = Bll.GetRegistrosRecompensasChofer(rntt);

            var today = DateTime.Now;

            var _1Jan = new DateTime(today.Year, 01, 01);
            var _30Jum = new DateTime(today.Year, 06, 30);
            var _1Jul = new DateTime(today.Year, 07, 01);
            var _31Dec = new DateTime(today.Year, 12, 31);


            if (Between(today, _1Jan, _30Jum))
            {
                model.ListaDeIncidencias = infracciones.Where(i => i.CREATED_DATE > _1Jan && i.CREATED_DATE < _30Jum).ToList();
                model.ListaDeCanjes = canjes.Where(c => c.CREATED_DATE > _1Jan && c.CREATED_DATE < _30Jum).ToList();

                model.ListaDeIncidencias_LY = infracciones.Where(i => i.CREATED_DATE > _1Jul.AddYears(-1) && i.CREATED_DATE < _31Dec.AddYears(-1)).ToList();
                model.ListaDeCanjes_LY = canjes.Where(c => c.CREATED_DATE > _1Jul.AddYears(-1) && c.CREATED_DATE < _31Dec.AddYears(-1)).ToList();
            }
            if (Between(today, _1Jul, _31Dec))
            {
                model.ListaDeIncidencias = infracciones.Where(i => i.CREATED_DATE > _1Jul && i.CREATED_DATE < _31Dec).ToList();
                model.ListaDeCanjes = canjes.Where(c => c.CREATED_DATE > _1Jul && c.CREATED_DATE < _31Dec).ToList();

                model.ListaDeIncidencias_LY = infracciones.Where(i => i.CREATED_DATE > _1Jan && i.CREATED_DATE < _30Jum).ToList();
                model.ListaDeCanjes_LY = canjes.Where(c => c.CREATED_DATE > _1Jan && c.CREATED_DATE < _30Jum).ToList();
            }


            //model.ListaDeIncidencias = infracciones.Where(i => i.CREATED_DATE.Year == DateTime.Now.Year).ToList();
            //model.ListaDeCanjes = canjes.Where(c => c.CREATED_DATE.Year == DateTime.Now.Year).ToList();
            //model.ListaDeIncidencias_LY = infracciones.Where(i => i.CREATED_DATE.Year == (DateTime.Now.Year - 1)).ToList();
            //model.ListaDeCanjes_LY = canjes.Where(c => c.CREATED_DATE.Year == (DateTime.Now.Year - 1)).ToList();              

            model.PuntosCanjeados = model.ListaDeCanjes.Sum(s => s.PUNTOS);
            model.PuntosCanjeados_LY = model.ListaDeCanjes_LY.Sum(s => s.PUNTOS);

            if (model.chofer == null)
            {
                return View("Error");
            }

            return View(model);

        }

        public static bool Between(DateTime input, DateTime date1, DateTime date2)
        {
            return (input > date1 && input < date2);
        }
        public ActionResult GetRegistroPorChofer(string rntt)
        {
            var registro = Bll.GetRegistrosDeIncidenciasChofer(rntt);
            return Json(new { data = registro }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCanjes(string rntt)
        {
            var registroChofer = Bll.GetRegistrosRecompensasChofer(rntt);
            return Json(new { data = registroChofer }, JsonRequestBehavior.AllowGet);
        }

    }
}