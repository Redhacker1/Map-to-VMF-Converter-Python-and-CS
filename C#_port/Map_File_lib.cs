using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MapConverter
{
    class Map_File_lib
    {
        //currently filled with temporary variables so I do not have to worry about variable placement
        General_libs.Text_Modification_Library Text = new General_libs.Text_Modification_Library();
        General_libs.Array_Dict_and_list_library List = new General_libs.Array_Dict_and_list_library();
        string oldcharacter_character = string.Empty;
        string current_entity = string.Empty;
        int[] Buffer = new int[2];



        //text library
        readonly General_libs.Text_Modification_Library TextLib = new General_libs.Text_Modification_Library();
        public void ImportMAPfile(string path)
        {
            int lines = 0;
            var fs = TextLib.Open(path);
            lines = TextLib.CountLinesReader(path);


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
