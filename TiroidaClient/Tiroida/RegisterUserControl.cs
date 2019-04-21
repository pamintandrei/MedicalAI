using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Mail;
using Newtonsoft.Json;


/* 
 * De implementat:
 * Schimbare panel dupa inregistrare cu succes
 * 
 * 
 * 
 */



namespace Tiroida
{
    public partial class RegisterUserControl : UserControl
    {
        public RegisterUserControl()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Login logform = new Login();
            FlowLayoutPanel f1 = (FlowLayoutPanel)this.Parent;
            f1.Controls.Clear();
            f1.Controls.Add(logform);

        }

        private void SendRegisterForm(RegisterContent content)
        {

            if (ConnectionClass.ClientTCP == null)
            {
                //SetGif(false, true);
                MessageBox.Show("Sunteti momentan offline", "Tiroida");
                return;
            }


            string jsonstring = JsonConvert.SerializeObject(content);
            if (ConnectionClass.ClientTCP != null)
            {
                ConnectionClass.ClientTCP.SendContent(jsonstring);
                ConnectionClass.ClientTCP.OnRegisterResponse += ClientTCP_OnRegisterResponse;
            }
        }

        private void ClientTCP_OnRegisterResponse(object sender, OnReceiveRegisterMessageArgs e)
        {
            MessageBox.Show(e.errormessage, "Tiroida");
            ConnectionClass.ClientTCP.OnRegisterResponse -= ClientTCP_OnRegisterResponse;
        }

        private bool ValidEmail(string email)
        {
            try
            {
                MailAddress ms = new MailAddress(email);
                return ms.Address == email;
            }
            catch
            {
                return false;
            }

        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (this.metroTextBox1.Text.Length < 3)
            {
                MessageBox.Show("Numele de utilizator este prea scurt", "Tiroida");
                return;
            }

            


            if (!ValidEmail(this.metroTextBox4.Text))
            {
                MessageBox.Show("Email invalid", "Tiroida");
                return;
            }


            if (this.metroTextBox2.Text == this.metroTextBox3.Text)
            {
                if (this.metroButton2.Text.Length < 3)
                {
                    MessageBox.Show("Parola este prea scurta", "Tiroida");
                    return;
                }



                RegisterContent content = new RegisterContent(this.metroTextBox1.Text, this.metroTextBox2.Text, this.metroTextBox4.Text);
                Thread t1 = new Thread(() => SendRegisterForm(content));
                t1.Start();
            }
            else
            {
                MessageBox.Show("Parola nu coincide!", "Tiroida");
            }
        }
    }
}
