using System;

namespace MapConverter
{
    class Map_converter

    { 

        static string beginpath = "/home/donovanariesstrawhacker/Documents/GitHub/Quake_source_Tools/maps/map_files/quake_1/";
        //static string beginpath_windows = "C:/Users/donov/Documents/GitHub/Quake_source_Tools/maps/map_files/quake_1/";
        static readonly Map_File_lib Maplib = new Map_File_lib();
        static readonly General_libs.Array_Dict_and_list_library List = new General_libs.Array_Dict_and_list_library();
        static void Main(string[] args)
        {
            int[] Buffer = { 123, 10 };

        Maplib.ImportMAPfile(beginpath + "B_ARMOR3.MAP");
        //Test_libscript.Testscripts.rotate_test();
        }
    }
}
