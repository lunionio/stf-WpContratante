using Admin.Helppser;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Admin.Controllers
{
    public class VagaController : Controller
    {
        // GET: Vaga
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PublicarAgora(VagaViewModel vaga)
        {
            vaga.status = 1;
            vaga.DataEvento = Convert.ToDateTime(vaga.Date);
            if (SaveVaga(vaga))
                return Json("ok");
            else
                return Json("Desculpe, o sistema encontrou um erro ao efetuar sua solicitação." +
                    " Entre em contato com nosso suporte técnico.");
        }

        [HttpPost]
        public ActionResult PublicarMaisTarde(VagaViewModel vaga)
        {
            vaga.status = 2;
            if (SaveVaga(vaga))
                return RedirectToAction("Gerenciar");
            else
                return Json("Desculpe, o sistema encontrou um erro ao efetuar sua solicitação. Entre em contato" +
                    " com nosso suporte técnico.");
        }
        
        public PartialViewResult ModalConfirmarVaga(VagaViewModel model)
        {
            return PartialV iew(model);
        }

        public PartialViewResult _listarOportunidades()
        {//ESSE contratante
            var usuario = PixCoreValues.UsuarioLogado;
            var jss = new JavaScriptSerializer();
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/WpOportunidades/BuscarOportunidadePorEmpresa/" + usuario.idCliente + "/" +
                PixCoreValues.UsuarioLogado.IdUsuario;

            var envio = new
            {
                usuario.idEmpresa,
            };

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

            var oportunidades = default(IEnumerable<OportunidadeViewModel>);
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                if (string.IsNullOrEmpty(result)
                    || "null".Equals(result.ToLower()))
                {
                    throw new Exception("Ouve um erro durante o processo.");
                }

                oportunidades = jss.Deserialize<IEnumerable<OportunidadeViewModel>>(result);
            }

            IList<VagaViewModel> vagas = new List<VagaViewModel>();

            foreach (var o in oportunidades)
            {
                vagas.Add(new VagaViewModel() { Id = o.ID, Nome = o.Nome, ProfissionalNome = o.DescProfissional });
            }

            return PartialView(vagas);
        }

        public PartialViewResult _lsitarProfissionais()
        {
            return PartialView();
        }

        public PartialViewResult _vincularPorifissionais()
        {
            return PartialView();
        }

        private static bool SaveVaga(VagaViewModel vaga)
        {
            try
            {
                var usuario = PixCoreValues.UsuarioLogado;
                var jss = new JavaScriptSerializer();
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/WpOportunidades/SalvarOportunidade/" + usuario.idCliente + "/" +
                    PixCoreValues.UsuarioLogado.IdUsuario;

                vaga.status = 1;
                vaga.IdEmpresa = PixCoreValues.UsuarioLogado.idEmpresa;

                var op = Oportundiade.Convert(vaga);

                var envio = new
                {
                    oportunidade = op,
                };

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
                    if (string.IsNullOrEmpty(result)
                        || "null".Equals(result.ToLower()))
                    {
                        throw new Exception("Ouve um erro durante o processo.");
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível salvar o usuário.", e);
            }
        }  
    }
}