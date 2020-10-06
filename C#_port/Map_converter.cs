using Py_embedded;
using System;
using System.Linq;
using System.Reflection;

namespace MapConverter
{
    class Map_converter

    {

        static string laptopTestPath = @"C:\Users\donov\Documents\GitHub\Quake_source_Tools\maps\map_files\quake_1";
        static string DesktopTestPath = @"C:\Users\Donovan\Documents\GitHub\Quake_source_Tools\maps\map_files\quake_1\";
        static void Main()
        {
            Map_File_lib Library = new Map_File_lib();
            Library.ImportMAPfile( laptopTestPath + "START.MAP");
        }
    }
}
