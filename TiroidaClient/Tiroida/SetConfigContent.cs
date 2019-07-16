using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class SetConfigContent
    {
        public string action { get; set; }
        public string cookie { get; set; }
        public bool register_verification;
        public bool medic_registration;

        public SetConfigContent(bool register_verification, bool medic_registration)
        {
            this.action = "setconfig";
            this.cookie = ConnectionClass.ClientTCP.Cookie;
            this.register_verification = register_verification;
            this.medic_registration = medic_registration;
        }

        public SetConfigContent(bool register_verification, bool medic_registration, string cookie)
        {
            this.action = "setconfig";
            this.cookie = cookie;
            this.register_verification = register_verification;
            this.medic_registration = medic_registration;
        }





    }
}
