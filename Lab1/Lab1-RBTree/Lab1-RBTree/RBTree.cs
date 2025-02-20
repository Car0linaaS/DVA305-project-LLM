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
            if (oldSubRoot == Nil || oldSubRoot.Left == Nil) { return; }

            var newSubRoot = oldSubRoot.Left;
            oldSubRoot.Left = newSubRoot.Right;

            if (newSubRoot.Right != Nil) { newSubRoot.Right!.Parent = oldSubRoot; }
            newSubRoot.Parent = oldSubRoot.Parent;

            if (oldSubRoot.Parent == Nil) { Root = newSubRoot; }
            else if (oldSubRoot == oldSubRoot.Parent.Right) { oldSubRoot.Parent.Right = newSubRoot; }
            else { oldSubRoot.Parent.Left = newSubRoot; }

            newSubRoot.Right = oldSubRoot;
            oldSubRoot.Parent = newSubRoot;
        }

        // GPT
        private void LeftRotate(RBNode<TElement> oldSubRoot)
        {
            if (oldSubRoot == Nil || oldSubRoot!.Right == Nil) { return; }

            var newSubRoot = oldSubRoot.Right;
            oldSubRoot.Right = newSubRoot.Left;

            if (newSubRoot.Left != Nil) { newSubRoot.Left!.Parent = oldSubRoot; }
            newSubRoot.Parent = oldSubRoot.Parent;

            if (oldSubRoot.Parent == Nil) { Root = newSubRoot; }
            else if (oldSubRoot == oldSubRoot.Parent.Left) { oldSubRoot.Parent.Left = newSubRoot; }
            else { oldSubRoot.Parent.Right = newSubRoot; }

            newSubRoot.Left = oldSubRoot;
            oldSubRoot.Parent = newSubRoot;
        }
        // GPT
        private void InsertFixup(RBNode<TElement> newNode)
        {
            while (newNode.Parent != null && newNode.Parent.Colour == Colour.Red)
            {
                if (newNode.Parent == newNode.Parent.Parent.Left)
                {
                    var uncle = newNode.Parent.Parent.Right;
                    if (uncle != null && uncle.Colour == Colour.Red)
                    {
                        newNode.Parent.Recolour(Colour.Black);
                        uncle.Recolour(Colour.Black);
                        newNode.Parent.Parent.Recolour(Colour.Red);
                        newNode = newNode.Parent.Parent;
                    }
                    else
                    {
                        if (newNode == newNode.Parent.Right)
                        {
                            newNode = newNode.Parent;
                            LeftRotate(newNode);
                        }
                        newNode.Parent.Recolour(Colour.Black);
                        newNode.Parent.Parent.Recolour(Colour.Red);
                        RightRotate(newNode.Parent.Parent);
                    }
                }
                else
                {
                    var uncle = newNode.Parent.Parent.Left;
                    if (uncle != null && uncle.Colour == Colour.Red)
                    {
                        newNode.Parent.Recolour(Colour.Black);
                        uncle.Recolour(Colour.Black);
                        newNode.Parent.Parent.Recolour(Colour.Red);
                        newNode = newNode.Parent.Parent;
                    }
                    else
                    {
                        if (newNode == newNode.Parent.Left)
                        {
                            newNode = newNode.Parent;
                            RightRotate(newNode);
                        }
                        newNode.Parent.Recolour(Colour.Black);
                        newNode.Parent.Parent.Recolour(Colour.Red);
                        LeftRotate(newNode.Parent.Parent);
                    }
                }
            }
            Root.Recolour(Colour.Black);
        }
        // GPT
        public void Insert(TElement element)
        {
            var newNode = new RBNode<TElement>(element, Nil);
            var newNodeParent = Nil;
            var currentNode = Root;

            while (currentNode != Nil)
            {
                newNodeParent = currentNode;

                if (element.CompareTo(currentNode!.Data) < 0) { currentNode = currentNode.Left; }
                else if (element.CompareTo(currentNode!.Data) > 0) { currentNode = currentNode.Right; }
                else { return; }
            }

            Count++;
            newNode.Parent = newNodeParent;

            if (newNodeParent == null) // Didn't find that null should be Nil
            {
                Root = newNode;
                Root.Recolour(Colour.Black);
                return;
            }
            else if (newNode.Data.CompareTo(newNodeParent!.Data) < 0) { newNodeParent.Left = newNode; }
            else { newNodeParent.Right = newNode; }

            InsertFixup(newNode);
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