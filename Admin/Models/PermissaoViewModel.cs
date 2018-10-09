using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class PermissaoViewModel : BaseModel
    {
        public bool Ativo { get; set; }
        public string vAdmin { get; set; }
        public string IdTipoAcao { get; set; }
        public int IdAux { get; set; }
    }
}