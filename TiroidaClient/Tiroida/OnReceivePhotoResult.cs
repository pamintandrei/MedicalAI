using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class OnReceivePhotoResult
    {
        public string pneumoniaChanse;
        public string nonPneumoniaChanse;
        public OnReceivePhotoResult(string pneumoniaChanse, string nonPneumoniaChanse)
        {
            this.pneumoniaChanse = pneumoniaChanse;
            this.nonPneumoniaChanse = nonPneumoniaChanse;

        }


    }
}
