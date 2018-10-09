using Admin.Helppser;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            //teste 2
            var url = HttpContext.Current.Request.Url.Host;
            var porta = HttpContext.Current.Request.Url.Port;
            var protocolo = HttpContext.Current.Request.Url.Scheme;

            var urlDoCliente = "";

            if (porta != 80)
                urlDoCliente = protocolo + "://" + url + ":" + porta.ToString() + "/";
            else
                urlDoCliente = protocolo + "://" + url + "/";

            int idCliente = PixCoreValues.VerificaUrlCliente(urlDoCliente);
            if (idCliente != 0)
            {
                var cookievalue = string.Empty;
                if (Request.Cookies["IdCliente"] != null)
                {
                    cookievalue = Request.Cookies["IdCliente"].ToString();
                }
                else
                {
                    Response.Cookies["IdCliente"].Value = idCliente.ToString();
                    Response.Cookies["IdCliente"].Expires = DateTime.Now.AddMinutes(1); // add expiry time
                }
                PixCoreValues.RenderUrlPage(HttpContext.Current);
            }
            else
            {                
                Response.StatusCode = 404;
            }

            var usuariologado = PixCoreValues.UsuarioLogado;
            if (usuariologado == null || usuariologado.IdUsuario == 0)
            {
                if (!HttpContext.Current.Request.Url.AbsoluteUri.Contains("Login"))
                {

                    //Verfica login
                    if (usuariologado == null || usuariologado.IdUsuario == 0)
                    {
                        HttpContext.Current.Response.Redirect(urlDoCliente + "Login/Index");
                    }
                    else
                        HttpContext.Current.Response.Redirect(urlDoCliente);
                }
            }
        }
    }
}