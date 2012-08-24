using MathNumericsStripped;
using RecognitionLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SchoolOfMagic
{
    public class TrainData
    {
        public TrainData(Tuple<Matrix, Matrix> weights, ObservableCollection<Spell> trained_spells)
        {
            this.Weights = weights;
            this.Trained_Spells = trained_spells;
        }

        public ObservableCollection<Spell> Trained_Spells { get; set; }
        public Tuple<Matrix,Matrix> Weights { get; set; }

        public static TrainData FromString(string text)
        {
            string[] lines = text.Split(new string[]{Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            Matrix w1 = Matrix.FromString(lines[0]);
            Matrix w2 = Matrix.FromString(lines[1]);
            ObservableCollection<Spell> trained_Spells = new ObservableCollection<Spell>();

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
                trained_Spells.Add(new Spell(name, lmid));
            }

            TrainData td = new TrainData(new Tuple<Matrix,Matrix>(w1, w2), trained_Spells);
            return td;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Weights.Item1.ToString());
            sb.AppendLine(Weights.Item2.ToString());
            foreach (var spell in Trained_Spells)
            {
                sb.AppendLine(spell.ToString());
            }
            return sb.ToString();
        }
    }
}
