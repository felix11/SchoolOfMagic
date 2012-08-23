using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathNumericsStripped
{
    public class Matrix
    {
        private float[,] _data;

        public Matrix(int height, int width)
        {
            this.Width = width;
            this.Height = height;

            _data = new float[Height,Width];
        }

        public Matrix(int height, int width, float val)
            : this(height, width)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _data[i,j] = val;
                }
            }
        }

        public Matrix(int height, int width, float[] val)
            : this(height, width)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _data[i,j] = val[i*width +j];
                }
            }
        }

        public Matrix()
        {
            // TODO: Complete member initialization
        }

        public float[,] Data
        {
            get { return _data; }
        }

        public int RowCount
        {
            get
            {
                return Height;
            }
        }

        public int ColumnCount
        {
            get
            {
                return Width;
            }
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }


        public Matrix Subtract(Matrix toSubtract)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    _data[i,j] -= toSubtract[i,j];
                }
            }
            return this;
        }

        public Matrix SetSubMatrix(int rowIndex, int rowLength, int colIndex, int colLength, Matrix matrix)
        {
            for (int i = rowIndex; i < rowIndex+rowLength; i++)
            {
                for (int j = colIndex; j < colIndex + colLength; j++)
                {
                    _data[i,j] = matrix[i-rowIndex, j-colIndex];
                }
            }
            return this;
        }

        public Matrix Multiply(Matrix toMultiply)
        {
            Matrix res = new Matrix(this.Height, toMultiply.Width);

            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < toMultiply.Width; j++)
                {
                    res[i, j] = 0.0f;
                    for (int k = 0; k < this.Width; k++)
                    {
                        res[i, j] += this[i, k] * toMultiply[k, j];
                    }
                }
            }
            return res;
        }

        public Matrix Multiply(float val)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    _data[i, j] *= val;
                }
            }
            return this;
        }

        public Matrix Add(Matrix toAdd)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    _data[i,j] += toAdd[i, j];
                }
            }
            return this;
        }

        public Matrix SubMatrix(int rowIndex, int rowLength, int colIndex, int colLength)
        {
            Matrix res = new Matrix(rowLength, colLength);
            
            for (int i = rowIndex; i < rowIndex+rowLength; i++)
            {
                for (int j = colIndex; j < colIndex + colLength; j++)
                {
                    res[i - rowIndex, j - colIndex] += this[i, j];
                }
            }
            return res;
        }

        public Matrix Transpose()
        {
            Matrix res = new Matrix(this.Width, this.Height);

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    res[j,i] = this[i,j];
                }
            }
            return res;
        }

        public static Matrix operator +(Matrix c1, Matrix c2)
        {
            return c1.Add(c2);
        }
        public static Matrix operator -(Matrix c1, Matrix c2)
        {
            return c1.Subtract(c2);
        }

        public float this[int row, int col]
        {
            get { return _data[row,col]; }
            set { _data[row,col] = value; }
        }

        public Matrix Column(int num)
        {
            Matrix res = new Matrix(this.Height,1);

            for (int i = 0; i < Height; i++)
            {
                res[i, 0] = this[i, 0];
            }
            return res;
        }

        public void SetColumn(int num, float val)
        {
            for (int i = 0; i < Height; i++)
            {
                this[i, num] = val;
            }
        }

        public Matrix PointwiseMultiply(Matrix res)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    res[i, j] *= this[i, j];
                }
            }
            return res;
        }

        public Matrix InsertRow(int num, Matrix denseVector)
        {
            Matrix res = new Matrix(this.Height+1, this.Width);
            if (num > 0)
            {
                res.SetSubMatrix(0, num - 1, 0, Width, this.SubMatrix(0, num-1, 0, Width));
                res.SetSubMatrix(num, 1, 0, Width, denseVector);
                res.SetSubMatrix(num + 1, Height - num, 0, Width, this.SubMatrix(num, Height-num, 0, Width));
            }
            else
            {
                res.SetSubMatrix(0, 1, 0, Width, denseVector);
                res.SetSubMatrix(1, Height, 0, Width, this);
            }
            return res;
        }

        public void SetColumn(int num, Matrix vector)
        {
            for (int i = 0; i < Height; i++)
            {
                this[i, num] = vector[i,0];
            }
        }
    }
}
