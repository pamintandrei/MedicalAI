using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class ChangePasswordContent
    {
        public ChangePasswordContent()
        {
            this.action = "changepassword";
        }

        public ChangePasswordContent(string cookie, string currentpassword, string newpassword)
        {
            this.action = "changepassword";
            this.cookie = cookie;
            this.currentpassword = currentpassword;
            this.newpassword = newpassword;
        }

        public string action { get; set; }
        public string cookie { get; set; }
        public string currentpassword { get; set; }
        public string newpassword { get; set; }


    }
}
