using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace MapConverter
{
    class Map_File_lib
    {
        //currently filled with temporary variables so I do not have to worry about variable placement
        General_libs.Text_Modification_Library Text = new General_libs.Text_Modification_Library();
        General_libs.Array_Dict_and_list_library List = new General_libs.Array_Dict_and_list_library();



        //text library
        readonly General_libs.Text_Modification_Library TextLib = new General_libs.Text_Modification_Library();
        //List library
        readonly General_libs.Array_Dict_and_list_library ListLib = new General_libs.Array_Dict_and_list_library();

        public string[] ImportMAPfile(string path)
        {
            List<string> entity_list = new List<string>();
            int Entity_Reset = 1;

            var fs = TextLib.Open(path);
            string[] FileLines = fs.ReadToEnd().Split('\n');
            int lines = FileLines.Count();

            for (int i = 1; i < lines; i++)
            {
                string file_line = FileLines[i] + '\n';
                for (int characterpos = 0; characterpos < file_line.Length; characterpos++)
                {
                    List<char> Buffer = new List<char>();
                    StringBuilder current_entity = new StringBuilder();
                    char character = file_line[characterpos];
                    if (Buffer.Count == 2)
                    {
                        Buffer[0] = Buffer[1];
                        Buffer[1] = character;
                    }
                    else
                    {
                        Buffer.Add(character);
                    }

                    if (Buffer.Count == 2)
                    {

                        if (Buffer[0] == '{' && Buffer[1] == (char)13)
                        {
                            if (Entity_Reset == 0)
                            {
                                Entity_Reset++;
                                current_entity = new StringBuilder();
                                current_entity.Append('{');
                            }
                        }

                        else if (Buffer[0] == '}' && Buffer[1] == (char)13)
                        { 
                            if (Entity_Reset == 1)
                            {
                                Entity_Reset--;
                                entity_list.Add(current_entity.ToString());
                            }
                        }
                    }

                    current_entity.Append(character);
                }

            }
            return entity_list.ToArray();
        }
    }
}

