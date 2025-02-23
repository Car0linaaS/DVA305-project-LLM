using System.Text;

namespace Huffman
{
    public class FileHandle
    {
        public string FilePath { get; private set; }
        private HuffmanTree HuffmanTree { get; set; }
        public FileHandle(string path)
        {
            FilePath = path;
            HuffmanTree = new();
        }

        public bool CompressFile
        {
            get
            {
                string encodedTree;
                List<bool> encodedFile;
                byte[] originalContent;

                if (!FileExists())
                {
                    Console.WriteLine("File does not exist. ");
                    return false;
                }

                try
                {
                    originalContent = File.ReadAllBytes(FilePath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + " ");
                    return false;
                }

                Console.WriteLine("File compression starting...");

                if (originalContent.Length >= 0)
                {
                    HuffmanTree.ConstructTreeFromArray(originalContent);

                    var extension = Path.GetExtension(FilePath);
                    encodedTree = HuffmanTree.EncodeTree(HuffmanTree.Root, "");
                    encodedFile = EncodeFile(originalContent);

                    WriteBinaryToFile(extension, encodedTree, encodedFile);
                    return true;
                }
                return false;
            }
        }

        private void WriteBinaryToFile(string ext, string encodedTree, List<bool> compressedData)
        {
            using BitStreamWriter writer = new BitStreamWriter(RemoveFileExtension() + ".hf");

            var extensionEncoded = Utilities.StrToBinStr(ext);

            writer.WriteInt32(extensionEncoded.Length);
            writer.WriteInt32(encodedTree.Length);
            writer.WriteInt32(compressedData.Count);

            foreach (var bit in extensionEncoded)
            {
                writer.WriteBit(bit == '1');
            }

            foreach (var bit in encodedTree)
            {
                writer.WriteBit(bit == '1');
            }

            foreach (var boolean in compressedData)
            {
                writer.WriteBit(boolean);
            }
        }

        public List<bool> EncodeFile(byte[] content)
        {
            List<bool> encodedBits = new();

            foreach (var b in content)
            {
                if (HuffmanTree.CodesDictionary.ContainsKey(b))
                {
                    string huffmanCode = HuffmanTree.CodesDictionary[b];

                    foreach (var bit in huffmanCode)
                    {
                        encodedBits.Add(bit == '1');
                    }
                }
            }

            return encodedBits;
        }

        // Decompress file, return true if successful otherwise false
        public bool DecompressFile()
        {
            // If not a .hf file, cannot decompress
            if (!Path.GetExtension(FilePath).Equals(".hf"))
            {
                Console.WriteLine("Decompression for HF files only. ");
                return false;
            }

            // Check that file exists
            if (!FileExists())
            {
                Console.Write("File does not exist. ");
                return false;
            }

            // Read binary file content 
            if (!ReadBinaryFromFile(out string binExtension, out string encodedTree, out List<bool> encodedData))
            {
                return false;
            }

            // Decode/construct huffman tree and decode file
            // Create new file with original extention and write the decoded content to it
            if (encodedData.Count > 0)
            {
                var extension = Utilities.BinStrToStr(binExtension);
                var newPath = RemoveFileExtension() + extension;

                HuffmanTree.DecodeTree(encodedTree);
                var decodeData = DecodeFile(encodedData);

                var fp = File.Create(newPath);
                fp.Write(decodeData);
                fp.Close();
            }

            return true;
        }


        // Read encoded tree and data from file
        private bool ReadBinaryFromFile(out string encodedExt, out string encodedTree, out List<bool> compressedData)
        {
            using BitStreamReader reader = new(FilePath);
            encodedTree = "";
            encodedExt = "";
            compressedData = new List<bool>();

            // Read encoded tree length as 32-bit int
            int extensionLength = reader.ReadInt32();
            int treeLength = reader.ReadInt32();
            int dataLength = reader.ReadInt32();

            // read encoded extension from the file
            StringBuilder encodedExtension = new();
            for (int i = 0; i < extensionLength; i++)
            {
                try
                {
                    var bit = reader.ReadBit();
                    encodedExtension.Append(bit ? '1' : '0');
                }
                catch (EndOfStreamException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            encodedExt = encodedExtension.ToString();


            // Read encoded tree from the file
            StringBuilder encodedTreeBuilder = new();
            for (int i = 0; i < treeLength; i++)
            {
                try
                {
                    var bit = reader.ReadBit();
                    encodedTreeBuilder.Append(bit ? '1' : '0');
                }
                catch (EndOfStreamException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            encodedTree = encodedTreeBuilder.ToString();

            // Read compressed data from the file
            for (int i = 0; i < dataLength; i++)
            {
                try
                {
                    var bit = reader.ReadBit();
                    compressedData.Add(bit);
                }
                catch (EndOfStreamException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }

            return true;
        }


        // Decode file content using huffman dictionary
        public byte[] DecodeFile(List<bool> encodedData)
        {
            List<byte> decodedBytes = new();
            var temp = HuffmanTree.Root;

            // read bit by bit, if 0 go left, if 1 go right
            // if leaf, write to decoded list, move back to root
            for (int i = 0; i < encodedData.Count; i++)
            {
                if (temp == null)
                {
                    break;
                }

                temp = encodedData[i] ? temp.Right : temp.Left;

                if (temp != null && temp.Data != null && temp.IsLeaf)
                {
                    decodedBytes.Add((byte)temp.Data);
                    temp = HuffmanTree.Root;
                }
            }

            return decodedBytes.ToArray();
        }

        // Removes existing file extension
        public string RemoveFileExtension()
        {
            return FilePath[0..FilePath.LastIndexOf('.')];
        }

        // Check if file exists
        private bool FileExists()
        {
            return File.Exists(FilePath);
        }
    }
}