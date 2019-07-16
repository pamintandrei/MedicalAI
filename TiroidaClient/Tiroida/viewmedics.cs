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
using MetroFramework.Controls;

namespace Tiroida
{
    public partial class viewmedics : UserControl
    {
        delegate void SetConfigInterfaceCallBack(bool register, bool medic);
        delegate void SetNonMedicsCallBack(List<string> users);
        delegate void SetMedicsCallBack(List<string> users);
        delegate void SetButton1CallBack(MetroButton button ,bool value);


        private void SetButton1(MetroButton button, bool value)
        {
            if (this.InvokeRequired)
            {
                SetButton1CallBack callback = new SetButton1CallBack(SetButton1);
                this.Invoke(callback, new object[] { button, value });

            }
            else
            {
                button.Enabled = value;
            }

        }



        private void SetConfigInterface(bool register, bool medic)
        {
            if (this.InvokeRequired)
            {
                SetConfigInterfaceCallBack callback = new SetConfigInterfaceCallBack(SetConfigInterface);
                this.Invoke(callback, new object[] { register, medic });
            }
            else
            {
                this.metroCheckBox1.Enabled = true;
                this.metroCheckBox2.Enabled = true;
                this.metroButton1.Enabled = true;
                this.metroCheckBox1.Checked = register;
                this.metroCheckBox2.Checked = medic;
            }
        }


        private void SetNonMedics(List<string> users)
        {
            if (this.InvokeRequired)
            {
                SetNonMedicsCallBack callback = new SetNonMedicsCallBack(SetNonMedics);
                this.Invoke(callback,new object[] { users });
            }
            else
            {
                this.metroComboBox1.Enabled = true;
                this.metroButton2.Enabled = true;
                foreach (string s in users)
                {
                    this.metroComboBox1.Items.Add(s);
                }
            }
        }



        private void SetMedics(List<string> users)
        {
            if (this.InvokeRequired)
            {
                SetMedicsCallBack callback = new SetMedicsCallBack(SetMedics);
                this.Invoke(callback, new object[] { users });
            }
            else
            {
                this.metroComboBox2.Enabled = true;
                this.metroButton3.Enabled = true;
                foreach (string s in users)
                {
                    this.metroComboBox2.Items.Add(s);
                }
            }
        }


        public viewmedics()
        {
            InitializeComponent();
            this.metroCheckBox1.Enabled = false;
            this.metroCheckBox1.Enabled = false;
            this.metroComboBox1.Enabled = false;
            this.metroComboBox2.Enabled = false;
            this.metroButton1.Enabled = false;
            this.metroButton2.Enabled = false;
            this.metroButton3.Enabled = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }


        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (ConnectionClass.ClientTCP.isconnected)
            {
                Application.UseWaitCursor = true;
                this.metroButton1.Enabled = false;
                SetConfigContent content = new SetConfigContent(this.metroCheckBox1.Checked, this.metroCheckBox2.Checked);
                string data_to_send = JsonConvert.SerializeObject(content);
                ConnectionClass.ClientTCP.SendContent(data_to_send);
                ConnectionClass.ClientTCP.OnReceiveSetConfig += ClientTCP_OnReceiveSetConfig;
            }
            else
            {
                MessageBox.Show("Fara conexiune!","MedicalAI");
            }
        }




