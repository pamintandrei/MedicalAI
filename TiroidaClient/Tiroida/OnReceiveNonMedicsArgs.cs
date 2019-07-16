using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class OnReceiveNonMedicsArgs
    {
        public string action { get; set; }
        public int errcode { get; set; }
        public List<string> medics { get; set; }

        public OnReceiveNonMedicsArgs()
        {
            
        }

        
    }
}
