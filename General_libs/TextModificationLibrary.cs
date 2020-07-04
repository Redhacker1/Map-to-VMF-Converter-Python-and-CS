using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace General_libs
{
   public class TextModificationLibrary
    {

        public string[] Human_list_to_CS_Array(string ListToTurn, char delimiter)
        {
            ListToTurn += Remove(ListToTurn, "\n");
            string[] ArrayOutput = ListToTurn.Split(delimiter);
            return ArrayOutput;

        }
        public string CS_Array_to_Human_List(dynamic[] ArrayToTurn)
        {
            string text_intermediate = ArrayToString(ArrayToTurn);
            text_intermediate = Replace(text_intermediate, ", ", ",");
            text_intermediate = Remove(text_intermediate, "[");
            text_intermediate = Remove(text_intermediate, "]");
            text_intermediate = Remove(text_intermediate, "'");

            return text_intermediate;
        }

        public string ArrayToString(dynamic[] array)
        {
            string array_string = string.Join(",", array);
            Add_to_both_sides(array_string, "[", "]");
            return array_string;
        }

        public string Remove(string Input, string TextToRemove)
        {
            Input = Replace(Input, TextToRemove, "");
            return Input;
        }

        public string Replace(string Input, string TextToReplace, string ReplaceWith)
        {
            Input = Input.Replace(TextToReplace, ReplaceWith);
            return Input;
        }

        //Adds a value to both sides of string
        public string Add_to_both_sides(string input, string left_side, string right_side)
        {
            input = left_side + input + right_side;
            return input;
        }

        // Converts a Delimited String to a dictionary, Used for my old Configuration File library and added so i can reimplement said library
        public Dictionary<string, dynamic> String_to_Dictionary(string input, char line_split = ';', char value_split = '=')
        {
            Dictionary<string, dynamic> dictionary = new Dictionary<string, dynamic>();
            var intermediate = input.Split(line_split);
            foreach (string item in intermediate)
            {
                string[] key_value_pair = item.Split(value_split);
                dictionary.Add(key_value_pair[0], key_value_pair[1]);
            }
            return dictionary;
        }

        //Combines an Array of strings to a single string.
        public string Recombine(string[] list_text, bool Add_Space = false)
        {
            string returnstring = string.Empty;
            if (Add_Space)
            {
                int index = 0;
                foreach (string item in list_text)
                {
                    string text = item + " ";
                    list_text[index] = text;
                    ++index;
                };

            }
            foreach (string value in list_text)
            {
                returnstring += value;
            }
            return returnstring;
        }

        //Counts The amount of lines in the file
        public int CountLinesReader(string path)
        {
            int count = 0;
            try
            {
                count = File.ReadAllLines(path).Length;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory Not found!");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File Not found!");
            }


            return count;
        }

        //Equivalent to readfile in python 
        public string Get_Contents_as_String(String path)
        {
            StreamReader file = Open(path);
            string text = file.ReadToEnd();
            file.Close();
            return text;
        }

        // Mimics Python's "open" function loosely 
        public dynamic Open(string path, string writemode = "r")
        {
            dynamic file;

            if (writemode == "w")
            {
                file = new StreamWriter(path);
            }
            else
                try
                {
                    file = new StreamReader(path);
                }
                catch (DirectoryNotFoundException)
                {
                    file = 0;
                }
                catch (FileNotFoundException)
                {
                    file = 0;
                }


            return file;
        }

        // (hopefully) gets the value of a line with Error Handling
        public string[] Get_Delimited_String_as_Array(Dictionary<int, dynamic> File_Contents_Imported, int line, bool LFS)
        {
            if (LFS)
            {
                string file_line = Get_In_Dictionary(File_Contents_Imported, line);
                file_line += Remove(file_line, "\n");
                string[] file_line_split = file_line.Split(',');
                return file_line_split;
            }
            else
            {
                return Get_In_Dictionary(File_Contents_Imported, line);
            }
        }

        public dynamic Get_In_Dictionary(Dictionary<int, dynamic> File_Contents_Imported, int index, dynamic return_value = null)
        {
            try
            {
                dynamic file_line = File_Contents_Imported[index];
                return file_line;
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Error, This Key is not found in the index!");
                return return_value;
            }
        }

    } 
}