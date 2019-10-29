using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace splaylist.Models
{
    /// <summary>
    /// Cache was becoming verbose with null handling, therefore this is here to tidy it up a bit
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class NullHandlingDictionary <TKey, TValue> : Dictionary<TKey, TValue>
    {
        public void Save(TKey ID, TValue obj)
        {
            if (ID == null) return;
            this[ID] = obj;
        }

        public new bool ContainsKey(TKey key)
        {
            if (key == null) return false;
            return base.ContainsKey(key);
        }


        public TValue Get(TKey ID)
        {
            if (ID == null) return default;
            if (this.TryGetValue(ID, out var result)) return result;
            return default;
        }
        
    }
}
