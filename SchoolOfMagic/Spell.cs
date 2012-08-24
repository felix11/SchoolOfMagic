using MathNumericsStripped;
using RecognitionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolOfMagic
{
    public class Spell
    {
        public string Name { get; set; }
        public List<MatrixInputData> TrainData { get; set; }

        public Spell(string name, List<MatrixInputData> trainData)
        {
            Name = name;
            TrainData = new List<MatrixInputData>();
            foreach (var item in trainData)
            {
                TrainData.Add(item);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(Name);
            foreach (var item in TrainData)
            {
                sb.AppendLine("#" + item.Value.ToString());
            }
            return sb.ToString();
        }

        public static Spell FromString(string str)
        {
            string[] lines = str.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string name = lines[0];
            List<MatrixInputData> trainData = new List<MatrixInputData>();

            // generate spell
            for (int i = 1; i < lines.Length; i++)
            {
                string[] mid = lines[i].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                string mid_name = mid[0];
                Matrix mat = Matrix.FromString(mid[1]);
                trainData.Add(new MatrixInputData(mid_name, mat));
            }

            return new Spell(name, trainData);
        }
    }
}
