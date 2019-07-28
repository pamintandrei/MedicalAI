using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class GetPatientAppointmentContent
    {
        public string action;
        public string cookie;
        public GetPatientAppointmentContent()
        {
            this.action = "getpatientappointment";
            this.cookie = ConnectionClass.ClientTCP.Cookie;
        }


    }
}
