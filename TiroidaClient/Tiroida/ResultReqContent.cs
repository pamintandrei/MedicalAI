using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class ResultReqContent
    {
        public ResultReqContent()
        {
            this.action = "getresult";
            this.cookie = ConnectionClass.ClientTCP.Cookie; 
        }

        public string action;
        public string cookie;
        

    }
}
