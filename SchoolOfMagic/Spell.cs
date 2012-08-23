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
    }
}
