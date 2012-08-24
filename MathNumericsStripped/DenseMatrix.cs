using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathNumericsStripped
{
    public class DenseMatrix : Matrix
    {
        public DenseMatrix(int height, int width)
            : base(height, width)
        {
        }
        public DenseMatrix(int height, int width, float val)
            : base(height, width, val)
        {
        }
        public DenseMatrix(int height, int width, float[] val)
            : base(height, width, val)
        {
        }
    }
}
