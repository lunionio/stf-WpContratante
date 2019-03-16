using Admin.Helppers;
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

                if (!string.IsNullOrEmpty(current.Request.Cookies["IdCliente"].Value))
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

                Usuario.UsuarioXPerfil = GetPerfilUsuario(Usuario.ID);

                var current = HttpContext.Current;
                string cookievalue;
                if (Usuario.ID != 0)
                {
                    if (Convert.ToBoolean(Usuario.VAdmin))
                    {
                        user.idCliente = Usuario.idCliente;
                        user.IdUsuario = (int)Usuario.ID;
                        user.idEmpresa = Usuario.IdEmpresa;
                        user.Nome = Usuario.Nome;
                        user.Avatar = Usuario.Avatar;
                        user.idPerfil = Usuario.UsuarioXPerfil.IdPerfil;

                        //if (current.Request.Cookies["UsuarioLogado"] != null)
                        //{
                        //    current.Request.Cookies["UsuarioLogado"].Value = string.Empty;
                        //}

                        current.Response.Cookies["UsuarioLogado"].Value = jss.Serialize(user);
                        current.Response.Cookies["UsuarioLogado"].Expires = DateTime.Now.AddMinutes(30); // add expiry time

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

        private static UsuarioXPerfil GetPerfilUsuario(int id)
        {
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = $"{ keyUrl }/Perfil/GetPerfilByUsuario/{ id }";

            var helper = new ServiceHelper();
            var usuarioXPerfil = helper.Get<UsuarioXPerfil>(url);

            return usuarioXPerfil;
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
            //var keyUrlIn = ConfigurationManager.AppSettings["UrlAPIIn"].ToString();
            //var urlAPIIn = keyUrlIn + "cliente";
            //var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            //var result = client.DownloadString(string.Format(urlAPIIn));
            //var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            //ClienteViewModel[] Cliente = jss.Deserialize<ClienteViewModel[]>(result);

            //var clienteLol = Cliente.Where(x => urlDoCliente.Contains(x.Url)).FirstOrDefault();
            //if (clienteLol != null)
            //{
            //    return clienteLol.ID;
            //}
            //else
            //{
            //    return 0;
            //}

            return 1;
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

        public static void AtualizarUsuarioLogado(UsuarioViewModel usuario)
        {
            usuario.UsuarioXPerfil = GetPerfilUsuario(usuario.ID);

            var login = new LoginViewModel()
            {
                idCliente = usuario.idCliente,
                idPerfil = usuario.UsuarioXPerfil.IdPerfil,
                IdUsuario = usuario.ID,
                idEmpresa = usuario.IdEmpresa,
                Nome = usuario.Nome,
                Avatar = usuario.Avatar,
            };

            HttpContext.Current.Response.Cookies["UsuarioLogado"].Value = null;
            HttpContext.Current.Response.Cookies["UsuarioLogado"].Value = new JavaScriptSerializer().Serialize(login);
        }

        public static void Sair()
        {
            var current = HttpContext.Current;

            if (!string.IsNullOrEmpty(current.Request.Cookies["UsuarioLogado"].Value))
            {
                current.Request.Cookies["UsuarioLogado"].Value = null;
            }

            //if (!string.IsNullOrEmpty(current.Request.Cookies["IdCliente"].Value))
            //{
            //    current.Request.Cookies["IdCliente"].Value = null;
            //}
        }
    }
}