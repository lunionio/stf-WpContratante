using Admin.Helppers;
using Admin.Helppser;
using Admin.Models;
using Admin.Models.Financeiro;
using Newtonsoft.Json;
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

            if (VerifcaSaldoCliente(vaga.Valor))
            {
                var op = SaveVaga(vaga);
                if (op != null && op.ID > 0)
                {
                    LancaTransacoes(op);
                    return Json("ok");
                }
                else
                    return Json("Desculpe, o sistema encontrou um erro ao efetuar sua solicitação." +
                        " Entre em contato com nosso suporte técnico.");
            }
            else
            {
                return Json("Saldo insuficiente.");
            }
        }

        [HttpPost]
        public ActionResult PublicarMaisTarde(VagaViewModel vaga)
        {
            vaga.status = 2;
            if (VerifcaSaldoCliente(vaga.Valor))
            {
                var op = SaveVaga(vaga);
                if (op != null && op.ID > 0)
                {
                    LancaTransacoes(op);
                    return RedirectToAction("Gerenciar");
                }
                else
                    return Json("Desculpe, o sistema encontrou um erro ao efetuar sua solicitação. Entre em contato" +
                        " com nosso suporte técnico.");
            }
            else
            {
                return Json("Saldo insuficiente.");
            }
        }
        
        public PartialViewResult ModalConfirmarVaga(VagaViewModel model)
        {
            return PartialView(model);
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

        private OportunidadeViewModel SaveVaga(VagaViewModel vaga)
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

                    return jss.Deserialize<OportunidadeViewModel>(result);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível salvar o usuário.", e);
            }
        }  

        private bool VerifcaSaldoCliente(decimal valorVaga)
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/WpFinanceiro/BuscarSaldo/" + usuario.idCliente + "/" + usuario.IdUsuario;

            var envio = new
            {
                usuario.idCliente,
                destino = usuario.idEmpresa
            };

            var helper = new ServiceHelper();
            var result = helper.Post<object>(url, envio);

            var saldo = Convert.ToDecimal(result);

            return saldo >= valorVaga;
        }

        private void LancaTransacoes(OportunidadeViewModel vaga)
        {
            IList<Extrato> extratos = new List<Extrato>();
            for (int i = 0; i < vaga.Quantidade; i++)
            {
                var valor1 = (vaga.Valor) * -1;

                var extrato1 = new Extrato(valor1 , 2, 1, PixCoreValues.UsuarioLogado.idEmpresa.ToString(), 
                    PixCoreValues.UsuarioLogado.idEmpresa.ToString(), vaga.ID, Status.Aprovado)
                {
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow,
                    DataEdicao = DateTime.UtcNow,
                    Descricao = "Debitando valor da vaga.",
                    IdCliente = PixCoreValues.IDCliente,
                    Nome = "Débito",
                    Status = 1,
                    UsuarioCriacao = PixCoreValues.UsuarioLogado.IdUsuario,
                    UsuarioEdicao = PixCoreValues.UsuarioLogado.IdUsuario,
                };

                var valor2 = vaga.Valor;

                var extrato2 = new Extrato(valor2, 2, 1, PixCoreValues.UsuarioLogado.idEmpresa.ToString(),
                    "16", vaga.ID, Status.Bloqueado)
                {
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow,
                    DataEdicao = DateTime.UtcNow,
                    Descricao = "Disponibilizando valor da vaga.",
                    IdCliente = PixCoreValues.IDCliente,
                    Nome = "Pagamento",
                    Status = 1,
                    UsuarioCriacao = PixCoreValues.UsuarioLogado.IdUsuario,
                    UsuarioEdicao = PixCoreValues.UsuarioLogado.IdUsuario,
                };

                extratos.Add(extrato1);
                extratos.Add(extrato2);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/WpFinanceiro/AlocarCredito/" + PixCoreValues.UsuarioLogado.idCliente + "/" +
                PixCoreValues.UsuarioLogado.IdUsuario;

            var envio = new
            {
                extratos,
            };

            var helper = new ServiceHelper();
            var result = helper.Post<object>(url, envio);
        }
    }
}