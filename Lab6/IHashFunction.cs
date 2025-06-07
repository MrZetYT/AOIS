using System;

namespace HashTableLiterature.Interfaces
{
    public interface IHashFunction
    {
        int ComputeHash(string key, int tableSize);
        int GetKeyValue(string key);
    }
}