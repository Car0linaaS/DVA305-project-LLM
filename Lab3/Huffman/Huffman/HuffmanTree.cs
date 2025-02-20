namespace Huffman
{
    public class HuffmanTree
    {
        public TreeNode Root { get; private set; }
        public Dictionary<byte, string> CodesDictionary { get; private set; }

        public HuffmanTree()
        {
            Root = new(default, default);
            CodesDictionary = new Dictionary<byte, string>();
        }
        public HuffmanTree(int occurence, byte data)
        {
            Root = new(occurence, data);
            CodesDictionary = new Dictionary<byte, string>();
        }
        public HuffmanTree(HuffmanTree l, HuffmanTree r, int occurence)
        {
            Root = new(occurence, default, l.Root, r.Root);
            CodesDictionary = new Dictionary<byte, string>();
        }

        // Constructs the tree from a byte array
        public void ConstructTreeFromArray(byte[] treeArray)
        {
            // Identify how many times each symbol occurs
            Dictionary<byte, int> occurances = new();

            foreach (var b in treeArray)
            {
                if (occurances.ContainsKey(b))
                {
                    occurances[b]++;
                }
                else
                {
                    occurances.Add(b, 1);
                }
            }

            // Store all trees is a priority queue sorted by the occurance values
            PriorityQueue<HuffmanTree, int> priorityQueue = new();

            foreach (var data in occurances)
            {
                priorityQueue.Enqueue(new HuffmanTree(data.Value, data.Key), data.Value);
            }

            // Remove 2 "smallest" trees L & R from the queue, and create a new tree with L & R as children
            // The value of the new tree is the combined value of L & R
            // Insert sorted in queue, done when only one tree left
            while (priorityQueue.Count > 1)
            {
                var left = priorityQueue.Dequeue();
                var right = priorityQueue.Dequeue();

                var value = left.Root.Occurence + right.Root.Occurence;

                priorityQueue.Enqueue(new HuffmanTree(left, right, value), value);
            }

            // The last tree left in the queue becomes our root
            Root = priorityQueue.Dequeue().Root;
            CreateCharacterDictionary(Root, "");
        }

        // Recursively build dictionary with the Huffman codes 
        private void CreateCharacterDictionary(TreeNode? root, string binStr)
        {
            if (root == null)
            {
                return;
            }

            if (root.Data != null)
            {
                CodesDictionary.Add((byte)root.Data, binStr);
            }

            CreateCharacterDictionary(root.Left, binStr + "0");
            CreateCharacterDictionary(root.Right, binStr + "1");
        }

        // Encodes the tree and returns it as a string
        public string EncodeTree(TreeNode? root, string binStr)
        {
            if (root == null)
            {
                return binStr;
            }

            // If the current root is not a leaf, add a zero to the string,
            // then call recursively for the left & right subtrees.
            if (!root.IsLeaf)
            {
                binStr += "0";
                binStr = EncodeTree(root.Left, binStr);
                binStr = EncodeTree(root.Right, binStr);
            }
            // If the current root is a leaf, add a one to the string, followed by the byte that that leaf represents.
            else
            {
                if(root.Data != null)
                {
                    binStr += "1" + Utilities.ByteToBin((byte)root.Data);
                }
            }

            return binStr;
        }

        // Decode string and construct the tree
        public void DecodeTree(string arr)
        {
            Root = DecodeAndConstructTree(arr, 0, out int _);
            CreateCharacterDictionary(Root, "");
        }

        // DecodeTree recursive helper
        private TreeNode DecodeAndConstructTree(string treeStr, int i, out int skip)
        {
            TreeNode tree = new();
            // Indexes to "skip" in the next recursive call, will be 1 for each non-leaf and 9 for each leaf
            skip = 0;

            // If we reached the end of the array
            if (i >= treeStr.Length)
            {
                return tree;
            }

            // If the current bit is a zero, skip it and create an internal node,
            // then recursively create the left & right children from the remaining string

            if (treeStr[i].Equals('0'))
            {

                TreeNode left = DecodeAndConstructTree(treeStr, i + 1, out int leftSkip);
                TreeNode right = DecodeAndConstructTree(treeStr, i + 1 + leftSkip, out int rightSkip);
                skip = 1 + leftSkip + rightSkip;

                return new TreeNode(default, default, left, right);
            }

            // If the current bit is a one, skip it and create a leaf node
            // using the next 8 bits as its value.

            else if (treeStr[i].Equals('1'))
            {
                skip = 9;
                return new TreeNode(default, Utilities.BinToByte(treeStr[(i + 1)..(i + 9)]));
            }

            return tree;
        }
    }
}
