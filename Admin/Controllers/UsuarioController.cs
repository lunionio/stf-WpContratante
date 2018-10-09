using Admin.Controllers.Attributes;
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
            var result = GetPermissoes();

            ViewBag.Perfis = new SelectList(result);
            return View();
        }

        private static IEnumerable<string> GetPermissoes()
        {
            try
            {
                IEnumerable<string> result;
                using (var client = new WebClient())
                {
                    var jss = new JavaScriptSerializer();
                    var url = ConfigurationManager.AppSettings["UrlAPI"];
                    //var serverUrl = $"{ url }/permissao/getallpermissao/{ _idCliente }"; //TODO: Necessário cadastrar perfil com id do usuário
                    var serverUrl = $"{ url }/permissao/getallpermissao/{ 1 }";
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var response = client.DownloadString(new Uri(serverUrl));
                    var permissoes = jss.Deserialize<IEnumerable<PermissaoViewModel>>(response);

                    result = permissoes.Select(p => p.Nome);
                }

                return result;
            }
            catch(Exception e)
            {
                return new List<string>();
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
                var result = GetPermissoes();
                ViewBag.Perfis = new SelectList(result);

                if (! string.IsNullOrEmpty(viewModel.Nome) && !string.IsNullOrEmpty(viewModel.Login) 
                    && !string.IsNullOrEmpty(viewModel.Senha) && !string.IsNullOrEmpty(viewModel.Perfil))
                {
                    viewModel.idCliente = _idCliente;

                    if(viewModel.ID == 0)
                        viewModel.UsuarioCriacao = PixCoreValues.UsuarioLogado.IdUsuario;

                    viewModel.UsuarioEdicao = PixCoreValues.UsuarioLogado.IdUsuario;
                    viewModel.Ativo = true;
                    if (SaveUsuario(viewModel))
                    {
                        ViewData["Resultado"] = new ResultadoViewModel("Usuário cadastrado com sucesso!", true);
                        ModelState.Clear();
                        return RedirectToAction("Listagem");
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
            var result = GetPermissoes();
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

                var users = GetUsuarios(_idCliente);
            try
            {
                var usuario = users.FirstOrDefault(u => u.ID.Equals(id));

                if (DeleteUsuario(usuario))
                {
                    var usuarios = GetUsuarios(_idCliente);
                    return View("Listagem", usuarios);
                }

                ViewData["ResultadoDelete"] = new ResultadoViewModel("Não foi possível deletar o usuário.", false);
                return View("Listagem", users);
            }
            catch(Exception e)
            {
                ViewData["ResultadoDelete"] = new ResultadoViewModel("Não foi possível deletar o usuário.", false);
                return View("Listagem", users);
            }
        }

        private bool SaveUsuario(UsuarioViewModel usuario)
        {
            try
            {
                var jss = new JavaScriptSerializer();
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/Principal/salvarUsuario/" + usuario.idCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                object envio = new 
                {
                    usuario = new
                    {
                        usuario.ID,
                        usuario.idCliente,
                        usuario.Nome,
                        usuario.Login,
                        usuario.Senha,
                        usuario.UsuarioCriacao,
                        usuario.UsuarioEdicao,
                        usuario.Ativo
                    }
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

        private IEnumerable<UsuarioViewModel> GetUsuarios(int idCliente)
        {
            try
            {
                using (var client = new WebClient())
                {
                    var jss = new JavaScriptSerializer();
                    var url = ConfigurationManager.AppSettings["UrlAPI"];
                    var serverUrl = $"{ url }/Seguranca/Principal/buscarUsuario/{ _idCliente }/{ PixCoreValues.UsuarioLogado.IdUsuario }";
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var response = client.DownloadString(new Uri(serverUrl));
                    var result = jss.Deserialize<IEnumerable<UsuarioViewModel>>(response);

                    return result;
                }
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
                var jss = new JavaScriptSerializer();
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var url = keyUrl + "/Seguranca/Principal/DeletarUsuario/" + usuario.idCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                object envio = new
                {
                    usuario = new
                    {
                        idUsuario = usuario.ID
                    }
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