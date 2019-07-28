using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiroida
{
    public partial class MedicAppointment : UserControl
    {
        private string username;
        private int time;
        public MedicAppointment(string username, string hour, int time)
        {
            InitializeComponent();
            this.username = username;
            this.time = time;
            this.metroLabel1.Text = username;
            this.metroLabel2.Text = hour;
        }


        


        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            RespondAppointment rp = new RespondAppointment(this.username,ConnectionClass.ClientTCP.Cookie, this.time);
            rp.ShowDialog();

            if (rp.changed)
            {
                FlowLayoutPanel flowpanel = (FlowLayoutPanel)this.Parent;
                MedicPanel medpan = (MedicPanel)flowpanel.Parent;
                medpan.GetAndSetAppointment();
            }

        }
    }
}
