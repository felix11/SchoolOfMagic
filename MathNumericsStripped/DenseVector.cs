using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathNumericsStripped
{
    public class DenseVector : DenseMatrix
    {
        public DenseVector(int height) : base (height,1)
        {
        }
        public DenseVector(int height, float val)
            : base(height,1, val)
        {
        }
        public DenseVector(int height, float[] val)
            : base(height,1, val)
        {
        }
    }
}
