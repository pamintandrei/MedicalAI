using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class AppointmentsResponse
    {
        public string action { get; set; }
        public List<Appointment> appointments { get; set; }
    }
}
