using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public class Profissional : Base
    {
        public string Email { get; set; }
        public int IdUsuario { get; set; }
        public DateTime DataNascimento { get; set; }
        public Telefone Telefone { get; set; }
        public Endereco Endereco { get; set; }
        public IList<ProfissionalFormacao> Formacoes { get; set; }
        public decimal Avaliacao { get; set; }

        public Profissional()
        {

        }
    }
}
