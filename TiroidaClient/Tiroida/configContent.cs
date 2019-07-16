using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class configContent
    {
        public configContent()
        {
            this.action = "getconfig";
            this.cookie = ConnectionClass.ClientTCP.Cookie;
        }

        public string action { get; set; }
        public string cookie { get; set; }

    }
}
