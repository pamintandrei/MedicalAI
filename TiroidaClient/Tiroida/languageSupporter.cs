using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tiroida
{
    class languageSupporter
    {
        private languagesettings CurrentLanguage;

        public languageSupporter(string jsonstring)
        {
            this.CurrentLanguage = JsonConvert.DeserializeObject<languagesettings>(jsonstring);
        }


        public languagesettings getLanguagesettings()
        {
            return this.CurrentLanguage;
        }


    }
}
