using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNumericsStripped;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace RecognitionLib
{
    /// <summary>
    /// Helper class for data manipulation and generation.
    /// </summary>
    public class DataManipulation
    {
        private static Random rGen = new Random();

        /// <summary>
        /// Converts a given bitmap into an input pattern of the network.
        /// </summary>
        /// <param name="inputBmp"></param>
        /// <returns>a Row*Col X 1 matrix containing the bitmap pixel</returns>
        public static Matrix List2Pattern(int height, int width, List<Point> list)
        {
            double thickness = 3.0f;
            double sampleLen = 10;
            // create the correct bitmap array
            Matrix resultVec = new DenseMatrix(height * width, 1, 0);
            foreach (var item in list)
            {
                // generate a cirular pattern around each point, with rad = thickness
                for (int phi = 0; phi < sampleLen; phi++)
                {
                    for (int rad = 0; rad < sampleLen; rad++)
                    {
                        int x = (int)(item.X + Math.Cos(phi / sampleLen * 2 * Math.PI) * rad / sampleLen * thickness);
                        int y = (int)(item.Y + Math.Sin(phi / sampleLen * 2 * Math.PI) * rad / sampleLen * thickness);

                        if (x >= 0 && x < width && y >= 0 && y < height)
                        {
                            resultVec[y * width + x, 0] = 1;
                        }
                    }
                }
            }

            return resultVec;
        }

        /// <summary>
        /// creates a random matrix with given size.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <returns>a matrix with uniform random elements from 0 .. 1</returns>
        internal static Matrix rand(int rows, int cols)
        {
            double[] rvals = genRandomDoublex(rows*cols);
            float[] frvals = new float[rvals.Length];
            for(int i=0; i<rvals.Length; i++)
                frvals[i] = (float)rvals[i];

            return new DenseMatrix(rows, cols, frvals);
        }

        private static double[] genRandomDoublex(int amount)
        {
            double[] res = new double[amount];
            for (int i = 0; i < amount; i++)
            {
                res[i] = rGen.NextDouble();
            }
            return res;
        }
    }
}
