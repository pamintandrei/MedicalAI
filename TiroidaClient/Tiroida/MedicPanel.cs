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
    public partial class MedicPanel : UserControl
    {
        delegate void AddToFlowpanelCallBack(string username, string time, int date);

        public MedicPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }


        private void AddToFlowpanel(string username, string time, int date)
        {
            if (this.InvokeRequired)
            {
                AddToFlowpanelCallBack callback = new AddToFlowpanelCallBack(AddToFlowpanel);
                this.Invoke(callback, new object[] { username,time,date });
            }
            else
            {
                MedicAppointment ap = new MedicAppointment(username,time,date);
                this.flowLayoutPanel1.Controls.Add(ap);
            }
        }



        public void GetAndSetAppointment()
        {
            this.flowLayoutPanel1.Controls.Clear();
            GetAppointmentContent content = new GetAppointmentContent();
            string data_to_send = JsonConvert.SerializeObject(content);
            ConnectionClass.ClientTCP.SendContent(data_to_send);
            ConnectionClass.ClientTCP.OnReceiveGetAppointments += ClientTCP_OnReceiveGetAppointments;
        }


        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }


        private void ClientTCP_OnReceiveGetAppointments(object sender, AppointmentsResponse e)
        {
            ConnectionClass.ClientTCP.OnReceiveGetAppointments -= ClientTCP_OnReceiveGetAppointments;
            foreach (Appointment ap in e.appointments)
            {
                DateTime dt = UnixTimeStampToDateTime(ap.time);
                AddToFlowpanel(ap.username, dt.ToString("MM/dd/yyyy hh:mm tt"), ap.time);
            }
        }

        private void MedicPanel_Load(object sender, EventArgs e)
        {
            GetAndSetAppointment();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            adminpanel panel = new adminpanel();
            Panel p1 = (Panel)this.Parent;
            p1.Controls.Clear();
            p1.Controls.Add(panel);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            
            GetAndSetAppointment();
        }
    }
}
