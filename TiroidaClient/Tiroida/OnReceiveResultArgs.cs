using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Tiroida
{
    class OnReceiveResultArgs
    {
        public OnReceiveResultArgs(int error, string action, List<ResultPatient> results)
        {
            this.error = error;
            this.action = action;
            this.results = results;

        }


        public OnReceiveResultArgs(int error, string action)
        {
            this.error = error;
            this.action = action;
            this.results = new List<ResultPatient>();
        }



        public OnReceiveResultArgs()
        {
            this.results = new List<ResultPatient>();
        }


        public int error { get; set; }
        public string action { get; set; }
        public List<ResultPatient> results { get; set; }

    }
}
