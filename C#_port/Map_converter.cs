using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MapConverter
{
    class Map_converter

    { 

        static string beginpath = "C:/Users/donov/Documents/GitHub/Quake_source_Tools/maps/map_files/quake_1/";
        static readonly Map_File_lib Maplib = new Map_File_lib();
        static readonly General_libs.List_Modification_Lib List = new General_libs.List_Modification_Lib();
        static void Main(string[] args)
        {
            int[] Buffer = { 123, 10 };

        Maplib.ImportMAPfile(beginpath + "E3M5.MAP");
        Console.WriteLine(List.Compare_Arrays(Buffer, new int[2] { 123, 10 }));
        }
    }
}
