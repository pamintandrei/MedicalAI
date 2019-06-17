using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tiroida
{
    public partial class Tiroida : MetroFramework.Forms.MetroForm
    {
        delegate void SetConnectioncallback(string text);
        delegate void SetInterfaceCallBack();
        public Tiroida()
        {
            InitializeComponent();


            conn();

            Login dataform = new Login();

            this.panel1.Controls.Add(dataform);

            this.MinimumSize = new Size(980, 666);
        }


        private void SetInterface()
        {
            if (this.InvokeRequired)
            {
                SetInterfaceCallBack callback = new SetInterfaceCallBack(SetInterface);
                this.Invoke(callback, new object[] {  });
            }
            else
            {
                PersonalDataForm dataform = new PersonalDataForm(ConnectionClass.ClientTCP.isloged);
                this.panel1.Controls.Clear();
                this.panel1.Controls.Add(dataform);
            }

        }


        private void setcookie()
        {
            string cookiejson = System.IO.File.ReadAllText(@"cookie.json");
            JObject config = JObject.Parse(cookiejson);

            if (string.IsNullOrEmpty((string)config["cookie"]))
            {
                ConnectionClass.ClientTCP.isloged = false;
            }
            else
            {
                ConnectionClass.ClientTCP.isloged = true;
                ConnectionClass.ClientTCP.Cookie = (string)config["cookie"];

                
                SetInterface();

            }


        }


        private void Form1_Load(object sender, EventArgs e)
        {
            

     



        }

        private void SetConnectionState(string text)
        {
            if (this.metroLabel2.InvokeRequired)
            {
                SetConnectioncallback callback = new SetConnectioncallback(SetConnectionState);
                this.Invoke(callback, new object[] { text });
            }
            else
            {
                if (String.Compare(text, "Connected") == 0)
                {
                    this.metroLabel2.Text = text;
                    this.metroLabel2.Location = new Point(this.metroLabel2.Location.X + 15, this.metroLabel2.Location.Y);
                    this.metroLabel2.ForeColor = Color.Green;
                }
                else
                {
                    if (String.Compare(text, "Connecting...") == 0)
                    {
                        this.metroLabel2.Text = text;
                        this.metroLabel2.Location = new Point(this.metroLabel2.Location.X - 15, this.metroLabel2.Location.Y);
                        this.metroLabel2.ForeColor = Color.SteelBlue;
                    }
                    else
                    {
                        this.metroLabel2.Text = text;
                    }
                }
            }
        }

        private void conn()
        {
            
            string configtext = System.IO.File.ReadAllText(@"config.json");
            Console.WriteLine("Content of the file: {0}", configtext);

            JObject config = JObject.Parse(configtext);

            string languageoption = (string)config["Language"];
            ConnectionClass.config = new configFile((string)config["IpAdress"],(int)config["port"], languageoption);

            if (languageoption != "Romanian")
            {
                string configlanguagepath = @"languages/" + languageoption + ".json";
                string jsonlanguageconfig =  System.IO.File.ReadAllText(configlanguagepath);

                languageSupporter supporter = new languageSupporter(jsonlanguageconfig);
                ConnectionClass.languagesupporter = supporter;
                
            }


            ClientTCP client = new ClientTCP((string)config["IpAdress"], (int)config["port"], 10000);
            ConnectionClass.ClientTCP = client;
            Console.WriteLine(config["IpAdress"]);
       
            Thread th = new Thread(new ParameterizedThreadStart(ReapetUntilConnected));

            th.Start(client);

            client.OnResponse += Client_OnResponse;
            client.OnConnectionLost += Client_OnConnectionLost;
        }

        private void Client_OnConnectionLost(object sender, EventArgs e)
        {
            Thread th = new Thread(new ParameterizedThreadStart(ReapetUntilConnected));
            th.Start(sender);
            SetConnectionState("Connecting...");
            ConnectionClass.ClientTCP.isconnected = false;
        }

        private void ReapetUntilConnected(object client)
        {
            ClientTCP tcpclient = (ClientTCP)client;
            while (true)
            {
                Task.Delay(100);
                int response = tcpclient.DoConnection();
                if (response == ClientTCP.CONECTIONSUCCESS)
                {
                    this.SetConnectionState("Connected");
                    //ConnectionClass.ClientTCP = tcpclient;
                    setcookie();


                    break;
                }
            }
        }

        private void Client_OnResponse(object sender, OnReceiveMessageClientEventArgs e)
        {
            MedicalProblems medicalproblem = e.Medical;
            /*
             * Here I will implement things 
             * Update GUI
             * Add shit
             * 
             */
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Click(object sender, EventArgs e)
        {
            
        }


        private void ReloadCurrentFormLanguage()
        {
            dynamic c = this.panel1.Controls[0];
            c.ReloadLanguage();


        }


        private void metroButton1_Click(object sender, EventArgs e)
        {
            
            IpConfigForm config = new IpConfigForm();
            

            if (config.ShowDialog(this) == DialogResult.Cancel)
            {
                
                
                if (config.languagechanged)
                {

                    ReloadCurrentFormLanguage();

                }
                
            }
        
            
        }
    }
}
