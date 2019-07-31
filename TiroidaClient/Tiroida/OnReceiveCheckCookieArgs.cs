using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class OnReceiveCheckCookieArgs
    {
        public string action;
        public int errcode;
        public string errmessage;
        public bool is_admin;
        public bool is_patient;
        public bool is_medic;
        public string cookie;
    }
}
