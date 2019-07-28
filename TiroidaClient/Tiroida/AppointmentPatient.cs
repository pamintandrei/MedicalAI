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

namespace Tiroida
{
    public partial class AppointmentPatient : UserControl
    {
        private string username;
        private string datetime;
        private int confirmation;
        private int time;

        delegate void ReloadPatientFormCallBack();

        public AppointmentPatient(string username, string datetime, int confirmation,int time)
        {
            InitializeComponent();
            this.username = username;
            this.datetime = datetime;
            this.confirmation = confirmation;
            this.time = time;
            this.metroButton2.Enabled = false;

            if (confirmation == -1 || confirmation == 1)
            {
                this.metroButton2.Enabled = true;
                this.metroLabel3.Text = "Raspuns primit";
            }
            else
            {
                this.metroLabel3.Text = "Fara raspuns";
            }

            this.metroLabel1.Text = username;
            this.metroLabel2.Text = datetime;

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void ReloadPatientForm()
        {
            if (this.InvokeRequired)
            {
                ReloadPatientFormCallBack callback = new ReloadPatientFormCallBack(ReloadPatientForm);
                this.Invoke(callback, new object[] { });
            }
            else
            {
                FlowLayoutPanel flowpanel = (FlowLayoutPanel)this.Parent;
                TableLayoutPanel layoutpanel = (TableLayoutPanel)flowpanel.Parent;
                PatientForm patform = (PatientForm)layoutpanel.Parent;

                patform.GetAppointments();

                //patpanel.GetAppointments();
            }
        }


        private void metroButton2_Click(object sender, EventArgs e)
        {
            AppointmentResponsePatientForm form = new AppointmentResponsePatientForm(this.username, this.time);
            form.ShowDialog();

            if (form.changed)
            {
                ReloadPatientForm();
            }

        }


        private void ClearMessage()
        {
            AppointmentDelete content = new AppointmentDelete(this.username, this.time);
            string data_to_send = JsonConvert.SerializeObject(content);
            ConnectionClass.ClientTCP.SendContent(data_to_send);
            ConnectionClass.ClientTCP.OnReceiveAddMedic += ClientTCP_OnReceiveAddMedic;
        }

        private void ClientTCP_OnReceiveAddMedic(object sender, OnReceiveSetConfigArgs e)
        {
            ConnectionClass.ClientTCP.OnReceiveAddMedic -= ClientTCP_OnReceiveAddMedic;
            if (e.errcode == 0)
            {
                MessageBox.Show("Programarea a fost stersa!");
                ReloadPatientForm();
            }
        }



        private void metroButton1_Click(object sender, EventArgs e)
        {
            ClearMessage();
        }
    }
}
