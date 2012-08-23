using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathNumericsStripped
{
    public class DenseMatrix : Matrix
    {
        public DenseMatrix(int width, int height) : base (width, height)
        {
        }
        public DenseMatrix(int width, int height, float val)
            : base(width, height, val)
        {
        }
        public DenseMatrix(int width, int height, float[] val)
            : base(width, height, val)
        {
        }
    }
}
