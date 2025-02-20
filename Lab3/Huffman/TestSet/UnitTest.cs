using Huffman;
using NUnit.Framework.Internal;

namespace TestSet
{
    public class Tests
    {
        FileHandle toCompress;
        FileHandle toDecompress;

        string directory;
        string originalTxtPath;
        string compressedTxtPath;

        [SetUp]
        public void Setup()
        {
            // Get project directory
            directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent?.FullName;
            originalTxtPath = directory + "\\testfiles\\Textfile.txt";
            compressedTxtPath = directory + "\\testfiles\\Textfile.hf";
            toCompress = new(originalTxtPath);
            toDecompress = new(compressedTxtPath);
        }

        // Test that compressing a file produces an output file
        [Test]
        public void CompressFile_ProducesOutputFile()
        {
            toCompress.CompressFile();
            File.Delete(originalTxtPath);
            toDecompress.DecompressFile();

            File.Delete(compressedTxtPath);

            Assert.That(File.Exists(originalTxtPath), Is.True);
        }


        // Test that the size of the compressed file is smaller than the original file
        [Test]
        public void CompressFile_CompressedFileIsSmallerThanOriginal()
        {
            FileInfo fi = new(originalTxtPath);
            var originalSize = fi.Length;
            toCompress.CompressFile();
            FileInfo fi2 = new(compressedTxtPath);
            var compressedSize = fi2.Length;
            Console.WriteLine($"og: {originalSize} comp: {compressedSize}");

            File.Delete(compressedTxtPath);

            Assert.That(originalSize, Is.AtLeast(compressedSize));
        }

        // Test that decompressing a compressed file produces the original file with same content
        [Test]
        public void DecompressFile_ProducesOriginalFile()
        {
            var originalContent = File.ReadAllBytes(originalTxtPath);
            toCompress.CompressFile();
            File.Delete(originalTxtPath);
            toDecompress.DecompressFile();
            Assert.That(File.Exists(originalTxtPath), Is.True);
            var decompressedContent = File.ReadAllBytes(originalTxtPath);

            File.Delete(compressedTxtPath);

            Assert.That(originalContent, Is.EqualTo(decompressedContent));
        }

        // Decompress an invalid .hf file should return false and not produce output file
        [Test]
        public void DecompressInvalidHFFile_ReturnsFalse()
        {
            string invalidFilePath = directory + "\\testfiles\\invalidHF.hf";
            FileHandle inv = new(invalidFilePath);

            string decompressedFilePath = directory + "\\testfiles\\invalidHF.txt";

            Assert.That(inv.DecompressFile(), Is.False);
            Assert.That(File.Exists(decompressedFilePath), Is.False);
        }

        // Test that decompress method returns false when file doesn't exist
        [Test]
        public void Decompress_NonExistingFile_ReturnsFalse()
        {
            FileHandle fakeFile = new("Fakefile");

            Assert.That(fakeFile.DecompressFile(), Is.False);
        }

        // Test that compress method returns false when file doesn't exist
        [Test]
        public void Compress_NonExistingFile_ReturnsFalse()
        {
            FileHandle fakeFile = new("Fakefile");

            Assert.That(fakeFile.CompressFile(), Is.False);
        }

        // Test that decompress method returns false when file is not of type .hf
        [Test]
        public void Decompressing_NonHFfile_ReturnsFalse()
        {
            Assert.That(toCompress.DecompressFile(), Is.False);
        }

        // Test that decompressing the compressed PDF file produces an output file with original content
        [Test]
        public void Compress_Decompress_PDF ()
        {
            var originalPath = directory + "\\testfiles\\pdfTest.pdf";
            var compressedPath = directory + "\\testfiles\\pdfTest.hf";
            FileHandle pdf = new(originalPath);
            var originalContent = File.ReadAllBytes(originalPath);
            pdf.CompressFile();

            File.Delete(originalPath);
            FileHandle pdfDeco = new(compressedPath);
            pdfDeco.DecompressFile();

            Assert.That(File.Exists(originalPath), Is.True);
            var decompressedContent = File.ReadAllBytes(originalPath);

            File.Delete(compressedPath);

            Assert.That(originalContent, Is.EqualTo(decompressedContent));
        }

