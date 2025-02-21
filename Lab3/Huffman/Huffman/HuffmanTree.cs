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

        public void ConstructTreeFromArray(byte[] treeArray)
        {
            Dictionary<byte, int> occurances = new(); // Added missing comma

            foreach (var b in treeArray)
            {
                if (occurances.ContainsKey(b))
                {
                    occurances[b]++;
                }
                else
                {
                    occurances.Add(b, 1); // Initialize count to 1
                }
            }

            PriorityQueue<HuffmanTree, int> priorityQueue = new(); // Corrected typo

            foreach (var data in occurances)
            {
                priorityQueue.Enqueue(new HuffmanTree(data.Value, data.Key), data.Value);
            }

            while (priorityQueue.Count > 1) // Corrected condition
            {
                var left = priorityQueue.Dequeue();
                var right = priorityQueue.Dequeue();

                var value = left.Root.Occurence + right.Root.Occurence;

                priorityQueue.Enqueue(new HuffmanTree(left, right, value), value);
            }

            Root = priorityQueue.Dequeue();
            CreateCharacterDictionary(Root, "");
        }

        private void CreateCharacterDictionary(TreeNode? root, string binStr)
        {
            if (root == null) // Corrected assignment to comparison
            {
                return;
            }

            if (root.Data != null)
            {
                CodesDictionary.Add((byte)root.Data, binStr); // Corrected cast
            }

            CreateCharacterDictionary(root.Left, binStr + "0");
            CreateCharacterDictionary(root.Right, binStr + "1"); // Corrected to "1"
        }

        public string EncodeTree(TreeNode? root, string binStr)
        {
            if (root == null)
            {
                return binStr;
            }

            if (root.IsLeaf)
            {
                binStr += "0";
                return binStr; // Return after leaf node
            }
            else
            {
                binStr += "1" + Utilities.ByteToBin((byte)root.Data); // Corrected concatenation
                binStr = EncodeTree(root.Left, binStr);
                binStr = EncodeTree(root.Right, binStr);
                return binStr;
            }
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
