﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using Newtonsoft.Json;
using System.Threading;

namespace Tiroida
{
    public partial class HypertiroidForm : UserControl
    {

        delegate void SetGitAndEnableStatusCallBack(bool gifstatus, bool enablestatus);
        delegate void SetInterfaceCallBack(string chanse, string chanse_to_have_nothing);
        delegate int GetInfoCallBack(MetroComboBox combobox);
        private List<string> unknownbox;
        public bool loged;


        public HypertiroidForm()
        {
            InitializeComponent();
            SetPanelLanguage();
            this.Dock = DockStyle.Fill;
            if (!ConnectionClass.ClientTCP.isloged)
            {
                this.metroButton2.Enabled = false;
            }

        }



        private int GetInfoText(MetroComboBox combobox)
        {
            if (combobox.InvokeRequired)
            {
                GetInfoCallBack callback = new GetInfoCallBack(GetInfoText);
                return (int)this.Invoke(callback, new object[] { combobox });
            }
            else
            {
                return combobox.SelectedIndex;
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
                ResponseUserControl usercontrol = new ResponseUserControl(ResponseUserControl.THYROID_HYPE);
                usercontrol.SetCancer(chanse);
                usercontrol.SetNonCancer(chanse_to_have_nothing);
                usercontrol.SetAnimateCancer((int)double.Parse(chanse_to_have_nothing));


                Panel panel1 = (Panel)this.Parent;
                panel1.Controls.Clear();
                panel1.Controls.Add(usercontrol);
            }
        }


        private void AddToList(params string[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                unknownbox.Add(list[i]);
            }
        }

        private void changeLanguageCombobox(MetroComboBox box, string yes, string no, string unknown, bool isunknown)
        {
            box.Items.Clear();
            box.Items.Add(yes);
            box.Items.Add(no);
            if (isunknown)
            {
                box.Items.Add(unknown);
            }

        }



        private void changeComboBoxlang(Control control)
        {

            languagesettings ls = ConnectionClass.languagesupporter.getLanguagesettings();
            if (control is MetroComboBox)
            {
                MetroComboBox box = (MetroComboBox)control;

                if (box != null)
                {
                    this.unknownbox = new List<string> { "metroComboBox16", "metroComboBox17", "metroComboBox15", "metroComboBox14", "metroComboBox13" };

                    if (unknownbox.Contains(box.Name))
                    {
                        changeLanguageCombobox(box, ls.yes_answer, ls.no_answer, ls.unknown_answer, true);
                    }
                    else
                    {
                        changeLanguageCombobox(box, ls.yes_answer, ls.no_answer, ls.unknown_answer, false);
                    }
                }

            }
            else
                foreach (Control child in control.Controls)
                {
                    changeComboBoxlang(child);
                }
        }


        private void changeSexLanguage()
        {
            languagesettings ls = ConnectionClass.languagesupporter.getLanguagesettings();
            this.metroComboBox1.Items.Clear();
            this.metroComboBox1.Items.Add(ls.male);
            this.metroComboBox1.Items.Add(ls.female);
            this.metroComboBox1.Items.Add(ls.unknown_answer);
        }

        public void ReloadLanguage()
        {
            languagesettings ls = ConnectionClass.languagesupporter.getLanguagesettings();
            this.metroLabel1.Text = ls.sex;
            this.metroLabel2.Text = ls.age;
            this.metroLabel3.Text = ls.on_thyroxine;
            this.metroLabel4.Text = ls.query_on_thyroxine;
            this.metroLabel5.Text = ls.on_antithyroid_medication;
            this.metroLabel11.Text = ls.tumor;
            this.metroLabel20.Text = ls.FTI_measured;
            this.metroLabel21.Text = ls.FTI;
            this.metroLabel6.Text = ls.thyroid_surgery;
            this.metroLabel8.Text = ls.query_hypothyroid;
            this.metroLabel7.Text = ls.query_hyperthyroid;
            this.metroLabel9.Text = ls.pregnant;
            this.metroLabel10.Text = ls.sick;
            this.metroLabel12.Text = ls.lithium;
            this.metroLabel22.Text = ls.TBG_measured;
            this.metroLabel23.Text = ls.TBG;
            this.metroLabel13.Text = ls.goitre;
            this.metroLabel14.Text = ls.TSH_measured;
            this.metroLabel15.Text = ls.TSH;
            this.metroLabel16.Text = ls.T3_measured;
            this.metroLabel17.Text = ls.T3;
            this.metroLabel18.Text = ls.TT4_measured;
            this.metroLabel19.Text = ls.TT4;
            this.metroLabel24.Text = ls.patient_name;
            this.metroButton2.Text = ls.result;
            this.metroButton1.Text = ls.send_data;
            this.metroButton3.Text = ls.verify_photo;
            this.metroLabel27.Text = ls.psych;
            this.metroLabel25.Text = ls.Il3l_treatment;
            this.metroLabel26.Text = ls.hypopituitary;
            this.metroButton4.Text = ls.verify_hypo;
            changeComboBoxlang(this);
            changeSexLanguage();
        }


