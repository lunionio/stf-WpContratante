using System;

namespace Admin.Models
{
    public enum OportunidadeStatus
    {
        Publico = 1,
        Pendente = 2
    }
    public class VagaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Cep { get; set; }
        public string Rua { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Date { get; set; }
        public string Hora { get; set; }
        public Decimal Valor { get; set; }
        public int Profissional { get; set; }
        public string ProfissionalNome { get; set; }
        public int Numero { get; set; }
        public string Total { get; set; }
        public int Qtd { get; set; }
        public string Complemento { get; set; }
        public string Referencia { get; set; }
        public DateTime DataEvento { get; set; }
        public int status { get; set; }
        public int IdEmpresa { get; set; }
        public int IdCliente { get; set; }
    }
}