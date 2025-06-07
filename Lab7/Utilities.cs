using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagonalMatrixLab
{
    public static class Utilities
    {
        /// <summary>
        /// Проверяет корректность двоичной строки
        /// </summary>
        public static bool IsValidBinaryString(string binary)
        {
            return !string.IsNullOrEmpty(binary) && binary.All(c => c == '0' || c == '1');
        }

        /// <summary>
        /// Генерирует случайную двоичную строку заданной длины
        /// </summary>
        public static string GenerateRandomBinaryString(int length)
        {
            var random = new Random();
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = random.Next(2) == 0 ? '0' : '1';
            }

            return new string(result);
        }

        /// <summary>
        /// Создает тестовую матрицу со случайными данными
        /// </summary>
        public static void FillMatrixWithRandomData(DiagonalMatrix matrix)
        {
            var random = new Random();

            for (int i = 0; i < 16; i++)
            {
                string randomWord = GenerateRandomBinaryString(16);
                matrix.WriteWord(i, randomWord);
            }
        }

        /// <summary>
        /// Экспортирует матрицу в текстовый формат
        /// </summary>
        public static string ExportMatrixToString(DiagonalMatrix matrix)
        {
            var result = new System.Text.StringBuilder();
            result.AppendLine("Экспорт матрицы:");
            result.AppendLine("================");

            // Экспорт в виде матрицы
            result.AppendLine("Матрица (построчно):");
            var matrixData = matrix.GetMatrix();
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    result.Append($"{matrixData[i, j]} ");
                }
                result.AppendLine();
            }

            result.AppendLine();
            result.AppendLine("Слова (по диагональной адресации):");
            for (int i = 0; i < 16; i++)
            {
                string word = matrix.ReadWord(i);
                int decimalValue = DiagonalMatrix.BinaryToDecimal(word);
                result.AppendLine($"Слово {i,2}: {word} = {decimalValue,5}");
            }

            return result.ToString();
        }

        /// <summary>
        /// Сравнивает две матрицы
        /// </summary>
        public static bool CompareMatrices(DiagonalMatrix matrix1, DiagonalMatrix matrix2)
        {
            for (int i = 0; i < 16; i++)
            {
                if (matrix1.ReadWord(i) != matrix2.ReadWord(i))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Создает резервную копию матрицы
        /// </summary>
        public static DiagonalMatrix BackupMatrix(DiagonalMatrix original)
        {
            var backup = new DiagonalMatrix();

            for (int i = 0; i < 16; i++)
            {
                string word = original.ReadWord(i);
                backup.WriteWord(i, word);
            }

            return backup;
        }

        /// <summary>
        /// Восстанавливает матрицу из резервной копии
        /// </summary>
        public static void RestoreMatrix(DiagonalMatrix target, DiagonalMatrix backup)
        {
            for (int i = 0; i < 16; i++)
            {
                string word = backup.ReadWord(i);
                target.WriteWord(i, word);
            }
        }

        /// <summary>
        /// Валидация индекса (0-15)
        /// </summary>
        public static bool IsValidIndex(int index)
        {
            return index >= 0 && index < 16;
        }

        /// <summary>
        /// Получение индекса с проверкой
        /// </summary>
        public static bool TryGetValidIndex(string input, out int index)
        {
            if (int.TryParse(input, out index))
            {
                return IsValidIndex(index);
            }
            return false;
        }

        /// <summary>
        /// Форматирование двоичной строки для удобного чтения
        /// </summary>
        public static string FormatBinaryString(string binary, int groupSize = 4)
        {
            if (string.IsNullOrEmpty(binary))
                return string.Empty;

            var result = new System.Text.StringBuilder();
            for (int i = 0; i < binary.Length; i++)
            {
                if (i > 0 && i % groupSize == 0)
                    result.Append(' ');
                result.Append(binary[i]);
            }
            return result.ToString();
        }

        /// <summary>
        /// Получение всех возможных трехбитных ключей
        /// </summary>
        public static List<string> GetAllThreeBitKeys()
        {
            var keys = new List<string>();
            for (int i = 0; i < 8; i++)
            {
                keys.Add(DiagonalMatrix.DecimalToBinary(i, 3));
            }
            return keys;
        }

        /// <summary>
        /// Статистика использования ключей V в матрице
        /// </summary>
        public static Dictionary<string, List<int>> GetKeyStatistics(DiagonalMatrix matrix)
        {
            var statistics = new Dictionary<string, List<int>>();

            for (int i = 0; i < 16; i++)
            {
                string word = matrix.ReadWord(i);
                string key = word.Substring(0, 3);

                if (!statistics.ContainsKey(key))
                    statistics[key] = new List<int>();

                statistics[key].Add(i);
            }

            return statistics;
        }

        /// <summary>
        /// Вывод статистики ключей
        /// </summary>
        public static void PrintKeyStatistics(DiagonalMatrix matrix)
        {
            var statistics = GetKeyStatistics(matrix);

            Console.WriteLine("Статистика использования ключей V:");
            Console.WriteLine("Ключ\tКоличество\tИндексы слов");
            Console.WriteLine("----------------------------------------");

            foreach (var kvp in statistics.OrderBy(x => x.Key))
            {
                string indices = string.Join(", ", kvp.Value);
                Console.WriteLine($"{kvp.Key}\t{kvp.Value.Count,8}\t{indices}");
            }
        }

        /// <summary>
        /// Проверка корректности структуры слова (V:3 + A:4 + B:4 + S:5 = 16)
        /// </summary>
        public static bool ValidateWordStructure(string word)
        {
            if (word.Length != 16)
                return false;

            return IsValidBinaryString(word);
        }

        /// <summary>
        /// Создание слова из компонентов
        /// </summary>
        public static string CreateWord(string v, string a, string b, string s)
        {
            if (v.Length != 3 || a.Length != 4 || b.Length != 4 || s.Length != 5)
                throw new ArgumentException("Неверная длина компонентов слова");

            if (!IsValidBinaryString(v + a + b + s))
                throw new ArgumentException("Компоненты должны содержать только 0 и 1");

            return v + a + b + s;
        }

        /// <summary>
        /// Разбор слова на компоненты
        /// </summary>
        public static (string V, string A, string B, string S) ParseWord(string word)
        {
            if (!ValidateWordStructure(word))
                throw new ArgumentException("Некорректная структура слова");

            return (
                V: word.Substring(0, 3),
                A: word.Substring(3, 4),
                B: word.Substring(7, 4),
                S: word.Substring(11, 5)
            );
        }
    }
}