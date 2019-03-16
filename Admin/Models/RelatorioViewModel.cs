using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class RelatorioViewModel
    {
        public string Codigo        { get; set; }
        public string Titulo        { get; set; }
        public DateTime CriadoEm      { get; set; }
        public DateTime DataEvento        { get; set; }
        public string Endereco      { get; set; }
        public string Categoria         { get; set; }
        public string Profissional      { get; set; }
        public string Valor             { get; set; }
        public string Quantidade             { get; set; }
        public string Total             { get; set; }
        public string Candidatos            { get; set; }
        public string Aprovados             { get; set; }
        public string Reprovados        { get; set; }


        [JsonIgnore]
        public string Criado
        {
            get
            {
                return CriadoEm.ToString("dd/MM/yyyy");
            }
        }

        [JsonIgnore]
        public string Data
        {
            get
            {
                return DataEvento.ToString("dd/MM/yyyy");
            }
        }

        public RelatorioViewModel()
        {

        }
    }
}