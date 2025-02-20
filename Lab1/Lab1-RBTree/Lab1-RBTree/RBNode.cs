using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab1_RBTree
{
    public class RBNode<TElement> where TElement : struct, IComparable<TElement>
    {
        public TElement Data { get; private set; }
        public Colour Colour { get; private set; }

        public RBNode<TElement>? Parent;
        public RBNode<TElement>? Left;
        public RBNode<TElement>? Right; 

        public RBNode(TElement data, RBNode<TElement> Nil)
        {
            Data = data;
            Parent = Nil;
            Left = Nil;
            Right = Nil;
            Colour = Colour.Red;
        }

        // GPT
        public void Recolour(Colour colour) { Colour = colour; }
    }
}
