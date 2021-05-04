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
    public class RecompensasController : Controller
    {
        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: Recompensas
        public ActionResult Index(string campania)
        {
            try
            {
                string root = ConfigurationManager.AppSettings["destinationFiles"];
                ViewBag.root = root;
                var model = new RecompensasVM();

                var listado = new List<ListadoEntregaRecompensas>();
                //campania = "Enero - Junio 2019";
                logger.Info(new { campania });
                ViewBag.campania = campania;
                //listado = Bll.GetListadoEntregaRecompensas(campania);
                //model.Recompensas = listado;
                return View(model);
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
                //var chofer = Bll.GetChoferes(parametro);
                var cho = Bll.GetChoferes().Where(m => m.RNTT == parametro).ToList();
                //var puntosDescon = cho[0].PUNTOS_DESCONTADOS;
                //var s = Bll.GetRegistrosDeIncidenciasChofer(parametro);

                //cho[0].PUNTOS_DESCONTADOS = s.Sum(x => x.PUNTOS);
                //if (cho[0].PUNTOS_BALANCE > 0)
                //{
                //    cho[0].PUNTOS_BALANCE = cho[0].PUNTOS_BALANCE + (-1 * puntosDescon + cho[0].PUNTOS_DESCONTADOS);
                //}
                //else
                //{
                //    cho[0].PUNTOS_BALANCE = 0;
                //    cho[0].PUNTOS_GANADOS = 0;
                //}
                //var te = cho;

                logger.Info(new { cho });
                return Json(new { data = cho }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }

        public ActionResult GetRecompensa(string id_recompensa)
        {
            try
            {
                logger.Info(new { id_recompensa });
                var recompensa = Bll.GetRecompensas(id_recompensa);
                logger.Info(new { recompensa });
                return Json(new { data = recompensa }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }

        public ActionResult EntregarRecompensa(EntregarRecompensaParams parametros)
        {
            try
            {
                parametros.CREATED_BY = HttpContext.User.Identity.Name;
                logger.Info(new { parametros });
                var entregar_recompensa = Bll.EntregarRecompensa(parametros);
                logger.Info(new { entregar_recompensa });
                return Json(new { data = entregar_recompensa }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }

        public ActionResult GetListadoEntregaRecompensas(string campania)
        {

            try
            {
                var listado = new List<ListadoEntregaRecompensas>();
                //campania = "Enero - Junio 2019";
                logger.Info(new { campania });
                listado = Bll.GetListadoEntregaRecompensas(campania);           
                logger.Info(new { listado });

                return PartialView("~/Views/Shared/ListadoRecompensas.cshtml", listado);
                //return Json(new { data = listado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }
        }
    }
}