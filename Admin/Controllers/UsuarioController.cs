using Admin.Controllers.Attributes;
using Admin.Helppers;
using Admin.Helppser;
using Admin.Models;
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
    [NoDirectAccess]
    public class UsuarioController : Controller
    {
        private int _idCliente;

        public UsuarioController()
        {
            _idCliente = PixCoreValues.IDCliente;
        }

        public ActionResult Cadastro()
        {
            var result = GetPerfis().Select(p => p.Nome);

            ViewBag.Perfis = new SelectList(result);
            return View();
        }

        private IEnumerable<Perfil> GetPerfis()
        {
            try
            {
                var usuario = PixCoreValues.UsuarioLogado;
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Perfil/GetAllPerfil/" + usuario.idCliente;

                var helper = new ServiceHelper();
                var result = helper.Get<IEnumerable<Perfil>>(url);

                return result;
            }
            catch(Exception e)
            {
                return new List<Perfil>();
            }
        }

        public ActionResult Listagem()
        {
            var usuarios = GetUsuarios(_idCliente);
            return View(usuarios);
        }

        [HttpPost]
        public ActionResult Cadastro(UsuarioViewModel viewModel)
        {
            try
            {
                var result = GetPerfis();
                ViewBag.Perfis = new SelectList(result.Select(p => p.Nome));

                if (! string.IsNullOrEmpty(viewModel.Nome) && !string.IsNullOrEmpty(viewModel.Login) 
                    && !string.IsNullOrEmpty(viewModel.Senha) && !string.IsNullOrEmpty(viewModel.Perfil))
                {
                    viewModel.idCliente = _idCliente;

                    if(viewModel.ID == 0)
                        viewModel.UsuarioCriacao = PixCoreValues.UsuarioLogado.IdUsuario;

                    viewModel.UsuarioEdicao = PixCoreValues.UsuarioLogado.IdUsuario;
                    viewModel.Ativo = true;
                    viewModel.VAdmin = "true";
                    viewModel.Status = 1;
                    viewModel.IdEmpresa = PixCoreValues.UsuarioLogado.idEmpresa;

                    var perfil = result.SingleOrDefault(p => p.Nome.Equals(viewModel.Perfil));

                    var user = SaveUsuario(viewModel);

                    if (user.ID > 0)
                    {
                        var usuarioXPerfil = VincularPerfil(user.ID, perfil.ID, viewModel.UsuarioXPerfil.Id);

                        if (usuarioXPerfil.Id > 0)
                        {
                            ViewData["Resultado"] = new ResultadoViewModel("Usuário cadastrado com sucesso!", true);
                            ModelState.Clear();
                            return RedirectToAction("Listagem");
                        }
                    }
                }

                ViewData["Resultado"] = new ResultadoViewModel("Informe todos os dados necessários.", false);
                return View("Cadastro", viewModel);
            }
            catch (Exception e)
            {
                ViewData["Resultado"] = new ResultadoViewModel("Não foi possível salvar o usuário.", false);                
                return View("Cadastro", viewModel);
            }
        }

        public ActionResult Editar(int? id)
        {
            var result = GetPerfis().Select(p => p.Nome);
            ViewBag.Perfis = new SelectList(result);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var usuarios = GetUsuarios(_idCliente);

            var usuarioFiltrado = usuarios.FirstOrDefault(x => x.ID.Equals(id));

            return View("Cadastro", usuarioFiltrado);
        }

        public ActionResult Excluir(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var usuario = GetUsuario((int)id, _idCliente);

                if (DeleteUsuario(usuario))
                {
                    return View("Listagem", GetUsuarios(_idCliente));
                }

                ViewData["ResultadoDelete"] = new ResultadoViewModel("Não foi possível deletar o usuário.", false);
                return View("Listagem", GetUsuarios(_idCliente));
            }
            catch(Exception e)
            {
                ViewData["ResultadoDelete"] = new ResultadoViewModel("Não foi possível deletar o usuário.", false);
                return View("Listagem", GetUsuarios(_idCliente));
            }
        }

        public UsuarioViewModel GetUsuario(int id, int idCliente)
        {
            try
            {
                var url = ConfigurationManager.AppSettings["UrlAPI"];
                var serverUrl = $"{ url }/Seguranca/Principal/BuscarUsuarioPorId/{ _idCliente }/{ PixCoreValues.UsuarioLogado.IdUsuario }";

                var helper = new ServiceHelper();
                var result = helper.Get<UsuarioViewModel>(serverUrl);

                result.UsuarioXPerfil = GetPerfilUsuario(result.ID);

                result.Perfil = GetPerfil(result.UsuarioXPerfil.IdPerfil).Nome;

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível listar os usuários.", e);
            }
        }

        private Perfil GetPerfil(int idPerfil)
        {
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = $"{ keyUrl }/Perfil/GetPerfilByID/{ idPerfil }";

            var helper = new ServiceHelper();
            var result = helper.Get<Perfil>(url);

            return result;
        }

        private UsuarioXPerfil GetPerfilUsuario(int usuarioId)
        {
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = $"{ keyUrl }/Perfil/GetPerfilByUsuario/{ usuarioId }";

            var helper = new ServiceHelper();
            var usuarioXPerfil = helper.Get<UsuarioXPerfil>(url);

            return usuarioXPerfil;
        }

        private UsuarioViewModel SaveUsuario(UsuarioViewModel usuario)
        {
            try
            {
                var jss = new JavaScriptSerializer();
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/Principal/salvarUsuario/" + usuario.idCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;

                var envio = new 
                {
                    usuario,
                };

                var helper = new ServiceHelper();
                var result = helper.Post<UsuarioViewModel>(url, envio);

                if ("null".Equals(Convert.ToString(result).ToLower()))
                {
                    throw new Exception("Ocorreu um erro durante o processo.");
                }

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível salvar o usuário.", e);
            }
        }

        private IEnumerable<UsuarioViewModel> GetUsuarios(int idCliente)
        {
            try
            {
                var url = ConfigurationManager.AppSettings["UrlAPI"];
                var serverUrl = $"{ url }/Seguranca/Principal/buscarUsuario/{ _idCliente }/{ PixCoreValues.UsuarioLogado.IdUsuario }";

                var helper = new ServiceHelper();
                var result = helper.Get<IEnumerable<UsuarioViewModel>>(serverUrl);

                var usuariosXPerfis = GetPerfisUsuarios(result.Select(u => u.ID));
                var perfis = GetPerfis();

                foreach (var item in result)
                {
                    item.UsuarioXPerfil = usuariosXPerfis.FirstOrDefault(x => x.IdUsuario.Equals(item.ID));
                    if (item.UsuarioXPerfil != null)
                    {
                        item.Perfil = perfis.FirstOrDefault(p => p.ID.Equals(item.UsuarioXPerfil.IdPerfil)).Nome;
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível listar os usuários.", e);
            }
        }

        private bool DeleteUsuario(UsuarioViewModel usuario)
        {
            try
            {
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/Principal/DeletarUsuario/" + usuario.idCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                object envio = new
                {
                    usuario = new
                    {
                        idUsuario = usuario.ID
                    }
                };

                var helper = new ServiceHelper();
                var result = helper.Post<object>(url, envio);

                DesvincularPerfil(usuario.ID);

                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível salvar o usuário.", e);
            }
        }

        private void DesvincularPerfil(int id)
        {
            var usuarioXPerfil = GetPerfilUsuario(id);

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = $"{ keyUrl }/Perfil/DesvincularPerfil/";

            var helper = new ServiceHelper();
            var result = helper.Post<object>(url, usuarioXPerfil);
        }

        public ActionResult EditarUsuario()
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/Principal/BuscarUsuarioPorId/" + usuario.idCliente + "/" + usuario.IdUsuario;

            var envio = new
            {
                usuario.idCliente,
                idUsuario = usuario.IdUsuario,
            };

            var helper = new ServiceHelper();
            var result = helper.Post<UsuarioViewModel>(url, envio);

            result.UsuarioXPerfil = GetPerfilUsuario(result.ID);

            ViewBag.Perfis = new SelectList(GetPerfis().Select(p => p.Nome));

            return View("Editar", result);
        }

        public ActionResult AltararUsuario(UsuarioViewModel model)
        {
            try
            {
                var usuario = PixCoreValues.UsuarioLogado;
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/Principal/salvarUsuario/" + usuario.idCliente + "/" + usuario.IdUsuario;

                model.UsuarioEdicao = PixCoreValues.UsuarioLogado.IdUsuario;
                model.Ativo = true;
                model.idCliente = _idCliente;
                model.Status = 1;

                var envio = new
                {
                    usuario = model,
                };

                var helper = new ServiceHelper();
                var result = helper.Post<UsuarioViewModel>(url, envio);

                PixCoreValues.AtualizarUsuarioLogado(result);

                ViewBag.Perfis = new SelectList(GetPerfis());

                return RedirectToAction("EditarUsuario");
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível editar o usuário.", e);
            }
        }

        private IEnumerable<UsuarioXPerfil> GetPerfisUsuarios(IEnumerable<int> ids)
        {
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = $"{ keyUrl }/Perfil/GetUsuariosXPerfis/";

            var helper = new ServiceHelper();
            var usuariosXPerfis = helper.Post<IEnumerable<UsuarioXPerfil>>(url, ids);

            return usuariosXPerfis;
        }

        private UsuarioXPerfil VincularPerfil(int usuarioId, int perfilId, int vinculoId = 0)
        {
            try
            {
                var usuarioXPerfil = new UsuarioXPerfil()
                {
                    Id = vinculoId,
                    DataCriacao = DateTime.UtcNow,
                    DataEdicao = DateTime.UtcNow,
                    IdPerfil = perfilId,
                    IdUsuario = usuarioId,
                    UsuarioCriacao = PixCoreValues.UsuarioLogado.IdUsuario,
                    UsuarioEdicao = PixCoreValues.UsuarioLogado.IdUsuario,
                };

                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = $"{ keyUrl }/Perfil/SaveUsuarioXPerfil/";

                var helper = new ServiceHelper();
                var result = helper.Post<UsuarioXPerfil>(url, usuarioXPerfil);

                return result;
            }
            catch(Exception e)
            {
                return new UsuarioXPerfil();
            }
        }
    }
}