using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class configFile
    {
        public configFile()
        {

        }

        public configFile(string IpAdress, int port, string Language)
        {
            this.IpAdress = IpAdress;
            this.port = port;
            this.Language = Language;
        }


        public string IpAdress;
        public string Language;
        public int port;

    }
}
