using System;

namespace DiagonalMatrixLab
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Лабораторная работа 7 - Диагональная адресация матрицы");
            Console.WriteLine("Вариант 2");
            Console.WriteLine("=========================================");

            var matrix = new DiagonalMatrix();

            // Инициализация матрицы тестовыми данными из примера
            matrix.InitializeTestData();

            Console.WriteLine("Исходная матрица:");
            matrix.PrintMatrix();

            var logicProcessor = new LogicProcessor();
            var searchProcessor = new SearchProcessor();
            var arithmeticProcessor = new ArithmeticProcessor();

            while (true)
            {
                Console.WriteLine("\nВыберите операцию:");
                Console.WriteLine("1. Считать слово по индексу");
                Console.WriteLine("2. Считать адресный столбец по индексу");
                Console.WriteLine("3. Выполнить логические функции f7∧f8");
                Console.WriteLine("4. Выполнить логические функции f2∧f13");
                Console.WriteLine("5. Поиск величин в заданном интервале");
                Console.WriteLine("6. Сложение полей Aj и Bj с ключом V");
                Console.WriteLine("7. Показать матрицу");
                Console.WriteLine("0. Выход");
                Console.Write("Введите номер операции: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Некорректный ввод!");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        ReadWord(matrix);
                        break;
                    case 2:
                        ReadColumn(matrix);
                        break;
                    case 3:
                        PerformLogicF7F8(matrix, logicProcessor);
                        break;
                    case 4:
                        PerformLogicF2F13(matrix, logicProcessor);
                        break;
                    case 5:
                        SearchInInterval(matrix, searchProcessor);
                        break;
                    case 6:
                        PerformArithmetic(matrix, arithmeticProcessor);
                        break;
                    case 7:
                        matrix.PrintMatrix();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
        }

        static void ReadWord(DiagonalMatrix matrix)
        {
            Console.Write("Введите номер слова (0-15): ");
            if (int.TryParse(Console.ReadLine(), out int wordIndex) && wordIndex >= 0 && wordIndex < 16)
            {
                var word = matrix.ReadWord(wordIndex);
                Console.WriteLine($"Слово #{wordIndex}: {word}");
            }
            else
            {
                Console.WriteLine("Некорректный индекс слова!");
            }
        }

        static void ReadColumn(DiagonalMatrix matrix)
        {
            Console.Write("Введите номер столбца (0-15): ");
            if (int.TryParse(Console.ReadLine(), out int columnIndex) && columnIndex >= 0 && columnIndex < 16)
            {
                var column = matrix.ReadColumn(columnIndex);
                Console.WriteLine($"Столбец #{columnIndex}: {column}");
            }
            else
            {
                Console.WriteLine("Некорректный индекс столбца!");
            }
        }

        public static void PerformLogicF7F8(DiagonalMatrix matrix, LogicProcessor processor)
        {
            Console.Write("Введите номер первого слова (0-15): ");
            if (!int.TryParse(Console.ReadLine(), out int word1) || word1 < 0 || word1 > 15) return;

            Console.Write("Введите номер второго слова (0-15): ");
            if (!int.TryParse(Console.ReadLine(), out int word2) || word2 < 0 || word2 > 15) return;

            Console.Write("Введите номер слова для записи результата (0-15): ");
            if (!int.TryParse(Console.ReadLine(), out int resultWord) || resultWord < 0 || resultWord > 15) return;

            processor.ApplyF7AndF8(matrix, word1, word2, resultWord);
            Console.WriteLine($"Результат логической операции f7∧f8 записан в слово #{resultWord}");
        }

        static void PerformLogicF2F13(DiagonalMatrix matrix, LogicProcessor processor)
        {
            Console.Write("Введите номер первого слова (0-15): ");
            if (!int.TryParse(Console.ReadLine(), out int word1) || word1 < 0 || word1 > 15) return;

            Console.Write("Введите номер второго слова (0-15): ");
            if (!int.TryParse(Console.ReadLine(), out int word2) || word2 < 0 || word2 > 15) return;

            Console.Write("Введите номер слова для записи результата (0-15): ");
            if (!int.TryParse(Console.ReadLine(), out int resultWord) || resultWord < 0 || resultWord > 15) return;

            processor.ApplyF2AndF13(matrix, word1, word2, resultWord);
            Console.WriteLine($"Результат логической операции f2∧f13 записан в слово #{resultWord}");
        }

        static void SearchInInterval(DiagonalMatrix matrix, SearchProcessor processor)
        {
            Console.Write("Введите нижнюю границу интервала (0-65535): ");
            if (!int.TryParse(Console.ReadLine(), out int min) || min < 0 || min > 65535) return;

            Console.Write("Введите верхнюю границу интервала (0-65535): ");
            if (!int.TryParse(Console.ReadLine(), out int max) || max < 0 || max > 65535) return;

            var results = processor.FindInInterval(matrix, min, max);
            Console.WriteLine($"Найдено {results.Count} слов в интервале [{min}, {max}]:");

            foreach (var result in results)
            {
                Console.WriteLine($"Слово #{result.WordIndex}: {result.WordValue} (значение: {result.DecimalValue})");
            }
        }

        static void PerformArithmetic(DiagonalMatrix matrix, ArithmeticProcessor processor)
        {
            Console.Write("Введите ключ V (3 бита, например 111): ");
            string key = Console.ReadLine();

            if (key?.Length != 3 || !key.All(c => c == '0' || c == '1'))
            {
                Console.WriteLine("Ключ должен содержать ровно 3 бита (0 или 1)!");
                return;
            }

            var results = processor.AddFieldsWithKey(matrix, key);
            Console.WriteLine($"Обработано {results.Count} слов с ключом {key}:");

            foreach (var result in results)
            {
                Console.WriteLine($"Слово #{result.WordIndex}: A={result.FieldA}, B={result.FieldB}, " +
                                $"A+B={result.Sum}, новое S={result.NewS}");
            }
        }
    }
}