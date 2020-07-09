using System;
using System.Collections.Generic;
using System.IO;



namespace General_libs
{
    public class CSVLibrary
    {
        readonly Text_Modification_Library TextLib = new Text_Modification_Library();
        readonly Array_Dict_and_list_library ListLib = new Array_Dict_and_list_library();


        public string[] GetLineArray(Dictionary<int, dynamic>File_contents_imported, int line_number, bool LFS)
        {
            var line_data = File_contents_imported[line_number];
            if(LFS)
            {
                return line_data.Split(',');
            }
            else
            {
                return line_data;
            }
        }


        public Dictionary<int, dynamic> ImportCSV(string path, bool ForceLFS = false)
        {
            Console.WriteLine(ForceLFS);
            var fs = TextLib.Open(path);
            bool LFS = false;
            if(ForceLFS == true)
            {
                LFS = true;
            }
            Dictionary<int, dynamic> dict = new Dictionary<int, dynamic>();
            int lines = (int)TextLib.CountLinesReader(path);
            for (int i = 1; i < lines; i++)
            {
                string file_line1 = fs.ReadLine();
                if (!LFS)
                {
                    string[] values1 = file_line1.Split(',');
                    dict.Add(i-0, values1);
                }

                else
                    {
                        dict.Add(i, file_line1);
                    }
                
                
            }

            dict.Add(-1, LFS);

            return dict;
        }

        public void Write_back_to_CSV(string path, bool LFS, Dictionary<int,dynamic> file_contents_imported)
        {
            StreamWriter file =  TextLib.Open(path,"w");
            if(file_contents_imported.ContainsKey(0))
            {
                if(LFS)
                {
                    string text = file_contents_imported[0];
                    file.Write(text + '\n');
                }
                else
                {
                    string text = ListLib.CS_Array_to_Human_List(file_contents_imported[0]);
                    file.Write(text + '\n');
                }
            }
            if (LFS)
            {
                foreach(var item in file_contents_imported)
                {
                    if(item.Key != 0)
                    {
                        var thing = item.Value;
                        if (thing.GetType().FullName == "System.String")
                        {
                            dynamic text = item.Value;
                            file.Write(text + '\n');
                        }

                    }
                    
                }
            }
            else
            {
                foreach (var item in file_contents_imported)
                {
                    if (item.Key != 0 )
                    {
                        var thing = item.Value;
                        if (thing.GetType().FullName == "System.String[]")
                        {
                            dynamic text = item.Value;
                            text = ListLib.CS_Array_to_Human_List(text);
                            file.Write(text + '\n');
                        }
                    }
                        
                }
            }
            file.Close();

        }

    }
}
