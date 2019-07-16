using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class AddMedicContent
    {
        public string action { get; set; }
        public string cookie { get; set; }
        public string username { get; set; }

        public AddMedicContent(string username)
        {
            this.action = "addmedic";
            this.cookie = ConnectionClass.ClientTCP.Cookie;
            this.username = username;
        }

        public AddMedicContent(string username, string action)
        {
            this.action = action;
            this.cookie = ConnectionClass.ClientTCP.Cookie;
            this.username = username;
        }


    }
}
