using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using DiagonalMatrixLab;

namespace DiagonalMatrixLab.Tests
{
    [TestFixture]
    public class UtilitiesTests
    {
        private DiagonalMatrix matrix;

        [SetUp]
        public void SetUp()
        {
            matrix = new DiagonalMatrix();
            matrix.InitializeTestData();
        }

        #region IsValidBinaryString Tests

        [Test]
        public void IsValidBinaryString_WithValidBinaryString_ReturnsTrue()
        {
            // Arrange & Act & Assert
            Assert.IsTrue(Utilities.IsValidBinaryString("0"));
            Assert.IsTrue(Utilities.IsValidBinaryString("1"));
            Assert.IsTrue(Utilities.IsValidBinaryString("01"));
            Assert.IsTrue(Utilities.IsValidBinaryString("10"));
            Assert.IsTrue(Utilities.IsValidBinaryString("0101010101"));
            Assert.IsTrue(Utilities.IsValidBinaryString("1111111111"));
            Assert.IsTrue(Utilities.IsValidBinaryString("0000000000"));
        }

        [Test]
        public void IsValidBinaryString_WithInvalidBinaryString_ReturnsFalse()
        {
            // Arrange & Act & Assert
            Assert.IsFalse(Utilities.IsValidBinaryString(""));
            Assert.IsFalse(Utilities.IsValidBinaryString(null));
            Assert.IsFalse(Utilities.IsValidBinaryString("2"));
            Assert.IsFalse(Utilities.IsValidBinaryString("a"));
            Assert.IsFalse(Utilities.IsValidBinaryString("01a"));
            Assert.IsFalse(Utilities.IsValidBinaryString("012"));
            Assert.IsFalse(Utilities.IsValidBinaryString("0 1"));
            Assert.IsFalse(Utilities.IsValidBinaryString("01-10"));
        }

        #endregion

        #region GenerateRandomBinaryString Tests

        [Test]
        public void GenerateRandomBinaryString_WithValidLength_ReturnsCorrectLength()
        {
            // Arrange
            int[] lengths = { 1, 5, 10, 16, 32 };

            foreach (int length in lengths)
            {
                // Act
                string result = Utilities.GenerateRandomBinaryString(length);

                // Assert
                Assert.AreEqual(length, result.Length);
                Assert.IsTrue(Utilities.IsValidBinaryString(result));
            }
        }

        [Test]
        public void GenerateRandomBinaryString_WithZeroLength_ReturnsEmptyString()
        {
            // Act
            string result = Utilities.GenerateRandomBinaryString(0);

            // Assert
            Assert.AreEqual("", result);
        }

        [Test]
        public void GenerateRandomBinaryString_MultipleCallsWithSameLength_ReturnsDifferentStrings()
        {
            // Arrange
            const int length = 16;
            var results = new HashSet<string>();

            // Act - Generate multiple random strings
            for (int i = 0; i < 100; i++)
            {
                results.Add(Utilities.GenerateRandomBinaryString(length));
            }

            // Assert - Should have some variety (at least 10 different strings out of 100)
            Assert.Greater(results.Count, 10);
        }

        #endregion

        #region FillMatrixWithRandomData Tests

        [Test]
        public void FillMatrixWithRandomData_FillsAllWords()
        {
            // Arrange
            var emptyMatrix = new DiagonalMatrix();

            // Act
            Utilities.FillMatrixWithRandomData(emptyMatrix);

            // Assert
            for (int i = 0; i < 16; i++)
            {
                string word = emptyMatrix.ReadWord(i);
                Assert.AreEqual(16, word.Length);
                Assert.IsTrue(Utilities.IsValidBinaryString(word));
            }
        }

        [Test]
        public void FillMatrixWithRandomData_CreatesRandomData()
        {
            // Arrange
            var matrix1 = new DiagonalMatrix();
            var matrix2 = new DiagonalMatrix();

            // Act
            Utilities.FillMatrixWithRandomData(matrix1);
            Utilities.FillMatrixWithRandomData(matrix2);

            // Assert - Matrices should be different
            Assert.IsFalse(Utilities.CompareMatrices(matrix1, matrix2));
        }

