using System;
using System.Collections.Generic;

namespace DiagonalMatrixLab
{
    public class SearchProcessor
    {
        public class SearchResult
        {
            public int WordIndex { get; set; }
            public string WordValue { get; set; }
            public int DecimalValue { get; set; }
        }

        /// <summary>
        /// Поиск величин, заключенных в данном интервале
        /// Для варианта 2: поиск слов, значения которых попадают в заданный интервал
        /// </summary>
        public List<SearchResult> FindInInterval(DiagonalMatrix matrix, int minValue, int maxValue)
        {
            var results = new List<SearchResult>();

            Console.WriteLine($"Поиск слов в интервале [{minValue}, {maxValue}]:");
            Console.WriteLine();

            for (int i = 0; i < 16; i++)
            {
                string word = matrix.ReadWord(i);
                int decimalValue = DiagonalMatrix.BinaryToDecimal(word);

                if (decimalValue >= minValue && decimalValue <= maxValue)
                {
                    results.Add(new SearchResult
                    {
                        WordIndex = i,
                        WordValue = word,
                        DecimalValue = decimalValue
                    });
                }
            }

            return results;
        }

        /// <summary>
        /// Поиск значения ближайшего сверху/снизу
        /// </summary>
        public SearchResult FindNearest(DiagonalMatrix matrix, int targetValue, bool findAbove = true)
        {
            SearchResult bestResult = null;
            int bestDifference = int.MaxValue;

            for (int i = 0; i < 16; i++)
            {
                string word = matrix.ReadWord(i);
                int decimalValue = DiagonalMatrix.BinaryToDecimal(word);

                int difference;
                bool isValid;

                if (findAbove)
                {
                    // Поиск ближайшего сверху
                    difference = decimalValue - targetValue;
                    isValid = difference >= 0;
                }
                else
                {
                    // Поиск ближайшего снизу
                    difference = targetValue - decimalValue;
                    isValid = difference >= 0;
                }

                if (isValid && difference < bestDifference)
                {
                    bestDifference = difference;
                    bestResult = new SearchResult
                    {
                        WordIndex = i,
                        WordValue = word,
                        DecimalValue = decimalValue
                    };
                }
            }

            return bestResult;
        }

        /// <summary>
        /// Упорядоченная выборка (сортировка) всех слов
        /// </summary>
        public List<SearchResult> GetSortedWords(DiagonalMatrix matrix, bool ascending = true)
        {
            var results = new List<SearchResult>();

            // Получаем все слова
            for (int i = 0; i < 16; i++)
            {
                string word = matrix.ReadWord(i);
                int decimalValue = DiagonalMatrix.BinaryToDecimal(word);

                results.Add(new SearchResult
                {
                    WordIndex = i,
                    WordValue = word,
                    DecimalValue = decimalValue
                });
            }

            // Сортируем
            if (ascending)
            {
                results.Sort((a, b) => a.DecimalValue.CompareTo(b.DecimalValue));
            }
            else
            {
                results.Sort((a, b) => b.DecimalValue.CompareTo(a.DecimalValue));
            }

            return results;
        }

        /// <summary>
        /// Поиск по соответствию (точное совпадение)
        /// </summary>
        public List<SearchResult> FindExactMatch(DiagonalMatrix matrix, int exactValue)
        {
            var results = new List<SearchResult>();

            for (int i = 0; i < 16; i++)
            {
                string word = matrix.ReadWord(i);
                int decimalValue = DiagonalMatrix.BinaryToDecimal(word);

                if (decimalValue == exactValue)
                {
                    results.Add(new SearchResult
                    {
                        WordIndex = i,
                        WordValue = word,
                        DecimalValue = decimalValue
                    });
                }
            }

            return results;
        }

        /// <summary>
        /// Статистика по всем словам
        /// </summary>
        public void PrintStatistics(DiagonalMatrix matrix)
        {
            var allWords = new List<int>();

            Console.WriteLine("Статистика по словам:");
            Console.WriteLine("Индекс\tДвоичное\t\tДесятичное");
            Console.WriteLine("----------------------------------------");

            for (int i = 0; i < 16; i++)
            {
                string word = matrix.ReadWord(i);
                int decimalValue = DiagonalMatrix.BinaryToDecimal(word);
                allWords.Add(decimalValue);

                Console.WriteLine($"{i,2}\t{word}\t{decimalValue,5}");
            }

            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Минимальное значение: {allWords.Min()}");
            Console.WriteLine($"Максимальное значение: {allWords.Max()}");
            Console.WriteLine($"Среднее значение: {allWords.Average():F2}");
            Console.WriteLine($"Сумма всех слов: {allWords.Sum()}");
        }
    }
}