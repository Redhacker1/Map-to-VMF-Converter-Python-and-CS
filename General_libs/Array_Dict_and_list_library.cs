using System;
using System.Collections.Generic;
using System.Linq;

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
                    return false;
                }
                    
            }

            return true;
        }


        public bool Compare_List(List<dynamic> array_1, List<dynamic> array_2)
        {

            if (array_1.Count != array_2.Count)
            {
                return false;
            }

            for (int increment = 0; increment < array_1.Count; increment++)
            {
                if (array_1[increment] != array_2[increment])
                {
                    return false;
                }

            }

            return true;
        }

        public bool Compare_List_int(List<int> array_1, List<int> array_2)
        {

            if (array_1.Count != array_2.Count)
            {
                return false;
            }

            for (int increment = 0; increment < array_1.Count; increment++)
            {
                if (array_1[increment] != array_2[increment])
                {
                    return false;
                }

            }

            return true;
        }

        public bool Compare_List_Linked(LinkedList<dynamic> LL_1, LinkedList<dynamic> LL_2)
        {
            List<dynamic> array_1 = LL_1.ToList();
            List<dynamic> array_2 = LL_2.ToList();
            if (array_1.Count != array_2.Count)
            {
                return false;
            }

            for (int increment = 0; increment < array_1.Count; increment++)
            {
                if (array_1[increment] != array_2[increment])
                {
                    return false;
                }

            }

            return true;
        }

        public bool Compare_List_Linked_int(LinkedList<int> LL_1, LinkedList<int> LL_2)
        {
            List<int> array_1 = LL_1.ToList();
            List<int> array_2 = LL_2.ToList();
            if (array_1.Count != array_2.Count)
            {
                return false;
            }

            for (int increment = 0; increment < array_1.Count; increment++)
            {
                if (array_1[increment] != array_2[increment])
                {
                    return false;
                }

            }

            return true;
        }

        public dynamic Rotate_Array(dynamic[] Array, int Times_To_Rotate)
        {

            Queue<dynamic> queue = new Queue<dynamic>(Array);
            Stack<int> stack = new Stack<int>();
            int length = Array.Length;

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

        public List<dynamic> Rotate_List(List<dynamic> List, int Times_To_Rotate)
        {
            int length = List.Count - 1;
            if (Times_To_Rotate < 0)
            {
                Times_To_Rotate = length + Math.Abs(Times_To_Rotate);
            }
            LinkedList<dynamic> list = new LinkedList<dynamic>();
            while (Times_To_Rotate > 0)
            {
                list = new LinkedList<dynamic>(List);
                list.RemoveFirst();
                list.AddLast(list);
                Times_To_Rotate--;
            }

            List = new List<dynamic>(list);
            return List;
        }

        public List<int> Rotate_List_int(List<int> List, int Times_To_Rotate)
        {
            int length = List.Count - 1;
            if (Times_To_Rotate < 0)
            {
                Times_To_Rotate = length + Math.Abs(Times_To_Rotate);
            }
            LinkedList<int> list = new LinkedList<int>();
            while (Times_To_Rotate > 0)
            {
                list = new LinkedList<int>(List);
                var newlast = list.First;
                list.RemoveFirst();
                list.AddLast(newlast);
                Times_To_Rotate--;
            }

            List = new List<int>(list);
            return List;
        }

        public LinkedList<dynamic> Rotate_List_Linked(LinkedList<dynamic> List, int Times_To_Rotate)
        {
            int length = List.Count - 1;
            if (Times_To_Rotate < 0)
            {
                Times_To_Rotate = length + Math.Abs(Times_To_Rotate);
            }
            while (Times_To_Rotate > 0)
            {
                List = new LinkedList<dynamic>(List);
                List.RemoveFirst();
                List.AddLast(List);
                Times_To_Rotate--;
            }
            return List;
        }

        public LinkedList<int> Rotate_List_Linked_int(LinkedList<int> List, int Times_To_Rotate)
        {
            int length = List.Count - 1;
            if (Times_To_Rotate < 0)
            {
                Times_To_Rotate = length + Math.Abs(Times_To_Rotate);
            }
            while (Times_To_Rotate > 0)
            {
                List = new LinkedList<int>(List);
                var newlast = List.First;
                List.RemoveFirst();
                List.AddLast(newlast);
                Times_To_Rotate--;
            }
            return List;
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
