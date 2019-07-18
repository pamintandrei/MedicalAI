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

namespace Tiroida
{
    public partial class adminpanel : UserControl
    {
        public adminpanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        private void RemoveCookie()
        {

            cookieobj cookie = new cookieobj("");
            string cookiesave = JsonConvert.SerializeObject(cookie);
            Console.WriteLine(cookiesave);
            System.IO.StreamWriter writer = new System.IO.StreamWriter(@"cookie.json", false);
            writer.Write(cookiesave);
            writer.Close();
        }



        private void pictureBox2_Click(object sender, EventArgs e)
        {
            RemoveCookie();



            Login lg = new Login();
            Panel panel1 = (Panel)this.Parent;
            panel1.Controls.Clear();
            panel1.Controls.Add(lg);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            PersonalDataForm dataform = new PersonalDataForm(false);
            Panel p1 = (Panel)this.Parent;
            p1.Controls.Clear();
            p1.Controls.Add(dataform);
            
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            viewmedics form = new viewmedics();
            Panel p1 = (Panel)this.Parent;
            p1.Controls.Clear();
            p1.Controls.Add(form);
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
