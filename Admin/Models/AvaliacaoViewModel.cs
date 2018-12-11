using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class AvaliacaoViewModel : Base
    {
        public int UsuarioAvaliadorId { get; set; }
        public int UsuarioAvaliadoId { get; set; }
        public decimal Estrelas { get; set; }
        public int OportunidadeId { get; set; }
        public string CodigoExterno { get; set; }

        public string Avatar { get; set; }
        public DateTime HoraCheckIn { get; set; }
    }
}