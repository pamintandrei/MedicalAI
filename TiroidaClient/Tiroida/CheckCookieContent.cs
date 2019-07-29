using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class CheckCookieContent
    {
        public string action;
        public string cookie;

        public CheckCookieContent(string cookie)
        {
            this.action = "check_cookie";
            this.cookie = cookie;
        }


    }
}
