using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{

    public class ResultPatient
    {
        public int ID { get; set; }
        public string name_patient { get; set; }
        public string Sex { get; set; }
        public string Age { get; set; }
        public string on_thyroxine { get; set; }
        public string query_on_thyroxine { get; set; }
        public string on_antithyroid_medication { get; set; }
        public string thyroid_surgery { get; set; }
        public string query_hypothyroid { get; set; }
        public string query_hyperthyroid { get; set; }
        public string pregnant { get; set; }
        public string sick { get; set; }
        public string tumor { get; set; }
        public string lithium { get; set; }
        public string goitre { get; set; }
        public string TSH_measured { get; set; }
        public string TSH { get; set; }
        public string T3_measured { get; set; }
        public string T3 { get; set; }
        public string TT4_measured { get; set; }
        public string TT4 { get; set; }
        public string FTI_measured { get; set; }
        public string FTI { get; set; }
        public string TBG_measured { get; set; }
        public string TBG { get; set; }
        public int medic_id { get; set; }
        public double create_time { get; set; }
        public string negative { get; set; }
        public string positive { get; set; }
    }
}
