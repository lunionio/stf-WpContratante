using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Models
{
    public class ProfissionalServico : Base
    {
        public int UsuarioId { get; set; }
        public int ServicoId { get; set; }
        public Servico Servico { get; set; }
        public Profissional Profissional { get; set; }
    }
}
