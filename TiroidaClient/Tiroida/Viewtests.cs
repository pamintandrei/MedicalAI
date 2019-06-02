using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tiroida
{
    public partial class Viewtests : UserControl
    {
        delegate void SetDisplayCallBack(OnReceiveResultArgs e);
        public Viewtests()
        {
            InitializeComponent();
        }

        private void Viewtests_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            this.SendGetResultData();
        }


        private void SendGetResultData()
        {
            ResultReqContent datacontent = new ResultReqContent();
            string sendcontent = JsonConvert.SerializeObject(datacontent);
            ConnectionClass.ClientTCP.SendContent(sendcontent);
            ConnectionClass.ClientTCP.OnReceiveResults += ClientTCP_OnReceiveResults;


        }


        private void DisplayResult(OnReceiveResultArgs e)
        {
            if (this.InvokeRequired)
            {
                SetDisplayCallBack callback = new SetDisplayCallBack(DisplayResult);
                this.Invoke(callback, new object[] { e });
            }
            else
            {
                foreach (ResultPatient result in e.results)
                {
                    this.metroGrid1.Rows.Add(result.name_patient, result.Sex, result.Age, result.on_thyroxine, result.query_on_thyroxine, result.on_antithyroid_medication, result.tumor, result.FTI_measured, result.FTI, result.thyroid_surgery, result.query_hypothyroid, result.query_hyperthyroid, result.pregnant, result.sick, result.lithium, result.TBG_measured, result.TBG, result.goitre, result.TSH_measured, result.TSH, result.T3_measured, result.T3, result.TT4_measured, result.TT4, result.positive, result.negative);
                }
            }

        }


        private void ClientTCP_OnReceiveResults(object sender, OnReceiveResultArgs e)
        {
            DisplayResult(e);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            PersonalDataForm personaldata = new PersonalDataForm(ConnectionClass.ClientTCP.isloged);

            Panel p1 = (Panel)this.Parent;
            p1.Controls.Clear();
            p1.Controls.Add(personaldata);
        }
    }
}
