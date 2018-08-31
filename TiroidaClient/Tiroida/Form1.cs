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
            PersoanlDataForm dataform = new PersoanlDataForm();
            this.flowLayoutPanel1.Controls.Add(dataform);
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
            ConnectionClass.ClientTCP = null;
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
                    ConnectionClass.ClientTCP = tcpclient;


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
    }
}
