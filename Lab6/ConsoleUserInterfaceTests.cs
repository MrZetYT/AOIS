using NUnit.Framework;
using HashTableLiterature.UI;
using HashTableLiterature.Core;
using HashTableLiterature.HashFunctions;
using System;
using System.IO;

namespace HashTableLiterature.Tests
{
    [TestFixture]
    public class ConsoleUserInterfaceTests
    {
        private ConsoleUserInterface ui;
        private HashTableWithTrees hashTable;
        private StringWriter stringWriter;
        private StringReader stringReader;

        [SetUp]
        public void SetUp()
        {
            var hashFunction = new RussianAlphabetHashFunction();
            hashTable = new HashTableWithTrees(20, hashFunction);
            ui = new ConsoleUserInterface(hashTable);

            stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
        }

        [TearDown]
        public void TearDown()
        {
            stringWriter?.Dispose();
            stringReader?.Dispose();

            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
        }

        [Test]
        public void Constructor_WithNullHashTable_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsoleUserInterface(null));
        }

        [Test]
        public void Constructor_WithValidHashTable_CreatesInterface()
        {
            // Act
            var interface1 = new ConsoleUserInterface(hashTable);

            // Assert
            Assert.IsNotNull(interface1);
        }

        [Test]
        public void ShowMenu_DisplaysAllMenuOptions()
        {
            // Act
            ui.ShowMenu();

            // Assert
            string output = stringWriter.ToString();

            Assert.IsTrue(output.Contains("ИНТЕРАКТИВНОЕ МЕНЮ"));
            Assert.IsTrue(output.Contains("1. Добавить литературный термин"));
            Assert.IsTrue(output.Contains("2. Найти термин"));
            Assert.IsTrue(output.Contains("3. Обновить определение термина"));
            Assert.IsTrue(output.Contains("4. Удалить термин"));
            Assert.IsTrue(output.Contains("5. Показать всю таблицу"));
            Assert.IsTrue(output.Contains("6. Показать статистику коллизий"));
            Assert.IsTrue(output.Contains("0. Выход"));
            Assert.IsTrue(output.Contains("Выберите действие:"));
        }

        [Test]
        public void ShowMenu_FormatsOutputCorrectly()
        {
            // Act
            ui.ShowMenu();

            // Assert
            string output = stringWriter.ToString();

            Assert.IsTrue(output.Contains("==="));
            Assert.IsTrue(output.Contains("\n"));
        }

        private void SetupConsoleInput(string input)
        {
            stringReader = new StringReader(input);
            Console.SetIn(stringReader);
        }

        [Test]
        public void RunInteractiveMode_WithExitOption_ExitsGracefully()
        {
            // Arrange
            SetupConsoleInput("0\n");

            // Act
            ui.RunInteractiveMode();

            // Assert
            string output = stringWriter.ToString();
            Assert.IsTrue(output.Contains("До свидания!"));
        }

        [Test]
        public void RunInteractiveMode_WithInvalidOption_ShowsErrorMessage()
        {
            // Arrange
            SetupConsoleInput("99\n0\n");

            // Act
            ui.RunInteractiveMode();

            // Assert
            string output = stringWriter.ToString();
            Assert.IsTrue(output.Contains("Неверный выбор!"));
            Assert.IsTrue(output.Contains("До свидания!"));
        }

        [Test]
        public void RunInteractiveMode_WithShowTableOption_CallsHashTablePrintTable()
        {
            // Arrange
            SetupConsoleInput("5\n0\n");

            hashTable.Insert("Тест", "Тестовое определение");

            // Act
            ui.RunInteractiveMode();

            // Assert
            string output = stringWriter.ToString();
            Assert.IsTrue(output.Contains("СОДЕРЖИМОЕ ХЕШ-ТАБЛИЦЫ") ||
                         output.Contains("Тест"));
        }

        [Test]
        public void RunInteractiveMode_WithShowStatisticsOption_CallsHashTablePrintCollisionStatistics()
        {
            // Arrange
            SetupConsoleInput("6\n0\n");

            // Act
            ui.RunInteractiveMode();

            // Assert
            string output = stringWriter.ToString();
            Assert.IsTrue(output.Contains("СТАТИСТИКА КОЛЛИЗИЙ") ||
                         output.Contains("Всего коллизий") ||
                         output.Contains("Коэффициент заполнения"));
        }

        [Test]
        public void RunInteractiveMode_WithAddTermOption_PromptsForInput()
        {
            // Arrange
            SetupConsoleInput("1\nТестТермин\nТестовое определение\n0\n");

            // Act
            ui.RunInteractiveMode();

            // Assert
            string output = stringWriter.ToString();
            Assert.IsTrue(output.Contains("Введите литературный термин:") ||
                         output.Contains("Введите определение:"));

            string result = hashTable.Search("ТестТермин");
            Assert.AreEqual("Тестовое определение", result);
        }

        [Test]
        public void RunInteractiveMode_WithSearchTermOption_PromptsForSearchAndShowsResult()
        {
            // Arrange
            hashTable.Insert("Метафора", "Скрытое сравнение");
            SetupConsoleInput("2\nМетафора\n0\n");

            // Act
            ui.RunInteractiveMode();

            // Assert
            string output = stringWriter.ToString();
            Assert.IsTrue(output.Contains("Введите термин для поиска:"));
            Assert.IsTrue(output.Contains("Найдено:") && output.Contains("Скрытое сравнение"));
        }

        [Test]
        public void RunInteractiveMode_WithSearchNonExistentTerm_ShowsNotFoundMessage()
        {
            // Arrange
            SetupConsoleInput("2\nНесуществующийТермин\n0\n");

            // Act
            ui.RunInteractiveMode();

            // Assert
            string output = stringWriter.ToString();
            Assert.IsTrue(output.Contains("Термин не найден"));
        }

        [Test]
        public void RunInteractiveMode_WithUpdateTermOption_PromptsForInputAndUpdates()
        {
            // Arrange
            hashTable.Insert("Эпитет", "Старое определение");
            SetupConsoleInput("3\nЭпитет\nНовое определение\n0\n");

            // Act
            ui.RunInteractiveMode();

            // Assert
            string output = stringWriter.ToString();
            Assert.IsTrue(output.Contains("Введите термин для обновления:"));
            Assert.IsTrue(output.Contains("Введите новое определение:"));

            string result = hashTable.Search("Эпитет");
            Assert.AreEqual("Новое определение", result);
        }

        [Test]
        public void RunInteractiveMode_WithDeleteTermOption_PromptsForInputAndDeletes()
        {
            // Arrange
            hashTable.Insert("Гипербола", "Художественное преувеличение");
            SetupConsoleInput("4\nГипербола\n0\n");

            // Act
            ui.RunInteractiveMode();

            // Assert
            string output = stringWriter.ToString();
            Assert.IsTrue(output.Contains("Введите термин для удаления:"));

            string result = hashTable.Search("Гипербола");
            Assert.IsNull(result);
        }

        [Test]
        public void RunInteractiveMode_MultipleOperations_WorksCorrectly()
        {
            // Arrange
            SetupConsoleInput("1\nТест\nПервое определение\n2\nТест\n3\nТест\nВторое определение\n2\nТест\n0\n");

            // Act
            ui.RunInteractiveMode();

            // Assert
            string output = stringWriter.ToString();

            Assert.IsTrue(output.Contains("Введите литературный термин:"));
            Assert.IsTrue(output.Contains("Введите термин для поиска:"));
            Assert.IsTrue(output.Contains("Введите термин для обновления:"));
            Assert.IsTrue(output.Contains("До свидания!"));

            string finalResult = hashTable.Search("Тест");
            Assert.AreEqual("Второе определение", finalResult);
        }

        [Test]
        public void RunInteractiveMode_ShowsMenuAfterEachOperation()
        {
            // Arrange
            SetupConsoleInput("5\n6\n0\n");

            // Act
            ui.RunInteractiveMode();

            // Assert
            string output = stringWriter.ToString();

            int menuCount = CountOccurrences(output, "ИНТЕРАКТИВНОЕ МЕНЮ");
            Assert.IsTrue(menuCount >= 3, "Меню должно показываться перед каждой операцией");
        }

        [Test]
        public void RunInteractiveMode_HandlesEmptyInputGracefully()
        {
            // Arrange
            SetupConsoleInput("1\n\n\n0\n");

            // Act & Assert
            Assert.DoesNotThrow(() => ui.RunInteractiveMode());
        }

        [Test]
        public void Interface_WithPrefilledHashTable_WorksWithExistingData()
        {
            // Arrange
            hashTable.Insert("Метафора", "Скрытое сравнение");
            hashTable.Insert("Эпитет", "Образное определение");
            hashTable.Insert("Гипербола", "Художественное преувеличение");

            SetupConsoleInput("2\nМетафора\n2\nНесуществующий\n0\n");

            // Act
            ui.RunInteractiveMode();

            // Assert
            string output = stringWriter.ToString();
            Assert.IsTrue(output.Contains("Найдено:") && output.Contains("Скрытое сравнение"));
            Assert.IsTrue(output.Contains("Термин не найден"));
        }

        private int CountOccurrences(string text, string pattern)
        {
            int count = 0;
            int index = 0;

            while ((index = text.IndexOf(pattern, index)) != -1)
            {
                count++;
                index += pattern.Length;
            }

            return count;
        }

        [Test]
        public void Interface_HandlesRussianTextCorrectly()
        {
            // Arrange
            SetupConsoleInput("1\nМетафора\nСкрытое сравнение на основе сходства\n2\nМетафора\n0\n");

            // Act
            ui.RunInteractiveMode();

            // Assert
            string output = stringWriter.ToString();

            string storedValue = hashTable.Search("Метафора");
            Assert.AreEqual("Скрытое сравнение на основе сходства", storedValue);
        }
    }
}