using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Collections
{
    public class EventList<T> : List<T>
    {
        public event EventHandler Changed;

        public new void Add(T item) { base.Add(item); Changed?.Invoke(this, new EventArgs()); }
        public new void AddRange(IEnumerable<T> collection) { base.AddRange(collection); Changed?.Invoke(this, new EventArgs()); }
        public new void Clear() { base.Clear(); Changed?.Invoke(this, new EventArgs()); }

        public new void Insert(int index, T item) { base.Insert(index, item); Changed?.Invoke(this, new EventArgs()); }
        public new void InsertRange(int index, IEnumerable<T> collection) { base.InsertRange(index, collection); Changed?.Invoke(this, new EventArgs()); }

        public new bool Remove(T item) { var ret = base.Remove(item); Changed?.Invoke(this, new EventArgs()); return ret; }
        public new int RemoveAll(Predicate<T> match) { var ret = base.RemoveAll(match); Changed?.Invoke(this, new EventArgs()); return ret; }
        public new void RemoveAt(int index) { base.RemoveAt(index); Changed?.Invoke(this, new EventArgs()); }
        public new void RemoveRange(int index, int count) { base.RemoveRange(index, count); Changed?.Invoke(this, new EventArgs()); }

        public new void Reverse(int index, int count) { base.Reverse(index, count); Changed?.Invoke(this, new EventArgs()); }
        public new void Reverse() { base.Reverse(); Changed?.Invoke(this, new EventArgs()); }

        public new void Sort(int index, int count, IComparer<T> comparer) { base.Sort(index, count, comparer); Changed?.Invoke(this, new EventArgs()); }
        public new void Sort(Comparison<T> comparison) { base.Sort(comparison); Changed?.Invoke(this, new EventArgs()); }
        public new void Sort() { base.Sort(); Changed?.Invoke(this, new EventArgs()); }
        public new void Sort(IComparer<T> comparer) { base.Sort(comparer); Changed?.Invoke(this, new EventArgs()); }
    }
}
