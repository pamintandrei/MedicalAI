using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MetroFramework.Controls;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Tiroida
{
    public partial class PersonalDataForm : UserControl
    {
        delegate void SetGitAndEnableStatusCallBack(bool gifstatus, bool enablestatus);
        delegate void SetInterfaceCallBack(string chanse, string chanse_to_have_nothing);
        delegate string GetInfoCallBack(MetroComboBox combobox);


        private string GetInfoText(MetroComboBox combobox)
        {
            if (combobox.InvokeRequired)
            {
                GetInfoCallBack callback = new GetInfoCallBack(GetInfoText);
                return (string)this.Invoke(callback, new object[] { combobox });
            }
            else
            {
                return combobox.Text;
            }
        }

        private void SetResponseInterface(string chanse, string chanse_to_have_nothing)
        {

            if (this.InvokeRequired)
            {
                SetInterfaceCallBack callback = new SetInterfaceCallBack(SetResponseInterface);
                this.Invoke(callback, new object[] { chanse, chanse_to_have_nothing });
            }
            else
            {
                Application.UseWaitCursor = false;
                ResponseUserControl usercontrol = new ResponseUserControl();
                usercontrol.SetCancer(chanse);
                usercontrol.SetNonCancer(chanse_to_have_nothing);
                usercontrol.SetAnimateCancer((int)double.Parse(chanse_to_have_nothing));


                Panel panel1 = (Panel)this.Parent;
                panel1.Controls.Clear();
                panel1.Controls.Add(usercontrol);
            }
        }

        private void SetGif(bool gifstatus, bool enablestatus)
        {
            if (this.metroButton1.InvokeRequired)
            {
                SetGitAndEnableStatusCallBack callback = new SetGitAndEnableStatusCallBack(SetGif);
                this.Invoke(callback, new object[] { gifstatus, enablestatus });
            }
            else
            {
                
                this.metroButton1.Enabled = enablestatus;
            }
        }

        public PersonalDataForm(bool loged)
        {
            InitializeComponent();

            if (loged)
            {
                pictureBox2.Image = new Bitmap(Properties.Resources.logout);
            }
            else
            {
                this.metroButton2.Enabled = false;
            }

        }

        private void PersoanlDataForm_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            //pictureBox2.Image = new Bitmap(Properties.Resources.logout);
        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            
            this.metroButton1.Enabled = false;
            this.metroButton2.Enabled = false;
            this.pictureBox2.Enabled = false;
            Application.UseWaitCursor = true;
            Thread th = new Thread(SendPersonalData);
            th.Start();
            //SendPersonalData();
            
        }

        private string GetSex()
        {
            string sex = GetInfoText(this.metroComboBox1);
            switch (sex)
            {
                case "Masculin":
                    return "M";
                    break;
                case "Feminin":
                    return "F";
                    break;
                default:
                    return "?";
                    break;
            }
        }

        private string GetOption(MetroComboBox combobox)
        {
            string selectedvalue = GetInfoText(combobox);
            switch (selectedvalue)
            {
                case "Da":
                    return "t";
                    break;
                case "Nu":
                    return "f";
                    break;
                default:
                    return "f";
                    break;
            }
        }

        private string GetValue(NumericUpDown numeric)
        {
            if (numeric.Enabled)
            {
                return numeric.Value.ToString();
            }
            else
            {
                return "?";
            }
        }

        private string GetAge()
        {
            return numericUpDown1.Value.ToString();
        }

        private string GetPregnantStatus()
        {
            if (!this.metroComboBox8.Enabled)
                return "f";

            switch (this.metroComboBox8.Text)
            {
                case "Da":
                    return "t";
                    break;
                case "Nu":
                    return "f";
                    break;
                default:
                    return "f";
                    break;
            }
        }

        private void SendPersonalData()
        {
            
            if (ConnectionClass.ClientTCP == null)
            {
                SetGif(false, true);
                MessageBox.Show("Sunteti momentan offline","Tiroida");
                return;
            }
            
            

            PersonalData data = new PersonalData();

            data.action = "analize";
            data.Age = GetAge();
            data.Sex = GetSex();
            data.on_thyroxine = GetOption(this.metroComboBox2);
            data.query_on_thyroxine = GetOption(this.metroComboBox3);
            data.on_antithyroid_medication = GetOption(this.metroComboBox4);
            data.thyroid_surgery = GetOption(this.metroComboBox5);
            data.query_hypothyroid = GetOption(this.metroComboBox6);
            data.query_hyperthyroid = GetOption(this.metroComboBox7);
            data.pregnant = GetOption(this.metroComboBox8);
            data.sick = GetOption(this.metroComboBox9);
            data.tumor = GetOption(this.metroComboBox10);
            data.lithium = GetOption(this.metroComboBox11);
            data.goitre = GetOption(this.metroComboBox12);
            data.TSH_measured = GetOption(this.metroComboBox13);
            data.TSH = GetValue(this.numericUpDown2);
            data.T3_measured = GetOption(this.metroComboBox14);
            data.T3 = GetValue(this.numericUpDown3);
            data.TT4_measured = GetOption(this.metroComboBox15);
            data.TT4 = GetValue(this.numericUpDown4);
            data.FTI_measured = GetOption(this.metroComboBox16);
            data.FTI = GetValue(this.numericUpDown5);
            data.TBG_measured = GetOption(this.metroComboBox17);
            data.TBG = GetValue(this.numericUpDown6);
            data.patient_name = this.metroTextBox1.Text;
            data.cookie = ConnectionClass.ClientTCP.Cookie;

            

            string datatosend = JsonConvert.SerializeObject(data);
            
            if (ConnectionClass.ClientTCP != null)
            {
                ConnectionClass.ClientTCP.SendContent(datatosend);
                ConnectionClass.ClientTCP.OnResponse += ClientTCP_OnResponse;
            }
        }

        private void ClientTCP_OnResponse(object sender, OnReceiveMessageClientEventArgs e)
        {
            
            SetGif(false, true);
            SetResponseInterface((e.Medical.Chanse_To_Have*100).ToString(), (e.Medical.Chanse_To_Have_Nothing*100).ToString());
            ConnectionClass.ClientTCP.OnResponse -= ClientTCP_OnResponse;
        }

        private void ChangeValueEvent(MetroComboBox combobox, NumericUpDown numeric)
        {
            if (combobox.Text == "Nu" || combobox.Text == "Necunoscut")
            {
                numeric.Enabled = false;
            }
            else
            {
                if (combobox.Text == "Da")
                {
                    numeric.Enabled = true;
                }
            }
        }

        private void metroComboBox16_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeValueEvent(this.metroComboBox16, this.numericUpDown5);
        }

        private void metroComboBox17_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeValueEvent(this.metroComboBox17, this.numericUpDown6);
        }

        private void metroComboBox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeValueEvent(this.metroComboBox13, this.numericUpDown2);
        }

        private void metroComboBox14_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeValueEvent(this.metroComboBox14,this.numericUpDown3);
        }

        private void metroComboBox15_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeValueEvent(metroComboBox15, numericUpDown4);
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (metroComboBox1.Text == "Masculin" || metroComboBox1.Text == "Necunoscut")
                metroComboBox8.Enabled = false;
            else
                metroComboBox8.Enabled = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {




            Login lg = new Login();
            Panel panel1 = (Panel)this.Parent;
            panel1.Controls.Clear();
            panel1.Controls.Add(lg);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (ConnectionClass.ClientTCP == null)
            {
                //SetGif(false, true);
                MessageBox.Show("Sunteti momentan offline", "Tiroida");
                return;
            }


            Viewtests vt = new Viewtests();
            Panel pan1 =  (Panel)this.Parent;
            pan1.Controls.Clear();
            pan1.Controls.Add(vt);



        }
    }
}
