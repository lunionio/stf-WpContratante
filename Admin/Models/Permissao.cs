using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class Permissao : Base
    {
        public int IdAux { get; set; }
        public string idTipoAcao { get; set; }
        public string VAdmin { get; set; }
    }
}