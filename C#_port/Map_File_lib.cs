using System;
using System.Collections.Generic;
using System.Text;

namespace MapConverter
{
    class Map_File_lib
    {
        //currently filled with temporary variables so I do not have to worry about variable placement
        General_libs.TextModificationLibrary Text = new General_libs.TextModificationLibrary();
        General_libs.List_Modification_Lib List = new General_libs.List_Modification_Lib();
        string oldcharacter_character = string.Empty;
        string current_entity = string.Empty;
        int[] Buffer = new int[2];



        //text library
        readonly General_libs.TextModificationLibrary TextLib = new General_libs.TextModificationLibrary();
        public void ImportMAPfile(string path)
        {
            var fs = TextLib.Open(path);
            int lines = (int)TextLib.CountLinesReader(path);
            for (int i = 1; i < lines; i++)
            {
                string file_line = fs.ReadLine();
                foreach (char character in file_line)
                    {
                    string stringcharacter = character.ToString();
                    if (Buffer.Length > 2)
                    {
                        Buffer = List.Rotate_List((dynamic)Buffer, -1);
                        Buffer[1] = (int)character;
                    }
                    if (oldcharacter_character + stringcharacter == "{\n")
                    {
                        current_entity = "";
                    }

                    else if (oldcharacter_character + stringcharacter == "}\n")
                    {
                            
                    }

                       oldcharacter_character = stringcharacter;
                    }




            }
        }
    }
}
