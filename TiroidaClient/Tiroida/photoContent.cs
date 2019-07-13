using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiroida
{
    class photoContent
    {
        public photoContent()
        {
            this.action = "pneumonia";
        }


        public photoContent(string imagePath)
        {
            this.action = "pneumonia";
            byte[] filebytes = File.ReadAllBytes(imagePath);
            string imagebase64 = Convert.ToBase64String(filebytes);
            this.imageContent = imagebase64;
        }


        public photoContent(string action, string imagePath)
        {
            this.action = action;
            byte[] filebytes = File.ReadAllBytes(imagePath);
            string imagebase64 = Convert.ToBase64String(filebytes);
            this.imageContent = imagebase64;
        }


        public void setImageString(string imageContent)
        {
            this.imageContent = imageContent;
        }

        public string action { get; set; }
        public string imageContent { get; set; }



    }
}
