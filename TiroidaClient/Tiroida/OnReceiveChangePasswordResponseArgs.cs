using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class OnReceiveChangePasswordResponseArgs
    {
        public OnReceiveChangePasswordResponseArgs(int errcode, string errmessage)
        {
            this.errcode = errcode;
            this.errmessage = errmessage;
        }

        public int getCode()
        {
            return this.errcode;
        }

        public string getMessage()
        {
            return this.errmessage;
        }

        private int errcode;
        private string errmessage;

    }
}
