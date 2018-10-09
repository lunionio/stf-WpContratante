using System;

namespace Admin.Models.Faleconosco
{
    public class Atendimento : Base
    {
        public int IdUserAdmin { get; set; }
        public string Resposta { get; set; }
        public Ticket Ticket { get; set; }
        public int TicketID { get; set; }
    }
}
