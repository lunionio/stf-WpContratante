using Admin.Helppser;
using Admin.Models;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Admin.Controllers
{
    public class FaleConoscoController : Controller
    {
        // GET: FaleConosco
        public ActionResult Index()
        {
            //GetTipos();

            ViewBag.email = PixCoreValues.UsuarioLogado.Login;
            return View();
        }

        public ActionResult GetTipos()
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/wpAtendimento/BuscarTipos/" + usuario.idCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;

            var result = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }

            var jss = new JavaScriptSerializer();
            var teste = jss.Deserialize<object>(result);
            return Json(teste, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Enviar(FaleconoscoVIewModel model)
        {
            var ticket = Save(model);

            return Json(ticket);
        }

        private string Save(FaleconoscoVIewModel model)
        {
            try
            {
                var usuario = PixCoreValues.UsuarioLogado;
                var jss = new JavaScriptSerializer();
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/wpAtendimento/AbrirTicket/" + usuario.idCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                object envio = Helppers.FaleConosco.Convert(model);

                var obj = new
                {
                    ticket = envio,
                };

                var data = jss.Serialize(obj);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(data);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var ticket = string.Empty;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    ticket = streamReader.ReadToEnd();
                    if (string.IsNullOrEmpty(ticket)
                        || "null".Equals(ticket.ToLower()))
                    {
                        throw new Exception("Ouve um erro durante o processo.");
                    }
                }

                return ticket;
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível salvar o usuário.", e);
            }
        }


    }
}