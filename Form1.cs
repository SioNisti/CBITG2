using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Tesseract;

namespace CBITG2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); 
            this.notifyIcon1.Text = "M1 to get text\nM2 to close the program";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
        }

        public void Recognize()
        {
            try
            {
                Bitmap bmp = new Bitmap(Clipboard.GetImage());
                Pix img = PixConverter.ToPix(bmp);
                TesseractEngine engine = new TesseractEngine("./tessdata", "jpn+eng+fin", EngineMode.LstmOnly);
                Page page = engine.Process(img, PageSegMode.Auto);
                Clipboard.SetText(page.GetText());
                label1.Text = page.GetText();
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Recognize();
            } else
            {
                this.Close();
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
