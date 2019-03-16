using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class RelatorioFinanceiroViewModel
    {
        public string ID { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Valor { get; set; }

        [JsonIgnore]
        public string Data
        {
            get
            {
                return DataCriacao.ToString("dd/MM/yyyy");
            }
        }
    }
}