using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class UsuarioViewModel : BaseModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Login é obrigatório")]
        public string Login { get; set; }
        public string SobreNome { get; set; }
        public string Email { get; set; }
        public int PerfilUsuario { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Senha é obrigatório")]
        public string Senha { get; set; }
        public string VAdmin { get; set; }
        public bool Ativo { get; set; }
        public int IdEmpresa { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Perfil é obrigatório")]
        public string Perfil { get; set; }
    }
}