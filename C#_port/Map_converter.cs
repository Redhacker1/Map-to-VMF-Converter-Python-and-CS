using System;

namespace MapConverter
{
    class Map_converter

    { 

        static string beginpath = "C:/Users/donov/Documents/GitHub/Quake_source_Tools/maps/map_files/quake_1/";
        static Map_File_lib Maplib = new Map_File_lib();
        static void Main(string[] args)
        {
            int[] Buffer = { 123, 10 };
            int[] firstcondition = { 123, 10 };
        //Maplib.ImportMAPfile(beginpath + "START.map");
        Console.WriteLine(Buffer == firstcondition);
        }
    }
}
