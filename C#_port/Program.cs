using System;

namespace MapConverter
{
    class Program
    {
        static string beginpath = "D:/Repo's/MAPtoVMF/C#Test/maps/";
        static Map_File_lib Maplib = new Map_File_lib();
        static void Main(string[] args)
        {
            Maplib.ImportMAPfile(beginpath + "START.map");
        }
    }
}
