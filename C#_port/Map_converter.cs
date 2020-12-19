using General_libs;
using System;
using System.Xml;

namespace MapConverter
{
    class Map_converter
    {
        //static string laptopTestPath = @"C:\Users\donov\Documents\GitHub\Quake_source_Tools\maps\map_files\quake_1";
        //static string DesktopTestPath = @"C:\Users\Donovan\Documents\GitHub\Quake_source_Tools\maps\map_files\quake_1\";
        //static readonly string ArcaneDimensionsDesktop = @"C:\Users\Donovan\Documents\GitHub\Quake_source_Tools\maps\arcane_dimentions_data\maps\";
        static void Main(string[] FileOpener)
        {
            string InputFilename = string.Empty;
            string OutputFilename = string.Empty;
            Map World;
            bool EndXML = false;

            if (FileOpener.Length == 0)
            {
                InputFilename = ConsoleLibrary.PromptCMD("Path and location to file", SingleChar: false).Trim();
                OutputFilename = ConsoleLibrary.PromptCMD("Name to save this under").Trim();
                string YN = ConsoleLibrary.PromptCMD("End in .XML? press y for true and anything else for false", SingleChar: true).ToLower();
                if(YN == "y")
                {
                    EndXML = true;
                }
                else
                {
                    EndXML = false;
                }
            }
            else if (FileOpener.Length == 2)
            {
                InputFilename = FileOpener[0];
                OutputFilename = FileOpener[1];
            }
            else if (FileOpener.Length == 3)
            {
                InputFilename = FileOpener[0];
                OutputFilename = FileOpener[1];
                try
                {
                    EndXML = Convert.ToBoolean(FileOpener[2]);
                }
                catch
                {
                    EndXML = false;
                }
            }
            else
            {
                Console.WriteLine("ERROR: Malformed Input");
            }

            if(!OutputFilename.ToLower().EndsWith(".xml") && EndXML)
            {
                OutputFilename += ".xml";
            }

            World = Map_File_lib.ImportMapFile(InputFilename);
            XmlWriter Mapfile = XMLMapWriter.MakeMapFile("", OutputFilename);


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
