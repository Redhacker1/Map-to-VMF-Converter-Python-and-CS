using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using General_libs;
using System.Numerics;

namespace MapConverter
{
    class Map_File_lib
    { 


        public string[] ImportMAPfile(string path)
        {
            List<string> entity_list = new List<string>();
            int Entity_Reset = 0;
            var fs = Text_Modification_Library.Open(path);
            string[] FileLines = fs.ReadToEnd().Split('\n');

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
                    current_entity.Append(character);
                    if (character == (char)13)
                    {
                        if (previous_char == '}')
                        {
                            --Entity_Reset;
                            if (Entity_Reset == 0)
                            {
                                entity_list.Add('{' + current_entity.ToString());
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
                    previous_char = character;
                }

            }

            return entity_list.ToArray();
        }

        public Map Create_entity_dictionary(string[] entities_list)
        {
            Map Current_map = new Map();
            List<Point_Entity> point_Entities = new List<Point_Entity>();
            List<BrushEntity> brush_Entities = new List<BrushEntity>();
            int i;
            try
            {
                for (int Entity_index= 0; Entity_index < entities_list.Length; Entity_index++)
                {
                    i = Entity_index + 1;
                    // Remove char 13 in ascii table because it fucks with \n as \n is ascii character 10
                    string entity = entities_list[Entity_index].Replace(((char)13).ToString(), "").Trim();

                    //get all attributes
                    Dictionary<string, string> entity_attributes = detect_attributes(entity);
                    // These lines get the classname and remove it from the previous dictionary it was attached to.
                    string classname = entity_attributes["classname"]; 
                    entity_attributes.Remove("classname");

                    // Only happens when dealing with brush entities!
                    if (entity.Contains("}\n}"))
                    {
                        // Grabs ALL brushes in entity as a single string Note: This is the ENTIRE ENTITY in string f
                        string[] ent_data = entity.Split("{\n");
                        // Offset to account for data pulled earlier 
                        int brushcount = ent_data.Length;
                        // The list of brushes in USABLE FORM to be appended to the brush entity
                        var allbrushes = new List<Brush>();

                        for (int brushnumber = 0; brushnumber < brushcount - 2; brushnumber++)
                        {
                            string[] sides = ent_data[brushnumber].Split('\n');
                            List<Side> Sides = new List<Side>(); 
                            for (int sideindex = 0; sideindex < sides.Length - 1; ++sideindex)
                            {
                                string[] side_string_coords = sides[sideindex].Split(")");

                                List<Vector3> TotalPoints = new List<Vector3>();
                                foreach (string side_attributes in side_string_coords)
                                {
                                    if (side_attributes != string.Empty && !side_attributes.Contains("//"))
                                    {
                                        string[] pointLocation = Text_Modification_Library.Remove(Text_Modification_Library.Remove(side_attributes, "("), "}").Trim().Split(' ');

                                        if (pointLocation.Length == 3)
                                        {
                                            TotalPoints.Add(new Vector3(Convert.ToInt32(pointLocation[0]), Convert.ToInt32(pointLocation[1]), Convert.ToInt32(pointLocation[2])));

                                        }

                                    }
                                }
                                if (side_string_coords.Length == 4)
                                {
                                    string[] Other_Values = side_string_coords[3].Split(' ');
                                    Sides.Add(new Side(Other_Values[1], TotalPoints.ToArray(), Convert.ToSingle(Other_Values[2]), Convert.ToSingle(Other_Values[3]), Convert.ToSingle(Other_Values[4]), Convert.ToSingle(Other_Values[6]), Convert.ToSingle(Other_Values[5].Trim()), new Vector3(0, 0, 0), new Vector3(0, 0, 0), sideindex));
                                }
                                else if (side_string_coords.Length != 1)
                                {
                                    Console.WriteLine("Malformed Brush... Skipping");
                                    Console.ReadKey();
                                    break;
                                }
                            }
                            allbrushes.Add(new Brush(Sides.ToArray()));
                        }
                        brush_Entities.Add(new BrushEntity(entity_attributes,classname, allbrushes.ToArray()));

                        //var current_entity  = new BrushEntity(entity_attributes,classname);
                        if ("a" == "worldspawn")
                        {

                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        if( entity_attributes.ContainsKey("origin"))
                        {
                            string[] preveclocation = entity_attributes["origin"].Split(' ');
                            entity_attributes.Remove("origin");
                            Vector3 Veclocation = new Vector3(Convert.ToInt32(preveclocation[0]), Convert.ToInt32(preveclocation[1]), Convert.ToInt32(preveclocation[2]));
                            point_Entities.Append(new Point_Entity(Veclocation, entity_attributes, classname));
                            
                        }
                    }
                }
                return new Map(point_Entities.ToArray(), brush_Entities.ToArray());
            }
            catch (Exception Error)
            {
                Console.WriteLine(Error.Message);
                //Console.WriteLine(Error.InnerException.Message);
                Environment.Exit(2);
            }

            return Current_map;
        }

        Dictionary<string, string> detect_attributes(string ent_data)
        {
            Dictionary<string, string> attributes_dict = new Dictionary<string, string>();
            string[] attributes_list = ent_data.Split("\"\n");

            for (int i = 0; i < attributes_list.Count(); ++i)
            {
                attributes_list[i] = attributes_list[i] + '"';
                string attribute_name = Text_Modification_Library.Find_between_two_values(attributes_list[i], "\"", "\"").Trim();
                if (attribute_name != string.Empty)
                {
                    string attribute = find_attribute(attributes_list[i], attribute_name, "\n").Trim();

                    if(attribute != string.Empty)
                    {
                        attributes_dict[attribute_name] = attribute;
                    }
                }  
                
            }

            return attributes_dict;
        }

        string find_attribute(string ent_data, string target_attribute, string end_location)
        {
            string attribute = Text_Modification_Library.Find_between_two_values(ent_data, target_attribute, end_location);
            if (attribute != string.Empty)
            {
                attribute = Text_Modification_Library.Remove(attribute, '"'.ToString());
                string wierdstring = "  ";
                attribute = Text_Modification_Library.Remove(attribute, wierdstring).Trim();
            }
            else
            {
                attribute = "";
            }
            return attribute;
        }

        string Remove_cpp_style_comments(string input)
        {
            string[] lines = input.Split('\n');
            Dictionary<int, string> lines_dict = new Dictionary<int, string>();
            int number = 0;
            StringBuilder Stringrebuilder = new StringBuilder();

            foreach (string line in lines)
            {
                if (line.Contains(@"//"))
                {
                    string line_modified = Text_Modification_Library.Remove(line, Text_Modification_Library.Find_between_two_values(line, "//", "\n"));
                    line_modified = Text_Modification_Library.Remove(line_modified, @"//");
                    lines_dict[number] = line_modified;
                }
                else
                {
                    lines_dict[number] = line;
                }
                number += 1;
            }
            foreach (var keyValuePair in lines_dict)
            {
                Stringrebuilder.Append(keyValuePair.Value + '\n');
            }
            return Stringrebuilder.ToString();
        }

        string Unwrap(string input_string, char left_side, char right_side, bool add_back_newline= false)
        {
            input_string = input_string.Trim();
            input_string = input_string.TrimStart(left_side);
            input_string = input_string.TrimEnd(right_side);

            if (add_back_newline)
            {
                input_string += '\n';
            }
            return input_string;
        }

        public Dictionary<int, Dictionary<string, dynamic>> Read_side(string[] lines, bool mute= true)
        {
            if (!mute)
                Console.WriteLine("Extracting Data");
            Dictionary<int, Dictionary<string, dynamic>> plane_dict = new Dictionary<int, Dictionary<string, dynamic>>();
            int internal_side_id = 0;

            foreach (string line in lines)
            {
                if (line.Contains('(') && line.Contains(')'))
                {
                    internal_side_id += 1;

                    string value = Text_Modification_Library.Remove(line, "[");
                    value = Text_Modification_Library.Remove(value, "]");
                    value = Text_Modification_Library.Remove(value, "( ");
                    value = Text_Modification_Library.Replace(value, "  ", " ");
                    string[] value_final = value.Split(" ) ");

                    List<string> other_values = value_final[3].Split(" ").ToList();

                    foreach(string item in other_values)
                    { 
                        if (item == " ")
                        {
                            other_values.Remove(" ");
                        }
                    }

                    if (true != other_values.Count < 6)
                    {
                        Console.WriteLine("Valid Brush");
                        Dictionary<string, dynamic> CurrentSideDict = new Dictionary<string, dynamic>
                        {
                            { "points", new string[3] { '(' + value_final[0] + ')', '(' + value_final[1] + ')', '(' + value_final[2] + ')' } },
                            { "Material", other_values[0] },
                            { "x_off", other_values[1] },
                            { "y_off", other_values[2] },
                            { "rot_angle", other_values[3] },
                            { "x_scale", other_values[4] },
                            { "y_scale", other_values[5] }
                        };
                        plane_dict.Add(internal_side_id, CurrentSideDict);
                    }
                    else
                    {
                        Console.WriteLine("Error");
                        Console.WriteLine(other_values.Count);
                    }

                }
                else
                {
                    internal_side_id = 0;
                }
            }
            Console.WriteLine(plane_dict.Keys.Count);
            return plane_dict;

        }

        public void WriteVMF(string filename, Dictionary<int, string> Entites, Dictionary<int, Dictionary<string, dynamic>> WorldSpawn_brushes)
        {
            bool first_write = true;
            Console.WriteLine("Writing Data, opening destination file...");
            StreamWriter file_1 = Text_Modification_Library.Open(filename + ".vmf", "w");
            file_1.Write("versioninfo\n{\n}\n\n'viewsettings\n{\n}\n\nworld\n{\n\"id\" \"1\"\n\t\"mapversion\" \"1\"\n\t\"classname\" \"worldspawn\"\n");
            Console.WriteLine("retrieving brush data...");
            string ending;
            foreach (KeyValuePair<int, Dictionary<string, dynamic>> item in WorldSpawn_brushes)
            {
                if( item.Key == 1)
                {
                    ending = End_brushes(first_write: first_write);
                    if (first_write)
                    {
                        first_write = false;
                    }
                    file_1.Write(ending);
                }
                string side_value = Make_Side_Of_Brush(item.Value);
                Console.WriteLine(side_value);
                file_1.Write(side_value);
            }
            Console.WriteLine("finishing up brush data...");
            ending = End_brushes(true);
            Console.WriteLine("finishing up brush data");
            file_1.Write(ending);
            Console.WriteLine("Creating entity list...");
            
        }

        private string Make_Side_Of_Brush(Dictionary<string, dynamic> properties)
        {
            string texture_dir = "quake/";
            Dictionary<string, string> Texture_replacement_dictionary = new Dictionary<string, string> { { "TRIGGER", "tools/toolstrigger" }, { "CLIP", "tools/toolsclip" } };
            string side = properties["points"];
            side = Text_Modification_Library.Remove(side,"[");
            side = Text_Modification_Library.Remove(side, "]");
            side = Text_Modification_Library.Remove(side, ",");
            side = Text_Modification_Library.Remove(side, "'");
            side.Trim();
            side = Text_Modification_Library.Replace(side, ") ", ")");
            string texture;
            if (!Texture_replacement_dictionary.ContainsValue(properties["Material"]))
            {
                texture = texture_dir + Text_Modification_Library.Remove(properties["Material"], "#");
                texture = Text_Modification_Library.Remove(texture, "*");
                texture = texture.ToLower();
            }
            else
            {
                texture = Texture_replacement_dictionary[properties["Material"]];
            }
            string y_scale = properties["y_scale"];
            string x_scale = properties["x_scale"];
            string offset_x = properties["x_off"];
            string offset_y = properties["y_off"];
            string plane = "side\n{{\n\t\"id\" \"" + properties["ID"] + "\"\n\t\"plane\" \"" + side + "\"\n\t\"material\" \"" + texture + "\"\n\t\"uaxis\" \"[0 0 0 " + offset_x + "] " + x_scale + "\"\n\t\"vaxis\" \"[0 0 0 " + offset_y + "] " + y_scale + "\"\n\t\"rotation\" \"" + properties["rot_angle"] + "\"\n\t\"lightmapscale\" \"16\"\n\t\"smoothing_groups\" \"0\"\n}}";
            return plane;
        }

    string End_brushes(bool end_file= false, bool first_write= false)
        {
            if (end_file)
            {
                return "}}";
            }
            else if (!first_write)
            {
                return "\n\t\t}\n\tsolid\n\t{";
            }
            else
            {
                return "\n\tsolid\n\t{";
            }
        }
    }
}

