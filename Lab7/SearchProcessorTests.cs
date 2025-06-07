using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using DiagonalMatrixLab;

namespace DiagonalMatrixLab.Tests
{
    [TestFixture]
    public class SearchProcessorTests
    {
        private DiagonalMatrix _matrix;
        private SearchProcessor _processor;

        [SetUp]
        public void SetUp()
        {
            _matrix = new DiagonalMatrix();
            _processor = new SearchProcessor();

            // Инициализируем матрицу тестовыми данными
            _matrix.InitializeTestData();
        }

        [Test]
        public void FindInInterval_ValidInterval_ReturnsCorrectResults()
        {
            // Arrange
            int minValue = 0;
            int maxValue = 1000;

            // Act
            var results = _processor.FindInInterval(_matrix, minValue, maxValue);

            // Assert
            Assert.IsNotNull(results);
            Assert.IsInstanceOf<List<SearchProcessor.SearchResult>>(results);

            foreach (var result in results)
            {
                Assert.GreaterOrEqual(result.DecimalValue, minValue);
                Assert.LessOrEqual(result.DecimalValue, maxValue);
                Assert.IsNotNull(result.WordValue);
                Assert.AreEqual(16, result.WordValue.Length);
                Assert.GreaterOrEqual(result.WordIndex, 0);
                Assert.LessOrEqual(result.WordIndex, 15);
            }
        }

        [Test]
        public void FindInInterval_EmptyInterval_ReturnsEmptyList()
        {
            // Arrange
            // Используем интервал, который заведомо не содержит никаких значений
            int minValue = 70000;
            int maxValue = 80000;

            // Act
            var results = _processor.FindInInterval(_matrix, minValue, maxValue);

            // Assert
            Assert.IsNotNull(results);
            Assert.IsEmpty(results);
        }

        [Test]
        public void FindInInterval_SingleValueInterval_ReturnsCorrectResults()
        {
            // Arrange
            // Сначала найдем какое-то существующее значение
            string firstWord = _matrix.ReadWord(0);
            int exactValue = DiagonalMatrix.BinaryToDecimal(firstWord);

            // Act
            var results = _processor.FindInInterval(_matrix, exactValue, exactValue);

            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count >= 1);
            Assert.IsTrue(results.All(r => r.DecimalValue == exactValue));
        }

        [Test]
        public void FindInInterval_FullRange_ReturnsAllWords()
        {
            // Arrange
            int minValue = 0;
            int maxValue = 65535; // Максимальное значение для 16-битного слова

            // Act
            var results = _processor.FindInInterval(_matrix, minValue, maxValue);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(16, results.Count); // Должны найти все 16 слов
        }

        [Test]
        public void FindNearest_FindAbove_ReturnsCorrectResult()
        {
            // Arrange
            int targetValue = 100;

            // Act
            var result = _processor.FindNearest(_matrix, targetValue, findAbove: true);

            // Assert
            if (result != null)
            {
                Assert.GreaterOrEqual(result.DecimalValue, targetValue);
                Assert.IsNotNull(result.WordValue);
                Assert.AreEqual(16, result.WordValue.Length);
                Assert.GreaterOrEqual(result.WordIndex, 0);
                Assert.LessOrEqual(result.WordIndex, 15);
            }
        }

        [Test]
        public void FindNearest_FindBelow_ReturnsCorrectResult()
        {
            // Arrange
            int targetValue = 30000;

            // Act
            var result = _processor.FindNearest(_matrix, targetValue, findAbove: false);

            // Assert
            if (result != null)
            {
                Assert.LessOrEqual(result.DecimalValue, targetValue);
                Assert.IsNotNull(result.WordValue);
                Assert.AreEqual(16, result.WordValue.Length);
                Assert.GreaterOrEqual(result.WordIndex, 0);
                Assert.LessOrEqual(result.WordIndex, 15);
            }
        }

