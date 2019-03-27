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
        private DateTime _dataEvento;
        private string _numeroString;

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cep { get; set; }
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
        public string NumeroString
        {
            get
            {
                if (string.IsNullOrEmpty(_numeroString) && Numero > 0)
                {
                    return Numero.ToString();
                }

                return _numeroString;
            }
            set => _numeroString = value; }
        public string Total { get; set; }
        public int Qtd { get; set; }
        public string Complemento { get; set; }
        public string Referencia { get; set; }
        public DateTime DataEvento { get => _dataEvento; set => _dataEvento = value; }
        public int status { get; set; }
        public int IdEmpresa { get; set; }
        public int IdCliente { get; set; }
        public int AreaAtuacao { get; set; }
        public int EnderecoId { get; set; }
        public string EnderecoDataCriacao { get; set; }
        public string DataCriacao { get; set; }

        public VagaViewModel()
        {

        }

        public VagaViewModel(int id, string nome, string cep, string rua, string bairro,
           string cidade, string uf, string hora, decimal valor, int profissional, string profissionalNome,
           int numero, string total, int qtd, string complemento, string referencia, string dataEvento, int status, int idEmpresa, int idCliente, int areaAtuacao)
        {
            Id = id;
            Nome = nome;
            Cep = cep;
            Rua = rua;
            Bairro = bairro;
            Cidade = cidade;
            Uf = uf;
            Hora = hora;
            Valor = valor;
            Profissional = profissional;
            ProfissionalNome = profissionalNome;
            Numero = numero;
            Total = total;
            Qtd = qtd;
            Complemento = complemento;
            Referencia = referencia;
            DataEvento = Convert.ToDateTime(dataEvento);
            this.status = status;
            IdEmpresa = idEmpresa;
            IdCliente = idCliente;
            AreaAtuacao = areaAtuacao;
        }
    }
}