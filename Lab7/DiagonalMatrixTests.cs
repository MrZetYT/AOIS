using NUnit.Framework;
using DiagonalMatrixLab;
using System;

namespace DiagonalMatrixLab.Tests
{
    [TestFixture]
    public class DiagonalMatrixTests
    {
        private DiagonalMatrix matrix;

        [SetUp]
        public void SetUp()
        {
            matrix = new DiagonalMatrix();
        }

        [Test]
        public void Constructor_ShouldCreateEmptyMatrix()
        {
            // Arrange & Act
            var newMatrix = new DiagonalMatrix();

            // Assert
            for (int i = 0; i < 16; i++)
            {
                string word = newMatrix.ReadWord(i);
                Assert.That(word, Is.EqualTo("0000000000000000"));
            }
        }

        [Test]
        public void GetBit_ValidCoordinates_ShouldReturnCorrectValue()
        {
            // Arrange
            matrix.SetBit(5, 10, 1);

            // Act
            int result = matrix.GetBit(5, 10);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void GetBit_InvalidCoordinates_ShouldReturnZero()
        {
            // Act & Assert
            Assert.That(matrix.GetBit(-1, 5), Is.EqualTo(0));
            Assert.That(matrix.GetBit(5, -1), Is.EqualTo(0));
            Assert.That(matrix.GetBit(16, 5), Is.EqualTo(0));
            Assert.That(matrix.GetBit(5, 16), Is.EqualTo(0));
        }

        [Test]
        public void SetBit_ValidCoordinates_ShouldSetValue()
        {
            // Act
            matrix.SetBit(3, 7, 1);

            // Assert
            Assert.That(matrix.GetBit(3, 7), Is.EqualTo(1));
        }

        [Test]
        public void SetBit_InvalidCoordinates_ShouldNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => matrix.SetBit(-1, 5, 1));
            Assert.DoesNotThrow(() => matrix.SetBit(5, -1, 1));
            Assert.DoesNotThrow(() => matrix.SetBit(16, 5, 1));
            Assert.DoesNotThrow(() => matrix.SetBit(5, 16, 1));
        }

        [Test]
        public void ReadWord_ValidIndex_ShouldReturnCorrectWord()
        {
            // Arrange
            matrix.InitializeTestData();

            // Act
            string word0 = matrix.ReadWord(0);

            // Assert
            Assert.That(word0.Length, Is.EqualTo(16));
            Assert.That(word0, Is.EqualTo("1000000010010100"));
        }

