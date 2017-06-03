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

        private void LoadAllImagesButton_Click(object sender, EventArgs e)
        {
            int numberOfChars = 10;
            int numberOfSamples = 3;
            string[] files = new string[numberOfChars* numberOfSamples];
            int ct = 0;

            for (int i = 0; i < numberOfChars; i++)
            {
                for (int j = 0; j < numberOfSamples; j++)
                {
                    files[ct++] = @"Chars_20x32\" + i + "_" + j + ".bmp";
                }
            }

            inputChars.AddImage(files);
        }

        private void ApplyPcaButton_Click(object sender, EventArgs e)
        {
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
                double max = double.MinValue;
                int indexOfmax = -1;

                for (int i = 0; i < output.Length; i++)
                {
                    if (output[i] > max)
                    {
                        max = output[i];
                        indexOfmax = i;
                    }
                }

                label1.Text = indexOfmax.ToString();
            }
        }
    }
}
