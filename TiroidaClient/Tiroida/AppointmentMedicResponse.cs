using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class AppointmentMedicResponse
    {
        public string action;
        public string cookie;
        public int time;
        public int confirmed;
        public string username;
        public string message;

        public AppointmentMedicResponse(string cookie, int time, string username, string message, int confirmed)
        {
            this.action = "appointment_confirm";
            this.cookie = cookie;
            this.time = time;
            this.username = username;
            this.message = message;
            this.confirmed = confirmed;
        }

    }
}
