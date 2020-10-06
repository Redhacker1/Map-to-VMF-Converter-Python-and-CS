using Py_embedded;
using System;
using System.Linq;
using System.Reflection;

namespace MapConverter
{
    class Map_converter

    {
        static void Main()
        {
            Map_File_lib Library = new Map_File_lib();
            Library.ImportMAPfile(@"C:\Users\Donovan\Documents\GitHub\Quake_source_Tools\maps\map_files\quake_1\START.MAP");
        }
    }
}
