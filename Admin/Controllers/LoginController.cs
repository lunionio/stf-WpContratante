using Admin.Helppers;
using Admin.Helppser;
using Admin.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string login, string senha)
        {
            var collection = new LoginViewModel
            {
                Login = login,
                Senha = senha
            };

            try
            {
                if (PixCoreValues.Login(collection))
                {
                    TempData["LoginMessage"] = string.Empty;

                    var op = GetOportunidade();
                    if(op != null)
                    {
                        return RedirectToAction("Index", "CheckIn", new { optId = op.ID });
                    }

                    return RedirectToAction("Index", "Vaga");
                }
                else
                {

                    TempData["LoginMessage"] = "Usuario ou senha invalida";
                    return RedirectToAction("Index");
                }
            }
            catch(Exception e)
            {
                TempData["LoginMessage"] = "Usuario ou senha invalida";
                return RedirectToAction("Index");
            }
        }

        private static OportunidadeViewModel GetOportunidade()
        {
            try
            {
                var usuario = PixCoreValues.UsuarioLogado;
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/WpOportunidades/BuscarOportunidadePorData/" + usuario.idCliente + "/" +
                    PixCoreValues.UsuarioLogado.IdUsuario;

                var envio = new
                {
                    usuario.idCliente,
                    date = DateTime.UtcNow.ToString("yyyy-dd-MM"),
                };

                var helper = new ServiceHelper();
                var oportunidades = helper.Post<IEnumerable<OportunidadeViewModel>>(url, envio);

                return oportunidades.FirstOrDefault();
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}