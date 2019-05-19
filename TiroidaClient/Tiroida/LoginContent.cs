using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class LoginContent
    {
        public LoginContent(string username, string password, string secret_code)
        {
            this.password = password;
            this.username = username;
            this.action = "login";
            this.secret_code = secret_code;
        }

        public LoginContent(string username, string password)
        {
            this.password = password;
            this.username = username;
            this.action = "login";
            this.secret_code = "";
        }

        public string action;
        public string username { get; set; }
        public string password { get; set; }
        public string secret_code { get; set; }


    }
}
