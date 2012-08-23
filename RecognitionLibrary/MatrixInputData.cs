using MathNumericsStripped;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecognitionLibrary
{
    public class MatrixInputData
    {
        private String key;

        public String Key
        {
            get { return key; }
            set { key = value; }
        }
        private Matrix value;

        public Matrix Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public MatrixInputData(string key, Matrix value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
