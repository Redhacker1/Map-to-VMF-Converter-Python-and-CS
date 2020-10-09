using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using General_libs;

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

            foreach (string line in FileLines)
            {
                FileLines[Entity_Reset] = line + '\n';
                ++Entity_Reset;
            }
            Entity_Reset = 0;

            StringBuilder current_entity = new StringBuilder();
            char previous_char = (char)short.MaxValue;

            foreach (string line in FileLines)
            {
                foreach (char character in line)
                {
                    current_entity.Append(character);
                    if (character == (char)13)
                    {
                        if (previous_char == (char)125)
                        {
                            --Entity_Reset;
                            if (Entity_Reset == 0)
                            {
                                entity_list.Add('{' + current_entity.ToString());
                            }
                        }
                        else if (previous_char == (char)123)
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

        public Dictionary<int, string> Create_entity_dictionary(string[] entities_list)
        {
            Console.WriteLine("Processing " + entities_list.Count() + " Different Entities");
            bool raise_exception = true;
            Dictionary<int, string> entities_dict = new Dictionary<int, string>();
            int i;
            try
            {
                Console.WriteLine("This working?");
                for (int Entity_index= 0; Entity_index < entities_list.Count(); ++Entity_index)
                {
                    i = Entity_index + 2; 
                    string entity = entities_list[Entity_index];
                    string classname = find_attribute(entity, "classname", "\n");
                
                    if ("worldspawn" == classname)
                    {
                        entities_dict[1] = entity;
                        i -= 1;
                        raise_exception = false;
                    }
                    else 
                    {
                        entities_dict[i] = entity;
                        i += 1;
                    }
                }
                if (raise_exception)
                {
                    throw new Exception(message: "ERROR: No worldspawn entity found, this is required!, exiting now...");
                }
                    
            }
            catch (Exception Error)
            {
                Console.WriteLine(Error.Message);
                Environment.Exit(2);
            }


            return entities_dict;
        }

        string find_attribute(string ent_data, string target_attribute, string end_location)
        {
            string attribute = find_between_two_values(ent_data, target_attribute, end_location);
            if (attribute != string.Empty)
            {
                attribute = Text_Modification_Library.Remove(attribute, '"'.ToString());
                char weirdchar = (char)9;
                string wierdstring = weirdchar.ToString();
                attribute = Text_Modification_Library.Remove(attribute, wierdstring).Trim();
            }
            else
            {
                attribute = "";
            }
            return attribute;
        }

        public string find_between_two_values(string search, string first_substring, string last_substring, bool suppress_error = false)
        {
            try
            {
                return (search.Split(first_substring))[1].Split(last_substring)[0].Trim();
            }
            catch (Exception)
            {
                if (suppress_error == false)
                {
                    Console.WriteLine(search);
                }
                return "";
            }
        }

        public string[] prep_brushes(string worldspawn)
        {
            string[] brushes = worldspawn.Split('\n');
            List<string> sides = new List<string>();
            foreach (string item in brushes)
            {
                string side = parse_brush(item);
                sides.Add(side);
            }
            return sides.ToArray();
        }

        string[] split_first_instance(string text, char split_by)
        {
            string[] text_split = new string[] { "", "" };
            int split_instance = 0;
            foreach (char item in text)
            {
                if (item == split_by && split_instance == 0)
                {
                    if (text_split[split_instance] != "")
                    {
                        text_split[1] = "";
                        ++split_instance;
                    }
                }
                else if (split_instance == 0)
                {
                    text_split[split_instance] = text_split[split_instance] + item;
                }
                else
                {
                    break;
                }
            }
            string cheat_string = Text_Modification_Library.Remove(text, text_split[0]);
            text_split[1] = cheat_string;
            return text_split;
        }

        string remove_cpp_style_comments(string input)
        {
            string[] lines = input.Split('\n');
            Dictionary<int, string> lines_dict = new Dictionary<int, string>();
            int number = 0;
            StringBuilder Stringrebuilder = new StringBuilder();

            foreach (string line in lines)
            {
                if (line.Contains(@"//"))
                {
                    string line_modified = Text_Modification_Library.Remove(line, find_between_two_values(line, "//", "\n"));
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

        string parse_brush(string brush)
        {
            //Removes comments
            string brush_data = remove_cpp_style_comments(brush);
            brush_data = unwrap(brush_data, '{', '}', true);
            return brush_data;

        }

        string unwrap(string input_string, char left_side, char right_side, bool add_back_newline= false)
        {
            input_string = input_string.Trim();
            input_string = input_string.TrimStart(left_side);
            input_string = input_string.TrimEnd(right_side);

            if (add_back_newline)
            {
                input_string = input_string +  '\n';
            }
            return input_string;
        }

        public Dictionary<int, Dictionary<string, dynamic>> read_side(string[] lines, bool mute= true)
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

                    if (true != other_values.Count < 7)
                    {
                        Console.WriteLine("Valid Brush");
                        Dictionary<string, dynamic> CurrentSideDict = new Dictionary<string, dynamic>();
                        CurrentSideDict.Add("points", new string[3] { '(' + value_final[0] + ')', '(' + value_final[1] + ')', '(' + value_final[2] + ')' });
                        CurrentSideDict.Add("Material", other_values[0]);
                        CurrentSideDict.Add("x_off", other_values[1]);
                        CurrentSideDict.Add("y_off", other_values[2]);
                        CurrentSideDict.Add("rot_angle", other_values[3]);
                        CurrentSideDict.Add("x_scale", other_values[4]);
                        CurrentSideDict.Add("y_scale", other_values[5]);
                        plane_dict.Add(internal_side_id, CurrentSideDict);
                    }

                }
                else
                {
                    internal_side_id = 0;
                }
            }
            Console.WriteLine(plane_dict.Keys.Count());
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
                    ending = end_brushes(first_write: first_write);
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
            ending = end_brushes(true);
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

    string end_brushes(bool end_file= false, bool first_write= false)
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

