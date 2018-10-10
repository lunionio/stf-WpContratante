using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class Estrutura
    {
        public Estrutura(int iD, string urlManual, string imagem, string nome)
        {
            ID = iD;
            UrlManual = urlManual;
            Imagem = imagem;
            Nome = nome;
        }

        public int ID { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string UrlManual
        {
            get => _urlManual;
            set
            {
                _urlManual = value;
                if (!string.IsNullOrEmpty(value))
                {                    
                    var url = value.Split('/');
                    Controller = url[1];
                    Action = url[2];
                }
            }
        }
        public string Imagem { get; set; }
        public string Nome { get; set; }
        public IEnumerable<Estrutura> SubEstruturas { get; set; }

        private string _urlManual;
    }
}