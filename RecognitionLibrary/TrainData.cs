using MathNumericsStripped;
using RecognitionLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RecognitionLib
{
    public class TrainData
    {
        public TrainData(Tuple<Matrix, Matrix> neural_net, ObservableCollection<TrainSet> trained_spells)
        {
            this.NeuralNet = neural_net;
            this.Trained_Sets = trained_spells;
        }

        public ObservableCollection<TrainSet> Trained_Sets { get; set; }
        public Tuple<Matrix,Matrix> NeuralNet { get; set; }

        public static TrainData FromString(string text)
        {
            string[] lines = text.Split(new string[]{Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            Matrix w1 = Matrix.FromString(lines[0]);
            Matrix w2 = Matrix.FromString(lines[1]);
            ObservableCollection<TrainSet> trained_sets = new ObservableCollection<TrainSet>();

            for (int i = 2; i < lines.Length-1; i++)
            {
                string name = lines[i];
                i++;
                List<MatrixInputData> lmid = new List<MatrixInputData>();
                while (i < lines.Length && lines[i].StartsWith("#"))
                {
                    Matrix mat = Matrix.FromString(lines[i].TrimStart('#'));
                    lmid.Add(new MatrixInputData(name, mat));
                    i++;
                }
                i--;
                trained_sets.Add(new TrainSet(name, lmid));
            }

            TrainData td = new TrainData(new Tuple<Matrix,Matrix>(w1, w2), trained_sets);
            return td;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(NeuralNet.Item1.ToString());
            sb.AppendLine(NeuralNet.Item2.ToString());
            foreach (var spell in Trained_Sets)
            {
                sb.AppendLine(spell.ToString());
            }
            return sb.ToString();
        }
    }
}
