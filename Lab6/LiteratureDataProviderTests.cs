using NUnit.Framework;
using HashTableLiterature.Data;
using System.Collections.Generic;
using System.Linq;

namespace HashTableLiterature.Tests
{
    [TestFixture]
    public class LiteratureDataProviderTests
    {
        [Test]
        public void GetLiteraryTerms_ReturnsNonEmptyDictionary()
        {
            // Act
            var terms = LiteratureDataProvider.GetLiteraryTerms();

            // Assert
            Assert.IsNotNull(terms);
            Assert.IsTrue(terms.Count > 0);
        }

        [Test]
        public void GetLiteraryTerms_ContainsExpectedTerms()
        {
            // Act
            var terms = LiteratureDataProvider.GetLiteraryTerms();

            // Assert
            var expectedTerms = new[]
            {
                "Метафора", "Эпитет", "Аллитерация", "Гипербола", "Синекдоха",
                "Олицетворение", "Оксюморон", "Анафора", "Антитеза", "Градация",
                "Литота", "Перифраза", "Ирония", "Сарказм", "Аллегория",
                "Символ", "Рефрен", "Строфа", "Ямб", "Хорей"
            };

            foreach (var expectedTerm in expectedTerms)
            {
                Assert.IsTrue(terms.ContainsKey(expectedTerm),
                    $"Словарь не содержит ожидаемый термин: {expectedTerm}");
            }
        }

        [Test]
        public void GetLiteraryTerms_AllTermsHaveDefinitions()
        {
            // Act
            var terms = LiteratureDataProvider.GetLiteraryTerms();

            // Assert
            foreach (var term in terms)
            {
                Assert.IsNotNull(term.Value, $"Термин '{term.Key}' не имеет определения");
                Assert.IsTrue(term.Value.Length > 0, $"Определение термина '{term.Key}' пустое");
            }
        }

        [Test]
        public void GetLiteraryTerms_SpecificTermsHaveCorrectDefinitions()
        {
            // Act
            var terms = LiteratureDataProvider.GetLiteraryTerms();

            // Assert
            Assert.AreEqual("Скрытое сравнение, основанное на сходстве предметов",
                terms["Метафора"]);
            Assert.AreEqual("Образное определение, подчеркивающее свойство предмета",
                terms["Эпитет"]);
            Assert.AreEqual("Повторение одинаковых согласных звуков",
                terms["Аллитерация"]);
            Assert.AreEqual("Художественное преувеличение",
                terms["Гипербола"]);
            Assert.AreEqual("Двусложная стопа с ударением на втором слоге",
                terms["Ямб"]);
            Assert.AreEqual("Двусложная стопа с ударением на первом слоге",
                terms["Хорей"]);
        }

        [Test]
        public void GetLiteraryTerms_ReturnsConsistentData()
        {
            // Act
            var terms1 = LiteratureDataProvider.GetLiteraryTerms();
            var terms2 = LiteratureDataProvider.GetLiteraryTerms();

            // Assert
            Assert.AreEqual(terms1.Count, terms2.Count);

            foreach (var term in terms1)
            {
                Assert.IsTrue(terms2.ContainsKey(term.Key));
                Assert.AreEqual(term.Value, terms2[term.Key]);
            }
        }

        [Test]
        public void GetLiteraryTerms_AllKeysAreValid()
        {
            // Act
            var terms = LiteratureDataProvider.GetLiteraryTerms();

            // Assert
            foreach (var key in terms.Keys)
            {
                Assert.IsNotNull(key, "Найден null ключ");
                Assert.IsTrue(key.Length > 0, "Найден пустой ключ");
                Assert.IsFalse(string.IsNullOrWhiteSpace(key), "Найден ключ из пробелов");
            }
        }

        [Test]
        public void GetLiteraryTerms_NoduplicateKeys()
        {
            // Act
            var terms = LiteratureDataProvider.GetLiteraryTerms();

            // Assert
            var keysList = terms.Keys.ToList();
            var uniqueKeys = keysList.Distinct().ToList();

            Assert.AreEqual(keysList.Count, uniqueKeys.Count, "Найдены дублирующиеся ключи");
        }

        [Test]
        public void GetTestSearchKeys_ReturnsNonEmptyArray()
        {
            // Act
            var searchKeys = LiteratureDataProvider.GetTestSearchKeys();

            // Assert
            Assert.IsNotNull(searchKeys);
            Assert.IsTrue(searchKeys.Length > 0);
        }

        [Test]
        public void GetTestSearchKeys_ContainsExpectedKeys()
        {
            // Act
            var searchKeys = LiteratureDataProvider.GetTestSearchKeys();

            // Assert
            var expectedKeys = new[] { "Метафора", "Ямб", "Аллюзия", "Эпитет", "Неологизм" };

            Assert.AreEqual(expectedKeys.Length, searchKeys.Length);

            foreach (var expectedKey in expectedKeys)
            {
                Assert.IsTrue(searchKeys.Contains(expectedKey),
                    $"Массив не содержит ожидаемый ключ: {expectedKey}");
            }
        }

