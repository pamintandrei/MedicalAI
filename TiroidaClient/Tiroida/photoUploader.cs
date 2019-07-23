using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;


namespace Tiroida
{
    public partial class photoUploader : UserControl
    {
        private string imagepath;
        delegate void changeScreenToResultCallBack(string result, int proc);


        public photoUploader()
        {
            InitializeComponent();
            this.pictureBox1.AllowDrop = true;
            SetPanelLanguage();
        }



        public void ReloadLanguage()
        {
            languagesettings ls = ConnectionClass.languagesupporter.getLanguagesettings();
            this.metroButton1.Text = ls.send_photo;
            this.metroButton2.Text = ls.update_photo;
            this.metroComboBox1.Items.Clear();
            this.metroComboBox1.Items.Add(ls.breast_cancer);
            this.metroComboBox1.Items.Add(ls.internal_bleeding);
            this.metroComboBox1.Items.Add(ls.leukemia);
            this.metroComboBox1.Items.Add(ls.malaria);
            this.metroComboBox1.Items.Add(ls.pneumonia);
            this.metroComboBox1.Items.Add(ls.tuberculosis);
            this.metroComboBox1.Items.Add(ls.parkinson);
        }

        private void changeScreenToResult(string result, int proc)
        {
            if (this.InvokeRequired)
            {
                changeScreenToResultCallBack callback = new changeScreenToResultCallBack(changeScreenToResult);
                this.Invoke(callback, new object[] { result, proc });
            }
            else
            {
                Application.UseWaitCursor = false;

                ResponseUserControl response = new ResponseUserControl(ResponseUserControl.PHOTO);
                

                response.SetCancer(result);
                response.SetNonCancer(result);
                response.SetAnimateCancer(proc);
                Console.WriteLine(result);

                Panel p = (Panel)this.Parent;
                p.Controls.Clear();
                p.Controls.Add(response);
                Console.WriteLine("We reached the response!");
            }

        }



        private void SetPanelLanguage()
        {
            if (ConnectionClass.config.Language != "Romanian")
            {
                ReloadLanguage();
            }

        }




        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileopen = new OpenFileDialog();
            fileopen.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (fileopen.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(fileopen.FileName);
              
            }
        }

        private void pneumoniaForm_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            
            foreach (string s in ((string[])e.Data.GetData(DataFormats.FileDrop)))
            {
                Image img = Image.FromFile(s);
                this.pictureBox1.Image = img;
                this.imagepath = s;
            }
        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            PersonalDataForm f = new PersonalDataForm(ConnectionClass.ClientTCP.isloged);
            Panel p = (Panel)this.Parent;

            p.Controls.Clear();
            p.Controls.Add(f);

        }


        private string getPhotoType()
        {
            int diseaseIndex = this.metroComboBox1.SelectedIndex;
            switch (diseaseIndex)
            {
                case 0:
                    return "cancersan";
                    break;
                case 1:
                    return "hemoragie";
                    break;
                case 2:
                    return "leucemie";
                    break;
                case 3:
                    return "malarie";
                    break;
                case 4:
                    return "pneumonia";
                    break;
                case 5:
                    return "tuberculoza";
                    break;
                case 6:
                    return "parkinson";
                    break;
                default:
                    MessageBox.Show("Please select a disease");
                    break;
                
            }
            return "";
        }



        private void metroButton1_Click(object sender, EventArgs e)
        {
            
            if (ConnectionClass.ClientTCP == null || !ConnectionClass.ClientTCP.isconnected)
            {
                MessageBox.Show("Sunteti momentan offline", "Tiroida");
                return;
            }
            

            if (string.IsNullOrWhiteSpace(this.metroComboBox1.Text))
            {
                MessageBox.Show("Va rugam sa selectati o boala");
                return;
            }


            if (!string.IsNullOrWhiteSpace(this.imagepath))
            {
                Application.UseWaitCursor = true;
                this.metroButton1.Enabled = false;
                string action = getPhotoType();
                photoContent data = new photoContent(action,this.imagepath);
                string datajson = JsonConvert.SerializeObject(data);
                ConnectionClass.ClientTCP.SendContent(datajson);
                
                ConnectionClass.ClientTCP.OnReceivePneumoniaResponse += ClientTCP_OnReceivePneumoniaResponse;


            }
            else
            {
                MessageBox.Show("Va rugam trimiteti o poza valida!");
                return;
            }
        }


        private void ClientTCP_OnReceivePneumoniaResponse(object sender, OnReceivePhotoResult e)
        {
            ConnectionClass.ClientTCP.OnReceivePneumoniaResponse -= ClientTCP_OnReceivePneumoniaResponse;
            double result = double.Parse(e.pneumoniaChanse);
            result *= 100;
            int procente = Convert.ToInt32(result);
            changeScreenToResult(result.ToString(), procente);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
