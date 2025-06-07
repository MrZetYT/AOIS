using System;
using HashTableLiterature.Interfaces;

namespace HashTableLiterature.HashFunctions
{
    public class RussianAlphabetHashFunction : IHashFunction
    {
        public int ComputeHash(string key, int tableSize)
        {
            int V = GetKeyValue(key);
            return V % tableSize;
        }

        public int GetKeyValue(string key)
        {
            if (string.IsNullOrEmpty(key))
                return 0;

            key = key.ToUpper();

            int firstChar = GetCharValue(key[0]);
            int secondChar = key.Length > 1 ? GetCharValue(key[1]) : 0;

            return firstChar * 33 + secondChar;
        }

        private int GetCharValue(char c)
        {
            if (c >= 'А' && c <= 'Я')
                return c - 'А';
            if (c >= 'A' && c <= 'Z')
                return (c - 'A') % 33;
            return 0;
        }
    }
}