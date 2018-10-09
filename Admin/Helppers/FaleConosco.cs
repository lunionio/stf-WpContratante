using Admin.Helppser;
using Admin.Models;
using Admin.Models.Faleconosco;
using System;

namespace Admin.Helppers
{
    public static class FaleConosco
    {
        public static Ticket Convert(FaleconoscoVIewModel faleConosco)
        {
           

            return new Ticket
            {
                IdUsuario = PixCoreValues.UsuarioLogado.IdUsuario,
                IdCliente = PixCoreValues.UsuarioLogado.idCliente,
                Nome = faleConosco.AssuntoNome + DateTime.Now.ToString() + " - " + PixCoreValues.UsuarioLogado.Login,
                Status = 1,
                //TipoID = int.Parse(faleConosco.Assunto),
                TipoID = 2,
                Email = faleConosco.Email,
                Descricao = faleConosco.Mensagem,
                Origem = "Admin Contratante",
                TicketStatusID = 1 //Status Aberto
            };

        }
    }
}