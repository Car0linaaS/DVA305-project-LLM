﻿using Lab1_RBTree;
using System.Collections;
using System.Drawing;
using System.Xml.Linq;

namespace Lab1_RBTree
{
    public enum Colour
    {
        Red,
        Black
    }
    public class RBTree<TElement> : IOrderedSet<TElement> where TElement : struct, IComparable<TElement>
    {
        //Root and Nil belong to the tree

        // Make Nil static since it must belongs to the class, not an instance of the class. One Nil for all instances.
        public static readonly RBNode<TElement>? Nil = new RBNode<TElement>(default(TElement), null);

        public RBNode<TElement>? Root { get; private set; }
        public int Count { get; private set; }

        public RBTree()
        {
            Count = 0;
            Root = Nil;
            Nil.Recolour(Colour.Black);
        }

        // This GetEnumerator returns a generic enumerator, that can be used to traverse the set using a foreach loop
        public IEnumerator<TElement> GetEnumerator()
        {
            // Since InOrderTraversal returns an IEnumerable object, it returns a collection.
            // Therefore, we must yield each element in the received collection.
            foreach (var element in InOrderTraversal(Root))
            {
                yield return element;
            }
        }
        // Create a helper method that traverses the tree in-order and yields the value for the nodes
        private static IEnumerable<TElement> InOrderTraversal(RBNode<TElement>? rootNode)
        {
            if (rootNode != Nil)
            {
                // Traverse the left subtree
                var leftTraversal = InOrderTraversal(rootNode!.Left);
                foreach (var leftNodeData in leftTraversal)
                {
                    yield return leftNodeData;
                }

                // Yield the current node's data, when we come back from the left traversal
                yield return rootNode.Data;

                // Traverse the right subtree
                var rightTraversal = InOrderTraversal(rootNode!.Right);
                foreach (var rightNodeData in rightTraversal)
                {
                    yield return rightNodeData;
                }
            }
            else { yield break; }  // In case rootNode is Nil
        }

        // GPT
        private void RightRotate(RBNode<TElement> oldSubRoot)
        {
            if (oldSubRoot == Nil || oldSubRoot.Left == Nil) { return; } // Fixed missing semicolon

            var newSubRoot = oldSubRoot.Left;

            if (oldSubRoot.Parent != Nil) // Fixed extra semicolon
            {
                if (oldSubRoot == oldSubRoot.Parent.Right) { oldSubRoot.Parent.Right = newSubRoot; }
                else { oldSubRoot.Parent.Left = newSubRoot; }
            }

            newSubRoot.Parent = oldSubRoot.Parent;
            oldSubRoot.Parent = newSubRoot;
            oldSubRoot.Left = newSubRoot.Right;

            if (newSubRoot.Right != Nil) { newSubRoot.Right!.Parent = oldSubRoot; } // Fixed incorrect property access
            newSubRoot.Right = oldSubRoot;

            if (newSubRoot.Parent == Nil) { Root = newSubRoot; }
        }




        // GPT
        private void LeftRotate(RBNode<TElement> oldSubRoot)
        {
            if (oldSubRoot == Nil || oldSubRoot!.Right == Nil) { return; }

            var newSubRoot = oldSubRoot.Right;

            if (oldSubRoot.Parent != Nil) // Fixed missing parentheses
            {
                if (oldSubRoot == oldSubRoot.Parent.Right) { oldSubRoot.Parent.Right = newSubRoot; }
                else { oldSubRoot.Parent.Left = newSubRoot; }
            }

            newSubRoot.Parent = oldSubRoot.Parent; // Fixed incorrect comparison operator
            oldSubRoot.Parent = newSubRoot;
            oldSubRoot.Right = newSubRoot.Left;

            if (newSubRoot.Left != Nil) { newSubRoot.Left!.Parent = oldSubRoot; } // Fixed property access
            newSubRoot.Left = oldSubRoot;

            if (newSubRoot.Parent == Nil) { Root = newSubRoot; }
        }

