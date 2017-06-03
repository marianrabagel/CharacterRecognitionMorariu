﻿using System;
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

        PrincipalComponentAnalysis pca;
        ActivationNetwork network;

        int headerSize = 1078;
        //20x32
        public InputChars(int width, int height)
        {
            Width = width;
            Height = height;
            bmpHeader = new byte[headerSize];
            int n = 30;
            charPixelMatrix = new double[n][][];

            for (int i = 0; i < n; i++)
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
            if (ct < charPixelMatrix.Length)
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
            double[][] pcaOutput = ApplyPca();
            ApplyBackpropagation(pcaOutput);
        }

        private void ApplyBackpropagation(double[][] inputGroup)
        {
            int networkInputs = inputGroup.Length;
            network = new ActivationNetwork(
                new SigmoidFunction(2),
                networkInputs,
                4,
                10
                );

            BackPropagationLearning teacher = new BackPropagationLearning(network);
            bool needToStop = false;
            double[][] output = new double[inputGroup.Length][];

            for (int i = 0; i < 30; i++)
            {
                output[i] = new double[10];

                for (int j = 0; j < 10; j++)
                {
                    output[i][j] = 0;
                }

                output[i][i/3] = 1;
            }
            int nrEpoci = 0;

            while (!needToStop)
            {
                nrEpoci++;
                double[][] input = inputGroup;
                double error = teacher.RunEpoch(input, output);
                if (error < 0.001 || nrEpoci == 10000)
                    needToStop = true;
            }
            Console.WriteLine(nrEpoci);
        }

        public double[][] ApplyPca()
        {
            pca = new PrincipalComponentAnalysis()
            {
                Method = PrincipalComponentMethod.Center,
                Whiten = true
            };
            double[][] data = GetDataForPca(30);
            MultivariateLinearRegression transform = pca.Learn(data);
            double[][] output = pca.Transform(data);
            return output;
        }

        private double[][] GetDataForPca(int numberOfImages)
        {
            double[][] data = new double[numberOfImages][];

            for (int index = 0; index < numberOfImages; index++)
            {
                data[index] = GetImageAsAnArrayFromCharPixelMatrix(index);
            }

            return data;
        }

        private double[] GetImageAsAnArrayFromCharPixelMatrix(int index)
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

        public double[] Evaluate(string fileName)
        {
            double[][] image = LoadImage(fileName);
            double[][] matrixWithOneImageAsAnArray = new double[1][];
            double[] imageAsAnArray = GetImageAsAnArray(image);
            matrixWithOneImageAsAnArray[0] = imageAsAnArray;
            double[][] pcaImage = pca.Transform(matrixWithOneImageAsAnArray);

            double[] outputVector = network.Compute(pcaImage[0]);

            return outputVector;
        }

        private double[] GetImageAsAnArray(double[][] image)
        {
            double[] imageAsAnArray = new double[image.GetLength(0) * image[0].Length];
            int i = 0;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    imageAsAnArray[i++] = image[y][x];
                }
            }

            return imageAsAnArray;
        }


        private static double[][] LoadImage(string fileName)
        {
            double[][] image;
            using (Bitmap bitmap = new Bitmap(fileName))
            {
                image = new double[bitmap.Height][];
                for (int i = 0; i < bitmap.Height; i++)
                {
                    image[i] = new double[bitmap.Width];
                }

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        var pixel = bitmap.GetPixel(x, y);
                        int value = 1;

                        if (pixel.R == 0xFF && pixel.G == 0xFF && pixel.B == 0xFF)
                            value = 0;
                        image[y][x] = value;
                    }
                }
            }
            return image;
        }
    }
}
