using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class CheckInViewModel
    {
        public int OportunidadeId { get; set; }
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime Hora { get; set; }
        public string Status { get; set; }
        public int StatusPagamento { get; set; }
    }
}