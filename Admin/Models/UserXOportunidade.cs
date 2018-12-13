using Admin.Helppser;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Admin.Models
{
    public class UserXOportunidade
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public int OportunidadeId { get; set; }
        public OportunidadeViewModel Oportunidade { get; set; }
        public UserStatus Status { get; set; }
        public int StatusID { get; set; }
        public Profissional Profissional { get; set; }

        public string EmailContratante { get; set; }
        public string EmailContratado { get; set; }
        public string NomeContratado { get; set; }
    }
}
