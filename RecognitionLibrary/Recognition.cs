using MathNumericsStripped;
using RecognitionLib;
using RecognitionLib.ANN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecognitionLibrary
{
    public class Recognition
    {
        private ArrayInputData trainData = null;
        private MultilayerNetwork mln;
        private int patternSize = 10000;
        private int HIDDEN_NODES = 10;

        /// <summary>
        /// A general recognition object, hiding a multilayer feed forward network.
        /// </summary>
        /// <param name="patternSize">The size of one input pattern.</param>
        public Recognition(int inputSize, int outputSize)
        {
            this.patternSize = inputSize;
            mln = new MultilayerNetwork(HIDDEN_NODES, patternSize, outputSize);
        }
        
        /// <summary>
        /// load the train data into the library.
        /// </summary>
        /// <param name="td">input and output data in matrix form</param>
        public void loadTrainData(List<ArrayInputData> inputOutputVectors)
        {
            int output_size = inputOutputVectors[0].Value.Height;
            trainData = new ArrayInputData(new DenseMatrix(patternSize, inputOutputVectors.Count), new DenseMatrix(output_size, inputOutputVectors.Count, 0));
            Matrix inputPattern;
            for (int i = 0; i < inputOutputVectors.Count; i++)
            {
                inputPattern = inputOutputVectors[i].Key;
                trainData.Key.SetColumn(i, inputPattern.Column(0));
                trainData.Value.SetColumn(i, inputOutputVectors[i].Value);
            }
        }

        /// <summary>
        /// Clears the training data.
        /// </summary>
        public void clearTrainData()
        {
            trainData = null;
        }

        /// <summary>
        /// Trains the network using backprop.
        /// </summary>
        public void train(int epochs)
        {
            if (trainData == null)
                throw new ArgumentException("No training data available!");

            mln.backpropTraining(trainData, epochs);
        }

        /// <summary>
        /// Resets the network to random values.
        /// </summary>
        public void reset()
        {
            mln.randomize();
        }

        /// <summary>
        /// Recognizes the character in the file of the given path.
        /// </summary>
        /// <param name="p">Path to a file with a character image.</param>
        /// <returns>The character in this image.</returns>
        public Matrix recognise(Matrix inputPattern)
        {
            ArrayInputData aid = new ArrayInputData(inputPattern, null);
            Matrix result = mln.use(aid);

            return result;
        }
    }
}
