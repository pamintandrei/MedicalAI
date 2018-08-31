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
    public partial class PersoanlDataForm : UserControl
    {
        delegate void SetGitCallBack(bool value);
        delegate void SetInterfaceCallBack(UserControl usercontrol);


        private void SetInterface(UserControl usercontrol)
        {
            FlowLayoutPanel panel = (FlowLayoutPanel)this.Parent;
            if (panel.InvokeRequired)
            {
                SetInterfaceCallBack callback = new SetInterfaceCallBack(SetInterface);
                this.Invoke(callback, new object[] { usercontrol });
            }
            else
            {
                panel.Controls.Clear();
                panel.Controls.Add(usercontrol);
            }
        }

        private void SetGif(bool value)
        {
            if (this.pictureBox1.InvokeRequired)
            {
                SetGitCallBack callback = new SetGitCallBack(SetGif);
                this.Invoke(callback, new object[] { value });
            }
            else
            {
                this.pictureBox1.Visible = value;
            }
        }

        public PersoanlDataForm()
        {
            InitializeComponent();
        }

        private void PersoanlDataForm_Load(object sender, EventArgs e)
        {
            
        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            //Thread th = new Thread(SendPersonalData);
            //th.Start();
            SendPersonalData();
            this.pictureBox1.Visible = true;
        }

        private string GetSex()
        {
            switch (metroComboBox1.Text)
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
            string selectedvalue = combobox.Text;
            switch (selectedvalue)
            {
                case "Da":
                    return "t";
                    break;
                case "Nu":
                    return "f";
                    break;
                default:
                    return "?";
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
            /*
            if (ConnectionClass.ClientTCP == null)
            {
                SetGif(false);
                MessageBox.Show("Sunte-ti momentan offline","Tiroida");
                return;
            }
            */

            PersonalData data = new PersonalData();


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

            

            string datatosend = JsonConvert.SerializeObject(data);
            Console.WriteLine("date trimise: " + datatosend);

            if (ConnectionClass.ClientTCP != null)
            {
                ConnectionClass.ClientTCP.SendContent(datatosend);
                ConnectionClass.ClientTCP.OnResponse += ClientTCP_OnResponse;
            }
        }

        private void ClientTCP_OnResponse(object sender, OnReceiveMessageClientEventArgs e)
        {
            
            SetGif(false);
            ResponseUserControl usercontrol = new ResponseUserControl();
            usercontrol.SetCancer(e.Medical.Chanse_To_Have.ToString());
            usercontrol.SetNonCancer(e.Medical.Chanse_To_Have_Nothing.ToString());
            SetInterface(usercontrol);
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
    }
}
