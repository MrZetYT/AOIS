using System;
using System.Collections.Generic;

namespace DiagonalMatrixLab
{
    public class ArithmeticProcessor
    {
        public class ArithmeticResult
        {
            public int WordIndex { get; set; }
            public string OriginalWord { get; set; }
            public string KeyV { get; set; }
            public string FieldA { get; set; }
            public string FieldB { get; set; }
            public string OriginalS { get; set; }
            public int Sum { get; set; }
            public string NewS { get; set; }
            public string ModifiedWord { get; set; }
        }

        /// <summary>
        /// Сложение полей Aj и Bj в словах Sj, у которых Vj совпадает с заданным V
        /// Структура слова: V(3 бита) + A(4 бита) + B(4 бита) + S(5 бит) = 16 бит
        /// </summary>
        public List<ArithmeticResult> AddFieldsWithKey(DiagonalMatrix matrix, string keyV)
        {
            if (keyV.Length != 3)
                throw new ArgumentException("Ключ V должен содержать ровно 3 бита");

            var results = new List<ArithmeticResult>();

            Console.WriteLine($"Поиск слов с ключом V = {keyV}:");
            Console.WriteLine();

            for (int i = 0; i < 16; i++)
            {
                string word = matrix.ReadWord(i);

                // Извлекаем поля из слова
                string wordV = word.Substring(0, 3);    // V: биты 0-2
                string fieldA = word.Substring(3, 4);   // A: биты 3-6
                string fieldB = word.Substring(7, 4);   // B: биты 7-10
                string fieldS = word.Substring(11, 5);  // S: биты 11-15

                Console.WriteLine($"Слово {i}: {word}");
                Console.WriteLine($"  V={wordV}, A={fieldA}, B={fieldB}, S={fieldS}");

                // Проверяем совпадение ключа
                if (wordV == keyV)
                {
                    Console.WriteLine($"  ✓ Ключ совпадает!");

                    // Преобразуем A и B в десятичные числа
                    int valueA = DiagonalMatrix.BinaryToDecimal(fieldA);
                    int valueB = DiagonalMatrix.BinaryToDecimal(fieldB);
                    int sum = valueA + valueB;

                    Console.WriteLine($"  A={valueA}, B={valueB}, A+B={sum}");

                    // Преобразуем сумму обратно в двоичное представление (5 бит)
                    string newS = DiagonalMatrix.DecimalToBinary(sum, 5);

                    // Если сумма больше 31 (максимум для 5 бит), берем младшие 5 бит
                    if (sum > 31)
                    {
                        sum = sum & 0x1F; // Маска для 5 младших бит
                        newS = DiagonalMatrix.DecimalToBinary(sum, 5);
                        Console.WriteLine($"  Переполнение! Используем младшие 5 бит: {sum}");
                    }

                    // Формируем новое слово
                    string modifiedWord = wordV + fieldA + fieldB + newS;

                    Console.WriteLine($"  Новое S={newS}");
                    Console.WriteLine($"  Модифицированное слово: {modifiedWord}");

                    // Записываем изменения в матрицу
                    matrix.WriteWord(i, modifiedWord);

                    var result = new ArithmeticResult
                    {
                        WordIndex = i,
                        OriginalWord = word,
                        KeyV = wordV,
                        FieldA = fieldA,
                        FieldB = fieldB,
                        OriginalS = fieldS,
                        Sum = sum,
                        NewS = newS,
                        ModifiedWord = modifiedWord
                    };

                    results.Add(result);
                }
                else
                {
                    Console.WriteLine($"  ✗ Ключ не совпадает");
                }
                Console.WriteLine();
            }

            return results;
        }

        /// <summary>
        /// Выполняет арифметические операции над отдельными полями
        /// </summary>
        public void PerformFieldOperations(DiagonalMatrix matrix, int wordIndex,
            Func<int, int, int> operation, string operationName)
        {
            string word = matrix.ReadWord(wordIndex);

            // Извлекаем поля
            string fieldA = word.Substring(3, 4);
            string fieldB = word.Substring(7, 4);

            int valueA = DiagonalMatrix.BinaryToDecimal(fieldA);
            int valueB = DiagonalMatrix.BinaryToDecimal(fieldB);

            int result = operation(valueA, valueB);

            Console.WriteLine($"Операция {operationName}:");
            Console.WriteLine($"A = {fieldA} ({valueA})");
            Console.WriteLine($"B = {fieldB} ({valueB})");
            Console.WriteLine($"Результат = {result}");

            // Записываем результат в поле S
            string newS = DiagonalMatrix.DecimalToBinary(result & 0x1F, 5); // 5 бит
            string modifiedWord = word.Substring(0, 11) + newS;

            matrix.WriteWord(wordIndex, modifiedWord);

            Console.WriteLine($"Новое слово: {modifiedWord}");
        }

