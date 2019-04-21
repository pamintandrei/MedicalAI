using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class OnReceiveRegisterMessageArgs : EventArgs
    {
        public int errorcode;
        public string errormessage;
    }
}
