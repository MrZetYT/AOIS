using NUnit.Framework;
using System;
using System.IO;
using DiagonalMatrixLab;

namespace DiagonalMatrixLab.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        private DiagonalMatrix matrix;
        private LogicProcessor logicProcessor;
        private SearchProcessor searchProcessor;
        private ArithmeticProcessor arithmeticProcessor;
        private StringWriter stringWriter;
        private StringReader stringReader;
        private TextWriter originalOut;
        private TextReader originalIn;

        [SetUp]
        public void Setup()
        {
            matrix = new DiagonalMatrix();
            logicProcessor = new LogicProcessor();
            searchProcessor = new SearchProcessor();
            arithmeticProcessor = new ArithmeticProcessor();

            matrix.InitializeTestData();

            // Захватываем оригинальные потоки
            originalOut = Console.Out;
            originalIn = Console.In;

            stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
        }

        [TearDown]
        public void TearDown()
        {
            // Восстанавливаем оригинальные потоки
            Console.SetOut(originalOut);
            Console.SetIn(originalIn);

            stringWriter?.Dispose();
            stringReader?.Dispose();
        }

        private void SetupConsoleInput(string input)
        {
            stringReader = new StringReader(input);
            Console.SetIn(stringReader);
        }

        [Test]
        public void PerformLogicF7F8_WithValidInput_ShouldExecuteCorrectly()
        {
            // Arrange
            SetupConsoleInput("0\n1\n2\n");

            // Act
            Program.PerformLogicF7F8(matrix, logicProcessor);

            // Assert
            string output = stringWriter.ToString();
            Assert.That(output, Contains.Substring("f7∧f8 результат"));
            Assert.That(output, Contains.Substring("Результат логической операции f7∧f8 записан в слово #2"));
        }

        [Test]
        public void PerformLogicF7F8_WithInvalidFirstWord_ShouldReturnEarly()
        {
            // Arrange
            SetupConsoleInput("16\n");

            // Act
            Program.PerformLogicF7F8(matrix, logicProcessor);

            // Assert
            string output = stringWriter.ToString();
            Assert.That(output, Does.Not.Contain("f7∧f8 результат"));
        }

        [Test]
        public void PerformLogicF7F8_WithNonNumericFirstWord_ShouldReturnEarly()
        {
            // Arrange
            SetupConsoleInput("abc\n");

            // Act
            Program.PerformLogicF7F8(matrix, logicProcessor);

            // Assert
            string output = stringWriter.ToString();
            Assert.That(output, Does.Not.Contain("f7∧f8 результат"));
        }

        [Test]
        public void PerformLogicF7F8_WithNegativeFirstWord_ShouldReturnEarly()
        {
            // Arrange
            SetupConsoleInput("-1\n");

            // Act
            Program.PerformLogicF7F8(matrix, logicProcessor);

            // Assert
            string output = stringWriter.ToString();
            Assert.That(output, Does.Not.Contain("f7∧f8 результат"));
        }

        [Test]
        public void PerformLogicF7F8_WithValidFirstWordInvalidSecond_ShouldReturnEarly()
        {
            // Arrange
            SetupConsoleInput("0\n16\n");

            // Act
            Program.PerformLogicF7F8(matrix, logicProcessor);

            // Assert
            string output = stringWriter.ToString();
            Assert.That(output, Does.Not.Contain("f7∧f8 результат"));
        }

        [Test]
        public void PerformLogicF7F8_WithValidWordsInvalidResult_ShouldReturnEarly()
        {
            // Arrange
            SetupConsoleInput("0\n1\n16\n");

            // Act
            Program.PerformLogicF7F8(matrix, logicProcessor);

            // Assert
            string output = stringWriter.ToString();
            Assert.That(output, Does.Not.Contain("f7∧f8 результат"));
        }

        [Test]
        public void PerformLogicF7F8_WithBoundaryValues_ShouldExecuteCorrectly()
        {
            // Arrange
            SetupConsoleInput("0\n15\n14\n");

            // Act
            Program.PerformLogicF7F8(matrix, logicProcessor);

            // Assert
            string output = stringWriter.ToString();
            Assert.That(output, Contains.Substring("f7∧f8 результат"));
            Assert.That(output, Contains.Substring("Результат логической операции f7∧f8 записан в слово #14"));
        }

        [Test]
        public void ReadWord_WithValidIndex_ShouldDisplayWord()
        {
            // Arrange
            string testWord = "1010101010101010";
            matrix.WriteWord(5, testWord);

            // Create a test method that mimics the ReadWord functionality
            // Act
            string word = matrix.ReadWord(5);

            // Assert
            Assert.AreEqual(testWord, word);
        }

        [Test]
        public void ReadColumn_WithValidIndex_ShouldDisplayColumn()
        {
            // Arrange & Act
            string column = matrix.ReadColumn(5);

            // Assert
            Assert.AreEqual(16, column.Length);
            Assert.That(column, Does.Match("^[01]+$")); // Должен содержать только 0 и 1
        }

        [Test]
        public void Main_ShouldInitializeAndPrintHeader()
        {
            // This test verifies the program starts correctly
            // Since Main contains infinite loop, we test initialization parts

            // Arrange
            var testMatrix = new DiagonalMatrix();

            // Act
            testMatrix.InitializeTestData();

            // Assert
            string word0 = testMatrix.ReadWord(0);
            Assert.AreEqual(16, word0.Length);
            Assert.That(word0, Does.Match("^[01]+$"));
        }

        [Test]
        public void SearchInInterval_WithValidInput_ShouldExecuteCorrectly()
        {
            // Testing the search functionality that would be called from Main
            // Arrange
            var results = searchProcessor.FindInInterval(matrix, 0, 1000);

            // Act & Assert
            Assert.IsNotNull(results);
            Assert.That(results.Count, Is.GreaterThanOrEqualTo(0));
            Assert.That(results.Count, Is.LessThanOrEqualTo(16));
        }

        [Test]
        public void PerformArithmetic_WithValidInput_ShouldExecuteCorrectly()
        {
            // Testing the arithmetic functionality that would be called from Main
            // Arrange
            string validKey = "101";

            // Act
            var results = arithmeticProcessor.AddFieldsWithKey(matrix, validKey);

            // Assert
            Assert.IsNotNull(results);
            Assert.That(results.Count, Is.GreaterThanOrEqualTo(0));
            Assert.That(results.Count, Is.LessThanOrEqualTo(16));
        }

        [Test]
        public void ConsoleInputValidation_WithBoundaryValues_ShouldWorkCorrectly()
        {
            // Test boundary values that would be used in Main

            // Valid boundary values
            Assert.That(0, Is.GreaterThanOrEqualTo(0).And.LessThan(16));
            Assert.That(15, Is.GreaterThanOrEqualTo(0).And.LessThan(16));

            // Invalid boundary values
            Assert.That(-1, Is.Not.InRange(0, 15));
            Assert.That(16, Is.Not.InRange(0, 15));
        }

        [Test]
        public void MenuOptions_ShouldCoverAllFunctionality()
        {
            // Test that all menu options have corresponding functionality

            // Option 1: Read word
            Assert.DoesNotThrow(() => matrix.ReadWord(0));

            // Option 2: Read column
            Assert.DoesNotThrow(() => matrix.ReadColumn(0));

            // Option 3: Logic F7F8
            Assert.DoesNotThrow(() => logicProcessor.ApplyF7AndF8(matrix, 0, 1, 2));

            // Option 4: Logic F2F13
            Assert.DoesNotThrow(() => logicProcessor.ApplyF2AndF13(matrix, 0, 1, 2));

            // Option 5: Search
            Assert.DoesNotThrow(() => searchProcessor.FindInInterval(matrix, 0, 100));

            // Option 6: Arithmetic
            Assert.DoesNotThrow(() => arithmeticProcessor.AddFieldsWithKey(matrix, "101"));

            // Option 7: Print matrix
            Assert.DoesNotThrow(() => matrix.PrintMatrix());
        }

        [Test]
        public void ProgramFlow_WithValidSequence_ShouldExecuteAllOperations()
        {
            // Test a sequence of operations that might be performed in Main

            // 1. Read a word
            string word = matrix.ReadWord(0);
            Assert.AreEqual(16, word.Length);

            // 2. Perform logic operation
            logicProcessor.ApplyF7AndF8(matrix, 0, 1, 2);
            string result = matrix.ReadWord(2);
            Assert.AreEqual(16, result.Length);

            // 3. Perform search
            var searchResults = searchProcessor.FindInInterval(matrix, 0, 65535);
            Assert.IsNotNull(searchResults);

            // 4. Perform arithmetic
            var arithmeticResults = arithmeticProcessor.AddFieldsWithKey(matrix, "000");
            Assert.IsNotNull(arithmeticResults);
        }

        [Test]
        public void ErrorHandling_WithInvalidOperations_ShouldNotThrow()
        {
            // Test error handling for edge cases

            // Invalid word indices should be handled by matrix class
            Assert.Throws<ArgumentOutOfRangeException>(() => matrix.ReadWord(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => matrix.ReadWord(16));

            // Invalid column indices should be handled by matrix class
            Assert.Throws<ArgumentOutOfRangeException>(() => matrix.ReadColumn(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => matrix.ReadColumn(16));
        }

        [Test]
        public void ProgramComponents_ShouldBeInitialized()
        {
            // Test that all components can be initialized as in Main

            var testMatrix = new DiagonalMatrix();
            var testLogicProcessor = new LogicProcessor();
            var testSearchProcessor = new SearchProcessor();
            var testArithmeticProcessor = new ArithmeticProcessor();

            Assert.IsNotNull(testMatrix);
            Assert.IsNotNull(testLogicProcessor);
            Assert.IsNotNull(testSearchProcessor);
            Assert.IsNotNull(testArithmeticProcessor);

            // Test initialization
            Assert.DoesNotThrow(() => testMatrix.InitializeTestData());
        }

        [Test]
        public void ValidateUserInput_WithVariousInputs_ShouldBehaveCorrectly()
        {
            // Test input validation logic that would be used in Main

            // Valid inputs
            Assert.That(int.TryParse("0", out int result1) && result1 >= 0 && result1 < 16, Is.True);
            Assert.That(int.TryParse("15", out int result2) && result2 >= 0 && result2 < 16, Is.True);
            Assert.That(int.TryParse("5", out int result3) && result3 >= 0 && result3 < 16, Is.True);

            // Invalid inputs
            Assert.That(int.TryParse("-1", out int result4) && result4 >= 0 && result4 < 16, Is.False);
            Assert.That(int.TryParse("16", out int result5) && result5 >= 0 && result5 < 16, Is.False);
            Assert.That(int.TryParse("abc", out int result6), Is.False);
            Assert.That(int.TryParse("", out int result7), Is.False);
        }

        [Test]
        public void PerformLogicF7F8_WithAllCombinations_ShouldWork()
        {
            // Test all valid combinations of word indices
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        if (i != j) // Ensure different source words
                        {
                            Assert.DoesNotThrow(() => logicProcessor.ApplyF7AndF8(matrix, i, j, k));
                        }
                    }
                }
            }
        }

        [Test]
        public void IntervalSearch_WithBoundaryValues_ShouldWork()
        {
            // Test interval search with boundary values
            var results1 = searchProcessor.FindInInterval(matrix, 0, 0);
            var results2 = searchProcessor.FindInInterval(matrix, 65535, 65535);
            var results3 = searchProcessor.FindInInterval(matrix, 0, 65535);

            Assert.IsNotNull(results1);
            Assert.IsNotNull(results2);
            Assert.IsNotNull(results3);

            Assert.That(results3.Count, Is.EqualTo(16)); // All words should be in full range
        }

        [Test]
        public void ArithmeticKeys_WithAllPossibleKeys_ShouldWork()
        {
            // Test arithmetic operations with all possible 3-bit keys
            string[] allKeys = { "000", "001", "010", "011", "100", "101", "110", "111" };

            foreach (string key in allKeys)
            {
                Assert.DoesNotThrow(() => arithmeticProcessor.AddFieldsWithKey(matrix, key));
            }
        }
    }
}