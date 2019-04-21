using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class LoginContent
    {
        public LoginContent(string username, string password)
        {
            this.password = password;
            this.username = username;
            this.action = "login";
        }

        public string action;
        public string username { get; set; }
        public string password { get; set; }


    }
}
