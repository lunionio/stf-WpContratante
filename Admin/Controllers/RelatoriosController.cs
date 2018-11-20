using Admin.Helppers;
using Admin.Helppser;
using Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class RelatoriosController : Controller
    {
        // GET: Relatorios
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Gerar()
        {
            var usuario = PixCoreValues.UsuarioLogado;
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "/Seguranca/WpRelatorios/OptsContratantes/" + usuario.idCliente + "/" +
                PixCoreValues.UsuarioLogado.IdUsuario;

            var envio = new
            {
                contratanteId = usuario.idEmpresa,
            };

            var helper = new ServiceHelper();
            var result = helper.Post<IEnumerable<RelatorioViewModel>>(url, envio);

            var json = JsonConvert.SerializeObject(result);
            var dt = (DataTable)JsonConvert.DeserializeObject(json, typeof(DataTable));

            var sb = new StringBuilder();
            var columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                var fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            var fileBytes = Encoding.UTF8.GetBytes(sb.ToString());
            using (var ms = new MemoryStream(fileBytes))
            {
                var bytes = ms.ToArray();
                Response.Clear();
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment;    filename=Relatorio.csv");
                Response.BinaryWrite(bytes);
                Response.End();
            }

            return View("Index");
        }
    }
}