        private void ClientTCP_OnReceiveSetConfig(object sender, OnReceiveSetConfigArgs e)
        {
            Application.UseWaitCursor = false;
            SetButton1(metroButton1,true);
            if (e.errcode == 0)
            {
                MessageBox.Show("Serverul a fost configurat cu succes", "MedicalAI");
            }
            ConnectionClass.ClientTCP.OnReceiveSetConfig -= ClientTCP_OnReceiveSetConfig;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void getCurrentConfigAndSetIt()
        {
            configContent content = new configContent();
            string data_to_send = JsonConvert.SerializeObject(content);
            ConnectionClass.ClientTCP.SendContent(data_to_send);
            ConnectionClass.ClientTCP.OnReceiveGetConfigResponse += ClientTCP_OnReceiveGetConfigResponse;
        }

        private void ClientTCP_OnReceiveGetConfigResponse(object sender, OnReceiveGetConfigResponseArgs e)
        {
            SetConfigInterface(e.register_verification, e.medic_registration);
            ConnectionClass.ClientTCP.OnReceiveGetConfigResponse -= ClientTCP_OnReceiveGetConfigResponse;
        }


        private void getNonMedics()
        {
            configContent content = new configContent();
            content.action = "getnonmedics";
            content.cookie = ConnectionClass.ClientTCP.Cookie;

            string data_to_send = JsonConvert.SerializeObject(content);
            ConnectionClass.ClientTCP.SendContent(data_to_send);
            ConnectionClass.ClientTCP.OnReceiveNonMedics += ClientTCP_OnReceiveNonMedics;
        }

        private void ClientTCP_OnReceiveNonMedics(object sender, OnReceiveNonMedicsArgs e)
        {
            SetNonMedics(e.medics);
            ConnectionClass.ClientTCP.OnReceiveNonMedics -= ClientTCP_OnReceiveNonMedics;
        }

        private void getMedics()
        {
            configContent content = new configContent();
            content.action = "getmedics";
            string data_to_send = JsonConvert.SerializeObject(content);
            ConnectionClass.ClientTCP.SendContent(data_to_send);
            ConnectionClass.ClientTCP.OnReceiveMedics += ClientTCP_OnReceiveMedics;
        }

        private void ClientTCP_OnReceiveMedics(object sender, OnReceiveNonMedicsArgs e)
        {
            SetMedics(e.medics);
            ConnectionClass.ClientTCP.OnReceiveMedics -= ClientTCP_OnReceiveMedics;
        }

        private void viewmedics_Load(object sender, EventArgs e)
        {
            if (ConnectionClass.ClientTCP.isconnected)
            {
                getCurrentConfigAndSetIt();
                getNonMedics();
                getMedics();
            }
            else
            {
                MessageBox.Show("Fara conexiune!", "MedicalAI");
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.metroComboBox1.Text))
            {
                MessageBox.Show("Va rugam selectati un medic");
                return;
            }

            if (ConnectionClass.ClientTCP.isconnected)
            {
                Application.UseWaitCursor = true;
                this.metroButton2.Enabled = false;
                AddMedicContent content = new AddMedicContent(this.metroComboBox1.Text);
                string data_to_send = JsonConvert.SerializeObject(content);
                ConnectionClass.ClientTCP.SendContent(data_to_send);
                ConnectionClass.ClientTCP.OnReceiveAddMedic += ClientTCP_OnReceiveAddMedic;
            }
            else
            {
                MessageBox.Show("Fara conexiune!", "MedicalAI");
            }


        }

        private void ClientTCP_OnReceiveAddMedic(object sender, OnReceiveSetConfigArgs e)
        {
            Application.UseWaitCursor = false;
            SetButton1(metroButton2, true);
            if (e.errcode == 0)
            {

                MessageBox.Show("Operatiune cu succes!");
            }
            ConnectionClass.ClientTCP.OnReceiveAddMedic -= ClientTCP_OnReceiveAddMedic;
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.metroComboBox2.Text))
            {
                MessageBox.Show("Va rugam selectati un medic");
                return;
            }

            if (ConnectionClass.ClientTCP.isconnected)
            {
                Application.UseWaitCursor = true;
                this.metroButton2.Enabled = false;
                AddMedicContent content = new AddMedicContent(this.metroComboBox2.Text, "removemedic");
                string data_to_send = JsonConvert.SerializeObject(content);
                ConnectionClass.ClientTCP.SendContent(data_to_send);
                ConnectionClass.ClientTCP.OnReceiveAddMedic += ClientTCP_OnReceiveAddMedic1;
            }
            else
            {
                MessageBox.Show("Fara conexiune!", "MedicalAI");
            }

        }

        private void ClientTCP_OnReceiveAddMedic1(object sender, OnReceiveSetConfigArgs e)
        {
            Application.UseWaitCursor = false;
            SetButton1(metroButton3, true);
            if (e.errcode == 0)
            {
                MessageBox.Show("Operatiune cu succes!");
            }
            ConnectionClass.ClientTCP.OnReceiveAddMedic -= ClientTCP_OnReceiveAddMedic1;
        }
    }
}