        [Test]
        public void GetTestSearchKeys_ReturnsConsistentData()
        {
            // Act
            var searchKeys1 = LiteratureDataProvider.GetTestSearchKeys();
            var searchKeys2 = LiteratureDataProvider.GetTestSearchKeys();

            // Assert
            Assert.AreEqual(searchKeys1.Length, searchKeys2.Length);

            for (int i = 0; i < searchKeys1.Length; i++)
            {
                Assert.AreEqual(searchKeys1[i], searchKeys2[i]);
            }
        }

        [Test]
        public void GetTestSearchKeys_ContainsBothExistingAndNonExistingKeys()
        {
            // Arrange
            var literaryTerms = LiteratureDataProvider.GetLiteraryTerms();
            var searchKeys = LiteratureDataProvider.GetTestSearchKeys();

            // Act & Assert
            bool hasExistingKey = false;
            bool hasNonExistingKey = false;

            foreach (var key in searchKeys)
            {
                if (literaryTerms.ContainsKey(key))
                {
                    hasExistingKey = true;
                }
                else
                {
                    hasNonExistingKey = true;
                }
            }

            // Assert
            Assert.IsTrue(hasExistingKey, "Тестовые ключи должны содержать хотя бы один существующий ключ");
            Assert.IsTrue(hasNonExistingKey, "Тестовые ключи должны содержать хотя бы один несуществующий ключ");
        }

        [Test]
        public void GetTestSearchKeys_SpecificKeysPresent()
        {
            // Act
            var searchKeys = LiteratureDataProvider.GetTestSearchKeys();

            // Assert
            Assert.IsTrue(searchKeys.Contains("Метафора"), "Должен содержать 'Метафора'");
            Assert.IsTrue(searchKeys.Contains("Ямб"), "Должен содержать 'Ямб'");
            Assert.IsTrue(searchKeys.Contains("Аллюзия"), "Должен содержать 'Аллюзия'");
            Assert.IsTrue(searchKeys.Contains("Эпитет"), "Должен содержать 'Эпитет'");
            Assert.IsTrue(searchKeys.Contains("Неологизм"), "Должен содержать 'Неологизм'");
        }

        [Test]
        public void DataProvider_Integration_TestKeysWorkWithLiteraryTerms()
        {
            // Arrange
            var literaryTerms = LiteratureDataProvider.GetLiteraryTerms();
            var testKeys = LiteratureDataProvider.GetTestSearchKeys();

            // Act & Assert
            foreach (var key in testKeys)
            {
                if (literaryTerms.ContainsKey(key))
                {
                    string definition = literaryTerms[key];
                    Assert.IsNotNull(definition, $"Термин '{key}' должен иметь определение");
                    Assert.IsTrue(definition.Length > 0, $"Определение термина '{key}' не должно быть пустым");
                }
            }
        }

        [Test]
        public void GetLiteraryTerms_TermsAreInRussian()
        {
            // Act
            var terms = LiteratureDataProvider.GetLiteraryTerms();

            // Assert
            foreach (var term in terms)
            {
                bool hasRussianChars = term.Key.Any(c => c >= 'А' && c <= 'я');
                Assert.IsTrue(hasRussianChars, $"Термин '{term.Key}' должен содержать русские символы");

                bool definitionHasRussianChars = term.Value.Any(c => c >= 'А' && c <= 'я');
                Assert.IsTrue(definitionHasRussianChars, $"Определение термина '{term.Key}' должно содержать русские символы");
            }
        }

        [Test]
        public void GetLiteraryTerms_CoversVariousLiteraryAreas()
        {
            // Act
            var terms = LiteratureDataProvider.GetLiteraryTerms();

            // Assert
            Assert.IsTrue(terms.ContainsKey("Метафора"));
            Assert.IsTrue(terms.ContainsKey("Эпитет"));
            Assert.IsTrue(terms.ContainsKey("Гипербола"));
            Assert.IsTrue(terms.ContainsKey("Олицетворение"));

            Assert.IsTrue(terms.ContainsKey("Аллитерация"));
            Assert.IsTrue(terms.ContainsKey("Анафора"));
            Assert.IsTrue(terms.ContainsKey("Антитеза"));

            Assert.IsTrue(terms.ContainsKey("Ямб"));
            Assert.IsTrue(terms.ContainsKey("Хорей"));
            Assert.IsTrue(terms.ContainsKey("Строфа"));

            Assert.IsTrue(terms.ContainsKey("Ирония"));
            Assert.IsTrue(terms.ContainsKey("Сарказм"));
            Assert.IsTrue(terms.ContainsKey("Аллегория"));
        }
    }
}