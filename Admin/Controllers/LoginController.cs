using Admin.Helppser;
using Admin.Models;
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
        public ActionResult Index(string login, string senha)
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
                    // Response.Redirect(PixCoreValues.defaultSiteUrl);
                    return Redirect("/home/index");
                }
                else
                {

                    ViewData["TemporariaMensagem"] = "Usuario ou senha invalida";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ViewData["TemporariaMensagem"] = "Usuario ou senha invalida";
                return RedirectToAction("Index");
            }

            
        }
    }
}