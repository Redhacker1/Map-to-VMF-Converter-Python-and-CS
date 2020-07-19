using Py_embedded_v37;
namespace MapConverter
{
    class Map_converter

    { 

        //static string beginpath = "/home/donovanariesstrawhacker/Documents/GitHub/Quake_source_Tools/maps/map_files/quake_1/";
        //static string beginpath_windows = "C:/Users/donov/Documents/GitHub/Quake_source_Tools/maps/map_files/quake_1/";
        static readonly Map_File_lib Maplib = new Map_File_lib();
        static readonly General_libs.Array_Dict_and_list_library List = new General_libs.Array_Dict_and_list_library();
        static readonly General_libs.Text_Modification_Library TextLib = new General_libs.Text_Modification_Library(); 
        static void Main(string[] args)
        {
            {

                PythonAbstractions Python_Functions = new PythonAbstractions();
                Python_Functions.RunFunction(ScriptName: "ConfigLib", FuncName: "main");
                
            }
        }
    }
}
