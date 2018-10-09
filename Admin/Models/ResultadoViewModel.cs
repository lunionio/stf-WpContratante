using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class ResultadoViewModel
    {
        public ResultadoViewModel(string mensagem, bool sucesso)
        {
            Mensagem = mensagem;
            Sucesso = sucesso;
        }

        public string Mensagem { get; set; }
        public bool Sucesso { get; set; }
    }
}