using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class GetAppointmentResponse
    {
        public string action;
        public string cookie;
        public int date;
        public string medic_username;

        public GetAppointmentResponse(int date, string medic_username)
        {
            this.action = "appointment_response";
            this.cookie = ConnectionClass.ClientTCP.Cookie;
            this.date = date;
            this.medic_username = medic_username;
        }


    }
}
