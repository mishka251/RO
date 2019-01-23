using System;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace RO1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            detector = new MyDetector();
            capture = new VideoCapture(CaptureType.Any);
            timer1.Start();
        }

        MyDetector detector;
        VideoCapture capture;

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();

            var image = capture.QueryFrame().ToImage<Rgb, byte>();

            pictureBox1.Image = image.Bitmap;
            try
            {
                detector.ProcessImage(image);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            pictureBox1.Image = image.ToBitmap();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            detector.Save();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();

            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK) return;

            Image<Rgb, byte> image = new Image<Rgb, byte>(ofd.FileName);

            image = image.Resize(pictureBox1.Width, pictureBox1.Height, Inter.Linear);

            pictureBox1.Image = image.Bitmap;
            try
            {
                detector.ProcessImage(image);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            pictureBox1.Image = image.ToBitmap();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox2.Image = capture.QueryFrame().ToImage<Rgb, byte>().Bitmap;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            detector.LoadSamples();
        }


    }
}

