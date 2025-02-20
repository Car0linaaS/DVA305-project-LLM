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

        public RBTree() {
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
        private void RightRotate(RBNode<TElement> oldSubRoot)
        {
            // Check that newSubRoot exists. If it does not, the rotation cannot be done
            if (oldSubRoot == Nil || oldSubRoot.Left == Nil) { return; }

            var newSubRoot = oldSubRoot.Left;

            // If the old subroot has a parent, its references to its children need to be updated
            // Check if the old subroot was a left or right child, and change that reference to the new subroot
            if (oldSubRoot.Parent != Nil)
            {
                if (oldSubRoot == oldSubRoot.Parent.Right) { oldSubRoot.Parent.Right = newSubRoot; }
                else {  oldSubRoot.Parent.Left = newSubRoot; }
            }

            newSubRoot.Parent = oldSubRoot.Parent;
            oldSubRoot.Parent = newSubRoot;
            oldSubRoot.Left = newSubRoot.Right;

            // Re-reference newSubRoot's left child's parent only if it's not Nil (we don't want to re-reference Nil's parent)
            if (newSubRoot.Right != Nil) { newSubRoot.Right!.Parent = oldSubRoot; }
            newSubRoot.Right = oldSubRoot;

            //If the new subroot is the root, the root-field needs to be updated
            if (newSubRoot.Parent == Nil) { Root = newSubRoot; }
        }

        private void LeftRotate(RBNode<TElement> oldSubRoot)
        {
            // Check that newSubRoot exists. If it does not, the rotation cannot be done
            if (oldSubRoot == Nil || oldSubRoot!.Right == Nil) { return; }

            var newSubRoot = oldSubRoot.Right;

            // If the old subroot has a parent, its references to its children need to be updated
            // Check if the old subroot was a left or right child, and change that reference to the new subroot
            if (oldSubRoot.Parent != Nil)
            {
                if (oldSubRoot == oldSubRoot.Parent.Right) { oldSubRoot.Parent.Right = newSubRoot; }
                else { oldSubRoot.Parent.Left = newSubRoot; }
            }

            newSubRoot.Parent = oldSubRoot.Parent;
            oldSubRoot.Parent = newSubRoot;
            oldSubRoot.Right = newSubRoot.Left;

            // Re-reference newSubRoot's left child's parent only if it's not Nil (we don't want to re-reference Nil's parent)
            if (newSubRoot.Left != Nil) { newSubRoot.Left!.Parent = oldSubRoot;  }
            newSubRoot.Left = oldSubRoot;

            //If the new subroot is the root, the root-field needs to be updated
            if (newSubRoot.Parent == Nil) { Root = newSubRoot; }
            
        }
        private void InsertFixup(RBNode<TElement> newNode)
        {
            while (newNode.Parent.Colour == Colour.Red)
            {
                // LEFT
                if (newNode.Parent == newNode.Parent.Parent.Left)
                {
                    var uncle = newNode.Parent.Parent.Right;
                    // Case 1
                    if (uncle.Colour == Colour.Red)
                    {
                        // Flip colors of nn, parent, uncle and grandparent
                        newNode.Parent.Recolour(Colour.Black);
                        uncle.Recolour(Colour.Black);
                        newNode.Parent.Parent.Recolour(Colour.Red);
                        // Recursively climb the tree to fix eventual errors
                        newNode = newNode.Parent.Parent;
                    }
                    else {
                        // Case 2
                        // Uncle is black and new node creates zig-zag pattern with parent and grandparent
                        // Rotate to acheieve case 3
                        if (newNode == newNode.Parent.Right)
                        {
                            // fix zig-zag, then proceed to case 3
                            newNode = newNode.Parent;
                            LeftRotate(newNode);
                        }
                        // Case 3
                        // Uncle is black and creates a straight line from grandparent and parent
                        newNode.Parent.Recolour(Colour.Black);
                        newNode.Parent.Parent.Recolour(Colour.Red);
                        RightRotate(newNode.Parent.Parent);
                    }

                }
                // RIGHT
                else if (newNode.Parent == newNode.Parent.Parent.Right)
                {
                    var uncle = newNode.Parent.Parent.Left;
                    // Case 1
                    if (uncle.Colour == Colour.Red)
                    {

                        newNode.Parent.Recolour(Colour.Black);
                        uncle.Recolour(Colour.Black);
                        newNode.Parent.Parent.Recolour(Colour.Red);
                        newNode = newNode.Parent.Parent;
                    }
                    else
                    {
                        // Case 2
                        // New node creates zig-zag pattern with parent and grandparent
                        if (newNode == newNode.Parent.Left)
                        {
                            // Fix zig-zag, then proceed to case 3
                            newNode = newNode.Parent;
                            RightRotate(newNode);
                        }
                        // Case 3
                        newNode.Parent.Recolour(Colour.Black);
                        newNode.Parent.Parent.Recolour(Colour.Red);
                        LeftRotate(newNode.Parent.Parent);
                    }
                }
                if (newNode.Parent == Nil) { Root.Recolour(Colour.Black); return; }
            }
            Root.Recolour(Colour.Black);
        }

        public void Insert(TElement element)
        {
            var newNode = new RBNode<TElement>(element, Nil);
            var newNodeParent = Nil;
            var currentNode = Root;

            // Traverse to find the correct position for the newNode
            while (currentNode != Nil)
            {
                newNodeParent = currentNode;

                if (element.CompareTo(currentNode!.Data) < 0) { currentNode = currentNode.Left; }
                else if (element.CompareTo(currentNode!.Data) > 0) { currentNode = currentNode.Right; }
                else { return; }  //if the value already exists, do not insert it again, just return
            }

            // Now we have found the correct position for newNode
            Count++;
            newNode.Parent = newNodeParent;

            // If the tree was empty from start, update the reference for Root
            if (newNodeParent == Nil)
            {
                // Root should now refere to the new node we inserted
                Root = newNode;
                Root.Recolour(Colour.Black);
                return;
            }
            else if (newNode.Data.CompareTo(newNodeParent!.Data) < 0) { newNodeParent.Left = newNode; }
            else if (newNode.Data.CompareTo(newNodeParent!.Data) > 0) { newNodeParent.Right = newNode; }

            // The colour for the node we insert is red as default. Call InsertFixup to fix and maintain RBTree rules
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
            if(Root == Nil) { return null; }

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