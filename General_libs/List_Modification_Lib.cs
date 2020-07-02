using System;
using System.Collections.Generic;
using System.Text;

namespace General_libs
{
   public class List_Modification_Lib
    {
        public bool Compare_Arrays(dynamic[] array_1, dynamic[] array_2)
        {

            if (array_1.Length != array_2.Length)
            {
                return false;
            }

            for (int increment = 0; increment < array_1.Length; increment++)
            {
                if (array_1[increment] != array_2[increment])
                    return false;
            }

            return true;
        }

        public dynamic Rotate_List(dynamic[] List, int Times_To_Rotate)
        {

            Queue<dynamic> queue = new Queue<dynamic>(List);
            Stack<int> stack = new Stack<int>();
            int length = List.Length - 1;

            if (Times_To_Rotate < 0)
            {
                Times_To_Rotate += Math.Abs(length);
            }

            while (Times_To_Rotate > 0)
            {
                stack.Push(queue.Dequeue());
                queue.Enqueue(stack.Pop());
                Times_To_Rotate--;
            }

            return queue.ToArray();
        }
    }
}
