using Admin.Helppser;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Admin.Controllers
{
    public class SharedController : Controller
    {
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult GetMenus()
        {
            var tipoAcoes = GetTipoAcoes();
            var model = GetEstruturas(tipoAcoes);

            return PartialView("PartialMenu", model);
        }

        private IEnumerable<Estrutura> GetEstruturas(IEnumerable<int> tipoAcoes)
        {
            var model = default(IList<Estrutura>);
            using (var client = new WebClient())
            {
                var jss = new JavaScriptSerializer();
                var url = ConfigurationManager.AppSettings["UrlAPI"];
                var serverUrl = $"{ url }/Seguranca/Principal/buscarEstruturas/{ PixCoreValues.IDCliente }/{ PixCoreValues.UsuarioLogado.IdUsuario }";
                client.Headers[HttpRequestHeader.ContentType] = "application/json";

                object envio = new
                {
                    PixCoreValues.UsuarioLogado.idCliente,
                    idTipoAcoes = tipoAcoes,
                };

                var data = jss.Serialize(envio);
                var response = client.UploadString(serverUrl, "POST", data);
                var result = jss.Deserialize<List<EstruturaViewModel>>(response).OrderBy(r => r.Ordem).ToList();

                if (result != null && result.Count() > 0)
                {
                    model = new List<Estrutura>();
                    foreach (var r in result)
                    {
                        r.SubMenus = result.Where(e => e.IdPai.Equals(r.ID)).OrderBy(x => x.Ordem);
                        
                        if(r.IdPai == 0) //TODO: Melhorar para sub-estrutura com filhos...
                        {
                            var estrutura = new Estrutura(r.ID, r.UrlManual, r.Imagem, r.Nome)
                            {
                                SubEstruturas = r.SubMenus.Select(s => new Estrutura(s.ID, s.UrlManual, s.Imagem, s.Nome))
                            };

                            model.Add(estrutura);
                        }
                    }
                }
            }

            return model;
        }

        private IEnumerable<int> GetTipoAcoes()
        {
            using (var client = new WebClient())
            {
                var url = ConfigurationManager.AppSettings["UrlAPI"];
                var serverUrl = $"{ url }/Permissao/GetPermissoesPorUsuario/{ PixCoreValues.UsuarioLogado.IdUsuario }";
                var result = client.DownloadString(serverUrl);

                var jss = new JavaScriptSerializer();
                var permissao = jss.Deserialize<Permissao>(result);

                var tipoAcoes = permissao.idTipoAcao.Split(',');

                return tipoAcoes.Select(id => Convert.ToInt32(id));
            }
        }
    }
}