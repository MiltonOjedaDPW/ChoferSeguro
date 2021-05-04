using Caucedo.LoyaltyProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Caucedo.LoyaltyProgram.BLL;
using System.IO;
using System.Configuration;

namespace LoyaltyProgram.Controllers
{
    public class AdminController : Controller
    {
        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //EventosVM model = new EventosVM();
        public AdminController()
        {

        }
        // GET: Admin
        public ActionResult Index()
        {
            try
            {
                string root = ConfigurationManager.AppSettings["destinationFiles"];
                var model = new EventosVM();
                ViewBag.root = root;
                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }
        public ActionResult GetReglas()
        {
            try
            {
                var reglas = Bll.GetReglas();
                return Json(new { data = reglas }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }
        public ActionResult GetRecompensas()
        {
            try
            {
                var recompensas = Bll.GetRecompensas();

                return Json(new { data = recompensas }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }
        public ActionResult GetEventos()
        {
            try
            {
                var eventos = Bll.GetEventos();

                return Json(new { data = eventos }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }
        public ActionResult GetCampanias()
        {
            try
            {
                var campanias = Bll.GetCampanias();

                return Json(new { data = campanias }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }

        public ActionResult InsertarRegla(InsertReglasParams parametersIR)
        {
            try
            {
                parametersIR.CREATED_BY = HttpContext.User.Identity.Name;
                logger.Info(new { parametersIR });
                var rpt = Bll.InsertarRegla(parametersIR);
                logger.Info(new { rpt });
                return Json(new { data = rpt }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }

        public ActionResult InsertarEvento(InsertEventoParams parametersIE)
        {
            try
            {
                parametersIE.CREATED_BY = HttpContext.User.Identity.Name;
                logger.Info(new { parametersIE });
                var rpt = Bll.InsertarEvento(parametersIE);
                logger.Info(new { rpt });
                return Json(new { data = rpt }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }

        public ActionResult CrearCampania(CrearCampaniaParams paramsCampania)
        {
            try
            {
                paramsCampania.CREATED_BY = HttpContext.User.Identity.Name;
                logger.Info(new { paramsCampania });
                var rpt = Bll.CrearCampania(paramsCampania);
                logger.Info(new { rpt });
                return Json(new { data = rpt }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }

        public ActionResult UpdateCampania(UpdateCampaniaParams paramsCampania)
        {
            try
            {
                logger.Info(new { paramsCampania });
                var rpt = Bll.UpdateCampania(paramsCampania);
                logger.Info(new { rpt });
                return Json(new { data = rpt }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }

        [HttpPost]
        public ActionResult CrearRegistroManual(RegistroManualParams parametros)
        {
            try
            {
                parametros.CREATED_BY = HttpContext.User.Identity.Name;
                logger.Info(new { parametros });
                var rpt = Bll.InsertarRegistroManual(parametros);
                logger.Info(new { rpt });
                return Json(new { data = rpt }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }


        [HttpPost]
        public ActionResult InsertarRecompensa(InsertRecompensaParams parametersIR)
        {
            try
            {
                parametersIR.CREATED_BY = HttpContext.User.Identity.Name;
                //string ruta = "~/img_recompensas/" + fileName;
                logger.Info(new { parametersIR });
                if (!parametersIR.IMAGEN.Contains(parametersIR.NOMBRE))
                {
                    string root = ConfigurationManager.AppSettings["saveImg"];
                    //string fileName = parametersIR.NOMBRE.Take(2) + DateTime.Now.ToString("ssmmff") + ".png";
                    string fileName = DateTime.Now.ToString("ssmmff") + ".png";
                    string fileNameWitPath = Path.Combine(Server.MapPath(root), fileName);

                    using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fs))
                        {
                            byte[] data = Convert.FromBase64String(parametersIR.IMAGEN);
                            bw.Write(data);
                            bw.Close();
                        }
                        fs.Close();
                    }
                    parametersIR.IMAGEN = fileName;
                }


                var rpt = Bll.InsertarRecompensa(parametersIR);
                logger.Info(new { rpt });
                return Json(new { data = rpt }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(new { ex });
                throw;
            }

        }


    }
}