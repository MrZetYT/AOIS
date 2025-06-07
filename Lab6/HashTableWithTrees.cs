using System;
using System.Collections.Generic;
using HashTableLiterature.Interfaces;

namespace HashTableLiterature.Core
{
    public class HashTableWithTrees : IHashTable<string, string>
    {
        private readonly IBalancedTree<string, string>[] table;
        private readonly IHashFunction hashFunction;
        private readonly int size;
        private int count;

        public HashTableWithTrees(int size, IHashFunction hashFunction)
        {
            this.size = size;
            this.hashFunction = hashFunction ?? throw new ArgumentNullException(nameof(hashFunction));
            this.count = 0;
            this.table = new IBalancedTree<string, string>[size];

            for (int i = 0; i < size; i++)
            {
                table[i] = new DataStructures.AVLTree();
            }
        }

        public bool Insert(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            int hash = hashFunction.ComputeHash(key, size);

            if (table[hash].Search(key) != null)
            {
                Console.WriteLine($"Ключ '{key}' уже существует в таблице!");
                return false;
            }

            table[hash].Insert(key, value);
            count++;

            Console.WriteLine($"Добавлен: {key} -> хеш = {hash}, V = {hashFunction.GetKeyValue(key)}");
            return true;
        }

        public string Search(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            int hash = hashFunction.ComputeHash(key, size);
            return table[hash].Search(key);
        }

        public bool Update(string key, string newValue)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            int hash = hashFunction.ComputeHash(key, size);

            if (table[hash].Search(key) == null)
            {
                Console.WriteLine($"Ключ '{key}' не найден для обновления!");
                return false;
            }

            table[hash].Insert(key, newValue);
            Console.WriteLine($"Обновлен: {key}");
            return true;
        }

        public bool Delete(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            int hash = hashFunction.ComputeHash(key, size);

            if (table[hash].Delete(key))
            {
                count--;
                Console.WriteLine($"Удален: {key}");
                return true;
            }

            Console.WriteLine($"Ключ '{key}' не найден для удаления!");
            return false;
        }

        public double LoadFactor()
        {
            return (double)count / size;
        }

        public List<(string Key, string Value)> GetAllElements()
        {
            var elements = new List<(string, string)>();

            for (int i = 0; i < size; i++)
            {
                var keys = table[i].GetAllKeys();
                foreach (var key in keys)
                {
                    var value = table[i].Search(key);
                    elements.Add((key, value));
                }
            }

            return elements;
        }

        public void PrintCollisionStatistics()
        {
            Console.WriteLine("\n=== СТАТИСТИКА КОЛЛИЗИЙ ===");

            int collisions = 0;
            int usedSlots = 0;

            for (int i = 0; i < size; i++)
            {
                int slotCount = table[i].Count();
                if (slotCount > 0)
                {
                    usedSlots++;
                    if (slotCount > 1)
                    {
                        collisions++;
                        Console.WriteLine($"Слот {i}: {slotCount} элементов (коллизия)");
                    }
                }
            }

            Console.WriteLine($"Всего коллизий: {collisions}");
            Console.WriteLine($"Занятых слотов: {usedSlots}");
            Console.WriteLine($"Коэффициент заполнения: {LoadFactor():F3}");
        }

        public void PrintTable()
        {
            Console.WriteLine("\n=== СОДЕРЖИМОЕ ХЕШ-ТАБЛИЦЫ ===");
            Console.WriteLine("Слот | Ключ -> Значение V -> Хеш-адрес -> Данные");
            Console.WriteLine(new string('-', 80));

            for (int i = 0; i < size; i++)
            {
                var keys = table[i].GetAllKeys();
                if (keys.Count > 0)
                {
                    Console.WriteLine($"[{i,2}]  | Элементов в дереве: {keys.Count}");
                    foreach (var key in keys)
                    {
                        var data = table[i].Search(key);
                        var keyValue = hashFunction.GetKeyValue(key);
                        var hash = hashFunction.ComputeHash(key, size);
                        Console.WriteLine($"     | {key} -> V={keyValue} -> h={hash} -> {data}");
                    }
                }
                else
                {
                    Console.WriteLine($"[{i,2}]  | (пусто)");
                }
            }
        }
    }
}