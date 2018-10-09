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
            var model = default(IList<EstruturaViewModel>);
            #region comentario
            //using (var client = new WebClient())
            //{
            //    var jss = new JavaScriptSerializer();
            //    var url = ConfigurationManager.AppSettings["UrlAPI"];
            //    var serverUrl = $"{ url }/Seguranca/Principal/buscarEstruturas/{ PixCoreValues.IDCliente }/{ PixCoreValues.UsuarioLogado.IdUsuario }";

            //    //Id Permissão passar no bpdy POST


            //    client.Headers[HttpRequestHeader.ContentType] = "application/json";
            //    var response = client.DownloadString(new Uri(serverUrl));
            //    var result = jss.Deserialize<List<EstruturaViewModel>>(response).OrderBy(r => r.Ordem).ToList();

            //    if (result != null && result.Count() > 0)
            //    {
            //        model = new List<EstruturaViewModel>();
            //        foreach (var r in result)
            //        {
            //            r.SubMenus = result.Where(x => x.IdPai.Equals(r.ID)).OrderBy(x => x.Ordem);

            //            if (r.IdPai == 0)
            //            {
            //                model.Add(r);
            //            }
            //        }
            //    }
            //}

            //ViewData["Menus"] = model ?? new List<EstruturaViewModel>();
            #endregion

            using (var client = new WebClient())
            {
                var jss = new JavaScriptSerializer();
                var url = ConfigurationManager.AppSettings["UrlAPI"];
                var serverUrl = $"{ url }/Seguranca/Principal/buscarEstruturas/{ PixCoreValues.IDCliente }/{ PixCoreValues.UsuarioLogado.IdUsuario }";
                client.Headers[HttpRequestHeader.ContentType] = "application/json";

                object envio = new
                {
                    idPermissao = 4,
                    idCliente = PixCoreValues.IDCliente,
                };

                var data = jss.Serialize(envio);
                var response = client.UploadString(serverUrl, "POST", data);
                var result = jss.Deserialize<List<EstruturaViewModel>>(response).OrderBy(r => r.Ordem).ToList();

                if (result != null && result.Count() > 0)
                {
                    model = new List<EstruturaViewModel>();
                    foreach (var r in result)
                    {
                        r.SubMenus = result.Where(x => x.IdPai.Equals(r.ID)).OrderBy(x => x.Ordem);

                        if (r.IdPai == 0)
                        {
                            model.Add(r);
                        }
                    }
                }
            }

            return PartialView("PartialMenu", model);
        }
    }
}