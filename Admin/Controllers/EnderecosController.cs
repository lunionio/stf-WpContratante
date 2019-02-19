using Admin.Helppers;
using Admin.Helppser;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class EnderecosController : Controller
    {
        [HttpPost]
        public ActionResult BuscarEnderecoPorCep(Endereco data)
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/endereco/BuscarEnderecoPorCep/" + usuario.idCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;

            var helper = new ServiceHelper();
            var endereco = helper.Post<object>(url, data);

            return Json(endereco, JsonRequestBehavior.AllowGet);
        }
    }
}
