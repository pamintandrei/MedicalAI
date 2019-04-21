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
using System.Threading;

namespace Tiroida
{
    public partial class Login : UserControl
    {
        delegate void OpenFormCallBack(string username);



        public Login()
        {
            InitializeComponent();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            PersonalDataForm persdata = new PersonalDataForm();
            FlowLayoutPanel flowpanel = (FlowLayoutPanel)this.Parent;
            flowpanel.Controls.Clear();
            flowpanel.Controls.Add(persdata);

        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            RegisterUserControl registercontrol = new RegisterUserControl();
            FlowLayoutPanel flowpanel = (FlowLayoutPanel)this.Parent;
            flowpanel.Controls.Clear();
            flowpanel.Controls.Add(registercontrol);
        }


        private void SendLoginMessage(LoginContent lg)
        {
            if (ConnectionClass.ClientTCP == null)
            {
                //SetGif(false, true);
                MessageBox.Show("Sunteti momentan offline", "Tiroida");
                return;
            }


            string datatosend = JsonConvert.SerializeObject(lg);
            if (ConnectionClass.ClientTCP != null)
            {
                ConnectionClass.ClientTCP.SendContent(datatosend);
                ConnectionClass.ClientTCP.OnLoginResponse += ClientTCP_OnLoginResponse;
            }
        }


        private void OpenConfirmationCode(string username)
        {
            if (this.InvokeRequired)
            {
                OpenFormCallBack ofcb = new OpenFormCallBack(OpenConfirmationCode);
                this.Invoke(ofcb, new object[] { username });
            }
            else
            {
                Codeinput ci = new Codeinput(username);
                ci.Show();
            }
        }


        private void ClientTCP_OnLoginResponse(object sender, OnReceiveLoginMessageArgs e)
        {
            if (e.errorcode == 2)
            {
                OpenConfirmationCode(e.username);
            }
            else
            {
                MessageBox.Show(e.errormessage, "Tiroida");
            }
            ConnectionClass.ClientTCP.OnLoginResponse -= ClientTCP_OnLoginResponse;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            LoginContent lg = new LoginContent(this.metroTextBox1.Text, this.metroTextBox2.Text);
            Thread th1 = new Thread(() => SendLoginMessage(lg));
            th1.Start();
        }
    }
}
