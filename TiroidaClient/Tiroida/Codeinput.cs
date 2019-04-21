using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Tiroida
{
    public partial class Codeinput : MetroFramework.Forms.MetroForm
    {
        private string username;


        public Codeinput(string username)
        {
            this.username = username;
            InitializeComponent();
        }

        private void Codeinput_Load(object sender, EventArgs e)
        {

        }

        private void SendCondeInputContent(CodeInputContent cic)
        {
            
            if (ConnectionClass.ClientTCP == null)
            {
                //SetGif(false, true);
                MessageBox.Show("Sunteti momentan offline", "Tiroida");
                return;
            }

            if (ConnectionClass.ClientTCP != null)
            {
                string datatosend = JsonConvert.SerializeObject(cic);
                ConnectionClass.ClientTCP.SendContent(datatosend);
                ConnectionClass.ClientTCP.OnCodeVerifyResponse += ClientTCP_OnCodeVerifyResponse;
            }

        }

        private void ClientTCP_OnCodeVerifyResponse(object sender, OnCodeVerifyResponseArgs e)
        {
            if (e.errorcode == 0)
            {
                MessageBox.Show(e.errormessage, "Tiroida");
                this.Hide();
            }
            else
            {
                MessageBox.Show(e.errormessage, "Tiroida");
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            CodeInputContent cic = new CodeInputContent(this.metroTextBox1.Text, this.username);
            Thread th1 = new Thread(() => SendCondeInputContent(cic));
            th1.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