        [Test]
        public void FindNearest_NoValueAbove_ReturnsNull()
        {
            // Arrange
            int targetValue = 65536; // Больше максимального значения для 16-битного слова

            // Act
            var result = _processor.FindNearest(_matrix, targetValue, findAbove: true);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void FindNearest_NoValueBelow_ReturnsNull()
        {
            // Arrange
            int targetValue = -1; // Меньше минимального значения

            // Act
            var result = _processor.FindNearest(_matrix, targetValue, findAbove: false);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void FindNearest_ExactMatch_ReturnsExactValue()
        {
            // Arrange
            string firstWord = _matrix.ReadWord(0);
            int exactValue = DiagonalMatrix.BinaryToDecimal(firstWord);

            // Act
            var resultAbove = _processor.FindNearest(_matrix, exactValue, findAbove: true);
            var resultBelow = _processor.FindNearest(_matrix, exactValue, findAbove: false);

            // Assert
            Assert.IsNotNull(resultAbove);
            Assert.AreEqual(exactValue, resultAbove.DecimalValue);
            Assert.IsNotNull(resultBelow);
            Assert.AreEqual(exactValue, resultBelow.DecimalValue);
        }

        [Test]
        public void GetSortedWords_Ascending_ReturnsSortedList()
        {
            // Act
            var results = _processor.GetSortedWords(_matrix, ascending: true);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(16, results.Count);

            for (int i = 1; i < results.Count; i++)
            {
                Assert.GreaterOrEqual(results[i].DecimalValue, results[i - 1].DecimalValue);
            }

            // Проверяем, что все слова присутствуют
            var indices = results.Select(r => r.WordIndex).OrderBy(x => x).ToList();
            var expectedIndices = Enumerable.Range(0, 16).ToList();
            CollectionAssert.AreEqual(expectedIndices, indices);
        }

        [Test]
        public void GetSortedWords_Descending_ReturnsSortedList()
        {
            // Act
            var results = _processor.GetSortedWords(_matrix, ascending: false);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(16, results.Count);

            for (int i = 1; i < results.Count; i++)
            {
                Assert.LessOrEqual(results[i].DecimalValue, results[i - 1].DecimalValue);
            }

            // Проверяем, что все слова присутствуют
            var indices = results.Select(r => r.WordIndex).OrderBy(x => x).ToList();
            var expectedIndices = Enumerable.Range(0, 16).ToList();
            CollectionAssert.AreEqual(expectedIndices, indices);
        }

        [Test]
        public void FindExactMatch_ExistingValue_ReturnsCorrectResults()
        {
            // Arrange
            string firstWord = _matrix.ReadWord(0);
            int exactValue = DiagonalMatrix.BinaryToDecimal(firstWord);

            // Act
            var results = _processor.FindExactMatch(_matrix, exactValue);

            // Assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count >= 1);
            Assert.IsTrue(results.All(r => r.DecimalValue == exactValue));
            Assert.IsTrue(results.Any(r => r.WordIndex == 0));
        }

        [Test]
        public void FindExactMatch_NonExistingValue_ReturnsEmptyList()
        {
            // Arrange
            int nonExistingValue = 99999; // Значение, которого точно нет в матрице

            // Act
            var results = _processor.FindExactMatch(_matrix, nonExistingValue);

            // Assert
            Assert.IsNotNull(results);
            Assert.IsEmpty(results);
        }

        [Test]
        public void FindExactMatch_ZeroValue_HandlesCorrectly()
        {
            // Arrange
            var zeroMatrix = new DiagonalMatrix();
            // Матрица по умолчанию заполнена нулями

            // Act
            var results = _processor.FindExactMatch(zeroMatrix, 0);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(16, results.Count); // Все слова должны быть равны 0
            Assert.IsTrue(results.All(r => r.DecimalValue == 0));
        }

        [Test]
        public void PrintStatistics_DoesNotThrowException()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => _processor.PrintStatistics(_matrix));
        }

        [Test]
        public void SearchResult_PropertiesWork()
        {
            // Arrange
            var result = new SearchProcessor.SearchResult
            {
                WordIndex = 5,
                WordValue = "1010101010101010",
                DecimalValue = 43690
            };

            // Assert
            Assert.AreEqual(5, result.WordIndex);
            Assert.AreEqual("1010101010101010", result.WordValue);
            Assert.AreEqual(43690, result.DecimalValue);
        }

        [Test]
        public void FindInInterval_LargeInterval_ReturnsAllValidResults()
        {
            // Arrange
            var customMatrix = new DiagonalMatrix();

            // Заполняем матрицу известными значениями
            customMatrix.WriteWord(0, "0000000000000000"); // 0
            customMatrix.WriteWord(1, "0000000000000001"); // 1
            customMatrix.WriteWord(2, "0000000000000010"); // 2
            customMatrix.WriteWord(3, "1111111111111111"); // 65535

            // Act
            var results = _processor.FindInInterval(customMatrix, 0, 10);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(15, results.Count); // Должны найти значения 0, 1, 2
            Assert.IsTrue(results.All(r => r.DecimalValue >= 0 && r.DecimalValue <= 10));
        }

        [Test]
        public void FindNearest_MultipleEqualDistances_ReturnsFirst()
        {
            // Arrange
            var customMatrix = new DiagonalMatrix();
            customMatrix.WriteWord(0, "0000000000000101"); // 5
            customMatrix.WriteWord(1, "0000000000001111"); // 15

            // Act - ищем ближайшее к 10 сверху (должно быть 15)
            var result = _processor.FindNearest(customMatrix, 10, findAbove: true);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(15, result.DecimalValue);
        }

        [Test]
        public void GetSortedWords_WithDuplicateValues_HandlesCorrectly()
        {
            // Arrange
            var customMatrix = new DiagonalMatrix();
            customMatrix.WriteWord(0, "0000000000000101"); // 5
            customMatrix.WriteWord(1, "0000000000000101"); // 5 (дублирует)
            customMatrix.WriteWord(2, "0000000000000001"); // 1

            // Act
            var results = _processor.GetSortedWords(customMatrix, ascending: true);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(16, results.Count);

            // Первые элементы должны быть отсортированы по возрастанию
            Assert.LessOrEqual(results[0].DecimalValue, results[1].DecimalValue);
            Assert.LessOrEqual(results[1].DecimalValue, results[2].DecimalValue);
        }
    }
}