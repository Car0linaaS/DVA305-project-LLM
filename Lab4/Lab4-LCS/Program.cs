/*************************************
      DVA246 - DoA Fortsättning
     Lab 4 - Dynamic Programming
Av Carolina Smigan och Stina Häggmark
**************************************/

using Lab4_LCS;

Console.WriteLine("Lab 4 - Dynamic Programming, by Carolina Smigan and Stina Häggmark");

LCS LCS_handler = new LCS();

Console.WriteLine("\nPlease enter string number 1: ");
string str1 = Console.ReadLine();
Console.WriteLine("\nPlease enter string number 2: ");
string str2 = Console.ReadLine();

int result = LCS_handler.GetLcsLength(str1, str2);

Console.WriteLine($"\nLCS length for {str2} and {str1} is: {result}");