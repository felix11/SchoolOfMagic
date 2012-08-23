using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNumericsStripped;

namespace RecognitionLib
{
    public class ArrayInputData
    {
        private Matrix key;

        public Matrix Key
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

        public ArrayInputData(Matrix key, Matrix value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
