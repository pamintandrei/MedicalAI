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
    public partial class AppointmentResponsePatientForm : MetroFramework.Forms.MetroForm
    {
        private int date;
        private string medic_username;
        public bool changed;
        delegate void SetTextCallBack(string text);
        delegate void CloseFormCallBack();



        public AppointmentResponsePatientForm(string medic_username, int date)
        {
            InitializeComponent();
            this.MinimumSize = new Size(805, 395);
            this.MaximumSize = new Size(805, 395);
            this.medic_username = medic_username;
            this.date = date;
            this.changed = false;
        }

        private void getAppointmentResponse()
        {
            GetAppointmentResponse content = new GetAppointmentResponse(this.date,this.medic_username);
            string converted = JsonConvert.SerializeObject(content);
            ConnectionClass.ClientTCP.SendContent(converted);
            ConnectionClass.ClientTCP.OnReceiveResponseAppointment += ClientTCP_OnReceiveResponseAppointment;
        }



        private void CloseForm()
        {
            if (this.InvokeRequired)
            {
                CloseFormCallBack callback = new CloseFormCallBack(CloseForm);
                this.Invoke(callback, new object[] { });
            }
            else
            {
                this.changed = true;
                this.Close();
            }
        }


        private void SetText(string text)
        {
            if (this.InvokeRequired)
            {
                SetTextCallBack callback = new SetTextCallBack(SetText);
                this.Invoke(callback, new object[] { text });
            }
            else
            {
                this.richTextBox1.Text = text;
            }
        }


        private void ClientTCP_OnReceiveResponseAppointment(object sender, string e)
        {
            ConnectionClass.ClientTCP.OnReceiveResponseAppointment -= ClientTCP_OnReceiveResponseAppointment;
            SetText(e);
        }

        private void AppointmentResponsePatientForm_Load(object sender, EventArgs e)
        {
            getAppointmentResponse();
        }


        private void ClearMessage()
        {
            AppointmentDelete content = new AppointmentDelete(this.medic_username, this.date);
            string data_to_send = JsonConvert.SerializeObject(content);
            ConnectionClass.ClientTCP.SendContent(data_to_send);
            ConnectionClass.ClientTCP.OnReceiveAddMedic += ClientTCP_OnReceiveAddMedic;
        }

        private void ClientTCP_OnReceiveAddMedic(object sender, OnReceiveSetConfigArgs e)
        {
            ConnectionClass.ClientTCP.OnReceiveAddMedic -= ClientTCP_OnReceiveAddMedic;
            if (e.errcode == 0)
            {
                MessageBox.Show("Mesajul a fost sters!");
                CloseForm();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            ClearMessage();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
