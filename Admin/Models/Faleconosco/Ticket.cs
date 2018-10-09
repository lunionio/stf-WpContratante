using System;
using System.Collections.Generic;

namespace Admin.Models.Faleconosco
{
    public class Ticket : Base
    {
        public string Assunto { get; set; }
        public int TipoID { get; set; }
        public int IdUsuario { get; set; }
        public int TicketStatusID { get; set; }
        public Status TicketStatus { get; set; }
        public string Origem { get; set; }
        public string Email { get; set; }
        public IList<Atendimento> Atendimentos { get; set; }
        public string Numero { get; set; }

   
    }
}