        /// <summary>
        /// Сложение полей A и B
        /// </summary>
        public void AddFields(DiagonalMatrix matrix, int wordIndex)
        {
            PerformFieldOperations(matrix, wordIndex, (a, b) => a + b, "сложение");
        }

        /// <summary>
        /// Вычитание полей A и B
        /// </summary>
        public void SubtractFields(DiagonalMatrix matrix, int wordIndex)
        {
            PerformFieldOperations(matrix, wordIndex, (a, b) => Math.Max(0, a - b), "вычитание");
        }

        /// <summary>
        /// Умножение полей A и B
        /// </summary>
        public void MultiplyFields(DiagonalMatrix matrix, int wordIndex)
        {
            PerformFieldOperations(matrix, wordIndex, (a, b) => a * b, "умножение");
        }

        /// <summary>
        /// Логическое И полей A и B
        /// </summary>
        public void AndFields(DiagonalMatrix matrix, int wordIndex)
        {
            PerformFieldOperations(matrix, wordIndex, (a, b) => a & b, "логическое И");
        }

        /// <summary>
        /// Логическое ИЛИ полей A и B
        /// </summary>
        public void OrFields(DiagonalMatrix matrix, int wordIndex)
        {
            PerformFieldOperations(matrix, wordIndex, (a, b) => a | b, "логическое ИЛИ");
        }

        /// <summary>
        /// Исключающее ИЛИ полей A и B
        /// </summary>
        public void XorFields(DiagonalMatrix matrix, int wordIndex)
        {
            PerformFieldOperations(matrix, wordIndex, (a, b) => a ^ b, "исключающее ИЛИ");
        }

        /// <summary>
        /// Анализ структуры всех слов
        /// </summary>
        public void AnalyzeWordStructure(DiagonalMatrix matrix)
        {
            Console.WriteLine("Анализ структуры слов:");
            Console.WriteLine("Индекс\tСлово\t\t\tV\tA\tB\tS\tA+B");
            Console.WriteLine("------------------------------------------------------");

            for (int i = 0; i < 16; i++)
            {
                string word = matrix.ReadWord(i);
                string wordV = word.Substring(0, 3);
                string fieldA = word.Substring(3, 4);
                string fieldB = word.Substring(7, 4);
                string fieldS = word.Substring(11, 5);

                int valueA = DiagonalMatrix.BinaryToDecimal(fieldA);
                int valueB = DiagonalMatrix.BinaryToDecimal(fieldB);
                int sum = valueA + valueB;

                Console.WriteLine($"{i,2}\t{word}\t{wordV}\t{fieldA}\t{fieldB}\t{fieldS}\t{sum}");
            }
        }

        /// <summary>
        /// Поиск слов с определенным значением в поле A
        /// </summary>
        public List<int> FindWordsWithFieldA(DiagonalMatrix matrix, int targetValue)
        {
            var results = new List<int>();

            for (int i = 0; i < 16; i++)
            {
                string word = matrix.ReadWord(i);
                string fieldA = word.Substring(3, 4);
                int valueA = DiagonalMatrix.BinaryToDecimal(fieldA);

                if (valueA == targetValue)
                {
                    results.Add(i);
                }
            }

            return results;
        }

        /// <summary>
        /// Поиск слов с определенным значением в поле B
        /// </summary>
        public List<int> FindWordsWithFieldB(DiagonalMatrix matrix, int targetValue)
        {
            var results = new List<int>();

            for (int i = 0; i < 16; i++)
            {
                string word = matrix.ReadWord(i);
                string fieldB = word.Substring(7, 4);
                int valueB = DiagonalMatrix.BinaryToDecimal(fieldB);

                if (valueB == targetValue)
                {
                    results.Add(i);
                }
            }

            return results;
        }
    }
}