using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;

namespace Tiroida
{
    public partial class IpConfigForm : MetroFramework.Forms.MetroForm
    {
        public bool languagechanged;
        public IpConfigForm()
        {
            InitializeComponent();
            SetPanelLanguage();
            this.languagechanged = false;
            this.MinimumSize = new Size(815, 375);
            this.MaximumSize = new Size(815, 375);


            if (!ConnectionClass.ClientTCP.isloged)
            {
                this.metroTextBox3.Enabled = false;
                this.metroTextBox4.Enabled = false;
                this.metroTextBox5.Enabled = false;
                this.metroLabel4.Enabled = false;
                this.metroLabel5.Enabled = false;
                this.metroLabel6.Enabled = false;
            }


        }

        public void ReloadLanguage()
        {
            languagesettings ls = ConnectionClass.languagesupporter.getLanguagesettings();
            this.metroLabel1.Text = ls.ip_addr;
            this.metroLabel2.Text = ls.port;
            this.metroLabel3.Text = ls.language;
            this.metroButton1.Text = ls.apply;
            this.Text = ls.config_ip;
        }


        private void SetPanelLanguage()
        {
            if (ConnectionClass.config.Language != "Romanian")
            {
                ReloadLanguage();
            }

        }




        private void getAndSetConfigFile()
        {
            string configtext = File.ReadAllText(@"config.json");
            
            JObject config = JObject.Parse(configtext);

            this.metroTextBox1.Text = (string)config["IpAdress"];
            this.metroTextBox2.Text = (string)config["port"];

        }

        private void ReloadLanguageSettings(string language)
        {
            string path = @"languages/" + language + ".json";
            string jsontext =  File.ReadAllText(path);
            languageSupporter ls = new languageSupporter(jsontext);
            ConnectionClass.languagesupporter = ls;
            ReloadLanguage();
        }


        private void SetConfig()
        {
            int right = 0;
            IPAddress address;

            
           
            if (!IPAddress.TryParse(this.metroTextBox1.Text, out address))
            {
                MessageBox.Show("Va rugam introduceti o adresa IP valida", "MedicalAI");
                return;
            }


            if (int.TryParse(this.metroTextBox2.Text, out right))
            {

                configFile config = new configFile(this.metroTextBox1.Text, Int32.Parse(this.metroTextBox2.Text), this.metroComboBox1.Text);

                string jsonconfig = JsonConvert.SerializeObject(config);

                File.WriteAllText(@"config.json", jsonconfig);

                ConnectionClass.ClientTCP.SetIp(this.metroTextBox1.Text);
                ConnectionClass.ClientTCP.SetPort(Int32.Parse(this.metroTextBox2.Text));

                if (ConnectionClass.config.Language != config.Language)
                {
                    // SetLaunguage();
                    this.languagechanged = true;
                    ReloadLanguageSettings(config.Language);
                }


                ConnectionClass.config = config;


                MessageBox.Show("Modificarile au fost facute!", "MedicalAI");

            }
            else
            {
                MessageBox.Show("Va rugam introduceti un port valid!","Tiroida");
            }
        }



        private void IpConfig_Load(object sender, EventArgs e)
        {
            this.metroComboBox1.Text = ConnectionClass.config.Language;
            getAndSetConfigFile();



        }

        private void ChangePassword()
        {
            if (!string.IsNullOrWhiteSpace(this.metroTextBox3.Text))
            {
                if (ConnectionClass.ClientTCP.isloged && ConnectionClass.ClientTCP.isconnected && !string.IsNullOrWhiteSpace(this.metroTextBox3.Text))
                {
                    if (this.metroTextBox4.Text.Length < 3)
                    {
                        MessageBox.Show("Parola este prea scurta!");
                        return;
                    }


                    if (this.metroTextBox4.Text == this.metroTextBox5.Text)
                    {
                        ChangePasswordContent content = new ChangePasswordContent(ConnectionClass.ClientTCP.Cookie, this.metroTextBox3.Text, this.metroTextBox5.Text);

                        string data_to_send = JsonConvert.SerializeObject(content);
                        ConnectionClass.ClientTCP.SendContent(data_to_send);
                        ConnectionClass.ClientTCP.OnReceiveChangePasswordResponse += ClientTCP_OnReceiveChangePasswordResponse;
                    }
                    else
                    {
                        MessageBox.Show("Parolele nu coincid", "MedicalAI");
                    }

                }
                else
                {
                    if (ConnectionClass.ClientTCP.isloged)
                    {
                        MessageBox.Show("Nu sunteti conectat");
                    }
                }
            }
        }

        private void ClientTCP_OnReceiveChangePasswordResponse(object sender, OnReceiveChangePasswordResponseArgs e)
        {
            ConnectionClass.ClientTCP.OnReceiveChangePasswordResponse -= ClientTCP_OnReceiveChangePasswordResponse;
            if (e.getCode() == -1)
            {
                MessageBox.Show("Parola curenta incorecta", "MedicalAI");
            }
            else
                if (e.getCode() == 0)
            {
                MessageBox.Show("Parola a fost schimbata cu succes!", "MedicalAI");
            }
            else
            {
                MessageBox.Show("Invalid cookie session");
            }

            
            
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            SetConfig();
            ChangePassword();
            Application.UseWaitCursor = false;
        }
    }
}
