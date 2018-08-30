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
        public Tiroida()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn();
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

            ClientTCP client = new ClientTCP((string)config["IpAdress"], (int)config["port"], 10000);
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

                    PersonalData data = new PersonalData();
                    data.Age = "72";
                    data.Sex = "M";
                    data.on_thyroxine = "f";
                    data.query_on_thyroxine = "f";
                    data.on_antithyroid_medication = "f";
                    data.thyroid_surgery = "f";
                    data.query_hypothyroid = "f";
                    data.query_hyperthyroid = "f";
                    data.pregnant = "f";
                    data.sick = "f";
                    data.tumor = "f";
                    data.lithium = "f";
                    data.goitre = "f";
                    data.TSH_measured = "t";
                    data.TSH = "30";
                    data.T3_measured = "t";
                    data.T3 = "0.60";
                    data.TT4_measured = "t";
                    data.TT4 = "15";
                    data.FTI_measured = "t";
                    data.FTI = "10";
                    data.TBG_measured = "f";
                    data.TBG = "?";

                    string datatosend = JsonConvert.SerializeObject(data);
                    Console.WriteLine("date trimise: " + datatosend);
                    tcpclient.SendContent(datatosend);

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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            PersoanlDataForm dataform = new PersoanlDataForm();
            this.panel1.Controls.Add(dataform);
        }
    }
}
