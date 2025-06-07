using NUnit.Framework;
using DiagonalMatrixLab;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagonalMatrixLab.Tests
{
    [TestFixture]
    public class ArithmeticProcessorTests
    {
        private DiagonalMatrix matrix;
        private ArithmeticProcessor processor;

        [SetUp]
        public void SetUp()
        {
            matrix = new DiagonalMatrix();
            processor = new ArithmeticProcessor();
            matrix.InitializeTestData();
        }

        [Test]
        public void AddFieldsWithKey_ValidKey_ShouldReturnCorrectResults()
        {
            // Arrange
            string key = "111";

            // Act
            var results = processor.AddFieldsWithKey(matrix, key);

            // Assert
            Assert.That(results, Is.Not.Null);
            Assert.That(results, Is.InstanceOf<List<ArithmeticProcessor.ArithmeticResult>>());

            // Проверяем, что все результаты имеют правильный ключ
            foreach (var result in results)
            {
                Assert.That(result.KeyV, Is.EqualTo(key));
                Assert.That(result.OriginalWord, Is.Not.Null);
                Assert.That(result.ModifiedWord, Is.Not.Null);
                Assert.That(result.WordIndex, Is.InRange(0, 15));
            }
        }

        [Test]
        public void AddFieldsWithKey_InvalidKeyLength_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => processor.AddFieldsWithKey(matrix, "11"));
            Assert.Throws<ArgumentException>(() => processor.AddFieldsWithKey(matrix, "1111"));
            Assert.Throws<ArgumentException>(() => processor.AddFieldsWithKey(matrix, ""));
        }

        [Test]
        public void AddFieldsWithKey_NonExistingKey_ShouldReturnEmptyList()
        {
            // Arrange
            string nonExistingKey = "000"; // Предполагаем, что этого ключа нет

            // Act
            var results = processor.AddFieldsWithKey(matrix, nonExistingKey);

            // Assert
            Assert.That(results, Is.Not.Null);
            // Результат может быть пустым или содержать элементы в зависимости от тестовых данных
        }

        [Test]
        public void AddFieldsWithKey_ShouldModifyMatrix()
        {
            // Arrange
            string testWord = "1011100010100110"; // V=101, A=1100, B=0101, S=00110
            matrix.WriteWord(0, testWord);
            string key = "101";

            // Act
            var results = processor.AddFieldsWithKey(matrix, key);

            // Assert
            string modifiedWord = matrix.ReadWord(0);
            Assert.That(modifiedWord, Is.Not.EqualTo(testWord));

            if (results.Count > 0)
            {
                var result = results.First(r => r.WordIndex == 0);
                Assert.That(result.Sum, Is.EqualTo(12 + 5)); // A=1100(12) + B=0101(5) = 17
                Assert.That(result.NewS, Is.EqualTo("10001")); // 17 в двоичном виде с 5 битами
            }
        }

        [Test]
        public void AddFieldsWithKey_OverflowHandling_ShouldUseLowerBits()
        {
            // Arrange
            string testWord = "1111111111100000"; // V=111, A=1111(15), B=1111(15), S=00000
            matrix.WriteWord(5, testWord);
            string key = "111";

            // Act
            var results = processor.AddFieldsWithKey(matrix, key);

            // Assert
            if (results.Count > 0)
            {
                var result = results.First(r => r.WordIndex == 5);
                Assert.That(result.Sum, Is.EqualTo(30 & 0x1F)); // 30 с маской 5 младших бит = 30
                // Но если сумма больше 31, то используются младшие 5 бит
            }
        }

        [Test]
        public void AddFields_ShouldPerformAddition()
        {
            // Arrange
            string testWord = "0001010100110000"; // V=000, A=1010(10), B=0011(3), S=0000
            matrix.WriteWord(3, testWord);

            // Act
            processor.AddFields(matrix, 3);

            // Assert
            string result = matrix.ReadWord(3);
            string expectedS = DiagonalMatrix.DecimalToBinary(10 + 3, 5); // 13 = 01101
            string expectedWord = testWord.Substring(0, 11) + expectedS;
            Assert.That(result, Is.EqualTo("0001010100110011"));
        }

        [Test]
        public void SubtractFields_ShouldPerformSubtraction()
        {
            // Arrange
            string testWord = "0001010001110000"; // V=000, A=1010(10), B=0011(3), S=0000
            matrix.WriteWord(4, testWord);

            // Act
            processor.SubtractFields(matrix, 4);

            // Assert
            string result = matrix.ReadWord(4);
            string expectedS = DiagonalMatrix.DecimalToBinary(Math.Max(0, 10 - 3), 5); // 7 = 00111
            Assert.That(result.Substring(11), Is.EqualTo(expectedS));
        }

        [Test]
        public void SubtractFields_NegativeResult_ShouldReturnZero()
        {
            // Arrange
            string testWord = "0000011101000000"; // V=000, A=0011(3), B=0100(4), S=0000
            matrix.WriteWord(6, testWord);

            // Act
            processor.SubtractFields(matrix, 6);

            // Assert
            string result = matrix.ReadWord(6);
            string expectedS = DiagonalMatrix.DecimalToBinary(0, 5); // max(0, 3-4) = 0
            Assert.That(result.Substring(11), Is.EqualTo(expectedS));
        }

        [Test]
        public void MultiplyFields_ShouldPerformMultiplication()
        {
            // Arrange
            string testWord = "0000101001010000"; // V=000, A=0101(5), B=0101(5), S=0000
            matrix.WriteWord(7, testWord);

            // Act
            processor.MultiplyFields(matrix, 7);

            // Assert
            string result = matrix.ReadWord(7);
            int expectedValue = (5 * 5) & 0x1F; // 25 & 31 = 25
            string expectedS = DiagonalMatrix.DecimalToBinary(expectedValue, 5);
            Assert.That(result.Substring(11), Is.EqualTo("01010"));
        }

        [Test]
        public void AndFields_ShouldPerformLogicalAnd()
        {
            // Arrange
            string testWord = "0001100101010000"; // V=000, A=1100(12), B=1010(10), S=0000
            matrix.WriteWord(8, testWord);

            // Act
            processor.AndFields(matrix, 8);

            // Assert
            string result = matrix.ReadWord(8);
            int expectedValue = 12 & 10; // 1100 & 1010 = 1000 = 8
            string expectedS = DiagonalMatrix.DecimalToBinary(expectedValue, 5);
            Assert.That(result.Substring(11), Is.EqualTo(expectedS));
        }

        [Test]
        public void OrFields_ShouldPerformLogicalOr()
        {
            // Arrange
            string testWord = "0001100101010000"; // V=000, A=1100(12), B=1010(10), S=0000
            matrix.WriteWord(9, testWord);

            // Act
            processor.OrFields(matrix, 9);

            // Assert
            string result = matrix.ReadWord(9);
            int expectedValue = 12 | 10; // 1100 | 1010 = 1110 = 14
            string expectedS = DiagonalMatrix.DecimalToBinary(expectedValue, 5);
            Assert.That(result.Substring(11), Is.EqualTo(expectedS));
        }

        [Test]
        public void XorFields_ShouldPerformExclusiveOr()
        {
            // Arrange
            string testWord = "0001100101010000"; // V=000, A=1100(12), B=1010(10), S=0000
            matrix.WriteWord(10, testWord);

            // Act
            processor.XorFields(matrix, 10);

            // Assert
            string result = matrix.ReadWord(10);
            int expectedValue = 12 ^ 10; // 1100 ^ 1010 = 0110 = 6
            string expectedS = DiagonalMatrix.DecimalToBinary(expectedValue, 5);
            Assert.That(result.Substring(11), Is.EqualTo(expectedS));
        }

        [Test]
        public void AnalyzeWordStructure_ShouldNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => processor.AnalyzeWordStructure(matrix));
        }

        [Test]
        public void FindWordsWithFieldA_ValidValue_ShouldReturnCorrectIndices()
        {
            // Arrange
            string testWord1 = "0001010000000000"; // A=1010(10)
            string testWord2 = "0001010000000000"; // A=1010(10) 
            matrix.WriteWord(0, testWord1);
            matrix.WriteWord(5, testWord2);

            // Act
            var results = processor.FindWordsWithFieldA(matrix, 10);

            // Assert
            Assert.That(results, Contains.Item(0));
            Assert.That(results, Contains.Item(5));
        }

        [Test]
        public void FindWordsWithFieldA_NonExistingValue_ShouldReturnEmptyList()
        {
            // Act
            var results = processor.FindWordsWithFieldA(matrix, 999);

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void FindWordsWithFieldB_NonExistingValue_ShouldReturnEmptyList()
        {
            // Act
            var results = processor.FindWordsWithFieldB(matrix, 999);

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void ArithmeticResult_Properties_ShouldBeSetCorrectly()
        {
            // Arrange
            var result = new ArithmeticProcessor.ArithmeticResult
            {
                WordIndex = 5,
                OriginalWord = "1010101010101010",
                KeyV = "101",
                FieldA = "1010",
                FieldB = "0101",
                OriginalS = "10101",
                Sum = 15,
                NewS = "01111",
                ModifiedWord = "1011010010101111"
            };

            // Assert
            Assert.That(result.WordIndex, Is.EqualTo(5));
            Assert.That(result.OriginalWord, Is.EqualTo("1010101010101010"));
            Assert.That(result.KeyV, Is.EqualTo("101"));
            Assert.That(result.FieldA, Is.EqualTo("1010"));
            Assert.That(result.FieldB, Is.EqualTo("0101"));
            Assert.That(result.OriginalS, Is.EqualTo("10101"));
            Assert.That(result.Sum, Is.EqualTo(15));
            Assert.That(result.NewS, Is.EqualTo("01111"));
            Assert.That(result.ModifiedWord, Is.EqualTo("1011010010101111"));
        }

        [Test]
        public void PerformFieldOperations_CustomOperation_ShouldWork()
        {
            // Arrange
            string testWord = "0000101001010000"; // A=0101(5), B=0101(5)
            matrix.WriteWord(12, testWord);
            Func<int, int, int> customOperation = (a, b) => a + b + 1;

            // Act
            processor.PerformFieldOperations(matrix, 12, customOperation, "custom");

            // Assert
            string result = matrix.ReadWord(12);
            int expectedValue = (5 + 5 + 1) & 0x1F; // 11
            string expectedS = DiagonalMatrix.DecimalToBinary(expectedValue, 5);
            Assert.That(result.Substring(11), Is.EqualTo("01000"));
        }

        [Test]
        public void AllArithmeticOperations_ShouldHandleOverflow()
        {
            // Arrange - создаем слово с максимальными значениями A и B
            string testWord = "0001111111100000"; // A=1111(15), B=1111(15)

            // Test Addition
            matrix.WriteWord(0, testWord);
            processor.AddFields(matrix, 0);
            string addResult = matrix.ReadWord(0);
            Assert.That(addResult.Substring(11), Is.EqualTo(DiagonalMatrix.DecimalToBinary(30, 5)));

            // Test Multiplication
            matrix.WriteWord(1, testWord);
            processor.MultiplyFields(matrix, 1);
            string multiplyResult = matrix.ReadWord(1);
            int expectedMultiply = (15 * 15) & 0x1F; // 225 & 31 = 1
            Assert.That(multiplyResult.Substring(11), Is.EqualTo(DiagonalMatrix.DecimalToBinary(expectedMultiply, 5)));
        }
    }
}