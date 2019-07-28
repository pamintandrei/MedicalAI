using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class AppointmentContent
    {
        public AppointmentContent(long timestamp, string cookie, string medic_username)
        {
            this.action = "appointment";
            this.timestamp = timestamp;
            this.cookie = cookie;
            this.medic_username = medic_username;
        }


        public string action { get; set; }
        public long timestamp { get; set; }
        public string cookie { get; set; }
        public string medic_username { get; set; }


    }
}
