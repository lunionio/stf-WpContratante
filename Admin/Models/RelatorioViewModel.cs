using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class RelatorioViewModel
    {
        public string Codigo { get; set; }
        public string Cliente { get; set; }
        public string Cnpj { get; set; }
        public string Titulo { get; set; }
        public string CriadoEm { get; set; }
        public string DataEvento { get; set; }
        public string Endereco { get; set; }
        public string Categoria { get; set; }
        public string Profissional { get; set; }
        public string Valor { get; set; }
        public string Quantidade { get; set; }
        public string Total { get; set; }
        public string Candidatos { get; set; }
        public string Aprovados { get; set; }
        public string Reprovados { get; set; }

        public RelatorioViewModel()
        {

        }
    }
}