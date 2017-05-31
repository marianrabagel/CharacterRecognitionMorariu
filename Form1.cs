using System;
using System.IO;
using System.Windows.Forms;

namespace CharacterRecognitionMorariu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        InputChars inputChars = new InputChars(20, 32);

        private void LoadFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(openFileDialog1.FileName).ToUpper() != ".BMP")
                {
                    MessageBox.Show("introduceti un fisier bmp");
                    return;
                }

                inputChars.AddImage(openFileDialog1.FileName);
                var bitmap = inputChars.GetBitmap();
                TestImagePanel.BackgroundImage = bitmap;
            }
        }

        private void LoadAllImagesButton_Click(object sender, EventArgs e)
        {
            /*sstring projectPath = @"C:\Users\Marian\Documents\Visual Studio 2015\Projects\CharacterRecognitionMorariu\CharacterRecognitionMorariu";
            string path = projectPath + @"\bin\Debug\Chars_20x32\0.bmp";*/
            string[] files = new string[10];

            for (int i = 0; i < 10; i++)
            {
                files[i] = @"Chars_20x32\" + i + ".bmp";
            }

            inputChars.AddImage(files);
        }

        private void ApplyPcaButton_Click(object sender, EventArgs e)
        {
            //invata
            inputChars.DoWork();
        }

        private void loadTestImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(openFileDialog1.FileName).ToUpper() != ".BMP")
                {
                    MessageBox.Show("alegeti bmp");
                    return;
                }

                double[] output = inputChars.Evaluate(openFileDialog1.FileName);
                label1.Text = "";
                foreach (double val in output)
                {
                    //max in loc de round
                    label1.Text += Math.Round(val).ToString() + "";
                }
            }
        }
    }
}
