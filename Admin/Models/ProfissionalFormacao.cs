using System;
using System.Collections.Generic;
using System.Text;

namespace Admin.Models
{
    public class ProfissionalFormacao : Base
    {
        public string Instituicao { get; set; }
        public DateTime InicioCurso { get; set; }
        public DateTime FinalCurso { get; set; }

        public int ProfissionalId { get; set; }
    }
}