        private void SetPanelLanguage()
        {
            if (ConnectionClass.config.Language != "Romanian")
            {
                ReloadLanguage();
            }

        }




        private string GetSex()
        {
            int sex = GetInfoText(this.metroComboBox1);
            switch (sex)
            {
                case 0:
                    return "M";
                    break;
                case 1:
                    return "F";
                    break;
                default:
                    return "?";
                    break;
            }
        }

        private string GetOption(MetroComboBox combobox)
        {


            int indexoption = GetInfoText(combobox);
            switch (indexoption)
            {
                case 0:
                    return "t";
                    break;
                case 1:
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

            switch (this.metroComboBox8.SelectedIndex)
            {
                case 0:
                    return "t";
                    break;
                case 1:
                    return "f";
                    break;
                default:
                    return "f";
                    break;
            }
        }




        private void SendPersonalData()
        {

            if (ConnectionClass.ClientTCP == null || !ConnectionClass.ClientTCP.isconnected)
            {
                Application.UseWaitCursor = false;
                SetGif(false, true);

                MessageBox.Show("Sunteti momentan offline", "Tiroida");
                return;
            }



            hypertiroidContent data = new hypertiroidContent();

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
            data.psych = GetOption(this.metroComboBox20);
            data.hypopituitary = GetOption(this.metroComboBox19);
            data.Il3l_treatment = GetOption(this.metroComboBox18);
            data.patient_name = this.metroTextBox1.Text;
            



            string datatosend = JsonConvert.SerializeObject(data);

            Console.WriteLine(datatosend);


            if (ConnectionClass.ClientTCP != null)
            {
                ConnectionClass.ClientTCP.SendContent(datatosend);
                ConnectionClass.ClientTCP.OnResponse += ClientTCP_OnResponse;
            }
        }

        private void ClientTCP_OnResponse(object sender, OnReceiveMessageClientEventArgs e)
        {
            ConnectionClass.ClientTCP.OnResponse -= ClientTCP_OnResponse;
            SetGif(false, true);
            SetResponseInterface((e.Medical.Chanse_To_Have * 100).ToString(), (e.Medical.Chanse_To_Have_Nothing * 100).ToString());
            
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







        private void metroButton4_Click(object sender, EventArgs e)
        {
            PersonalDataForm form;
            if (ConnectionClass.ClientTCP.isadmin || !ConnectionClass.ClientTCP.isloged)
            {
               form = new PersonalDataForm(false);
            }
            else
            {
               form = new PersonalDataForm(true);
            }

            Panel p1 = (Panel)this.Parent;
            p1.Controls.Clear();
            p1.Controls.Add(form);

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

    







        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
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
        }

        private void metroComboBox17_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeValueEvent(this.metroComboBox17, this.numericUpDown6);
        }

        private void metroComboBox15_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeValueEvent(this.metroComboBox15, this.numericUpDown4);
        }

        private void metroComboBox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeValueEvent(this.metroComboBox13, this.numericUpDown2);
        }

        private void metroComboBox14_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeValueEvent(this.metroComboBox14, this.numericUpDown3);
        }

        private void metroComboBox16_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeValueEvent(this.metroComboBox16, this.numericUpDown5);
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (metroComboBox1.SelectedIndex == 0 || metroComboBox1.SelectedIndex == 2)
                metroComboBox8.Enabled = false;
            else
                metroComboBox8.Enabled = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {


            if (!ConnectionClass.ClientTCP.isloged)
            {
                Login lg = new Login();
                Panel panel1 = (Panel)this.Parent;
                panel1.Controls.Clear();
                panel1.Controls.Add(lg);
            }
            else
            {
                adminpanel lg = new adminpanel();
                Panel panel1 = (Panel)this.Parent;
                panel1.Controls.Clear();
                panel1.Controls.Add(lg);
            }
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            photoUploader pneumonia = new photoUploader();

            Panel panel = (Panel)this.Parent;
            panel.Controls.Clear();
            panel.Controls.Add(pneumonia);
        }
    }
}
