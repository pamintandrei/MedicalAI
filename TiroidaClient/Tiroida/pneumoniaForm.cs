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
    public partial class pneumoniaForm : UserControl
    {
        private string imagepath;
        public pneumoniaForm()
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

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (ConnectionClass.ClientTCP == null)
            {
                MessageBox.Show("Sunteti momentan offline", "Tiroida");
                return;
            }


            if (!string.IsNullOrWhiteSpace(this.imagepath))
            {
                pneumoniaContent data = new pneumoniaContent(this.imagepath);
                string datajson = JsonConvert.SerializeObject(data);
                ConnectionClass.ClientTCP.SendContent(datajson);
                Cursor.Current = Cursors.WaitCursor;
                ConnectionClass.ClientTCP.OnReceivePneumoniaResponse += ClientTCP_OnReceivePneumoniaResponse;


            }
            else
            {
                MessageBox.Show("Va rugam trimiteti o poza valida!");
                return;
            }
        }

        private void ClientTCP_OnReceivePneumoniaResponse(object sender, OnReceivePneumoniaResponseArgs e)
        {
            Cursor.Current = Cursors.Arrow;

            ResponseUserControl response = new ResponseUserControl(ResponseUserControl.PNEUMONIA);
            response.SetCancer(e.pneumoniaChanse);
            response.SetNonCancer(e.nonPneumoniaChanse);

            Console.WriteLine(e.pneumoniaChanse);
            Console.WriteLine(e.nonPneumoniaChanse);



            Panel p = (Panel)this.Parent;
            p.Controls.Clear();
            p.Controls.Add(response);

            
        }
    }
}
