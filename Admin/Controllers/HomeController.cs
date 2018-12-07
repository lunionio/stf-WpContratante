using Admin.Helppers;
using Admin.Helppser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public JsonResult GetTotais()
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/WpRelatorios/TotaisContratante/" + usuario.idCliente + "/" + usuario.IdUsuario;

            var envio = new
            {
                contratanteId = usuario.idEmpresa,
            };

            var helper = new ServiceHelper();
            var result = helper.Post<IEnumerable<object>>(url, envio);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}