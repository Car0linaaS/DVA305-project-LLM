using Huffman;
using System.Collections;

FileHandle file;

switch (args.Length)
{
    case 0:
        Console.WriteLine("Please provide full file path as argument.");
        break;

    case 1:
        file = new(args.First());
        if(file.CompressFile())
        {
            Console.WriteLine("File successfully compressed.");
        }
        else
        {
            Console.WriteLine("Error compressing file.");
        }
        break;

    case 2:
        if (args.First().Equals("-d"))
        {
            file = new(args[1]);
            if (file.DecompressFile())
            {
                Console.WriteLine("File successfully decompressed.");
            }
            else
            {
                Console.WriteLine("Error decompressing file.");
            }
        }
        else
        {
            Console.WriteLine("Argument error. Use -d followed by the full .hf file path to decompress file");
        }
        break;

    default:
        Console.WriteLine("Argument error. To compress file:\nProvide full file path as argument.\nTo decompress file:\nUse -d followed by the full .hf file path to decompress file");
        break;
}