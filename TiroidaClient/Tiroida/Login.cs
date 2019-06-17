using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Threading;

namespace Tiroida
{
    public partial class Login : UserControl
    {
        delegate void OpenFormCallBack(string username);
        delegate void OpenPersonalDataCallback(bool isloged);


        public Login()
        {
            InitializeComponent();
            SetPanelLanguage();
        }

        public void ReloadLanguage()
        {
            languagesettings ls = ConnectionClass.languagesupporter.getLanguagesettings();
            this.metroLabel1.Text = ls.username;
            this.metroLabel2.Text = ls.password;
            this.checkBox1.Text = ls.rememberme;
            this.metroButton3.Text = ls.tryapp;
            this.metroButton2.Text = ls.register;
            this.metroButton1.Text = ls.login;
        }


        private void SetPanelLanguage()
        {
            if (ConnectionClass.config.Language != "Romanian")
            {
                ReloadLanguage();
            }
            
        }


        private void metroButton3_Click(object sender, EventArgs e)
        {
            PersonalDataForm persdata = new PersonalDataForm(false);
            Panel Panel1 = (Panel)this.Parent;
            Panel1.Controls.Clear();
            Panel1.Controls.Add(persdata);

        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            RegisterUserControl registercontrol = new RegisterUserControl();
            Panel Panel1 = (Panel)this.Parent;
            Panel1.Controls.Clear();
            Panel1.Controls.Add(registercontrol);
        }


        private void SendLoginMessage(LoginContent lg)
        {
            if (!ConnectionClass.ClientTCP.isconnected)
            {
                //SetGif(false, true);
                MessageBox.Show("Sunteti momentan offline", "Tiroida");
                return;
            }


            string datatosend = JsonConvert.SerializeObject(lg);
            if (!ConnectionClass.ClientTCP.isconnected)
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


        private void SaveCookie()
        {
            cookieobj cookie = new cookieobj(ConnectionClass.ClientTCP.Cookie);
            string cookiesave = JsonConvert.SerializeObject(cookie);
            StreamWriter writer = new StreamWriter(@"cookie.json", false);
            writer.Write(cookiesave);
            writer.Close();
        }


        private void OpenPersonalData(bool isloged)
        {
            if (this.InvokeRequired)
            {
                OpenPersonalDataCallback ofcb = new OpenPersonalDataCallback(OpenPersonalData);
                this.Invoke(ofcb, new object[] { isloged });
            }
            else
            {
                if (this.checkBox1.Checked)
                {
                    SaveCookie();
                }


                PersonalDataForm form = new PersonalDataForm(isloged);
                Panel p1 =  (Panel)this.Parent;
                p1.Controls.Clear();
                p1.Controls.Add(form);
            }
        }

        private bool isgot = false;
        private void ClientTCP_OnLoginResponse(object sender, OnReceiveLoginMessageArgs e)
        {
            Console.WriteLine("Login Response received: " + e.errorcode.ToString());
            if (!isgot)
            {
                Console.WriteLine("Yeah, here we go");
                isgot = true;
                if (e.errorcode == 2)
                {
                    OpenConfirmationCode(e.username);
                }
                else
                {
                    if (e.errorcode == 0)
                    {
                        
                        OpenPersonalData(true);

                    }
                    else
                    {
                        MessageBox.Show("Nume de utilizator sau parola incorecta", "Tiroida");
                    }
                }
                
            }
            this.isgot = false;
            ConnectionClass.ClientTCP.OnLoginResponse -= ClientTCP_OnLoginResponse;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            LoginContent lg = new LoginContent(this.metroTextBox1.Text, this.metroTextBox2.Text);
            Thread th1 = new Thread(() => SendLoginMessage(lg));
            th1.Start();
        }


        private void SetLanguage()
        {
            
        }


        private void Login_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;

            

           // Console.WriteLine("Compiled");
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
