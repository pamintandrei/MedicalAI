using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class RegisterContent
    {
        public RegisterContent(string username, string password, string email, bool medic)
        {
            this.username = username;
            this.password = password;
            this.email = email;
            this.action = "register";
            this.medic = medic;
        }
        public string action { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public bool medic { get; set; }
    }
}
