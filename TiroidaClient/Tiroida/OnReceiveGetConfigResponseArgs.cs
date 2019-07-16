using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class OnReceiveGetConfigResponseArgs
    {

        public OnReceiveGetConfigResponseArgs(bool register_verification, bool medic_registration)
        {
            this.register_verification = register_verification;
            this.medic_registration = medic_registration;
        }

        public bool register_verification;
        public bool medic_registration;

    }
}
