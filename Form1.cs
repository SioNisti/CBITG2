using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Tesseract;

namespace CBITG2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); 
            this.notifyIcon1.Text = "M1 to get text (eng,fin,jpn)\nM2 to open menu";
        }
        List<string> historyL = new List<string>();

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            if (!File.Exists("history.txt"))
            {
                File.Create("history.txt"); 
                Application.Restart();
                Environment.Exit(0);
            }
            historyL = System.IO.File.ReadLines("history.txt").ToList();
        }

        public void Recognize(string lang)
        {
            try
            {
                Bitmap bmp = new Bitmap(Clipboard.GetImage());
                Pix img = PixConverter.ToPix(bmp);
                TesseractEngine engine = new TesseractEngine("./tessdata", lang, EngineMode.LstmOnly);
                Page page = engine.Process(img, PageSegMode.Auto);
                Clipboard.SetText(page.GetText());
                label1.Text = page.GetText();
                historyL.Add($"({DateTime.Now} [{lang}]){page.GetText()}");
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
                label1.Text = e.ToString();
                historyL.Add($"({DateTime.Now} [{lang}]){e.ToString()}");
            }
            System.IO.File.WriteAllLines("history.txt", historyL);
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Recognize("jpn+eng+fin");
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Recognize("jpn");
        }

        private void fIToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Recognize("fin");
        }

        private void eNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Recognize("eng");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
