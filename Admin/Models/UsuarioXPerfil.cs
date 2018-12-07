using System;
using System.Collections.Generic;
using System.Text;

namespace Admin.Models
{
    public class UsuarioXPerfil
    {
        public UsuarioXPerfil(int idUsuario, int idPerfil, int usuarioCriacao, int usuarioEdicao, DateTime dataCriacao, DateTime dataEdicao)
        {
            IdUsuario = idUsuario;
            IdPerfil = idPerfil;
            UsuarioCriacao = usuarioCriacao;
            UsuarioEdicao = usuarioEdicao;
            DataCriacao = dataCriacao;
            DataEdicao = dataEdicao;
        }

        public UsuarioXPerfil()
        {

        }

        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }
        public int UsuarioCriacao { get; set; }
        public int UsuarioEdicao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataEdicao { get; set; }
    }
}
