using System;
using HashTableLiterature.Interfaces;
using HashTableLiterature.Core;

namespace HashTableLiterature.UI
{
    public class ConsoleUserInterface : IUserInterface
    {
        private readonly HashTableWithTrees hashTable;

        public ConsoleUserInterface(HashTableWithTrees hashTable)
        {
            this.hashTable = hashTable ?? throw new ArgumentNullException(nameof(hashTable));
        }

        public void ShowMenu()
        {
            Console.WriteLine("\n=== ИНТЕРАКТИВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Добавить литературный термин");
            Console.WriteLine("2. Найти термин");
            Console.WriteLine("3. Обновить определение термина");
            Console.WriteLine("4. Удалить термин");
            Console.WriteLine("5. Показать всю таблицу");
            Console.WriteLine("6. Показать статистику коллизий");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");
        }

        public void RunInteractiveMode()
        {
            while (true)
            {
                ShowMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTerm();
                        break;
                    case "2":
                        SearchTerm();
                        break;
                    case "3":
                        UpdateTerm();
                        break;
                    case "4":
                        DeleteTerm();
                        break;
                    case "5":
                        hashTable.PrintTable();
                        break;
                    case "6":
                        hashTable.PrintCollisionStatistics();
                        break;
                    case "0":
                        Console.WriteLine("До свидания!");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
        }

        private void AddTerm()
        {
            Console.Write("Введите литературный термин: ");
            var term = Console.ReadLine();
            Console.Write("Введите определение: ");
            var definition = Console.ReadLine();
            hashTable.Insert(term, definition);
        }

        private void SearchTerm()
        {
            Console.Write("Введите термин для поиска: ");
            var term = Console.ReadLine();
            var result = hashTable.Search(term);
            if (result != null)
                Console.WriteLine($"Найдено: {result}");
            else
                Console.WriteLine("Термин не найден");
        }

        private void UpdateTerm()
        {
            Console.Write("Введите термин для обновления: ");
            var term = Console.ReadLine();
            Console.Write("Введите новое определение: ");
            var newDefinition = Console.ReadLine();
            hashTable.Update(term, newDefinition);
        }

        private void DeleteTerm()
        {
            Console.Write("Введите термин для удаления: ");
            var term = Console.ReadLine();
            hashTable.Delete(term);
        }
    }
}