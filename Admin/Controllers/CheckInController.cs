using Admin.Helppers;
using Admin.Helppser;
using Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Admin.Controllers
{
    public class CheckInController : Controller
    {
        public static int _opId;

        // GET: CheckIn
        public ActionResult Index(int optId)
        {
            _opId = optId;
            return View();
        }

        [HttpGet]
        public string GetOpInfo()
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/WpOportunidades/BuscarUsuariosPorOportunidade/" + usuario.idCliente + "/" + usuario.IdUsuario;

            var envio = new
            {
                idOpt = _opId,
            };

            var helper = new ServiceHelper();
            var result = helper.Post<IEnumerable<UserXOportunidade>>(url, envio);

            url = keyUrl + "/Seguranca/wpProfissionais/BuscarPorUsersIds/" + usuario.idCliente + "/" + usuario.IdUsuario;

            var ids = result.Select(x => x.UserId);

            var obj = new
            {
                usuario.idCliente,
                ids,
            };

            var response = helper.Post<IEnumerable<ProfissionalServico>>(url, obj);

            foreach (var item in result)
            {
                item.Profissional = response.FirstOrDefault(p => p.Profissional.IdUsuario.Equals(item.UserId))?.Profissional;
            }

            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        public string GetQrCode()
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/WpQrCode/GeraCodigoQr/" + usuario.idCliente + "/" + usuario.IdUsuario;

            object envio = new
            {
               usuario.idCliente,
               idUsuario = usuario.IdUsuario,
            };

            var jss = new JavaScriptSerializer();
            var data = jss.Serialize(envio);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }
    }
}