using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class EstruturaViewModel : BaseModel
    {
        public bool Ativo { get; set; }
        public int Tipo { get; set; }
        public int IdPermissao { get; set; }
        public int IdPai { get; set; }
        public string Principal { get; set; }
        public string Imagem { get; set; }
        public string ImagemMenu { get; set; }
        public string LinkManual { get; set; }
        public string UrlManual { get; set; }
        public int Ordem { get; set; }

        public virtual IEnumerable<EstruturaViewModel> SubMenus { get; set; }
    }
}