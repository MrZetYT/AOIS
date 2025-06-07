using System;
using System.Text;

namespace DiagonalMatrixLab
{
    public class DiagonalMatrix
    {
        private int[,] matrix;
        private const int SIZE = 16;

        public DiagonalMatrix()
        {
            matrix = new int[SIZE, SIZE];
        }

        public int GetBit(int row, int col)
        {
            if (row < 0 || row >= SIZE || col < 0 || col >= SIZE)
                return 0;
            return matrix[row, col];
        }

        public void SetBit(int row, int col, int value)
        {
            if (row >= 0 && row < SIZE && col >= 0 && col < SIZE)
                matrix[row, col] = value;
        }

        /// <summary>
        /// Считывает слово по диагональной адресации
        /// Слово j читается из позиций: (0,j), (1,(j+SIZE-1)%SIZE), (2,(j+SIZE-2)%SIZE), ..., (SIZE-1,(j+1)%SIZE)
        /// </summary>
        public string ReadWord(int wordIndex)
        {
            if (wordIndex < 0 || wordIndex >= SIZE)
                throw new ArgumentOutOfRangeException(nameof(wordIndex));

            var word = new StringBuilder(SIZE);

            for (int bit = 0; bit < SIZE; bit++)
            {
                int row = bit;
                int col = (wordIndex + SIZE - bit) % SIZE;
                word.Append(matrix[row, col]);
            }

            return word.ToString();
        }

        /// <summary>
        /// Записывает слово по диагональной адресации
        /// </summary>
        public void WriteWord(int wordIndex, string word)
        {
            if (wordIndex < 0 || wordIndex >= SIZE)
                throw new ArgumentOutOfRangeException(nameof(wordIndex));

            if (word.Length != SIZE)
                throw new ArgumentException($"Слово должно содержать {SIZE} бит");

            for (int bit = 0; bit < SIZE; bit++)
            {
                int row = bit;
                int col = (wordIndex + SIZE - bit) % SIZE;
                matrix[row, col] = word[bit] - '0';
            }
        }

        /// <summary>
        /// Считывает адресный столбец
        /// </summary>
        public string ReadColumn(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= SIZE)
                throw new ArgumentOutOfRangeException(nameof(columnIndex));

            var column = new StringBuilder(SIZE);

            for (int row = 0; row < SIZE; row++)
            {
                column.Append(matrix[row, columnIndex]);
            }

            return column.ToString();
        }

        /// <summary>
        /// Записывает адресный столбец
        /// </summary>
        public void WriteColumn(int columnIndex, string column)
        {
            if (columnIndex < 0 || columnIndex >= SIZE)
                throw new ArgumentOutOfRangeException(nameof(columnIndex));

            if (column.Length != SIZE)
                throw new ArgumentException($"Столбец должен содержать {SIZE} бит");

            for (int row = 0; row < SIZE; row++)
            {
                matrix[row, columnIndex] = column[row] - '0';
            }
        }

        /// <summary>
        /// Преобразует двоичную строку в десятичное число
        /// </summary>
        public static int BinaryToDecimal(string binary)
        {
            return Convert.ToInt32(binary, 2);
        }

        /// <summary>
        /// Преобразует десятичное число в двоичную строку заданной длины
        /// </summary>
        public static string DecimalToBinary(int value, int length)
        {
            string binary = Convert.ToString(value, 2);
            return binary.PadLeft(length, '0');
        }

        /// <summary>
        /// Инициализирует матрицу тестовыми данными из примера
        /// </summary>
        public void InitializeTestData()
        {
            int[,] testData = {
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0},
                {1,1,0,1,1,0,0,0,1,1,1,1,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,1,1,0,1,0,0,0,0,0,0,1,0,0,0,0},
                {0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,1,0,1,1,0,0,0,0,0,1,1,0,0,0},
                {0,0,0,0,1,1,0,0,0,0,0,0,1,0,0,0},
                {0,0,0,0,0,1,1,0,1,0,1,0,1,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1},
                {0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
                {0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
                {0,1,0,1,0,1,0,1,0,1,0,1,0,1,1,1},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
            };

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    matrix[i, j] = testData[i, j];
                }
            }
        }

        /// <summary>
        /// Выводит матрицу на экран
        /// </summary>
        public void PrintMatrix()
        {
            Console.WriteLine();
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    Console.Write($"{matrix[i, j],2} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Получает значение матрицы в виде двумерного массива (для внутреннего использования)
        /// </summary>
        public int[,] GetMatrix()
        {
            return (int[,])matrix.Clone();
        }
    }
}