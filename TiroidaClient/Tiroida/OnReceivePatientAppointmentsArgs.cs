using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class OnReceivePatientAppointmentsArgs
    {
        public string action { get; set; }
        public List<PatientAppointment> appointments { get; set; }
    }
}