        #endregion

        #region ExportMatrixToString Tests

        [Test]
        public void ExportMatrixToString_ReturnsNonEmptyString()
        {
            // Act
            string result = Utilities.ExportMatrixToString(matrix);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.That(result, Contains.Substring("Экспорт матрицы"));
            Assert.That(result, Contains.Substring("Матрица (построчно)"));
            Assert.That(result, Contains.Substring("Слова (по диагональной адресации)"));
        }

        [Test]
        public void ExportMatrixToString_ContainsAllWords()
        {
            // Act
            string result = Utilities.ExportMatrixToString(matrix);

            // Assert
            for (int i = 0; i < 16; i++)
            {
                Assert.That(result, Contains.Substring($"Слово {i,2}:"));
            }
        }

        #endregion

        #region CompareMatrices Tests

        [Test]
        public void CompareMatrices_WithIdenticalMatrices_ReturnsTrue()
        {
            // Arrange
            var matrix1 = new DiagonalMatrix();
            var matrix2 = new DiagonalMatrix();
            matrix1.InitializeTestData();
            matrix2.InitializeTestData();

            // Act & Assert
            Assert.IsTrue(Utilities.CompareMatrices(matrix1, matrix2));
        }

        [Test]
        public void CompareMatrices_WithDifferentMatrices_ReturnsFalse()
        {
            // Arrange
            var matrix1 = new DiagonalMatrix();
            var matrix2 = new DiagonalMatrix();
            matrix1.InitializeTestData();
            Utilities.FillMatrixWithRandomData(matrix2);

            // Act & Assert
            Assert.IsFalse(Utilities.CompareMatrices(matrix1, matrix2));
        }

        [Test]
        public void CompareMatrices_WithSameReference_ReturnsTrue()
        {
            // Act & Assert
            Assert.IsTrue(Utilities.CompareMatrices(matrix, matrix));
        }

        [Test]
        public void CompareMatrices_WithOneWordDifferent_ReturnsFalse()
        {
            // Arrange
            var matrix1 = new DiagonalMatrix();
            var matrix2 = new DiagonalMatrix();
            matrix1.InitializeTestData();
            matrix2.InitializeTestData();

            // Modify one word in matrix2
            string originalWord = matrix2.ReadWord(0);
            string modifiedWord = InvertFirstBit(originalWord);
            matrix2.WriteWord(0, modifiedWord);

            // Act & Assert
            Assert.IsFalse(Utilities.CompareMatrices(matrix1, matrix2));
        }

        #endregion

        #region BackupMatrix and RestoreMatrix Tests

        [Test]
        public void BackupMatrix_CreatesExactCopy()
        {
            // Act
            var backup = Utilities.BackupMatrix(matrix);

            // Assert
            Assert.IsTrue(Utilities.CompareMatrices(matrix, backup));
            Assert.AreNotSame(matrix, backup); // Different instances
        }

        [Test]
        public void RestoreMatrix_RestoresOriginalState()
        {
            // Arrange
            var backup = Utilities.BackupMatrix(matrix);
            var modifiedMatrix = new DiagonalMatrix();
            Utilities.FillMatrixWithRandomData(modifiedMatrix);

            // Act
            Utilities.RestoreMatrix(modifiedMatrix, backup);

            // Assert
            Assert.IsTrue(Utilities.CompareMatrices(matrix, modifiedMatrix));
        }

        [Test]
        public void BackupAndRestore_FullCycle()
        {
            // Arrange
            var originalMatrix = new DiagonalMatrix();
            originalMatrix.InitializeTestData();
            var backup = Utilities.BackupMatrix(originalMatrix);

            // Modify original matrix
            Utilities.FillMatrixWithRandomData(originalMatrix);
            Assert.IsFalse(Utilities.CompareMatrices(originalMatrix, backup));

            // Act - Restore from backup
            Utilities.RestoreMatrix(originalMatrix, backup);

            // Assert
            Assert.IsTrue(Utilities.CompareMatrices(originalMatrix, backup));
        }

