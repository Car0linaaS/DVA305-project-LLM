namespace Huffman
{
    public class TreeNode
    {
        public int Occurence { get; private set; }
        public byte? Data { get; private set; }
        public TreeNode? Left { get; private set; }
        public TreeNode? Right { get; private set; }
        public bool IsLeaf => Left == null && Right == null;
        public TreeNode(int occurence = 0, byte? data = null, TreeNode? left = null, TreeNode? right = null)
        {
            Occurence = occurence;
            Data = data;
            Left = left;
            Right = right;
        }
    }
}
