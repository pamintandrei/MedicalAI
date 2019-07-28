using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class AppointmentDelete
    {

        public string action;
        public string cookie;
        public int date;
        public string medic_username;

        public AppointmentDelete(string username, int date)
        {
            this.action = "delete_appointment";
            this.cookie = ConnectionClass.ClientTCP.Cookie;
            this.medic_username = username;
            this.date = date;
        }


    }
}
