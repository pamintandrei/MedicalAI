using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class GetAppointmentContent
    {
        public GetAppointmentContent()
        {
            this.action = "getappointment";
            this.cookie = ConnectionClass.ClientTCP.Cookie;
        }

        public string action;
        public string cookie;
    }
}
