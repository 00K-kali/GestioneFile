using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioneFile
{
    public class Settings
    {
        public TimeSpan delay { get; set; }
        public string destpath {  get; set; }
        public string path { get; set; }
        public bool copy { get; set; }
        public bool firstrun { get; set; }

    }
}
