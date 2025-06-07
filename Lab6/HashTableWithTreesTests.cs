using NUnit.Framework;
using HashTableLiterature.Core;
using HashTableLiterature.HashFunctions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HashTableLiterature.Tests
{
    [TestFixture]
    public class HashTableWithTreesTests
    {
        private HashTableWithTrees hashTable;
        private RussianAlphabetHashFunction hashFunction;

        [SetUp]
        public void SetUp()
        {
            hashFunction = new RussianAlphabetHashFunction();
            hashTable = new HashTableWithTrees(20, hashFunction);
        }

        [Test]
        public void Constructor_WithNullHashFunction_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new HashTableWithTrees(10, null));
        }

        [Test]
        public void Constructor_WithValidParameters_CreatesHashTable()
        {
            // Act
            var table = new HashTableWithTrees(10, hashFunction);

            // Assert
            Assert.IsNotNull(table);
            Assert.AreEqual(0, table.LoadFactor());
        }

        [Test]
        public void Insert_ValidKeyValue_ReturnsTrue()
        {
            // Arrange
            string key = "Метафора";
            string value = "Скрытое сравнение";

            // Act
            bool result = hashTable.Insert(key, value);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(value, hashTable.Search(key));
        }

        [Test]
        public void Insert_NullKey_ReturnsFalse()
        {
            // Act
            bool result = hashTable.Insert(null, "Некоторое значение");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Insert_EmptyKey_ReturnsFalse()
        {
            // Act
            bool result = hashTable.Insert("", "Некоторое значение");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Insert_DuplicateKey_ReturnsFalse()
        {
            // Arrange
            string key = "Метафора";
            hashTable.Insert(key, "Первое значение");

            // Act
            bool result = hashTable.Insert(key, "Второе значение");

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("Первое значение", hashTable.Search(key));
        }

        [Test]
        public void Insert_MultipleElements_StoresCorrectly()
        {
            // Arrange
            var testData = new Dictionary<string, string>
            {
                {"Метафора", "Скрытое сравнение"},
                {"Эпитет", "Образное определение"},
                {"Гипербола", "Художественное преувеличение"},
                {"Аллитерация", "Повторение согласных"},
                {"Анафора", "Повторение в начале"}
            };

            // Act
            foreach (var item in testData)
            {
                bool result = hashTable.Insert(item.Key, item.Value);
                Assert.IsTrue(result);
            }

            // Assert
            foreach (var item in testData)
            {
                Assert.AreEqual(item.Value, hashTable.Search(item.Key));
            }
        }

        [Test]
        public void Search_ExistingKey_ReturnsCorrectValue()
        {
            // Arrange
            string key = "Эпитет";
            string value = "Образное определение";
            hashTable.Insert(key, value);

            // Act
            string result = hashTable.Search(key);

            // Assert
            Assert.AreEqual(value, result);
        }

        [Test]
        public void Search_NonExistingKey_ReturnsNull()
        {
            // Arrange
            hashTable.Insert("Метафора", "Скрытое сравнение");

            // Act
            string result = hashTable.Search("НесуществующийТермин");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Search_NullKey_ReturnsNull()
        {
            // Act
            string result = hashTable.Search(null);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Search_EmptyKey_ReturnsNull()
        {
            // Act
            string result = hashTable.Search("");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Update_ExistingKey_ReturnsTrue()
        {
            // Arrange
            string key = "Метафора";
            string oldValue = "Старое определение";
            string newValue = "Новое определение";
            hashTable.Insert(key, oldValue);

            // Act
            bool result = hashTable.Update(key, newValue);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newValue, hashTable.Search(key));
        }

        [Test]
        public void Update_NonExistingKey_ReturnsFalse()
        {
            // Act
            bool result = hashTable.Update("НесуществующийТермин", "Новое значение");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Update_NullKey_ReturnsFalse()
        {
            // Act
            bool result = hashTable.Update(null, "Новое значение");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Update_EmptyKey_ReturnsFalse()
        {
            // Act
            bool result = hashTable.Update("", "Новое значение");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Delete_ExistingKey_ReturnsTrue()
        {
            // Arrange
            string key = "Метафора";
            hashTable.Insert(key, "Скрытое сравнение");

            // Act
            bool result = hashTable.Delete(key);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(hashTable.Search(key));
        }

        [Test]
        public void Delete_NonExistingKey_ReturnsFalse()
        {
            // Act
            bool result = hashTable.Delete("НесуществующийТермин");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Delete_NullKey_ReturnsFalse()
        {
            // Act
            bool result = hashTable.Delete(null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Delete_EmptyKey_ReturnsFalse()
        {
            // Act
            bool result = hashTable.Delete("");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void LoadFactor_EmptyTable_ReturnsZero()
        {
            // Act
            double loadFactor = hashTable.LoadFactor();

            // Assert
            Assert.AreEqual(0.0, loadFactor, 0.001);
        }

        [Test]
        public void LoadFactor_WithElements_ReturnsCorrectValue()
        {
            // Arrange
            int elementCount = 5;
            int tableSize = 20;

            for (int i = 0; i < elementCount; i++)
            {
                hashTable.Insert($"Ключ{i}", $"Значение{i}");
            }

            // Act
            double loadFactor = hashTable.LoadFactor();

            // Assert
            double expected = (double)elementCount / tableSize;
            Assert.AreEqual(expected, loadFactor, 0.001);
        }

        [Test]
        public void GetAllElements_EmptyTable_ReturnsEmptyList()
        {
            // Act
            var elements = hashTable.GetAllElements();

            // Assert
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [Test]
        public void GetAllElements_WithElements_ReturnsAllElements()
        {
            // Arrange
            var testData = new Dictionary<string, string>
            {
                {"Метафора", "Скрытое сравнение"},
                {"Эпитет", "Образное определение"},
                {"Гипербола", "Художественное преувеличение"}
            };

            foreach (var item in testData)
            {
                hashTable.Insert(item.Key, item.Value);
            }

            // Act
            var elements = hashTable.GetAllElements();

            // Assert
            Assert.AreEqual(testData.Count, elements.Count);

            foreach (var element in elements)
            {
                Assert.IsTrue(testData.ContainsKey(element.Key));
                Assert.AreEqual(testData[element.Key], element.Value);
            }
        }

        [Test]
        public void HashTable_HandlesCollisions_Correctly()
        {
            // Arrange
            var smallHashTable = new HashTableWithTrees(3, hashFunction);

            var testData = new Dictionary<string, string>
            {
                {"А", "Первый"},
                {"Б", "Второй"},
                {"В", "Третий"},
                {"Г", "Четвертый"},
                {"Д", "Пятый"}
            };

            // Act
            foreach (var item in testData)
            {
                bool result = smallHashTable.Insert(item.Key, item.Value);
                Assert.IsTrue(result);
            }

            // Assert
            foreach (var item in testData)
            {
                Assert.AreEqual(item.Value, smallHashTable.Search(item.Key));
            }

            Assert.AreEqual((double)testData.Count / 3, smallHashTable.LoadFactor(), 0.001);
        }

        [Test]
        public void ComplexOperations_InsertUpdateDeleteSearch_WorkCorrectly()
        {
            // Arrange
            var initialData = new Dictionary<string, string>
            {
                {"Метафора", "Определение 1"},
                {"Эпитет", "Определение 2"},
                {"Гипербола", "Определение 3"},
                {"Аллитерация", "Определение 4"}
            };

            // Act & Assert
            foreach (var item in initialData)
            {
                Assert.IsTrue(hashTable.Insert(item.Key, item.Value));
            }

            // Act & Assert
            Assert.IsTrue(hashTable.Update("Метафора", "Обновленное определение метафоры"));
            Assert.AreEqual("Обновленное определение метафоры", hashTable.Search("Метафора"));

            // Act & Assert
            Assert.IsTrue(hashTable.Delete("Эпитет"));
            Assert.IsNull(hashTable.Search("Эпитет"));

            // Act & Assert
            Assert.AreEqual("Обновленное определение метафоры", hashTable.Search("Метафора"));
            Assert.AreEqual("Определение 3", hashTable.Search("Гипербола"));
            Assert.AreEqual("Определение 4", hashTable.Search("Аллитерация"));

            // Act & Assert
            double expectedLoadFactor = 3.0 / 20.0;
            Assert.AreEqual(expectedLoadFactor, hashTable.LoadFactor(), 0.001);
        }

        [Test]
        public void HashTable_WithRealLiteraryTerms_WorksCorrectly()
        {
            // Arrange
            var literaryTerms = new Dictionary<string, string>
            {
                {"Метафора", "Скрытое сравнение, основанное на сходстве предметов"},
                {"Эпитет", "Образное определение, подчеркивающее свойство предмета"},
                {"Аллитерация", "Повторение одинаковых согласных звуков"},
                {"Гипербола", "Художественное преувеличение"},
                {"Синекдоха", "Замещение целого частью или наоборот"},
                {"Олицетворение", "Наделение неживых предметов человеческими качествами"},
                {"Оксюморон", "Сочетание противоречащих друг другу понятий"},
                {"Анафора", "Повторение слов в начале смежных отрезков речи"}
            };

            // Act
            foreach (var term in literaryTerms)
            {
                bool insertResult = hashTable.Insert(term.Key, term.Value);
                Assert.IsTrue(insertResult, $"Не удалось вставить термин: {term.Key}");
            }

            // Assert
            foreach (var term in literaryTerms)
            {
                string found = hashTable.Search(term.Key);
                Assert.AreEqual(term.Value, found, $"Неверное определение для термина: {term.Key}");
            }

            // Assert
            var allElements = hashTable.GetAllElements();
            Assert.AreEqual(literaryTerms.Count, allElements.Count);

            // Assert
            double expectedLoadFactor = (double)literaryTerms.Count / 20;
            Assert.AreEqual(expectedLoadFactor, hashTable.LoadFactor(), 0.001);
        }

        [Test]
        public void HashTable_StressTest_HandlesLargeDataSet()
        {
            // Arrange
            var largeHashTable = new HashTableWithTrees(100, hashFunction);
            int elementCount = 200;

            // Act
            for (int i = 0; i < elementCount; i++)
            {
                string key = $"Термин{i}";
                string value = $"Определение термина номер {i}";
                bool result = largeHashTable.Insert(key, value);
                Assert.IsTrue(result, $"Не удалось вставить элемент {i}");
            }

            // Assert
            for (int i = 0; i < elementCount; i++)
            {
                string key = $"Термин{i}";
                string expectedValue = $"Определение термина номер {i}";
                string actualValue = largeHashTable.Search(key);
                Assert.AreEqual(expectedValue, actualValue, $"Неверное значение для ключа {key}");
            }

            // Assert
            Assert.AreEqual(2.0, largeHashTable.LoadFactor(), 0.01);
        }

        [Test]
        public void HashTable_OperationsAfterDeletion_WorkCorrectly()
        {
            // Arrange
            var testData = new[]
            {
                ("Ключ1", "Значение1"),
                ("Ключ2", "Значение2"),
                ("Ключ3", "Значение3"),
                ("Ключ4", "Значение4"),
                ("Ключ5", "Значение5")
            };

            foreach (var (key, value) in testData)
            {
                hashTable.Insert(key, value);
            }

            // Act
            Assert.IsTrue(hashTable.Delete("Ключ3"));

            // Assert
            Assert.AreEqual("Значение1", hashTable.Search("Ключ1"));
            Assert.AreEqual("Значение2", hashTable.Search("Ключ2"));
            Assert.IsNull(hashTable.Search("Ключ3"));
            Assert.AreEqual("Значение4", hashTable.Search("Ключ4"));
            Assert.AreEqual("Значение5", hashTable.Search("Ключ5"));

            // Act
            Assert.IsTrue(hashTable.Insert("НовыйКлюч", "НовоеЗначение"));
            Assert.AreEqual("НовоеЗначение", hashTable.Search("НовыйКлюч"));

            // Act
            Assert.IsTrue(hashTable.Insert("Ключ3", "ВосстановленноеЗначение"));
            Assert.AreEqual("ВосстановленноеЗначение", hashTable.Search("Ключ3"));
        }
    }
}