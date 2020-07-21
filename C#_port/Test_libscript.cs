using System;
using System.Collections.Generic;
using System.IO;

namespace MapConverter
{
    public class Testscripts
    {
        static General_libs.Array_Dict_and_list_library TestingListLibrary = new General_libs.Array_Dict_and_list_library();
        static readonly General_libs.Text_Modification_Library TextLib = new General_libs.Text_Modification_Library();
        static public void Rotate_test()
        {
            Console.WriteLine(new List<dynamic> { 8, 83, 80, 70 }[2]);
            Console.WriteLine(TestingListLibrary.Rotate_List(new List<dynamic> { 8, 83, 80, 70 }, -1)[1]);
            Console.WriteLine(TestingListLibrary.Rotate_List(new List<dynamic> { 8, 83, 80, 70 }, -1)[2]);
            Console.WriteLine(new List<dynamic> { 8, 83, 80, 70 }[1]);
        }
        static public void ReadTest(string path)
        {
            int lines;
            StreamReader fs = TextLib.Open(path);
            lines = TextLib.CountLinesReader(path);
            string[] file_line = fs.ReadToEnd().Split('\n');

            for (int linecount = 0; linecount < file_line.Length; linecount++)
            {
                file_line[linecount] += '\n';
            }


            foreach (var Line in file_line)
            {
                Console.WriteLine(Line);
            }


        }
    }
}