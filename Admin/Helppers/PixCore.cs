using Admin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace Admin.Helppser
{
    public class PixCoreValues
    {
        #region Propiedades
        private LoginViewModel usuarioLogado;
        public static LoginViewModel UsuarioLogado
        {
            get { return VerificaLogado(); }
        }

        private static int IdCliente;
        public static int IDCliente
        {
            get
            {
                string url = HttpContext.Current.Request.Url.Host;
                int porta = HttpContext.Current.Request.Url.Port;
                string protocolo = HttpContext.Current.Request.Url.Scheme;

                var urlDoCliente = protocolo + "://" + url + ":" + porta.ToString() + HttpContext.Current.Request.Url.PathAndQuery;
                var DefaultSiteUrl = protocolo + "://" + url + ":" + porta.ToString() + "/";
                var current = HttpContext.Current;

                if (current.Request.Cookies["IdCliente"] != null)
                {
                    var cookiesValido = current.Request.Cookies["IdCliente"].Value;
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    IdCliente = jss.Deserialize<int>(cookiesValido);
                    return IdCliente;
                }
                else
                {
                    current.Response.Redirect(DefaultSiteUrl);
                    return 0;
                }

            }

        }

        private static string DefaultSiteUrl;
        public static string defaultSiteUrl
        {
            get
            {
                string url = HttpContext.Current.Request.Url.Host;
                int porta = HttpContext.Current.Request.Url.Port;
                string protocolo = HttpContext.Current.Request.Url.Scheme;
                if (porta != 80)
                {
                    DefaultSiteUrl = protocolo + "://" + url + ":" + porta.ToString() + "/";
                }
                else
                {
                    DefaultSiteUrl = protocolo + "://" + url + "/";
                }
                return DefaultSiteUrl;
            }
        }

        #endregion

        //Controle de login deus me ajuda OMG :O
        public static bool Login(LoginViewModel user)
        {
            user.idCliente = IDCliente;

            using (var client = new WebClient())
            {
                var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/Principal/loginUsuario/" + IDCliente + "/" + 999;
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                object envio = new { ObjLogin = user };
                var data = jss.Serialize(envio);
                var result = client.UploadString(url, "POST", data);
                UsuarioViewModel Usuario = jss.Deserialize<UsuarioViewModel>(result);


                var current = HttpContext.Current;
                string cookievalue;
                if (Usuario.ID != 0)
                {
                    if (Convert.ToBoolean(Usuario.VAdmin))
                    {
                        user.idCliente = 1;
                        user.idPerfil = Usuario.PerfilUsuario;
                        user.IdUsuario = (int)Usuario.ID;

                        if (current.Request.Cookies["UsuarioLogado"] != null)
                        {
                            cookievalue = current.Request.Cookies["UsuarioLogado"].ToString();
                        }
                        else
                        {
                            current.Response.Cookies["UsuarioLogado"].Value = jss.Serialize(user);
                            current.Response.Cookies["UsuarioLogado"].Expires = DateTime.Now.AddMinutes(30); // add expiry time
                        }
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }


        }

        public static LoginViewModel VerificaLogado()
        {
            var current = HttpContext.Current;

            if (current.Request.Cookies["UsuarioLogado"] != null)
            {
                var cookiesValido = current.Request.Cookies["UsuarioLogado"].Value;
                var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                LoginViewModel Usuario = jss.Deserialize<LoginViewModel>(cookiesValido);
                return Usuario;
            }
            else
            {
                //current.Response.Redirect("http://localhost:49983/login/login");
                return new LoginViewModel();
            }
        }

        public static int VerificaUrlCliente(string urlDoCliente)
        {
            var keyUrlIn = ConfigurationManager.AppSettings["UrlAPIIn"].ToString();
            var urlAPIIn = keyUrlIn + "cliente";
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(urlAPIIn));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            ClienteViewModel[] Cliente = jss.Deserialize<ClienteViewModel[]>(result);

            var clienteLol = Cliente.Where(x => urlDoCliente.Contains(x.Url)).FirstOrDefault();
            if (clienteLol != null)
            {
                return clienteLol.ID;
            }
            else
            {
                return 0;
            }
        }

        public static void RenderUrlPage(HttpContext context)
        {
            var keyUrlIn = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var urlAPIIn = keyUrlIn + "/Seguranca/Principal/buscarEstilo/" + IDCliente + "/" + 999;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(urlAPIIn));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            PageViewModel[] Cliente = jss.Deserialize<PageViewModel[]>(result);

            string url = HttpContext.Current.Request.Url.Host;
            int porta = HttpContext.Current.Request.Url.Port;
            string protocolo = HttpContext.Current.Request.Url.Scheme;
            var urlDoCliente = "";

            if (porta != 80)
            {
                urlDoCliente = protocolo + "://" + url + ":" + porta.ToString() + HttpContext.Current.Request.Url.PathAndQuery;
            }
            else
            {
                urlDoCliente = protocolo + "://" + url + HttpContext.Current.Request.Url.PathAndQuery;
            }

            PageViewModel page = Cliente.Where(x => x.Url == urlDoCliente).FirstOrDefault();
            if (page != null)
            {
                if (HttpContext.Current.Request.Url.AbsoluteUri != (urlDoCliente + "page/index/" + page.ID.ToString()))
                {

                    HttpContext.Current.Response.Status = "301 Moved Permanently";
                    HttpContext.Current.Response.AddHeader("Location", DefaultSiteUrl + "page/index/" + page.ID.ToString());
                }
            }
            else
            {
                //TODO: Necessário? página criada localmente via html...
                //HttpContext.Current.Response.StatusCode = 404;
            }
        }
    }
}