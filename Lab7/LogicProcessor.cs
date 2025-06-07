using NUnit.Framework;
using System;
using System.IO;
using DiagonalMatrixLab;

namespace DiagonalMatrixLab.Tests
{
    [TestFixture]
    public class LogicProcessorTests
    {
        private DiagonalMatrix matrix;
        private LogicProcessor logicProcessor;

        [SetUp]
        public void Setup()
        {
            matrix = new DiagonalMatrix();
            logicProcessor = new LogicProcessor();

            // Инициализируем матрицу тестовыми данными
            matrix.InitializeTestData();
        }

        [Test]
        public void ApplyF7AndF8_WithValidWords_ShouldApplyLogicCorrectly()
        {
            // Arrange
            string word1 = "1111000011110000";
            string word2 = "1010101010101010";
            matrix.WriteWord(0, word1);
            matrix.WriteWord(1, word2);

            // Act
            logicProcessor.ApplyF7AndF8(matrix, 0, 1, 2);
            string result = matrix.ReadWord(2);

            // Assert
            // f7∧f8 = (a∧b)∧(a∨b) = a∧b
            string expected = "1010000010100000";
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ApplyF7AndF8_WithAllZeros_ShouldReturnAllZeros()
        {
            // Arrange
            string word1 = "0000000000000000";
            string word2 = "0000000000000000";
            matrix.WriteWord(0, word1);
            matrix.WriteWord(1, word2);

            // Act
            logicProcessor.ApplyF7AndF8(matrix, 0, 1, 2);
            string result = matrix.ReadWord(2);

            // Assert
            Assert.AreEqual("0000000000000000", result);
        }

        [Test]
        public void ApplyF7AndF8_WithAllOnes_ShouldReturnAllOnes()
        {
            // Arrange
            string word1 = "1111111111111111";
            string word2 = "1111111111111111";
            matrix.WriteWord(0, word1);
            matrix.WriteWord(1, word2);

            // Act
            logicProcessor.ApplyF7AndF8(matrix, 0, 1, 2);
            string result = matrix.ReadWord(2);

            // Assert
            Assert.AreEqual("1111111111111111", result);
        }

        [Test]
        public void ApplyF7AndF8_WithConsoleOutput_ShouldWriteToConsole()
        {
            // Arrange
            var originalOut = Console.Out;
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            string word1 = "1010101010101010";
            string word2 = "0101010101010101";
            matrix.WriteWord(0, word1);
            matrix.WriteWord(1, word2);

            // Act
            logicProcessor.ApplyF7AndF8(matrix, 0, 1, 2);

            // Assert
            Console.SetOut(originalOut);
            string output = stringWriter.ToString();
            Assert.That(output, Contains.Substring("Слово 0:"));
            Assert.That(output, Contains.Substring("Слово 1:"));
            Assert.That(output, Contains.Substring("f7∧f8 результат:"));
        }

        [Test]
        public void ApplyF2AndF13_WithValidWords_ShouldApplyLogicCorrectly()
        {
            // Arrange
            string word1 = "1111000011110000";
            string word2 = "1010101010101010";
            matrix.WriteWord(0, word1);
            matrix.WriteWord(1, word2);

            // Act
            logicProcessor.ApplyF2AndF13(matrix, 0, 1, 2);
            string result = matrix.ReadWord(2);

            // Assert
            // f2∧f13 = (a∧¬b)∧(a∨¬b) = a∧¬b
            string expected = "0101000001010000";
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ApplyF2AndF13_WithFirstWordAllOnes_SecondWordAllZeros_ShouldReturnAllOnes()
        {
            // Arrange
            string word1 = "1111111111111111";
            string word2 = "0000000000000000";
            matrix.WriteWord(0, word1);
            matrix.WriteWord(1, word2);

            // Act
            logicProcessor.ApplyF2AndF13(matrix, 0, 1, 2);
            string result = matrix.ReadWord(2);

            // Assert
            Assert.AreEqual("1111111111111111", result);
        }

        [Test]
        public void ApplyF2AndF13_WithFirstWordAllZeros_SecondWordAllOnes_ShouldReturnAllZeros()
        {
            // Arrange
            string word1 = "0000000000000000";
            string word2 = "1111111111111111";
            matrix.WriteWord(0, word1);
            matrix.WriteWord(1, word2);

            // Act
            logicProcessor.ApplyF2AndF13(matrix, 0, 1, 2);
            string result = matrix.ReadWord(2);

            // Assert
            Assert.AreEqual("0000000000000000", result);
        }

        [Test]
        public void ApplyF2AndF13_WithConsoleOutput_ShouldWriteToConsole()
        {
            // Arrange
            var originalOut = Console.Out;
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            string word1 = "1010101010101010";
            string word2 = "0101010101010101";
            matrix.WriteWord(0, word1);
            matrix.WriteWord(1, word2);

            // Act
            logicProcessor.ApplyF2AndF13(matrix, 0, 1, 2);

            // Assert
            Console.SetOut(originalOut);
            string output = stringWriter.ToString();
            Assert.That(output, Contains.Substring("Слово 0:"));
            Assert.That(output, Contains.Substring("Слово 1:"));
            Assert.That(output, Contains.Substring("f2∧f13 результат:"));
        }

        [Test]
        [TestCase("1010", "0101", "0000")] // a∧b test
        [TestCase("1111", "0000", "0000")] // a∧b with b=0
        [TestCase("0000", "1111", "0000")] // a∧b with a=0
        [TestCase("1111", "1111", "1111")] // a∧b with a=b=1
        public void F7AndF8Logic_ShouldFollowAndLogic(string word1Bits, string word2Bits, string expectedBits)
        {
            // Arrange
            string word1 = word1Bits.PadRight(16, '0');
            string word2 = word2Bits.PadRight(16, '0');
            matrix.WriteWord(0, word1);
            matrix.WriteWord(1, word2);

            // Act
            logicProcessor.ApplyF7AndF8(matrix, 0, 1, 2);
            string result = matrix.ReadWord(2);

            // Assert
            string expected = expectedBits.PadRight(16, '0');
            Assert.AreEqual(expected, result.Substring(0, expectedBits.Length) + result.Substring(expectedBits.Length));
        }

        [Test]
        [TestCase("1010", "0101", "1010")] // a∧¬b test
        [TestCase("1111", "0000", "1111")] // a∧¬b with b=0
        [TestCase("0000", "1111", "0000")] // a∧¬b with a=0
        [TestCase("1111", "1111", "0000")] // a∧¬b with a=b=1
        public void F2AndF13Logic_ShouldFollowAndNotLogic(string word1Bits, string word2Bits, string expectedBits)
        {
            // Arrange
            string word1 = word1Bits.PadRight(16, '0');
            string word2 = word2Bits.PadRight(16, '0');
            matrix.WriteWord(0, word1);
            matrix.WriteWord(1, word2);

            // Act
            logicProcessor.ApplyF2AndF13(matrix, 0, 1, 2);
            string result = matrix.ReadWord(2);

            // Assert
            string expected = expectedBits.PadRight(16, '0');
            Assert.AreEqual(expected, result.Substring(0, expectedBits.Length) + result.Substring(expectedBits.Length));
        }

        [Test]
        public void PrintLogicFunctions_ShouldWriteAllFunctionsToConsole()
        {
            // Arrange
            var originalOut = Console.Out;
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act
            logicProcessor.PrintLogicFunctions();

            // Assert
            Console.SetOut(originalOut);
            string output = stringWriter.ToString();

            Assert.That(output, Contains.Substring("f0 = 0"));
            Assert.That(output, Contains.Substring("f1 = a∧b"));
            Assert.That(output, Contains.Substring("f2 = a∧¬b"));
            Assert.That(output, Contains.Substring("f7 = a∨b"));
            Assert.That(output, Contains.Substring("f15 = 1"));
            Assert.That(output, Contains.Substring("Для варианта 2:"));
            Assert.That(output, Contains.Substring("f7∧f8"));
            Assert.That(output, Contains.Substring("f2∧f13"));
        }

        [Test]
        public void ApplyF7AndF8_WithBoundaryIndices_ShouldWorkCorrectly()
        {
            // Arrange
            string word1 = "1010101010101010";
            string word2 = "0101010101010101";
            matrix.WriteWord(0, word1);
            matrix.WriteWord(15, word2);

            // Act & Assert - should not throw
            Assert.DoesNotThrow(() => logicProcessor.ApplyF7AndF8(matrix, 0, 15, 14));

            string result = matrix.ReadWord(14);
            Assert.AreEqual(16, result.Length);
        }

        [Test]
        public void ApplyF2AndF13_WithBoundaryIndices_ShouldWorkCorrectly()
        {
            // Arrange
            string word1 = "1010101010101010";
            string word2 = "0101010101010101";
            matrix.WriteWord(0, word1);
            matrix.WriteWord(15, word2);

            // Act & Assert - should not throw
            Assert.DoesNotThrow(() => logicProcessor.ApplyF2AndF13(matrix, 0, 15, 14));

            string result = matrix.ReadWord(14);
            Assert.AreEqual(16, result.Length);
        }

        [Test]
        public void ApplyF7AndF8_WithSameSourceAndTarget_ShouldWorkCorrectly()
        {
            // Arrange
            string word1 = "1010101010101010";
            string word2 = "0101010101010101";
            matrix.WriteWord(0, word1);
            matrix.WriteWord(1, word2);

            // Act - using same index for source and target
            logicProcessor.ApplyF7AndF8(matrix, 0, 1, 0);

            // Assert
            string result = matrix.ReadWord(0);
            Assert.AreNotEqual(word1, result); // Should be modified
            Assert.AreEqual(16, result.Length);
        }

        [Test]
        public void F7AndF8_ComplexPattern_ShouldProduceCorrectResult()
        {
            // Arrange - создаем сложный паттерн для тестирования
            string word1 = "1100110011001100";
            string word2 = "1010101010101010";
            matrix.WriteWord(5, word1);
            matrix.WriteWord(7, word2);

            // Act
            logicProcessor.ApplyF7AndF8(matrix, 5, 7, 9);
            string result = matrix.ReadWord(9);

            // Assert - проверяем побитовое И
            string expected = "1000100010001000";
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void F2AndF13_ComplexPattern_ShouldProduceCorrectResult()
        {
            // Arrange
            string word1 = "1100110011001100";
            string word2 = "1010101010101010";
            matrix.WriteWord(5, word1);
            matrix.WriteWord(7, word2);

            // Act
            logicProcessor.ApplyF2AndF13(matrix, 5, 7, 9);
            string result = matrix.ReadWord(9);

            // Assert - проверяем a∧¬b
            string expected = "0100010001000100";
            Assert.AreEqual(expected, result);
        }
    }
}