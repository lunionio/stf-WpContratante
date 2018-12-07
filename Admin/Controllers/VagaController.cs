using Admin.Helppers;
using Admin.Helppser;
using Admin.Models;
using Admin.Models.Financeiro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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

        public ActionResult Cadastrar(VagaViewModel model)
        {
            if (model != null)
            {
                return View(model);
            }

            return View(new VagaViewModel());
        }

        [HttpPost]
        public ActionResult PublicarAgora(VagaViewModel vaga)
        {
            vaga.status = 1;
            vaga.DataEvento = Convert.ToDateTime(vaga.Date);

            if (FinanceiroHelper.VerifcaSaldoCliente(vaga.Valor, PixCoreValues.UsuarioLogado))
            {
                var op = SaveVaga(vaga);
                if (op != null && op.ID > 0)
                {
                    FinanceiroHelper.LancaTransacoes(op, PixCoreValues.UsuarioLogado);
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
            if (FinanceiroHelper.VerifcaSaldoCliente(vaga.Valor, PixCoreValues.UsuarioLogado))
            {
                var op = SaveVaga(vaga);
                if (op != null && op.ID > 0)
                {
                    FinanceiroHelper.LancaTransacoes(op, PixCoreValues.UsuarioLogado);
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
        {
            var vagas = GetOportunidades(PixCoreValues.UsuarioLogado.idEmpresa);

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

        public PartialViewResult ModalMatch(int optId)
        {
            var userXOportunidades = GetProfissionaisByOpt(optId);

            var op = GetOportunidade(optId);
            ViewBag.OptNome = op.Nome;

            var profissionais = GetProfissionais(userXOportunidades.Select(x => x.UserId));
            var users = GetUsers(profissionais.Select(x => x.Profissional.IdUsuario));

            IList<ProfissionalViewModel> models = new List<ProfissionalViewModel>();

            foreach (var item in profissionais)
            {
                var user = users.FirstOrDefault(u => u.ID.Equals(item.Profissional.IdUsuario))?.Nome;
                var model = new ProfissionalViewModel(item.Profissional.ID, user, item.Servico.Nome, item.Profissional.Telefone.Numero,
                    item.Profissional.Telefone.ID, item.Profissional.DataNascimento.ToShortDateString(), item.Profissional.Email, item.Profissional.IdUsuario, item.Profissional.Endereco)
                {
                    StatusId = userXOportunidades.FirstOrDefault(x => x.UserId.Equals(item.Profissional.ID))?.Status.ID,
                    UserXOportunidadeId = userXOportunidades.FirstOrDefault(x => x.UserId.Equals(item.Profissional.ID))?.ID,
                    OportunidadeId = op.Id,
                    Valor = op.Valor,
                };

                models.Add(model);
            }

            ViewBag.CheckIns = GetProfissionaisQueFizeramCheckIn(optId);

            return PartialView(models);
        }

        private IEnumerable<UserXOportunidade> GetProfissionaisByOpt(int idOpt)
        {
            try
            {
                var usuario = PixCoreValues.UsuarioLogado;
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/WpOportunidades/BuscarUsuariosPorOportunidade/" + usuario.idCliente + "/" +
                    PixCoreValues.UsuarioLogado.IdUsuario;

                var envio = new
                {
                    idOpt,
                };

                var helper = new ServiceHelper();
                var result = helper.Post<IEnumerable<UserXOportunidade>>(url, envio);

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível completar a operação.", e);
            }
        }

        public VagaViewModel GetOportunidade(int id)
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/WpOportunidades/BuscarOportunidadePorID/" + usuario.idCliente + "/" +
                PixCoreValues.UsuarioLogado.IdUsuario;

            var envio = new
            {
                id,
            };

            var helper = new ServiceHelper();
            var o = helper.Post<OportunidadeViewModel>(url, envio);

            var vaga = new VagaViewModel()
            {
                Id = o.ID,
                Nome = o.Nome,
                ProfissionalNome = o.DescProfissional,
                Qtd = o.Quantidade,
                Valor = o.Valor,
                DataEvento = o.DataOportunidade,
                IdEmpresa = o.IdEmpresa
            };

            return vaga;
        }

        private IEnumerable<ProfissionalServico> GetProfissionais(IEnumerable<int> ids)
        {
            try
            {
                var usuario = PixCoreValues.UsuarioLogado;
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/wpProfissionais/BuscarPorIds/" + usuario.idCliente + "/" +
                    PixCoreValues.UsuarioLogado.IdUsuario;

                var envio = new
                {
                    usuario.idCliente,
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
                var url = keyUrl + "/Seguranca/Principal/BuscarUsuarios/" + usuario.idCliente + "/" +
                    PixCoreValues.UsuarioLogado.IdUsuario;

                var envio = new
                {
                    usuario.idCliente,
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

        public ActionResult Remover(VagaViewModel model)
        {
            try
            {
                var usuario = PixCoreValues.UsuarioLogado;
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/WpOportunidades/DeletarOportunidade/" + usuario.idCliente + "/" +
                    PixCoreValues.UsuarioLogado.IdUsuario;

                var op = Oportundiade.Convert(model);

                var envio = new
                {
                    oportunidade = op,
                };

                var helper = new ServiceHelper();
                var resut = helper.Post<object>(url, envio);

                var oportunidades = GetOportunidades(op.IdEmpresa);

                return View("Index");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = "Não foi possível desativar a oportunidade.";
                return View("Index");
            }
        }

        private static IList<VagaViewModel> GetOportunidades(int? idEmpresa)
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/WpOportunidades/BuscarOportunidadePorEmpresa/" + usuario.idCliente + "/" +
                PixCoreValues.UsuarioLogado.IdUsuario;

            var envio = new
            {
                idEmpresa,
            };

            var helper = new ServiceHelper();
            var oportunidades = helper.Post<IEnumerable<OportunidadeViewModel>>(url, envio);

            IList<VagaViewModel> vagas = new List<VagaViewModel>();

            foreach (var o in oportunidades)
            {
                vagas.Add(new VagaViewModel(o.ID, o.Nome, o.Endereco.CEP, o.Endereco.Local, o.Endereco.Bairro, o.Endereco.Cidade,
                    o.Endereco.Estado, o.HoraInicio, o.Valor, o.TipoProfissional, o.DescProfissional, o.Endereco.NumeroLocal,
                    (o.Valor * o.Quantidade).ToString(), o.Quantidade, o.Endereco.Complemento, o.Endereco.Complemento,
                    o.DataOportunidade.ToShortDateString(), o.Status, o.IdEmpresa, o.IdCliente, o.TipoServico)
                {
                    EnderecoId = o.Endereco.ID,
                    EnderecoDataCriacao = o.Endereco.DataCriacao.ToShortDateString(),
                    DataCriacao = o.DataCriacao.ToShortDateString()
                });
            }

            return vagas;
        }

        [HttpPost]
        public string Match(UserXOportunidade userXOportunidade)
        {
            try
            {
                var usuario = PixCoreValues.UsuarioLogado;
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/WpOportunidades/CanditarOportunidade/" + usuario.idCliente + "/" +
                    PixCoreValues.UsuarioLogado.IdUsuario;

                var pServico = GetProfissionais(new List<int>() { userXOportunidade.UserId }).SingleOrDefault();
                var user = GetUsers(new List<int>() { pServico.Profissional.IdUsuario }).SingleOrDefault();
                var op = GetOportunidade(userXOportunidade.OportunidadeId);

                var candidatos = GetProfissionaisByOpt(userXOportunidade.OportunidadeId);
                var qtdCandAprovados = candidatos.Where(c => c.OportunidadeId.Equals(op.Id) && c.StatusID == 1).Count();

                if (op.Qtd > qtdCandAprovados && userXOportunidade.StatusID != 3)
                {
                    if (!FinanceiroHelper.VerifcaSaldoCliente(op.Valor, PixCoreValues.UsuarioLogado))
                    {
                        return "Saldo insuficiente para a contratação.";
                    }

                    var envio = new
                    {
                        userXOportunidade,
                    };

                    var helper = new ServiceHelper();
                    var o = helper.Post<object>(url, envio);

                    if (userXOportunidade.StatusID == 1) //Aprovado
                    {
                        FinanceiroHelper.LancaTransacoes(op.Valor * -1, "16", 3,
                            "16", 3, 2, 2, "Pagando contratado.", PixCoreValues.UsuarioLogado, op.Id);

                        FinanceiroHelper.LancaTransacoes(op.Valor, "16", 3,
                            pServico.Profissional.ID.ToString(), 1, 2, 1, "Pagando contratado.", PixCoreValues.UsuarioLogado, op.Id);
                    }

                    return JsonConvert.SerializeObject(new ProfissionalViewModel(pServico.Profissional.ID, user.Nome, pServico.Servico.Nome, pServico.Profissional.Telefone.Numero,
                        pServico.Profissional.Telefone.ID, pServico.Profissional.DataNascimento.ToShortDateString(), pServico.Profissional.Email, pServico.Profissional.IdUsuario, pServico.Profissional.Endereco)
                    {
                        Valor = op.Valor,
                    });
                }

                //Reprovando os demais profissionais
                var profissionais = candidatos.Where(c => c.OportunidadeId.Equals(op.Id) && c.StatusID == 2);
                foreach (var item in profissionais)
                {
                    item.StatusID = 3;

                    var envio = new
                    {
                        userXOportunidade = item,
                    };

                    var helper = new ServiceHelper();
                    var o = helper.Post<object>(url, envio);
                }


                return "Não é possível aprovar mais profissionais para essa vaga.";
            }
            catch (Exception e)
            {
                return "Não foi possível completar a operação.";
            }
        }

        public ActionResult ModalProfissional(int pId)
        {
            var p = GetProfissional(pId);
            p.Profissional.Formacoes = GetFormacoes(p.Profissional.ID);
            var u = GetUsuario(p.Profissional.IdUsuario);

            var profissional = new ProfissionalViewModel()
            {
                Nome = p.Profissional.Nome,
                Avatar = u.Avatar,
                AreaAtuacao = p.Servico.Nome,
                Telefone = p.Profissional.Telefone.Numero,
                Formacoes = p.Profissional.Formacoes.Select(f => f.Nome),
                Referencia = p.Profissional.Descricao,
            };

            return PartialView(profissional);
        }

        private ProfissionalServico GetProfissional(int id)
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/wpProfissionais/BuscarPorId/" + usuario.idCliente + "/" +
                PixCoreValues.UsuarioLogado.IdUsuario;

            var envio = new
            {
                usuario.idCliente,
                idProfissional = id,
            };

            var helper = new ServiceHelper();
            var p = helper.Post<ProfissionalServico>(url, envio);

            return p;
        }

        private IList<ProfissionalFormacao> GetFormacoes(int pId)
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/wpProfissionais/buscarFormacoes/" + usuario.idCliente + "/" +
                PixCoreValues.UsuarioLogado.IdUsuario;

            var envio = new
            {
                usuario.idCliente,
                idProfissional = pId,
            };

            var helper = new ServiceHelper();
            var formacoes = helper.Post<IList<ProfissionalFormacao>>(url, envio);

            return formacoes;
        }

        private UsuarioViewModel GetUsuario(int id)
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/Principal/BuscarUsuarioPorId/" + usuario.idCliente + "/" +
                PixCoreValues.UsuarioLogado.IdUsuario;

            var envio = new
            {
                usuario.idCliente,
                idUsuario = id,
            };

            var helper = new ServiceHelper();
            var u = helper.Post<UsuarioViewModel>(url, envio);

            return u;
        }

        public ActionResult ModalCheckIn(int pId)
        {
            var p = GetProfissional(pId);
            p.Profissional.Formacoes = GetFormacoes(p.Profissional.ID);
            var u = GetUsuario(p.Profissional.IdUsuario);
            var d = GetDadosBancarios(p.Profissional.ID);

            var profissional = new ProfissionalViewModel()
            {
                Nome = p.Profissional.Nome,
                Avatar = u.Avatar,
                AreaAtuacao = p.Servico.Nome,
                Telefone = p.Profissional.Telefone.Numero,
                Formacoes = p.Profissional.Formacoes.Select(f => f.Nome),
                Referencia = p.Profissional.Descricao,
                Agencia = d.Agencia,
                Conta = d.Conta,
                Banco = d.Banco,
            };

            return PartialView(profissional);
        }

        public DadosBancarios GetDadosBancarios(int profissionalId)
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/WpFinanceiro/BuscarDadosBancariosPorUsuario/" + usuario.idCliente + "/" +
                PixCoreValues.UsuarioLogado.IdUsuario;

            var envio = new
            {
                usuario.idCliente,
                codigoExterno = profissionalId
            };

            var helper = new ServiceHelper();
            var dados = helper.Post<DadosBancarios>(url, envio);

            return dados;
        }

        private IEnumerable<CheckInViewModel> GetProfissionaisQueFizeramCheckIn(int oportunidadeId)
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/WpCheckIn/BuscarPorIdExterno/" + usuario.idCliente + "/" +
                PixCoreValues.UsuarioLogado.IdUsuario;

            var envio = new
            {
                usuario.idCliente,
                idExterno = oportunidadeId,
            };

            var helper = new ServiceHelper();
            var result = helper.Post<IEnumerable<CheckIn>>(url, envio);

            var profissionais = GetProfissionais(result.Select(x => x.IdUsuario));
            var users = GetUsers(profissionais.Select(x => x.Profissional.IdUsuario));
            var extratos = GetExtratos(oportunidadeId);

            IList<CheckInViewModel> response = new List<CheckInViewModel>();

            foreach (var item in profissionais)
            {
                var user = users.FirstOrDefault(u => u.ID.Equals(item.Profissional.IdUsuario));
                var ck = result.FirstOrDefault(c => c.IdUsuario.Equals(item.Profissional.ID));
                var extrato = extratos.FirstOrDefault(e => e.Destino.Equals(item.Profissional.ID.ToString()));

                var checkin = new CheckInViewModel()
                {
                    Id = item.Profissional.ID,
                    OportunidadeId = oportunidadeId,
                    Hora = ck.DataCriacao,
                    Nome = user.Nome,
                    StatusPagamento = (int)extrato.StatusId,
                };

                response.Add(checkin);
            }

            return response;
        }

        private IEnumerable<Extrato> GetExtratos(int codigoExterno)
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/WpFinanceiro/BuscaExtratos/" + usuario.idCliente + "/" +
                PixCoreValues.UsuarioLogado.IdUsuario;

            var envio = new
            {
                extrato = new
                {
                    CodigoExterno = codigoExterno,
                    TipoDestino = 1
                },
            };

            var helper = new ServiceHelper();
            var result = helper.Post<IEnumerable<Extrato>>(url, envio);

            return result.Where(r => r.StatusId != Models.Financeiro.Status.Aguardando);
        }

        [HttpPost]
        public string LiberarPagamentos(IEnumerable<CheckInViewModel> models)
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/WpFinanceiro/LiberarPagto/" + usuario.idCliente + "/" +
                PixCoreValues.UsuarioLogado.IdUsuario;

            var result = string.Empty;
            foreach (var model in models)
            {
                var envio = new
                {
                    usuario.idCliente,
                    codigoExterno = model.OportunidadeId,
                    destino = model.Id,
                    tipoDestino = 1
                };

                var helper = new ServiceHelper();
                var response = helper.Post<object>(url, envio);

                result = Convert.ToString(response);
            }

            return result;
        }
    }
}