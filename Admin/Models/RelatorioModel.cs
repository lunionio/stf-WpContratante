using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class RelatorioModel
    {
        public int ClienteId { get; set; }
        public string ProcName { get; set; }
        public string Parametros { get; set; }
        public int Tipo { get; set; }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public bool Ativo { get; set; }
    }
}