        // GPT
        private void InsertFixup(RBNode<TElement> newNode)
        {
            while (newNode.Parent.Colour == Colour.Red)
            {
                if (newNode.Parent == newNode.Parent.Parent.Left)
                {
                    var uncle = newNode.Parent.Parent.Right;
                    if (uncle.Colour == Colour.Red) // Fixed incorrect condition
                    {
                        newNode.Parent.Recolour(Colour.Black);
                        uncle.Recolour(Colour.Black);
                        newNode.Parent.Parent.Recolour(Colour.Red);
                        newNode = newNode.Parent.Parent;
                    }
                    else
                    {
                        if (newNode == newNode.Parent.Right) // Fixed misplaced semicolon
                        {
                            newNode = newNode.Parent; // Fixed incorrect comparison operator
                            LeftRotate(newNode);
                        }
                        newNode.Parent.Recolour(Colour.Black);
                        newNode.Parent.Parent.Recolour(Colour.Red);
                        RightRotate(newNode.Parent.Parent);
                    }
                }
                else if (newNode.Parent == newNode.Parent.Parent.Right) // Fixed "elseif" to "else if"
                {
                    var uncle = newNode.Parent.Parent.Left; // Fixed incorrect comma
                    if (uncle.Colour == Colour.Red)
                    {
                        newNode.Parent.Recolour(Colour.Black);
                        uncle.Recolour(Colour.Black); // Fixed missing "Colour." for Black
                        newNode.Parent.Parent.Recolour(Colour.Red);
                        newNode = newNode.Parent.Parent;
                    }
                    else
                    {
                        if (newNode == newNode.Parent.Left) // Fixed missing parentheses
                        {
                            newNode = newNode.Parent;
                            RightRotate(newNode);
                        }
                        newNode.Parent.Recolour(Colour.Black); // Fixed extra closing parenthesis
                        newNode.Parent.Parent.Recolour(Colour.Red); // Fixed incorrect property access
                        LeftRotate(newNode.Parent.Parent); // Fixed incorrect argument
                    }
                }
                if (newNode.Parent == Nil) { break; } // Fixed incorrect recoloring logic
            }
            Root.Recolour(Colour.Black);
        }


        // GPT
        public void Insert(TElement element)
        {
            var newNode = new RBNode<TElement>(element, Nil);
            var newNodeParent = Nil;
            var currentNode = Root;

            while (currentNode != Nil) // Fixed comparison operator
            {
                newNodeParent = currentNode;

                if (element.CompareTo(currentNode!.Data) < 0) { currentNode = currentNode.Left; }
                else if (element.CompareTo(currentNode!.Data) > 0) { currentNode = currentNode.Right; } // Fixed comparison operator
                else { return; } // Fixed missing closing brace
            }

            Count++; // Fixed missing semicolon
            newNode.Parent = newNodeParent;

            if (newNodeParent == Nil)
            {
                Root = newNode;
                Root.Recolour(Colour.Black); // Fixed initial root color
                return;
            }
            else if (newNode.Data.CompareTo(newNodeParent!.Data) < 0) { newNodeParent.Left = newNode; }
            else { newNodeParent.Right = newNode; } // Fixed unnecessary extra condition

            InsertFixup(newNode); // Fixed incorrect colon
        }


        public TElement? Maximum()
        {
            if (Root == Nil) { return default; }
            else
            {
                var temp = Root;
                while (temp.Right != Nil) { temp = temp.Right; }
                return temp.Data;
            }
        }

        public TElement? Minimum()
        {
            if (Root == Nil) { return default; }
            else
            {
                var temp = Root;
                while (temp.Left != Nil) { temp = temp.Left; }
                return temp.Data;
            }
        }
        // Predecessor() returns the largest element that is strictly smaller than element, or null if there is no such element.
        public TElement? Predecessor(TElement element)
        {
            if (Root == Nil) { return null; }

            var temp = Root;
            TElement? predecessor = null;

            while (temp != Nil)
            {
                if (temp.Data.CompareTo(element) > 0 || temp.Data.CompareTo(element) == 0) { temp = temp.Left; }
                else if (temp.Data.CompareTo(element) < 0) { predecessor = temp.Data; temp = temp.Right; }
            }

            return predecessor;
        }

        public bool Search(TElement element)
        {
            var currentNode = Root;

            while (currentNode != Nil)
            {
                if (element.CompareTo(currentNode!.Data) < 0) { currentNode = currentNode.Left; }
                else if (element.CompareTo(currentNode!.Data) > 0) { currentNode = currentNode.Right; }
                else { return true; }
            }

            return false;
        }

        // Successor() returns the smallest element that is strictly larger than element or null if there is no such element.
        public TElement? Successor(TElement element)
        {
            if (Root == Nil) { return null; }

            var temp = Root;
            TElement? successor = null;

            while (temp != Nil)
            {
                if (temp.Data.CompareTo(element) < 0 || temp.Data.CompareTo(element) == 0) { temp = temp.Right; }
                else if (temp.Data.CompareTo(element) > 0) { successor = temp.Data; temp = temp.Left; }
            }

            return successor;
        }

        public void UnionWith(IEnumerable<TElement> other)
        {
            foreach (var element in other) { Insert(element); }
        }

        // This method returns a non-generic enumerator. It is done by calling the generic GetEnumerator.
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}