        // Test that decompressing the compressed JPG file produces an output file with original content
        [Test]
        public void Compress_Decompress_JPG()
        {
            var originalPath = directory + "\\testfiles\\jpgfile.jpg";
            var compressedPath = directory + "\\testfiles\\jpgfile.hf";
            FileHandle pdf = new(originalPath);
            var originalContent = File.ReadAllBytes(originalPath);
            pdf.CompressFile();

            File.Delete(originalPath);
            FileHandle jpgDeco = new(compressedPath);
            jpgDeco.DecompressFile();

            Assert.That(File.Exists(originalPath), Is.True);
            var decompressedContent = File.ReadAllBytes(originalPath);

            File.Delete(compressedPath);

            Assert.That(originalContent, Is.EqualTo(decompressedContent));
        }

        // Test that decompressing the compressed DOCX file produces an output file with original content
        [Test]
        public void Compress_Decompress_DOCX()
        {
            var originalPath = directory + "\\testfiles\\wordTest.docx";
            var compressedPath = directory + "\\testfiles\\wordTest.hf";
            FileHandle pdf = new(originalPath);
            var originalContent = File.ReadAllBytes(originalPath);
            pdf.CompressFile();

            File.Delete(originalPath);
            FileHandle docDeco = new(compressedPath);
            docDeco.DecompressFile();

            Assert.That(File.Exists(originalPath), Is.True);
            var decompressedContent = File.ReadAllBytes(originalPath);
            
            File.Delete(compressedPath);

            Assert.That(originalContent, Is.EqualTo(decompressedContent));
            
        }

        // Test that compress method returns false when the file supplied is empty
        [Test]
        public void Compress_CompressingEmptyFile_ReturnsFalse()
        {
            FileHandle emptyFile = new(directory + "\\testfiles\\emptyfile.txt");
            Assert.That(emptyFile.CompressFile(), Is.False);
        }

        // Test the ByteToBin method, converts a byte to a binary string
        [Test]
        public void TestByteToBin()
        {
            byte testByte = 170; 
            string binaryString = Utilities.ByteToBin(testByte);
            Assert.That(binaryString, Is.EqualTo("10101010"));
        }

        // Test the BinToByte method, converts a binary string to a byte
        [Test]
        public void TestBinToByte()
        {
            string binaryString = "10101010"; 
            byte resultByte = Utilities.BinToByte(binaryString);
            Assert.That(resultByte, Is.EqualTo(170));
        }

        // Test the StrToBinStr method, converts a string to its binary representation
        [Test]
        public void TestStrToBinStr()
        {
            string input = "Hello";
            string expected = "0100100001100101011011000110110001101111";
            string result = Utilities.StrToBinStr(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        // Test the BinStrToStr method, converts a binary string to its original
        [Test]
        public void TestBinStrToStr()
        {
            string input = "0100100001100101011011000110110001101111";
            string expected = "Hello";
            string result = Utilities.BinStrToStr(input);
            Assert.That(result, Is.EqualTo(expected));
        }

        // Test that decoding encoded file content produces the same byte array as the original
        [Test] 
        public void DecodingEncodedFileContent_ProducesOriginalByteArray()
        {
            var originalContent = File.ReadAllBytes(originalTxtPath);
            toCompress.CompressFile();
            var encodedContent = toCompress.EncodeFile(originalContent);
            var decodedContent = toCompress.DecodeFile(encodedContent);

            Assert.That(decodedContent, Is.EqualTo(originalContent));
        }

        // Test that decoding an encoded tree produces the same tree as the original
        [Test]
        public void DecodingEncodedTree_ProducesOriginalTree()
        {
            HuffmanTree tree = new();
            byte[] arr = { 1, 3, 6, 8, 102, 187, 137, 23 };
            tree.ConstructTreeFromArray(arr);
            var encodedTree = tree.EncodeTree(tree.Root, "");

            HuffmanTree newTree = new();
            newTree.DecodeTree(encodedTree);

            Assert.That(CompareTrees(tree.Root, newTree.Root), Is.True);
        }

        // Helper method to check if two trees are the same
        // we don't look at occurences since they are not restored, only the actual data in the node
        private bool CompareTrees(TreeNode? node1, TreeNode? node2)
        {
            // If both nodes are null, they are identical
            if (node1 == null && node2 == null)
            {
                return true;
            }

            if(node1 == null && node2 != null)
            {
                return false;
            }

            if (node1 != null && node2 == null)
            {
                return false;
            }

            if (node1.Data != node2.Data)
            {
                return false;
            }

            return CompareTrees(node1.Left, node2.Left) && CompareTrees(node1.Right, node2.Right);
        }

    }
}