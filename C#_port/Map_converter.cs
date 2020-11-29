using General_libs;
using System;

namespace MapConverter
{
    class Map_converter

    {
        static Text_Modification_Library Textlibrary = new Text_Modification_Library();
        //static string laptopTestPath = @"C:\Users\donov\Documents\GitHub\Quake_source_Tools\maps\map_files\quake_1";
        static string DesktopTestPath = @"C:\Users\Donovan\Documents\GitHub\Quake_source_Tools\maps\map_files\quake_1\";
        static string ArcaneDimensionsDesktop = @"C:\Users\Donovan\Documents\GitHub\Quake_source_Tools\maps\arcane_dimentions_data\maps\";
        static void Main(string[] FileOpener)
        {
            foreach(string Argument in FileOpener)
            {
                Console.WriteLine(Argument);
            }
            //string[] Entities = Map_File_lib.ImportMAPfile( DesktopTestPath + "START.MAP");
            var World = Map_File_lib.ImportMapFile(ArcaneDimensionsDesktop + "ad_sepulcher.map");
            var Mapfile = XMLMapWriter.MakeMapFile("", "TestMap.xml", false);

            
        using (Mapfile)
            {
                Console.WriteLine("Hello?");
                XMLMapWriter.StartEntities(Mapfile);
                foreach (Point_Entity entity in World.PEntities)
                {
                    XMLMapWriter.PointEntityToXML(Mapfile, entity);
                }
                foreach (BrushEntity entity in World.BEntities)
                {
                    XMLMapWriter.BrushEntityToXML(Mapfile,entity, true);
                }
                XMLMapWriter.EndEntities(Mapfile);
                XMLMapWriter.EndFile(Mapfile);
                Console.WriteLine("Hello?");
                Mapfile.Close();


            }
        




        }
    }
}
