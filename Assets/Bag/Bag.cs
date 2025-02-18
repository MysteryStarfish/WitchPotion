using System.Collections.Generic;
using UnityEngine;

namespace WitchPotion.Bag
{
    public interface Bag<T>
    {
        public int GetCount(string itemCode);
        public void SetCount(string itemCode, int count);
        public T Get(string itemCode);

        public IEnumerable<(T, int)> GetAll();
    }
}
