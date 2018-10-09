using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class ClienteViewModel : BaseModel
    {
        public string CNPJ { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string Ativo { get; set; }
    }
}