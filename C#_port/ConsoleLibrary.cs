using System;
using System.Collections.Generic;
using System.Text;

namespace MapConverter
{
    class ConsoleLibrary
    {
        public static string PromptCMD(string Prompt_Text, string Line_Marker = ":>", bool SingleChar = false)
        {
            Console.WriteLine(Prompt_Text);
            Console.Write(Line_Marker);
            string Output;
            if (!SingleChar)
            {
                Output = Console.ReadLine();
            }
            else
            {
                Output = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();
            }
            return Output;
        }
    }
}
