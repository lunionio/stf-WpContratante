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

            var checkins = GetProfissionaisQueFizeramCheckIn(_opId);

            var profissionais = GetProfissionais(result.Select(x => x.UserId));
            var users = GetUsers(profissionais.Select(x => x.Profissional.IdUsuario));

            var ids = checkins.Select(c => c.IdUsuario);

            IList<object> objects = new List<object>();
            foreach (var item in result)
            {
                item.Profissional = profissionais.FirstOrDefault(p => p.Profissional.IdUsuario.Equals(item.UserId))?.Profissional;

                if (item.Profissional != null)
                {
                    var confirmado = ids.Contains(item.UserId);

                    var resposta = new
                    {
                        Confirmado = confirmado,
                        item.Profissional.Nome,
                        Telefone = item.Profissional.Telefone.Numero,
                    };

                    objects.Add(resposta);
                }
            }

            return JsonConvert.SerializeObject(objects);
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

        private IEnumerable<CheckIn> GetProfissionaisQueFizeramCheckIn(int oportunidadeId)
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/WpCheckIn/BuscarPorIdExterno/" + usuario.idCliente + "/" + usuario.IdUsuario;

            var envio = new
            {
                idCliente = 1,
                idExterno = oportunidadeId,
            };

            var helper = new ServiceHelper();
            var result = helper.Post<IEnumerable<CheckIn>>(url, envio);

            return result;
        }

        private IEnumerable<ProfissionalServico> GetProfissionais(IEnumerable<int> ids)
        {
            try
            {
                var usuario = PixCoreValues.UsuarioLogado;
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/wpProfissionais/BuscarPorUsersIds/" + usuario.idCliente + "/" + usuario.IdUsuario;

                var envio = new
                {
                    idCliente = 1,
                    ids,
                };

                var helper = new ServiceHelper();
                var result = helper.Post<IEnumerable<ProfissionalServico>>(url, envio);

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível completar a operação.", e);
            }
        }

        private IEnumerable<UsuarioViewModel> GetUsers(IEnumerable<int> ids)
        {
            try
            {
                var usuario = PixCoreValues.UsuarioLogado;
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/Principal/BuscarUsuarios/" + usuario.idCliente + "/" + usuario.IdUsuario;

                var envio = new
                {
                    idCliente = 1,
                    ids,
                };

                var helper = new ServiceHelper();
                var result = helper.Post<IEnumerable<UsuarioViewModel>>(url, envio);

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível completar a operação.", e);
            }
        }
    }
}