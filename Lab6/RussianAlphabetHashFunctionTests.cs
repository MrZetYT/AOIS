using NUnit.Framework;
using HashTableLiterature.HashFunctions;

namespace HashTableLiterature.Tests
{
    [TestFixture]
    public class RussianAlphabetHashFunctionTests
    {
        private RussianAlphabetHashFunction hashFunction;

        [SetUp]
        public void SetUp()
        {
            hashFunction = new RussianAlphabetHashFunction();
        }

        [Test]
        public void GetKeyValue_WithRussianSingleChar_ReturnsCorrectValue()
        {
            // Arrange
            string key = "А";

            // Act
            int result = hashFunction.GetKeyValue(key);

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetKeyValue_WithRussianTwoChars_ReturnsCorrectValue()
        {
            // Arrange
            string key = "АБ";

            // Act
            int result = hashFunction.GetKeyValue(key);

            // Assert
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GetKeyValue_WithMetafora_ReturnsCorrectValue()
        {
            // Arrange
            string key = "Метафора";

            // Act
            int result = hashFunction.GetKeyValue(key);

            // Assert
            Assert.AreEqual(401, result);
        }

        [Test]
        public void GetKeyValue_WithEnglishChars_ReturnsCorrectValue()
        {
            // Arrange
            string key = "AB";

            // Act
            int result = hashFunction.GetKeyValue(key);

            // Assert
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GetKeyValue_WithLowerCase_ConvertsToUpperCase()
        {
            // Arrange
            string lowerKey = "метафора";
            string upperKey = "МЕТАФОРА";

            // Act
            int lowerResult = hashFunction.GetKeyValue(lowerKey);
            int upperResult = hashFunction.GetKeyValue(upperKey);

            // Assert
            Assert.AreEqual(upperResult, lowerResult);
        }

        [Test]
        public void GetKeyValue_WithNullKey_ReturnsZero()
        {
            // Act
            int result = hashFunction.GetKeyValue(null);

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetKeyValue_WithEmptyKey_ReturnsZero()
        {
            // Act
            int result = hashFunction.GetKeyValue("");

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void ComputeHash_WithValidKey_ReturnsCorrectHash()
        {
            // Arrange
            string key = "Метафора";
            int tableSize = 20;

            // Act
            int hash = hashFunction.ComputeHash(key, tableSize);

            // Assert
            int expectedKeyValue = 401;
            int expectedHash = expectedKeyValue % tableSize;
            Assert.AreEqual(expectedHash, hash);
        }

        [Test]
        public void ComputeHash_WithDifferentTableSizes_ReturnsValidHash()
        {
            // Arrange
            string key = "Эпитет";

            // Act & Assert
            for (int tableSize = 1; tableSize <= 100; tableSize++)
            {
                int hash = hashFunction.ComputeHash(key, tableSize);
                Assert.IsTrue(hash >= 0 && hash < tableSize,
                    $"Hash {hash} is not in valid range [0, {tableSize})");
            }
        }

        [Test]
        public void ComputeHash_WithSameKey_ReturnsSameHash()
        {
            // Arrange
            string key = "Аллитерация";
            int tableSize = 15;

            // Act
            int hash1 = hashFunction.ComputeHash(key, tableSize);
            int hash2 = hashFunction.ComputeHash(key, tableSize);

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void ComputeHash_WithSpecialChars_HandlesGracefully()
        {
            // Arrange
            string key = "Тест123!@#";
            int tableSize = 10;

            // Act
            int hash = hashFunction.ComputeHash(key, tableSize);

            // Assert
            Assert.IsTrue(hash >= 0 && hash < tableSize);
        }

        [TestCase("А", 0)]
        [TestCase("Б", 33)]
        [TestCase("В", 66)]
        [TestCase("Я", 1023)]
        public void GetKeyValue_WithSingleRussianChars_ReturnsExpectedValues(string key, int expected)
        {
            // Act
            int result = hashFunction.GetKeyValue(key);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestCase("A", 0)]
        [TestCase("B", 33)]
        [TestCase("Z", 825)]
        public void GetKeyValue_WithSingleEnglishChars_ReturnsExpectedValues(string key, int expected)
        {
            // Act
            int result = hashFunction.GetKeyValue(key);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}