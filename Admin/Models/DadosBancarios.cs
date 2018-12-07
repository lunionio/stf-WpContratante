using System;
using System.Collections.Generic;
using System.Text;

namespace Admin.Models
{
    public class DadosBancarios : Base
    {
        public int CodigoExterno { get; set; }
        public int REF { get; set; }
        public string Cpf { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string TitularCpf { get; set; }
        public string TitularNome { get; set; }
        public string Tipo { get; set; }
        public int Status { get; set; }
    }
}
