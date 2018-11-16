using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Admin.Models.Financeiro
{
    public class Extrato : Base
    {
        public Extrato(decimal valor, int naturezaId, int tipoId, string origem, string destino, int? codigoExterno, Status statusId)
        {
            Valor = valor;
            NaturezaId = naturezaId;
            TipoId = tipoId;
            Origem = origem;
            Destino = destino;
            CodigoExterno = codigoExterno;
            StatusId = statusId;
        }

        public decimal Valor { get; set; }
        public int NaturezaId { get; set; }
        public int TipoId { get; set; }
        public string Origem { get; set; }
        public string Destino { get; set; }
        public int? CodigoExterno { get; set; }
        public Status StatusId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int TipoDestino { get; set; }
        public int TipoOrigem { get; set; }
    }
}
