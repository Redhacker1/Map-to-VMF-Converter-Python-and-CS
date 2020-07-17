using System;
using System.Collections.Generic;

namespace MapConverter
{
    class Map_converter

    { 

        //static string beginpath = "/home/donovanariesstrawhacker/Documents/GitHub/Quake_source_Tools/maps/map_files/quake_1/";
        static string beginpath_windows = "C:/Users/donov/Documents/GitHub/Quake_source_Tools/maps/map_files/quake_1/";
        static readonly Map_File_lib Maplib = new Map_File_lib();
        static readonly General_libs.Array_Dict_and_list_library List = new General_libs.Array_Dict_and_list_library();
        static readonly General_libs.Text_Modification_Library TextLib = new General_libs.Text_Modification_Library(); 
        static void Main(string[] args)
        {
            {
                //var pythonPath = @"C:\Users\donov\Documents\GitHub\Map-to-VMF-Converter-Python-and-CS\C#_port\bin\x64\Debug\netcoreapp3.1\thirdparty\python";

                Py_embedded_v37.PythonAbstractions Python = new Py_embedded_v37.PythonAbstractions();
                Python.RunScript(@"C:\Users\donov\Documents\GitHub\Map-to-VMF-Converter-Python-and-CS\EmbededPy3.7\Scripts", "ConfigLib.py");
                
            }
        }
    }
}
