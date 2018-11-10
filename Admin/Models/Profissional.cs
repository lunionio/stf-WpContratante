using System;

namespace Admin.Models
{
    public class Profissional : Base
    {
        public string Email { get; set; }
        public int IdUsuario { get; set; }
        public DateTime DataNascimento { get; set; }
        public Telefone Telefone { get; set; }
    }
}