        #endregion

        #region IsValidIndex Tests

        [Test]
        public void IsValidIndex_WithValidIndices_ReturnsTrue()
        {
            // Act & Assert
            for (int i = 0; i < 16; i++)
            {
                Assert.IsTrue(Utilities.IsValidIndex(i));
            }
        }

        [Test]
        public void IsValidIndex_WithInvalidIndices_ReturnsFalse()
        {
            // Arrange
            int[] invalidIndices = { -1, -10, 16, 17, 100, int.MinValue, int.MaxValue };

            // Act & Assert
            foreach (int index in invalidIndices)
            {
                Assert.IsFalse(Utilities.IsValidIndex(index));
            }
        }

        #endregion

        #region TryGetValidIndex Tests

        [Test]
        public void TryGetValidIndex_WithValidStringIndices_ReturnsTrueAndCorrectIndex()
        {
            // Arrange
            string[] validInputs = { "0", "1", "5", "10", "15" };

            foreach (string input in validInputs)
            {
                // Act
                bool result = Utilities.TryGetValidIndex(input, out int index);

                // Assert
                Assert.IsTrue(result);
                Assert.AreEqual(int.Parse(input), index);
            }
        }

        [Test]
        public void TryGetValidIndex_WithInvalidStringIndices_ReturnsFalse()
        {
            // Arrange
            string[] invalidInputs = { "-1", "16", "abc", "", null, "1.5", "10a" };

            foreach (string input in invalidInputs)
            {
                // Act
                bool result = Utilities.TryGetValidIndex(input, out int index);

                // Assert
                Assert.IsFalse(result);
            }
        }

        #endregion

        #region FormatBinaryString Tests

        [Test]
        public void FormatBinaryString_WithDefaultGroupSize_FormatsCorrectly()
        {
            // Arrange
            string input = "1010110011001100";

            // Act
            string result = Utilities.FormatBinaryString(input);

            // Assert
            Assert.AreEqual("1010 1100 1100 1100", result);
        }

        [Test]
        public void FormatBinaryString_WithCustomGroupSize_FormatsCorrectly()
        {
            // Arrange
            string input = "101011001100";

            // Act
            string result = Utilities.FormatBinaryString(input, 3);

            // Assert
            Assert.AreEqual("101 011 001 100", result);
        }

        [Test]
        public void FormatBinaryString_WithEmptyString_ReturnsEmpty()
        {
            // Act
            string result = Utilities.FormatBinaryString("");

            // Assert
            Assert.AreEqual("", result);
        }

        [Test]
        public void FormatBinaryString_WithNull_ReturnsEmpty()
        {
            // Act
            string result = Utilities.FormatBinaryString(null);

            // Assert
            Assert.AreEqual("", result);
        }

        [Test]
        public void FormatBinaryString_WithStringShorterThanGroupSize_ReturnsOriginal()
        {
            // Arrange
            string input = "101";

            // Act
            string result = Utilities.FormatBinaryString(input, 4);

            // Assert
            Assert.AreEqual("101", result);
        }

        [Test]
        public void FormatBinaryString_WithGroupSizeOne_AddsSpaceBetweenEachBit()
        {
            // Arrange
            string input = "1010";

            // Act
            string result = Utilities.FormatBinaryString(input, 1);

            // Assert
            Assert.AreEqual("1 0 1 0", result);
        }

        #endregion

        #region GetAllThreeBitKeys Tests

        [Test]
        public void GetAllThreeBitKeys_ReturnsAllEightKeys()
        {
            // Act
            var keys = Utilities.GetAllThreeBitKeys();

            // Assert
            Assert.AreEqual(8, keys.Count);

            var expectedKeys = new[] { "000", "001", "010", "011", "100", "101", "110", "111" };
            CollectionAssert.AreEquivalent(expectedKeys, keys);
        }

