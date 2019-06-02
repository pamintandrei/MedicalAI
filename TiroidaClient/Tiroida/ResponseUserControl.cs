using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Tiroida
{
    public partial class ResponseUserControl : UserControl
    {
        delegate void SetInterfaceCallBack(UserControl usercontrol);
        private int cancervalue;

        private void SetInterface(UserControl usercontrol)
        {
            Panel panel = (Panel)this.Parent;
            if (panel.InvokeRequired)
            {
                SetInterfaceCallBack callback = new SetInterfaceCallBack(SetInterface);
                this.Invoke(callback, new object[] { usercontrol });
            }
            else
            {
                panel.Controls.Clear();
                panel.Controls.Add(usercontrol);
            }
        }

        public ResponseUserControl()
        {
            InitializeComponent();
            this.cancervalue = 100;
            this.circularProgressBar1.Minimum = 0;
            this.circularProgressBar1.Maximum = 100;
            //this.circularProgressBar1.Value = 100;
        }

        private void ResponseUserControl_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;

        }

        internal void SetAnimateCancer(int cancervalue)
        {
            this.cancervalue = cancervalue;
            this.circularProgressBar1.Value = cancervalue;
        }


        internal void SetCancer(string s)
        {
           // this.metroLabel3.Text = s;
        }


        internal void SetNonCancer(string s)
        {
            this.metroLabel4.Text = s; 
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            PersonalDataForm form = new PersonalDataForm(ConnectionClass.ClientTCP.isloged);
            SetInterface(form);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
