using System;
using System.Collections.Generic;
using System.Text;

namespace Admin.Models
{
    public class Telefone : Base
    {
        public string Numero { get; set; }
        public int ProfissionalId { get; set; }
    }
}
