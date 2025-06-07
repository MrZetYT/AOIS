using System;
using HashTableLiterature.Core;
using HashTableLiterature.HashFunctions;
using HashTableLiterature.Data;
using HashTableLiterature.UI;

namespace HashTableLiterature
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hashFunction = new RussianAlphabetHashFunction();
            var hashTable = new HashTableWithTrees(20, hashFunction);
            var userInterface = new ConsoleUserInterface(hashTable);

            Console.WriteLine("=== ЗАГРУЗКА ЛИТЕРАТУРНЫХ ТЕРМИНОВ ===");
            var literaryTerms = LiteratureDataProvider.GetLiteraryTerms();

            foreach (var term in literaryTerms)
            {
                hashTable.Insert(term.Key, term.Value);
            }

            Console.WriteLine("\n=== ТЕСТ ДУБЛИКАТА ===");
            hashTable.Insert("Метафора", "Попытка добавить дубликат");

            hashTable.PrintTable();
            hashTable.PrintCollisionStatistics();

            Console.WriteLine("\n=== ТЕСТИРОВАНИЕ ПОИСКА ===");
            var searchKeys = LiteratureDataProvider.GetTestSearchKeys();

            foreach (var key in searchKeys)
            {
                var result = hashTable.Search(key);
                if (result != null)
                    Console.WriteLine($"Найдено: {key} -> {result}");
                else
                    Console.WriteLine($"Не найдено: {key}");
            }

            Console.WriteLine("\n=== ТЕСТИРОВАНИЕ ОБНОВЛЕНИЯ ===");
            hashTable.Update("Ямб", "Двусложная стихотворная стопа с ударением на втором слоге (та-ТА)");
            hashTable.Update("НесуществующийТермин", "Попытка обновить несуществующий термин");

            Console.WriteLine("\n=== ТЕСТИРОВАНИЕ УДАЛЕНИЯ ===");
            hashTable.Delete("Литота");
            hashTable.Delete("НесуществующийТермин");

            Console.WriteLine("\n=== ФИНАЛЬНОЕ СОСТОЯНИЕ ТАБЛИЦЫ ===");
            hashTable.PrintTable();
            hashTable.PrintCollisionStatistics();

            userInterface.RunInteractiveMode();
        }
    }
}