using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class OnReceiveLoginMessageArgs
    {
        public OnReceiveLoginMessageArgs()
        {

        }


        public int errorcode;
        public string errormessage;
        public string username;
        public bool is_admin;
    }
}
