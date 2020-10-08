using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using General_libs;

namespace MapConverter
{
    class Map_converter

    {
        static Text_Modification_Library Textlibrary = new Text_Modification_Library();
        static string laptopTestPath = @"C:\Users\donov\Documents\GitHub\Quake_source_Tools\maps\map_files\quake_1";
        static string DesktopTestPath = @"C:\Users\Donovan\Documents\GitHub\Quake_source_Tools\maps\map_files\quake_1\";
        static string ArcaneDimensionsDesktop = @"C:\Users\Donovan\Documents\GitHub\Quake_source_Tools\maps\arcane_dimentions_data\maps\";
        static void Main()
        {
            Map_File_lib Library = new Map_File_lib();
            string[] Entities = Library.ImportMAPfile( ArcaneDimensionsDesktop + "ad_sepulcher.map");
            Dictionary<int,string> entity_Dictionary = Library.Create_entity_dictionary(Entities);
            string[] brushes = Library.prep_brushes(entity_Dictionary[1]);
            Library.read_side(brushes, false);
        }
    }
}
