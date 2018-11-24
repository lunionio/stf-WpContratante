using System;
using System.Collections.Generic;
using System.Text;

namespace Admin.Models
{
    public class CheckIn : Base
    {
        public int CodigoExterno { get; set; }
        public int IdUsuario { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int QrCodeId { get; set; }
    }
}
