using System;
using System.Linq;
using System.Collections.Generic;

namespace MapConverter
{
    class Map_File_lib
    {
        //currently filled with temporary variables so I do not have to worry about variable placement
        General_libs.Text_Modification_Library Text = new General_libs.Text_Modification_Library();
        General_libs.Array_Dict_and_list_library List = new General_libs.Array_Dict_and_list_library();
        string current_entity = string.Empty;
        List<string> entity_list = new List<string>(); 
        List<int> Buffer = new List<int>();
        int Entity_Reset = 0;



        //text library
        readonly General_libs.Text_Modification_Library TextLib = new General_libs.Text_Modification_Library();
        //List library
        readonly General_libs.Array_Dict_and_list_library ListLib = new General_libs.Array_Dict_and_list_library();
        public void ImportMAPfile(string path)
        {
            string current_entity = string.Empty;
            int lines;
            var fs = TextLib.Open(path);
            lines = TextLib.CountLinesReader(path);

                for (int i = 1; i < lines; i++)
                {
                string file_line = TextLib.AppendString(fs.ReadLine(), "\n");
                    for (int item = 0; item < file_line.Length; item++)
                    {
                        char character = file_line[item];
                        if (Buffer.Count == 2)
                        {
                            Buffer = List.Rotate_List_int(Buffer, -1);
                            Buffer.RemoveAt(Buffer.Count());
                            Buffer.Add(character);
                        }
                        if (ListLib.Compare_List_int(Buffer, new List<int> { '{', '\n' }))
                        {
                            Entity_Reset++;
                            if (Entity_Reset == 1)
                            {
                                current_entity = "";
                            }
                        }

                        else if (ListLib.Compare_List_int(Buffer, new List<int> { '}', '\n' }))
                        {
                            Entity_Reset--;
                            if (Entity_Reset == 0)
                            {
                                entity_list.Add(current_entity);
                            }
                        }

                    current_entity = TextLib.AppendString(current_entity, character.ToString());
                    }

                    Console.WriteLine(i);
                }

            //Console.WriteLine(Entity_Reset);
        }
            
    }
}

