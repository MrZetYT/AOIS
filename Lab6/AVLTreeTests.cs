using NUnit.Framework;
using HashTableLiterature.DataStructures;
using System.Collections.Generic;
using System.Linq;

namespace HashTableLiterature.Tests
{
    [TestFixture]
    public class AVLTreeTests
    {
        private AVLTree tree;

        [SetUp]
        public void SetUp()
        {
            tree = new AVLTree();
        }

        [Test]
        public void Insert_SingleElement_StoresCorrectly()
        {
            // Arrange
            string key = "Метафора";
            string value = "Скрытое сравнение";

            // Act
            tree.Insert(key, value);

            // Assert
            string result = tree.Search(key);
            Assert.AreEqual(value, result);
            Assert.AreEqual(1, tree.Count());
        }

        [Test]
        public void Insert_MultipleElements_StoresCorrectly()
        {
            // Arrange
            var testData = new Dictionary<string, string>
            {
                {"Метафора", "Скрытое сравнение"},
                {"Эпитет", "Образное определение"},
                {"Гипербола", "Художественное преувеличение"}
            };

            // Act
            foreach (var item in testData)
            {
                tree.Insert(item.Key, item.Value);
            }

            // Assert
            foreach (var item in testData)
            {
                Assert.AreEqual(item.Value, tree.Search(item.Key));
            }
            Assert.AreEqual(testData.Count, tree.Count());
        }

        [Test]
        public void Insert_DuplicateKey_UpdatesValue()
        {
            // Arrange
            string key = "Метафора";
            string oldValue = "Старое определение";
            string newValue = "Новое определение";

            // Act
            tree.Insert(key, oldValue);
            tree.Insert(key, newValue);

            // Assert
            Assert.AreEqual(newValue, tree.Search(key));
            Assert.AreEqual(1, tree.Count());
        }

        [Test]
        public void Search_ExistingKey_ReturnsCorrectValue()
        {
            // Arrange
            tree.Insert("Аллитерация", "Повторение согласных");
            tree.Insert("Анафора", "Повторение в начале");
            tree.Insert("Эпитет", "Образное определение");

            // Act
            string result = tree.Search("Анафора");

            // Assert
            Assert.AreEqual("Повторение в начале", result);
        }

        [Test]
        public void Search_NonExistingKey_ReturnsNull()
        {
            // Arrange
            tree.Insert("Метафора", "Скрытое сравнение");

            // Act
            string result = tree.Search("НесуществующийТермин");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Search_EmptyTree_ReturnsNull()
        {
            // Act
            string result = tree.Search("Любой термин");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Delete_ExistingKey_RemovesSuccessfully()
        {
            // Arrange
            tree.Insert("Метафора", "Скрытое сравнение");
            tree.Insert("Эпитет", "Образное определение");
            tree.Insert("Гипербола", "Художественное преувеличение");

            // Act
            bool result = tree.Delete("Эпитет");

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(tree.Search("Эпитет"));
            Assert.AreEqual(2, tree.Count());
            Assert.IsNotNull(tree.Search("Метафора"));
            Assert.IsNotNull(tree.Search("Гипербола"));
        }

        [Test]
        public void Delete_NonExistingKey_ReturnsFalse()
        {
            // Arrange
            tree.Insert("Метафора", "Скрытое сравнение");

            // Act
            bool result = tree.Delete("НесуществующийТермин");

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(1, tree.Count());
        }

        [Test]
        public void Delete_EmptyTree_ReturnsFalse()
        {
            // Act
            bool result = tree.Delete("Любой термин");

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, tree.Count());
        }

        [Test]
        public void Delete_LastElement_MakesTreeEmpty()
        {
            // Arrange
            tree.Insert("Единственный", "Единственное значение");

            // Act
            bool result = tree.Delete("Единственный");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, tree.Count());
            Assert.IsNull(tree.Search("Единственный"));
        }

        [Test]
        public void GetAllKeys_EmptyTree_ReturnsEmptyList()
        {
            // Act
            List<string> keys = tree.GetAllKeys();

            // Assert
            Assert.IsNotNull(keys);
            Assert.AreEqual(0, keys.Count);
        }

