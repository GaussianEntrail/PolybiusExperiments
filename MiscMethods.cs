using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherThing
{
    public static class MiscMethods
    {
        //random number generation
        static long last = DateTime.Now.Ticks | 1;
        static long inc = DateTime.Now.Ticks;

        public static int Random(int max)
        {
            last ^= last << 21;
            last ^= (long)((ulong)last) >> 35;
            last ^= last << 4;
            inc += 123456789123456789L;
            int n = (int)((last + inc) % max);
            return (n < 0) ? -n : n;
        }

        //Modular arithmetic
        public static int Mod(int amount, int shift, int max) => ((amount + shift) + max) % max;

        public static Tuple<int, int> CoordinatesOf<T>(this T[,] matrix, T value)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (matrix[x, y].Equals(value)) { return Tuple.Create(x, y); } 
                    
                }
            }

            return Tuple.Create(-1, -1);
        }

        public static void Shuffle<T>(this T[] array) { 
            int n = array.Length, k;
            while (n > 1)
            {
                k = Random(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        public static void MatrixShuffle<T>(this T[,] matrix) {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);
            int n = w * h, nx, ny, k, kx, ky;
            while (n > 1)
            {
                ny = ((int) n / w) % h;
                nx = n % w;
                
                k = Random(n--);
                
                ky = ((int) k / w) % h;
                kx = k % w;
                
                T temp = matrix[nx, ny];
                matrix[nx, ny] = matrix[kx, ky];
                matrix[kx, ky] = temp;
            }
        }
        
        public static void ArrayShift<T>(this T[] array, int shift)
        {
            int L = array.Length;
            T[] temp = new T[L];

            for (int k = 0; k < L; k++)
            {
                temp[Mod(k, shift, L)] = array[k];
            }

            for (int k = 0; k < L; k++)
            {
                array[k] = temp[k];
            }
        }

        public static void MatrixRowShift<T>(this T[,] matrix, int index, int amount, bool isRow)
        {
            //shifts a row (or column) n in the square by shift amount
            int L = (isRow ? matrix.GetLength(0) : matrix.GetLength(1));
            int n = index % L;
            int shift = amount % L;
            int k;

            T[] temp = new T[L];
                
            if (isRow)
            {
                for (k = 0; k < L; k++)
                {
                    temp[k] = matrix[n, Mod(k, -shift, L)];
                }


                for (k = 0; k < L; k++)
                {
                    matrix[n, k] = temp[k];
                }
            }
            else
            {
                for (k = 0; k < L; k++)
                {
                    temp[k] = matrix[ Mod(k, -shift, L), n];
                }

                for (k = 0; k < L; k++)
                {
                    matrix[k, n] = temp[k];
                }
            }

        }

        public static string ToMatrixString<T>(this T[,] matrix, string delimiter = "\t")
        {
            var s = new StringBuilder();

            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    s.Append(matrix[i, j]).Append(delimiter);
                }

                s.AppendLine();
            }

            return s.ToString();
        }

        public static T[,] MakeDuplicate<T>(this T[,] matrix)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);
            T[,] copy = new T[w, h];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    copy[i, j] = matrix[i, j];
                }
            }


            return copy;
        }


    }
}
