using System.Collections.Generic;

namespace HashTableLiterature.Interfaces
{
    public interface IBalancedTree<TKey, TValue>
    {
        void Insert(TKey key, TValue value);
        TValue Search(TKey key);
        bool Delete(TKey key);
        List<TKey> GetAllKeys();
        int Count();
    }
}