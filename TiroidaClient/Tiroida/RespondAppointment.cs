using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiroida
{
    public partial class RespondAppointment : MetroFramework.Forms.MetroForm
    {
        private string user_name;
        private string cookie;
        private int hour;
        public bool changed;

        delegate void cancelformcallback();

        public RespondAppointment(string user_name, string cookie, int hour)
        {
            InitializeComponent();
            this.MinimumSize = new Size(805, 395);
            this.MaximumSize = new Size(805, 395);
            this.user_name = user_name;
            this.cookie = cookie;
            this.hour = hour;
            this.richTextBox1.Enabled = false;
            this.changed = false;
        }


        private void cancelform()
        {
            if (this.InvokeRequired)
            {
                cancelformcallback callback = new cancelformcallback(cancelform);
                this.Invoke(callback, new object[] { });
            }
            else
            {
                this.Close();
            }

        }


        private void RespondAppointment_Load(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.metroComboBox1.Text))
            {
                MessageBox.Show("Va rugam selectati un raspuns!");
                return;
            }

            int confirmed = 0;
            if (this.metroComboBox1.SelectedIndex == 0)
            {
                confirmed = 1;
            }
            else
            {
                if (this.metroComboBox1.SelectedIndex == 1)
                {
                    confirmed = -1;
                }
            }

            AppointmentMedicResponse response;
            if (this.metroComboBox1.SelectedIndex == 0)
            {
                response = new AppointmentMedicResponse(ConnectionClass.ClientTCP.Cookie, this.hour, this.user_name, "", confirmed);
            }
            else
            {
                response = new AppointmentMedicResponse(ConnectionClass.ClientTCP.Cookie, this.hour, this.user_name, this.richTextBox1.Text, confirmed);
            }
            string data_to_send = JsonConvert.SerializeObject(response);
            ConnectionClass.ClientTCP.SendContent(data_to_send);
            ConnectionClass.ClientTCP.OnReceiveAddMedic += ClientTCP_OnReceiveAddMedic;
        }

        private void ClientTCP_OnReceiveAddMedic(object sender, OnReceiveSetConfigArgs e)
        {
            ConnectionClass.ClientTCP.OnReceiveAddMedic -= ClientTCP_OnReceiveAddMedic;
            this.changed = true;
            if (e.errcode == -1)
            {
                MessageBox.Show("Programare deja facuta!", "MedicalAI");
            }
            else
            {
                MessageBox.Show("Programarea a fost stabilita!", "MedicalAI");
                cancelform();
            }
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.metroComboBox1.SelectedIndex == 0)
            {
                this.richTextBox1.Enabled = false;
            }
            else
            {
                if(this.metroComboBox1.SelectedIndex == 1)
                {

                    this.richTextBox1.Enabled = true;
                    
                }
            }
        }
    }
}
