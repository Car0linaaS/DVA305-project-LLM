using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_RBTree
{
    public interface IOrderedSet<TElement> : IEnumerable<TElement> where TElement : struct, IComparable<TElement>
    {
        public int Count { get; }
        public void Insert(TElement element);
        public bool Search(TElement element);
        public TElement? Minimum();
        public TElement? Maximum();
        public TElement? Successor(TElement element);
        public TElement? Predecessor(TElement element);
        public void UnionWith(IEnumerable<TElement> other);
    }
}
