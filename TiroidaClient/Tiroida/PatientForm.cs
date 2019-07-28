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
    public partial class PatientForm : UserControl
    {
        delegate void SetMedicsCallBack(List<string> users);
        delegate void AddToFlowCallBack(string username, string time, int confirmed,int date);
        delegate void RefreshAppointmentsCallBack();




        public PatientForm()
        {
            InitializeComponent();
            metroDateTime1.Format = DateTimePickerFormat.Custom;
            metroDateTime1.CustomFormat = "dd/MM/yyyy";
            this.metroDateTime1.MinDate = DateTime.Now;

            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.Format = DateTimePickerFormat.Custom;
            this.dateTimePicker1.CustomFormat = "hh:mm tt";
            this.metroComboBox1.Enabled = false;

            this.Dock = DockStyle.Fill;
        }


        private void RefreshAppointment()
        {
            if (this.InvokeRequired)
            {
                RefreshAppointmentsCallBack callback = new RefreshAppointmentsCallBack(RefreshAppointment);
                this.Invoke(callback, new object[] { });
            }
            else
            {
                GetAppointments();
            }
        }


        private void AddToFlow(string username, string time, int confirmed, int date)
        {
            if (this.InvokeRequired)
            {
                AddToFlowCallBack callback = new AddToFlowCallBack(AddToFlow);
                this.Invoke(callback, new object[] {username,time,confirmed,date });
            }
            else
            {
                AppointmentPatient apoint = new AppointmentPatient(username, time, confirmed, date);
                this.flowLayoutPanel1.Controls.Add(apoint);
            }
        }



        private void SetMedics(List<string> users)
        {
            if (this.InvokeRequired)
            {
                SetMedicsCallBack callback = new SetMedicsCallBack(SetMedics);
                this.Invoke(callback, new object[] { users });
            }
            else
            {
                this.metroComboBox1.Enabled = true;
                this.metroComboBox1.Items.Clear();
                foreach (string s in users)
                {
                    this.metroComboBox1.Items.Add(s);
                }
            }
        }


        private void getMedics()
        {
            configContent content = new configContent();
            content.action = "getmedics";
            string data_to_send = JsonConvert.SerializeObject(content);
            ConnectionClass.ClientTCP.SendContent(data_to_send);
            ConnectionClass.ClientTCP.OnReceiveMedics += ClientTCP_OnReceiveMedics;
        }

        private void ClientTCP_OnReceiveMedics(object sender, OnReceiveNonMedicsArgs e)
        {
            ConnectionClass.ClientTCP.OnReceiveMedics -= ClientTCP_OnReceiveMedics;
            SetMedics(e.medics);
        }





        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            adminpanel ad = new adminpanel();
            Panel p1 = (Panel)this.Parent;
            p1.Controls.Clear();
            p1.Controls.Add(ad);
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {

            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public void GetAppointments()
        {

            GetPatientAppointmentContent content = new GetPatientAppointmentContent();
            string data_to_send =  JsonConvert.SerializeObject(content);
            ConnectionClass.ClientTCP.SendContent(data_to_send);
            this.flowLayoutPanel1.Controls.Clear();
            ConnectionClass.ClientTCP.OnReceivePatientAppointments += ClientTCP_OnReceivePatientAppointments;

        }

        private void ClientTCP_OnReceivePatientAppointments(object sender, OnReceivePatientAppointmentsArgs e)
        {
            ConnectionClass.ClientTCP.OnReceivePatientAppointments -= ClientTCP_OnReceivePatientAppointments;
            foreach (PatientAppointment ap in e.appointments)
            {
                DateTime dt = UnixTimeStampToDateTime(ap.time);
                AddToFlow(ap.username,dt.ToString("MM/dd/yyyy hh:mm tt"), ap.confirmed, ap.time);
            }
        }

        private void PatientForm_Load(object sender, EventArgs e)
        {
            getMedics();
            GetAppointments();
        }


        private long ConvertTimeStamp(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)(datetime - sTime).TotalSeconds;
        }


        private long getTimeStamp()
        {

            DateTime calendartime = this.metroDateTime1.Value;
            DateTime hourtime = this.dateTimePicker1.Value;

            TimeSpan ts = new TimeSpan(hourtime.Hour,hourtime.Minute,0);
            calendartime = calendartime.Date + ts;

            return ConvertTimeStamp(calendartime);
        }



        private void metroButton1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(this.metroComboBox1.Text))
            {
                MessageBox.Show("Va rugam selectati un medic", "MedicalAI");
                return;
            }

            long time = getTimeStamp();
            AppointmentContent content = new AppointmentContent(time, ConnectionClass.ClientTCP.Cookie, this.metroComboBox1.Text);
            string json_data = JsonConvert.SerializeObject(content);
            ConnectionClass.ClientTCP.SendContent(json_data);
            ConnectionClass.ClientTCP.OnReceiveAddMedic += ClientTCP_OnReceiveAddMedic;
            Application.UseWaitCursor = true;

        }

        private void ClientTCP_OnReceiveAddMedic(object sender, OnReceiveSetConfigArgs e)
        {
            ConnectionClass.ClientTCP.OnReceiveAddMedic -= ClientTCP_OnReceiveAddMedic;
            Application.UseWaitCursor = false;
            
            if (e.errcode == 0)
            {
                MessageBox.Show("Programarea a fost facuta.", "MedicalAI");
            }
            else
            {
                if (e.errcode == -2)
                {
                    MessageBox.Show("Ati facut deja o programare!", "MedicalAI");
                }
                else
                    if (e.errcode == -3)
                    {
                    MessageBox.Show("Medicul are deja o programare la aceasta ora!", "MedicalAI");
                    }
            }

            RefreshAppointment();

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
