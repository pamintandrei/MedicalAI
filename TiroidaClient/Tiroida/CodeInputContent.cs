using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class CodeInputContent
    {
        public CodeInputContent(string secret_code, string username)
        {
            this.secret_code = secret_code;
            this.username = username;
            this.action = "code_verify";
        }


        public string action;
        public string secret_code;
        public string username;

    }
}