        [Test]
        public void GetAllKeys_WithElements_ReturnsAllKeysInOrder()
        {
            // Arrange
            var testKeys = new[] { "Гипербола", "Анафора", "Эпитет", "Метафора", "Аллитерация" };
            foreach (var key in testKeys)
            {
                tree.Insert(key, $"Определение {key}");
            }

            // Act
            List<string> keys = tree.GetAllKeys();

            // Assert
            Assert.AreEqual(testKeys.Length, keys.Count);

            var sortedExpected = testKeys.OrderBy(k => k).ToList();
            CollectionAssert.AreEqual(sortedExpected, keys);
        }

        [Test]
        public void Count_EmptyTree_ReturnsZero()
        {
            // Act
            int count = tree.Count();

            // Assert
            Assert.AreEqual(0, count);
        }

        [Test]
        public void Count_WithElements_ReturnsCorrectCount()
        {
            // Arrange
            var testData = new[]
            {
                ("Метафора", "Определение 1"),
                ("Эпитет", "Определение 2"),
                ("Гипербола", "Определение 3"),
                ("Аллитерация", "Определение 4"),
                ("Анафора", "Определение 5")
            };

            // Act
            foreach (var (key, value) in testData)
            {
                tree.Insert(key, value);
            }

            // Assert
            Assert.AreEqual(testData.Length, tree.Count());
        }

        [Test]
        public void AVLTree_MaintainsBalance_WithSequentialInserts()
        {
            // Arrange
            var keys = new[] { "А", "Б", "В", "Г", "Д", "Е", "Ж", "З" };

            // Act
            foreach (var key in keys)
            {
                tree.Insert(key, $"Значение {key}");
            }

            // Assert
            foreach (var key in keys)
            {
                Assert.IsNotNull(tree.Search(key), $"Ключ {key} должен быть найден");
            }
            Assert.AreEqual(keys.Length, tree.Count());
        }

        [Test]
        public void AVLTree_MaintainsBalance_WithReverseSequentialInserts()
        {
            // Arrange
            var keys = new[] { "З", "Ж", "Е", "Д", "Г", "В", "Б", "А" };

            // Act
            foreach (var key in keys)
            {
                tree.Insert(key, $"Значение {key}");
            }

            // Assert
            foreach (var key in keys)
            {
                Assert.IsNotNull(tree.Search(key), $"Ключ {key} должен быть найден");
            }
            Assert.AreEqual(keys.Length, tree.Count());
        }

        [Test]
        public void ComplexOperations_InsertSearchDeleteUpdate_WorksCorrectly()
        {
            // Arrange
            var initialData = new Dictionary<string, string>
            {
                {"Метафора", "Определение 1"},
                {"Эпитет", "Определение 2"},
                {"Гипербола", "Определение 3"}
            };

            // Act & Assert
            foreach (var item in initialData)
            {
                tree.Insert(item.Key, item.Value);
            }
            Assert.AreEqual(3, tree.Count());

            // Act & Assert
            foreach (var item in initialData)
            {
                Assert.AreEqual(item.Value, tree.Search(item.Key));
            }

            // Act & Assert
            tree.Insert("Метафора", "Новое определение метафоры");
            Assert.AreEqual("Новое определение метафоры", tree.Search("Метафора"));
            Assert.AreEqual(3, tree.Count());

            // Act & Assert
            Assert.IsTrue(tree.Delete("Эпитет"));
            Assert.IsNull(tree.Search("Эпитет"));
            Assert.AreEqual(2, tree.Count());

            Assert.AreEqual("Новое определение метафоры", tree.Search("Метафора"));
            Assert.AreEqual("Определение 3", tree.Search("Гипербола"));
        }

        [Test]
        public void GetAllKeys_AfterMultipleOperations_ReturnsCorrectKeys()
        {
            // Arrange & Act
            tree.Insert("Д", "4");
            tree.Insert("Б", "2");
            tree.Insert("Е", "5");
            tree.Insert("А", "1");
            tree.Insert("В", "3");
            tree.Delete("Б");
            tree.Insert("Ж", "6");

            // Act
            var keys = tree.GetAllKeys();

            // Assert
            var expected = new[] { "А", "В", "Д", "Е", "Ж" };
            CollectionAssert.AreEqual(expected, keys);
        }
    }
}