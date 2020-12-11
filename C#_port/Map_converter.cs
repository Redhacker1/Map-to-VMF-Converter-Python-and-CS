using General_libs;
using System;

namespace MapConverter
{
    class Map_converter
    {
        //static string laptopTestPath = @"C:\Users\donov\Documents\GitHub\Quake_source_Tools\maps\map_files\quake_1";
        //static string DesktopTestPath = @"C:\Users\Donovan\Documents\GitHub\Quake_source_Tools\maps\map_files\quake_1\";
        static readonly string ArcaneDimensionsDesktop = @"C:\Users\Donovan\Documents\GitHub\Quake_source_Tools\maps\arcane_dimentions_data\maps\";
        static void Main(string[] FileOpener)
        {
            var OpenFile = ConsoleLibrary.PromptCMD("Path and location to file", SingleChar: false);
            var OutputFilename = ConsoleLibrary.PromptCMD("Name to save this under");
            //string[] Entities = Map_File_lib.ImportMAPfile( DesktopTestPath + "START.MAP");
            var World = Map_File_lib.ImportMapFile(OpenFile);
            var Mapfile = XMLMapWriter.MakeMapFile("/Converted/", OutputFilename);

            
        using (Mapfile)
            {
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
                Mapfile.Close();


            }
        




        }
    }
}
