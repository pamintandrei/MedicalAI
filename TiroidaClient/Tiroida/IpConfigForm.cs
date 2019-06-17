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

        private void metroButton1_Click(object sender, EventArgs e)
        {
            SetConfig();
            
        }
    }
}
