using System;
using System.Collections.Generic;
using System.Text;

namespace MapConverter
{
    class Map_File_lib
    {
        //currently filled with temporary variables so I do not have to worry about variable placement
        string oldcharacter = string.Empty;
        string current_entity = string.Empty;


        //text library
        readonly TextModificationLibrary TextLib = new TextModificationLibrary();
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
                        if (oldcharacter + stringcharacter == "{\n")
                        {
                            current_entity =
                        }

                        else if (oldcharacter + stringcharacter == "}\n")
                        {

                        }

                         oldcharacter = stringcharacter;
                    }




            }
        }
    }
}
