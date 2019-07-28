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

        public const int PHOTO = 1;
        public const int THYROID_HYPO = 2;
        public const int THYROID_HYPE = 3;

        private int DISEASE;


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
            this.DISEASE = THYROID_HYPO;
            ReloadLanguage();
            //this.circularProgressBar1.Value = 100;
        }


        public ResponseUserControl(int DISEASE)
        {
            InitializeComponent();
            this.cancervalue = 100;
            this.circularProgressBar1.Minimum = 0;
            this.circularProgressBar1.Maximum = 100;
            this.DISEASE = DISEASE;
            ReloadLanguage();
        }



        public void ReloadLanguage()
        {

            languagesettings lss = ConnectionClass.languagesupporter.getLanguagesettings();
            switch (this.DISEASE)
            {
                case THYROID_HYPO:
                    this.metroLabel1.Text = lss.hypothyroidism_chanse;
                    

                    break;

                case PHOTO:

                    
                    this.metroLabel1.Text = lss.pneo_chanse;
                    

                    break;

                case THYROID_HYPE:
                    this.metroLabel1.Text = lss.hyperthyroidism_chanse;
                    

                    break;

            }

            this.metroButton2.Text = lss.back_panel;


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
            PersonalDataForm form = new PersonalDataForm(false);
            SetInterface(form);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
