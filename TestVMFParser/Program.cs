using MapConverter_Shared;
using System;
using System.Collections.Generic;
using System.IO;
using VMF_Parser;

namespace TestVMFParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Console.ReadLine();
            StreamReader file = new StreamReader(path);

            List<string> First_stage = VMFParser.TokenizeString(file.ReadToEnd());
            Node Data = VMFParser.VMFParser_logic(First_stage);
        }
    }
}
