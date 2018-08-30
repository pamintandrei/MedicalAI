using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class OnReceiveMessageClientEventArgs : EventArgs
    {
        public MedicalProblems Medical { get; internal set; }
    }
}
