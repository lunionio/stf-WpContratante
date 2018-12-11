using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public class ProfissionalViewModel
    {
        public ProfissionalViewModel(int id, string nome, string especialidade, string telefone, int telefoneId, string dataNascimento, string email, int usuarioId, Endereco endereco)
        {
            Id = id;
            Nome = nome;
            Especialidade = especialidade;
            Telefone = telefone;
            TelefoneId = TelefoneId;
            DataNascimento = dataNascimento;
            Email = email;
            Endereco = endereco;
            UsuarioId = usuarioId;
        }

        public ProfissionalViewModel(int iD, string nome)
        {

        }

        public ProfissionalViewModel()
        {

        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Especialidade { get; set; }
        public string Telefone { get; set; }
        public int TelefoneId { get; set; }
        public DateTime DataCriacao { get; set; }
        public string DataNascimento { get; set; }
        public string Email { get; set; }
        public Endereco Endereco { get; set; }
        //public IList<DocumentoViewModel> Documentos { get; set; }
        public string Avatar { get; set; }
        public int UsuarioId { get; set; }
        public int? StatusId { get; set; }
        public string AreaAtuacao { get; set; }
        public IEnumerable<string> Formacoes { get; set; }
        public string Referencia { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }

        public int OportunidadeId { get; set; }
        public int? UserXOportunidadeId { get; set; }
        public decimal Valor { get; set; }
        public int JobQuantidade { get; set; }
        public decimal Avaliacao { get; set; }
    }
}