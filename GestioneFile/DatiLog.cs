using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GestioneFile
{
    public class DatiLog
    {
        public string file { get; set; }
        public string data { get; set; }
        public string azione { get; set; }
        public string data_creazione { get; set; }
        public string data_modifica { get; set; }
    }
}
