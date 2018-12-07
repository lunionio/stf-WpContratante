using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class LoginViewModel
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public int idCliente { get; set; }
        public int idPerfil { get; set; } //Permissao????
        public int IdUsuario { get; set; }
        public string Ativo { get; set; }
        public int idEmpresa { get; set; }
        public string Nome { get; set; }
        public string Avatar { get; set; }
    }
}