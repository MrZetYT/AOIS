using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using HashTableLiterature;
using HashTableLiterature.Core;
using HashTableLiterature.HashFunctions;
using HashTableLiterature.Data;
using HashTableLiterature.UI;

namespace HashTableLiterature.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        private StringWriter stringWriter;
        private TextWriter originalOutput;
        private StringReader stringReader;
        private TextReader originalInput;

        [SetUp]
        public void SetUp()
        {
            stringWriter = new StringWriter();
            originalOutput = Console.Out;
            Console.SetOut(stringWriter);
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetOut(originalOutput);
            stringWriter?.Dispose();
            
            if (originalInput != null)
            {
                Console.SetIn(originalInput);
                originalInput = null;
            }
            stringReader?.Dispose();
        }

        [Test]
        public void Main_WithEmptyArgs_ExecutesWithoutException()
        {
            // Arrange
            var args = new string[] { };
            
            SetupConsoleInput("0\n");

            // Act & Assert
            Assert.DoesNotThrow(() => Program.Main(args));
        }

        [Test]
        public void Main_WithNullArgs_ExecutesWithoutException()
        {
            // Arrange
            string[] args = null;
            
            SetupConsoleInput("0\n");

            // Act & Assert
            Assert.DoesNotThrow(() => Program.Main(args));
        }

        [Test]
        public void Main_WithArbitraryArgs_ExecutesWithoutException()
        {
            // Arrange
            var args = new string[] { "arg1", "arg2", "test" };
            
            SetupConsoleInput("0\n");

            // Act & Assert
            Assert.DoesNotThrow(() => Program.Main(args));
        }

        [Test]
        public void Main_LoadsLiteraryTerms()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("ЗАГРУЗКА ЛИТЕРАТУРНЫХ ТЕРМИНОВ"));
            Assert.That(output, Does.Contain("Метафора"));
            Assert.That(output, Does.Contain("Эпитет"));
        }

        [Test]
        public void Main_TestsDuplicateInsertion()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("ТЕСТ ДУБЛИКАТА"));
            Assert.That(output, Does.Contain("уже существует в таблице"));
        }

        [Test]
        public void Main_PrintsTableAndStatistics()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("СОДЕРЖИМОЕ ХЕШ-ТАБЛИЦЫ"));
            Assert.That(output, Does.Contain("СТАТИСТИКА КОЛЛИЗИЙ"));
        }

        [Test]
        public void Main_TestsSearchFunctionality()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("ТЕСТИРОВАНИЕ ПОИСКА"));
            Assert.That(output, Does.Contain("Найдено:"));
        }

        [Test]
        public void Main_TestsUpdateFunctionality()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("ТЕСТИРОВАНИЕ ОБНОВЛЕНИЯ"));
            Assert.That(output, Does.Contain("Обновлен: Ямб"));
        }

        [Test]
        public void Main_TestsDeleteFunctionality()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("ТЕСТИРОВАНИЕ УДАЛЕНИЯ"));
            Assert.That(output, Does.Contain("Удален:"));
        }

        [Test]
        public void Main_ShowsFinalTableState()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("ФИНАЛЬНОЕ СОСТОЯНИЕ ТАБЛИЦЫ"));
        }

        [Test]
        public void Main_StartsInteractiveMode()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("ИНТЕРАКТИВНОЕ МЕНЮ"));
            Assert.That(output, Does.Contain("До свидания!"));
        }

        [Test]
        public void Main_InteractiveMode_AddTerm()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("1\nТестТермин\nТестОпределение\n0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("Добавлен: ТестТермин"));
        }

        [Test]
        public void Main_InteractiveMode_SearchTerm()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("2\nМетафора\n0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("Найдено:"));
        }

        [Test]
        public void Main_InteractiveMode_UpdateTerm()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("3\nМетафора\nНовоеОпределение\n0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("Обновлен:"));
        }

        [Test]
        public void Main_InteractiveMode_DeleteTerm()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("4\nХорей\n0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("Удален:"));
        }

        [Test]
        public void Main_InteractiveMode_ShowTable()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("5\n0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("СОДЕРЖИМОЕ ХЕШ-ТАБЛИЦЫ"));
        }

        [Test]
        public void Main_InteractiveMode_ShowStatistics()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("6\n0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("СТАТИСТИКА КОЛЛИЗИЙ"));
        }

        [Test]
        public void Main_InteractiveMode_InvalidChoice()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("9\n0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("Неверный выбор!"));
        }

        [Test]
        public void Main_InteractiveMode_MultipleInvalidChoices()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("abc\n-1\n99\n0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            var invalidChoiceCount = CountOccurrences(output, "Неверный выбор!");
            Assert.That(invalidChoiceCount, Is.GreaterThanOrEqualTo(3));
        }

        [Test]
        public void Main_InteractiveMode_ComplexSession()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("1\nНовыйТермин\nНовоеОпределение\n2\nНовыйТермин\n3\nНовыйТермин\nОбновленноеОпределение\n4\nНовыйТермин\n5\n6\n0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("Добавлен: НовыйТермин"));
            Assert.That(output, Does.Contain("Найдено:"));
            Assert.That(output, Does.Contain("Обновлен: НовыйТермин"));
            Assert.That(output, Does.Contain("Удален: НовыйТермин"));
        }

        [Test]
        public void Main_CreatesCorrectDependencies()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Does.Contain("хеш = "));
            Assert.That(output, Does.Contain("V = "));
        }

        [Test]
        public void Main_HandlesEmptyInput()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("\n\n\n0\n");

            // Act & Assert
            Assert.DoesNotThrow(() => Program.Main(args));
        }

        [Test]
        public void Main_ExecutesAllTestScenarios()
        {
            // Arrange
            var args = new string[] { };
            SetupConsoleInput("0\n");

            // Act
            Program.Main(args);

            // Assert
            var output = stringWriter.ToString();
            
            Assert.That(output, Does.Contain("ЗАГРУЗКА ЛИТЕРАТУРНЫХ ТЕРМИНОВ"));
            Assert.That(output, Does.Contain("ТЕСТ ДУБЛИКАТА"));
            Assert.That(output, Does.Contain("ТЕСТИРОВАНИЕ ПОИСКА"));
            Assert.That(output, Does.Contain("ТЕСТИРОВАНИЕ ОБНОВЛЕНИЯ"));
            Assert.That(output, Does.Contain("ТЕСТИРОВАНИЕ УДАЛЕНИЯ"));
            Assert.That(output, Does.Contain("ФИНАЛЬНОЕ СОСТОЯНИЕ ТАБЛИЦЫ"));
            Assert.That(output, Does.Contain("ИНТЕРАКТИВНОЕ МЕНЮ"));
        }

        private void SetupConsoleInput(string input)
        {
            stringReader = new StringReader(input);
            originalInput = Console.In;
            Console.SetIn(stringReader);
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
    }
}