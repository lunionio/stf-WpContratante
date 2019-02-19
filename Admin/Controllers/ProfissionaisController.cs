using Admin.Helppers;
using Admin.Helppser;
using System.Configuration;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class ProfissionaisController : Controller
    {
        [HttpGet]
        public ActionResult BuscarServicoTipo()
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/wpProfissionais/BuscarServicoTipo/" + usuario.idCliente + "/" + usuario.IdUsuario;

            var helper = new ServiceHelper();
            var result = helper.Get<object>(url);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BuscarServico()
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/wpProfissionais/BuscarServico/" + usuario.idCliente + "/" + usuario.IdUsuario;

            var helper = new ServiceHelper();
            var result = helper.Get<object>(url);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}