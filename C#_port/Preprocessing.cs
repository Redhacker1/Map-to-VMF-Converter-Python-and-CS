using System;
using System.Collections.Generic;
using System.Text;

namespace MapConverter
{
    class Preprocessing
    {
        public static string[] BreakBrushes(Node Entity)
        {
            StringBuilder current_Brush = new StringBuilder();
            int BrushCount = 0;
            List<string> Brushes = new List<string>();
            char Previous_Value = '\n';
            foreach (char Character in Entity.Keyvalue)
            {
                if (Previous_Value == '{' && Character == '\n')
                {
                    current_Brush.Clear();
                    BrushCount++;
                }
                else if (Character == '}' && Previous_Value == '\n')
                {
                    Brushes.Add(current_Brush.ToString());
                }
                else
                {
                    current_Brush.Append(Character);
                }
                Previous_Value = Character;
            }
            Entity.Keyvalue = string.Empty;
            return Brushes.ToArray();
        }

        public static Node Break_Entities(string MapFile)
        {
            MapFile = MapFile.Remove(MapFile.IndexOf("versioninfo"), MapFile.IndexOf("}")+1);
            MapFile = MapFile.Remove(MapFile.IndexOf("visgroups"), MapFile.IndexOf("}") + 1);
            MapFile = MapFile.Remove(MapFile.IndexOf("viewsettings"), MapFile.IndexOf("}") + 1);
            Console.WriteLine(MapFile);
            Node World = new Node("World");
            int Entity_Reset = 0;
            int Entity_Count = 0;

            string[] FileLines = MapFile.Split('\n');

            for (int increment = 0; increment < FileLines.Length; increment++)
            {
                FileLines[increment] += '\n';
            }
            StringBuilder current_entity = new StringBuilder();
            char previous_char = '\0';

            foreach (string line in FileLines)
            {
                foreach (char character in line)
                {
                    if (character == (char)13)
                    {
                        if (previous_char == '}')
                        {
                            --Entity_Reset;
                            if (Entity_Reset == 0)
                            {
                                if (current_entity[current_entity.Length - 1] == '}')
                                {
                                    current_entity.Remove(current_entity.Length - 1, 1);
                                }
                                if(current_entity.ToString().Contains("editor\n"))
                                {
                                    current_entity.Remove(current_entity.ToString().IndexOf("editor\n"), current_entity.Length -1 - current_entity.ToString().IndexOf("editor\n"));
                                }
                                Node EntityNode = new Node("Entity")
                                {
                                    Keyvalue = current_entity.ToString()
                                };
                                World.Children.Add(EntityNode);
                                Entity_Count++;
                            }
                        }
                        else if (previous_char == '{')
                        {
                            ++Entity_Reset;
                            if (Entity_Reset == 1)
                            {
                                current_entity.Clear();
                            }
                        }
                    }
                    else
                    {
                        current_entity.Append(character);
                        previous_char = character;
                    }
                }

            }

            return World;
        }


    }
}