        [Test]
        public void GetAllThreeBitKeys_AllKeysAreValidThreeBitStrings()
        {
            // Act
            var keys = Utilities.GetAllThreeBitKeys();

            // Assert
            foreach (string key in keys)
            {
                Assert.AreEqual(3, key.Length);
                Assert.IsTrue(Utilities.IsValidBinaryString(key));
            }
        }

        #endregion

        #region GetKeyStatistics Tests

        [Test]
        public void GetKeyStatistics_ReturnsCorrectStatistics()
        {
            // Act
            var statistics = Utilities.GetKeyStatistics(matrix);

            // Assert
            Assert.IsNotNull(statistics);
            Assert.Greater(statistics.Count, 0);

            // Verify that all keys are 3-bit strings
            foreach (string key in statistics.Keys)
            {
                Assert.AreEqual(3, key.Length);
                Assert.IsTrue(Utilities.IsValidBinaryString(key));
            }

            // Verify that all word indices are valid
            foreach (var wordList in statistics.Values)
            {
                foreach (int wordIndex in wordList)
                {
                    Assert.IsTrue(Utilities.IsValidIndex(wordIndex));
                }
            }
        }

        [Test]
        public void GetKeyStatistics_AllWordsAccountedFor()
        {
            // Act
            var statistics = Utilities.GetKeyStatistics(matrix);

            // Assert
            int totalWords = statistics.Values.Sum(list => list.Count);
            Assert.AreEqual(16, totalWords);
        }

        #endregion

        #region PrintKeyStatistics Tests

