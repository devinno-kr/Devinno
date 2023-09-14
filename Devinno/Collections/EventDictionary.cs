using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Collections
{
    public class EventDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public event EventHandler Changed;
    
        public EventDictionary()
        {
        }

        public new bool TryAdd(TKey key, TValue value)
        {
            var ret = base.TryAdd(key, value);
            if (ret) Changed?.Invoke(this, null);
            return ret;
        }
 
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            Changed?.Invoke(this, null);
        }

        public new void Clear() 
        {
            base.Clear();
            Changed?.Invoke(this, null);
        }

        public new bool Remove(TKey key)
        {
            var ret = base.Remove(key);
            if (ret) Changed?.Invoke(this, null);
            return ret;
        }
    }
}