        [Test]
        public void ReadWord_InvalidIndex_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => matrix.ReadWord(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => matrix.ReadWord(16));
        }

        [Test]
        public void WriteWord_ValidWord_ShouldWriteCorrectly()
        {
            // Arrange
            string testWord = "1010101010101010";

            // Act
            matrix.WriteWord(5, testWord);

            // Assert
            Assert.That(matrix.ReadWord(5), Is.EqualTo(testWord));
        }

        [Test]
        public void WriteWord_InvalidIndex_ShouldThrowException()
        {
            // Arrange
            string testWord = "1010101010101010";

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => matrix.WriteWord(-1, testWord));
            Assert.Throws<ArgumentOutOfRangeException>(() => matrix.WriteWord(16, testWord));
        }

        [Test]
        public void WriteWord_InvalidWordLength_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => matrix.WriteWord(0, "101010"));
            Assert.Throws<ArgumentException>(() => matrix.WriteWord(0, "10101010101010101"));
        }

        [Test]
        public void ReadColumn_ValidIndex_ShouldReturnCorrectColumn()
        {
            // Arrange
            matrix.InitializeTestData();

            // Act
            string column0 = matrix.ReadColumn(0);

            // Assert
            Assert.That(column0.Length, Is.EqualTo(16));
            Assert.That(column0, Is.EqualTo("1111100001110000"));
        }

        [Test]
        public void ReadColumn_InvalidIndex_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => matrix.ReadColumn(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => matrix.ReadColumn(16));
        }

        [Test]
        public void WriteColumn_ValidColumn_ShouldWriteCorrectly()
        {
            // Arrange
            string testColumn = "1111000011110000";

            // Act
            matrix.WriteColumn(3, testColumn);

            // Assert
            Assert.That(matrix.ReadColumn(3), Is.EqualTo(testColumn));
        }

        [Test]
        public void WriteColumn_InvalidIndex_ShouldThrowException()
        {
            // Arrange
            string testColumn = "1111000011110000";

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => matrix.WriteColumn(-1, testColumn));
            Assert.Throws<ArgumentOutOfRangeException>(() => matrix.WriteColumn(16, testColumn));
        }

        [Test]
        public void WriteColumn_InvalidColumnLength_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => matrix.WriteColumn(0, "10101"));
            Assert.Throws<ArgumentException>(() => matrix.WriteColumn(0, "1010101010101010101"));
        }

        [Test]
        public void BinaryToDecimal_ValidBinary_ShouldReturnCorrectValue()
        {
            // Act & Assert
            Assert.That(DiagonalMatrix.BinaryToDecimal("1010"), Is.EqualTo(10));
            Assert.That(DiagonalMatrix.BinaryToDecimal("1111"), Is.EqualTo(15));
            Assert.That(DiagonalMatrix.BinaryToDecimal("0000"), Is.EqualTo(0));
            Assert.That(DiagonalMatrix.BinaryToDecimal("1"), Is.EqualTo(1));
        }

        [Test]
        public void DecimalToBinary_ValidInput_ShouldReturnCorrectBinary()
        {
            // Act & Assert
            Assert.That(DiagonalMatrix.DecimalToBinary(10, 4), Is.EqualTo("1010"));
            Assert.That(DiagonalMatrix.DecimalToBinary(15, 4), Is.EqualTo("1111"));
            Assert.That(DiagonalMatrix.DecimalToBinary(0, 4), Is.EqualTo("0000"));
            Assert.That(DiagonalMatrix.DecimalToBinary(5, 8), Is.EqualTo("00000101"));
        }

        [Test]
        public void InitializeTestData_ShouldLoadCorrectData()
        {
            // Act
            matrix.InitializeTestData();

            // Assert
            // ѕровер€ем несколько известных значений из тестовых данных
            Assert.That(matrix.GetBit(0, 0), Is.EqualTo(1));
            Assert.That(matrix.GetBit(1, 8), Is.EqualTo(1));
            Assert.That(matrix.GetBit(2, 1), Is.EqualTo(1));
            Assert.That(matrix.GetBit(15, 15), Is.EqualTo(0));
        }

        [Test]
        public void GetMatrix_ShouldReturnCopyOfMatrix()
        {
            // Arrange
            matrix.InitializeTestData();

            // Act
            int[,] matrixCopy = matrix.GetMatrix();

            // Assert
            Assert.That(matrixCopy, Is.Not.Null);
            Assert.That(matrixCopy.GetLength(0), Is.EqualTo(16));
            Assert.That(matrixCopy.GetLength(1), Is.EqualTo(16));

            // ѕровер€ем, что это копи€, а не ссылка
            matrixCopy[0, 0] = 999; // »змен€ем копию
            Assert.That(matrix.GetBit(0, 0), Is.Not.EqualTo(999)); // ќригинал не изменилс€
        }

        [Test]
        public void PrintMatrix_ShouldNotThrow()
        {
            // Arrange
            matrix.InitializeTestData();

            // Act & Assert
            Assert.DoesNotThrow(() => matrix.PrintMatrix());
        }

        [Test]
        public void MultipleWordsWriteRead_ShouldMaintainIntegrity()
        {
            // Arrange
            string[] testWords = {
                "1010101010101010",
                "1111000011110000",
                "0000111100001111",
                "1100110011001100"
            };

            // Act
            for (int i = 0; i < testWords.Length; i++)
            {
                matrix.WriteWord(i, testWords[i]);
            }

            // Assert
            for (int i = 0; i < testWords.Length; i++)
            {
                Assert.That(matrix.ReadWord(i), Is.EqualTo(testWords[i]));
            }
        }

        [Test]
        public void BinaryConversion_EdgeCases_ShouldHandleCorrectly()
        {
            // Test максимальные значени€ дл€ разных битовых длин
            Assert.That(DiagonalMatrix.DecimalToBinary(31, 5), Is.EqualTo("11111"));
            Assert.That(DiagonalMatrix.DecimalToBinary(15, 4), Is.EqualTo("1111"));
            Assert.That(DiagonalMatrix.DecimalToBinary(7, 3), Is.EqualTo("111"));

            // Test минимальные значени€
            Assert.That(DiagonalMatrix.DecimalToBinary(0, 5), Is.EqualTo("00000"));
            Assert.That(DiagonalMatrix.DecimalToBinary(0, 1), Is.EqualTo("0"));

            // Test обратна€ конвертаци€
            Assert.That(DiagonalMatrix.BinaryToDecimal("11111"), Is.EqualTo(31));
            Assert.That(DiagonalMatrix.BinaryToDecimal("00000"), Is.EqualTo(0));
        }
    }
}