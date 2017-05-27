using System;
using System.Drawing;
using System.IO;
using Accord;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Statistics.Analysis;
using Accord.Statistics.Models.Regression.Linear;

namespace CharacterRecognitionMorariu
{
    public class InputChars
    {
        private int Width;
        private int Height;
        private byte[] bmpHeader;
        protected double[][][] charPixelMatrix;
        private int ct = 0;

        int headerSize = 1078;
        //20x32
        public InputChars(int width, int height)
        {
            Width = width;
            Height = height;
            bmpHeader = new byte[headerSize];
            charPixelMatrix = new double[10][][];

            for (int i = 0; i < 10; i++)
            { 
                charPixelMatrix[i] = new double[Height][];

                for (int j = 0; j < Height; j++)
                {
                    charPixelMatrix[i][j] = new double[width];
                }
            }
        }

        public void AddImage(string inputFileName)
        {
            if (ct < 10)
            {
                using (Bitmap bitmap = new Bitmap(inputFileName))
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            var pixel = bitmap.GetPixel(x, y);
                            int value = 1;

                            if (pixel.R == 0xFF && pixel.G == 0xFF && pixel.B == 0xFF)
                                value = 0;

                            charPixelMatrix[ct][y][x] = value;
                        }
                    }
                }

                ct++;
            }
        }

        private void ReadBmpHeader(FileStream reader)
        {
            for (int i = 0; i < headerSize; i++)
            {
                bmpHeader[i] = (byte) reader.ReadByte();
            }
        }

        public Bitmap GetBitmap(int index = 0)
        {
            double[][] matrix = charPixelMatrix[index];
            Bitmap bitmap = new Bitmap(matrix.GetLength(1), matrix.GetLength(0));

            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                for (int x = 0; x < matrix.GetLength(1); x++)
                {
                    int value = Convert.ToInt32(matrix[y][x]);

                    if (value < 0)
                        value = 0;
                    if (value == 1)
                        value = 255;

                    Color color = Color.FromArgb(255, value, value, value);
                    bitmap.SetPixel(x, y, color);
                }
            }

            return bitmap;
        }

        public void WriteToFile(int index = 0)
        {
            using (StreamWriter writer = new StreamWriter("test.txt"))
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        var output = charPixelMatrix[index][y][x] + " ";
                        writer.Write(output);
                    }
                    writer.WriteLine();
                }
            }
        }
        
        public void AddImage(string[] inputFileName)
        {
            foreach (string fileName in inputFileName)
            {
                AddImage(fileName);
            }
        }

        public void DoWork()
        {
            double[][][] pcaOutput = new double[10][][];
            
            for (int index = 0; index < 10; index++)
            {
                 pcaOutput[index] = ApplyPca(index);
            }

            ApplyBackpropagation(pcaOutput);
        }

        private void ApplyBackpropagation(double[][][] inputGroup)
        {
           int networkInputs = inputGroup[0].GetLength(0);
            ActivationNetwork network = new ActivationNetwork(
               new SigmoidFunction(2),
               networkInputs,
               40,
               40
               );

            BackPropagationLearning teacher = new BackPropagationLearning(network);
            bool needToStop = false;

            for (int index = 0; index < 10; index++)
            {
                double[][] output = new double[10][];
                for (int i = 0; i < 10; i++)
                    output[i] = new double[] { index };

                while (!needToStop)
                {
                    double[][] input = inputGroup[index];
                    double error = teacher.RunEpoch(input, output);
                    if (error < 0.5)
                        needToStop = true;
                }
            }
        }

        public double[][] ApplyPca(int index)
        {
            var pca = new PrincipalComponentAnalysis()
            {
                Method = PrincipalComponentMethod.Center,
                Whiten = true
            };
            double[][] data = charPixelMatrix[index];
            //GetDataForPca(1);
            MultivariateLinearRegression transform = pca.Learn(data);
            double[][] output = pca.Transform(data);

            return output;
        }

        private double[][] GetDataForPca(int numberOfImages = 10)
        {
            double[][] data = new double[numberOfImages][];
            for (int index = 0; index < numberOfImages; index++)
            {
                data[index] = GetImageAsAnArray(index);
            }

            return data;
        }

        private double[] GetImageAsAnArray(int index)
        {
            double[] imageAsAnArray = new double[Height * Width];
            int i = 0;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    imageAsAnArray[i++] = charPixelMatrix[index][y][x];
                }
            }
            return imageAsAnArray;
        }
    }
}
