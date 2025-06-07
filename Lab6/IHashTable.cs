using System.Collections.Generic;

namespace HashTableLiterature.Interfaces
{
    public interface IHashTable<TKey, TValue>
    {
        bool Insert(TKey key, TValue value);
        TValue Search(TKey key);
        bool Update(TKey key, TValue newValue);
        bool Delete(TKey key);
        double LoadFactor();
        List<(TKey Key, TValue Value)> GetAllElements();
    }
}