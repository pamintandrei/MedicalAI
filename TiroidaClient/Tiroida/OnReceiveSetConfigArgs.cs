using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class OnReceiveSetConfigArgs
    {
        public int errcode { get; set; }
        public string errmessage { get; set; }

        public OnReceiveSetConfigArgs()
        {

        }


        public OnReceiveSetConfigArgs(int errcode, string errmessage)
        {
            this.errcode = errcode;
            this.errmessage = errmessage;
        }

    }
}