        [Test]
        public void PrintKeyStatistics_DoesNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => Utilities.PrintKeyStatistics(matrix));
        }

        #endregion

        #region ValidateWordStructure Tests

        [Test]
        public void ValidateWordStructure_WithValidWord_ReturnsTrue()
        {
            // Arrange
            string validWord = "1010110011001100";

            // Act & Assert
            Assert.IsTrue(Utilities.ValidateWordStructure(validWord));
        }

        [Test]
        public void ValidateWordStructure_WithInvalidLength_ReturnsFalse()
        {
            // Arrange
            string[] invalidWords = { "", "1", "101011001100110", "10101100110011001" };

            foreach (string word in invalidWords)
            {
                // Act & Assert
                Assert.IsFalse(Utilities.ValidateWordStructure(word));
            }
        }

        [Test]
        public void ValidateWordStructure_WithInvalidCharacters_ReturnsFalse()
        {
            // Arrange
            string[] invalidWords = { "101011001100110a", "1010110011001102", "1010 11001100110" };

            foreach (string word in invalidWords)
            {
                // Act & Assert
                Assert.IsFalse(Utilities.ValidateWordStructure(word));
            }
        }

        #endregion

        #region CreateWord Tests

        [Test]
        public void CreateWord_WithValidComponents_ReturnsCorrectWord()
        {
            // Arrange
            string v = "101";
            string a = "1100";
            string b = "0011";
            string s = "11010";

            // Act
            string result = Utilities.CreateWord(v, a, b, s);

            // Assert
            Assert.AreEqual("1011100001111010", result);
            Assert.AreEqual(16, result.Length);
        }

        [Test]
        public void CreateWord_WithInvalidComponentLengths_ThrowsException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => Utilities.CreateWord("10", "1100", "0011", "11010")); // V wrong length
            Assert.Throws<ArgumentException>(() => Utilities.CreateWord("101", "110", "0011", "11010")); // A wrong length
            Assert.Throws<ArgumentException>(() => Utilities.CreateWord("101", "1100", "001", "11010")); // B wrong length
            Assert.Throws<ArgumentException>(() => Utilities.CreateWord("101", "1100", "0011", "1101")); // S wrong length
        }

        [Test]
        public void CreateWord_WithInvalidCharacters_ThrowsException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => Utilities.CreateWord("10a", "1100", "0011", "11010"));
            Assert.Throws<ArgumentException>(() => Utilities.CreateWord("101", "11c0", "0011", "11010"));
            Assert.Throws<ArgumentException>(() => Utilities.CreateWord("101", "1100", "00x1", "11010"));
            Assert.Throws<ArgumentException>(() => Utilities.CreateWord("101", "1100", "0011", "1101y"));
        }

        #endregion

        #region ParseWord Tests

        [Test]
        public void ParseWord_WithValidWord_ReturnsCorrectComponents()
        {
            // Arrange
            string word = "1011100001111010";

            // Act
            var (V, A, B, S) = Utilities.ParseWord(word);

            // Assert
            Assert.AreEqual("101", V);
            Assert.AreEqual("1100", A);
            Assert.AreEqual("0011", B);
            Assert.AreEqual("11010", S);
        }

        [Test]
        public void ParseWord_WithInvalidWord_ThrowsException()
        {
            // Arrange
            string[] invalidWords = { "", "101110000111101", "10111000011110101", "101110000111101a" };

            foreach (string word in invalidWords)
            {
                // Act & Assert
                Assert.Throws<ArgumentException>(() => Utilities.ParseWord(word));
            }
        }

        [Test]
        public void CreateWordAndParseWord_RoundTrip_ReturnsOriginalComponents()
        {
            // Arrange
            string originalV = "110";
            string originalA = "1010";
            string originalB = "0101";
            string originalS = "11100";

            // Act
            string word = Utilities.CreateWord(originalV, originalA, originalB, originalS);
            var (V, A, B, S) = Utilities.ParseWord(word);

            // Assert
            Assert.AreEqual(originalV, V);
            Assert.AreEqual(originalA, A);
            Assert.AreEqual(originalB, B);
            Assert.AreEqual(originalS, S);
        }

        #endregion

        #region Helper Methods

        private string InvertFirstBit(string binary)
        {
            if (string.IsNullOrEmpty(binary))
                return binary;

            char[] chars = binary.ToCharArray();
            chars[0] = chars[0] == '0' ? '1' : '0';
            return new string(chars);
        }

        #endregion

        #region Edge Cases and Integration Tests

        [Test]
        public void Utilities_WorksWithEmptyMatrix()
        {
            // Arrange
            var emptyMatrix = new DiagonalMatrix();

            // Act & Assert - Should not throw
            Assert.DoesNotThrow(() => {
                var statistics = Utilities.GetKeyStatistics(emptyMatrix);
                var backup = Utilities.BackupMatrix(emptyMatrix);
                string export = Utilities.ExportMatrixToString(emptyMatrix);
            });
        }

        [Test]
        public void Utilities_WorksWithModifiedMatrix()
        {
            // Arrange
            matrix.WriteWord(0, "1111000011110000");
            matrix.WriteWord(15, "0000111100001111");

            // Act & Assert - Should work with modified data
            var statistics = Utilities.GetKeyStatistics(matrix);
            var backup = Utilities.BackupMatrix(matrix);
            string export = Utilities.ExportMatrixToString(matrix);

            Assert.IsNotNull(statistics);
            Assert.IsNotNull(backup);
            Assert.IsNotEmpty(export);
        }

        [Test]
        public void GenerateRandomBinaryString_WithLargeLength_Works()
        {
            // Act
            string result = Utilities.GenerateRandomBinaryString(1000);

            // Assert
            Assert.AreEqual(1000, result.Length);
            Assert.IsTrue(Utilities.IsValidBinaryString(result));
        }

        [Test]
        public void FormatBinaryString_WithVeryLongString_Works()
        {
            // Arrange
            string longString = new string('1', 100);

            // Act
            string result = Utilities.FormatBinaryString(longString, 8);

            // Assert
            Assert.IsNotEmpty(result);
            Assert.That(result, Contains.Substring(" "));
        }

        #endregion
    }
}