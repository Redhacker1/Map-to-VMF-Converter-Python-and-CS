using System;
using System.Collections.Generic;

namespace General_libs
{
    public class Array_Dict_and_list_library
    {
        readonly Text_Modification_Library TextLib = new Text_Modification_Library();
        public bool Compare_Arrays(dynamic array_1, dynamic array_2)
        {

            if (array_1.Length != array_2.Length)
            {
                return false;
            }

            for (int increment = 0; increment < array_1.Length; increment++)
            {
                if (array_1[increment] != array_2[increment])
                {
                    if (array_1[increment] is int)
                    {
                        Console.WriteLine(array_1.ArrayToString);
                    }
                    Console.WriteLine(array_1[0]);
                    return false;
                }
                    
            }

            return true;
        }

        public dynamic Rotate_List(dynamic[] List, int Times_To_Rotate)
        {

            Queue<dynamic> queue = new Queue<dynamic>(List);
            Stack<int> stack = new Stack<int>();
            int length = List.Length;

            if (Times_To_Rotate < 0)
            {
                Times_To_Rotate = length + Math.Abs(Times_To_Rotate);
            }

            while (Times_To_Rotate > 0)
            {
                stack.Push(queue.Dequeue());
                queue.Enqueue(stack.Pop());
                Times_To_Rotate--;
            }

            return queue.ToArray();
        }

        public string[] Human_list_to_CS_Array(string ListToTurn, char delimiter)
        {
            ListToTurn += TextLib.Remove(ListToTurn, "\n");
            string[] ArrayOutput = ListToTurn.Split(delimiter);
            return ArrayOutput;

        }
        public string CS_Array_to_Human_List(dynamic[] ArrayToTurn)
        {
            string text_intermediate = ArrayToString(ArrayToTurn);
            text_intermediate = TextLib.Replace(text_intermediate, ", ", ",");
            text_intermediate = TextLib.Remove(text_intermediate, "[");
            text_intermediate = TextLib.Remove(text_intermediate, "]");
            text_intermediate = TextLib.Remove(text_intermediate, "'");

            return text_intermediate;
        }

        public string ArrayToString(dynamic[] array)
        {
            string array_string = string.Join(",", array);
            TextLib.Add_to_both_sides(array_string, "[", "]");
            return array_string;
        }
    }
}
