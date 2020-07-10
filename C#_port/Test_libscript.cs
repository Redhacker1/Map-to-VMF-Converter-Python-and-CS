using System;

namespace Test_libscript
{
    class Testscripts
    {
        static General_libs.Array_Dict_and_list_library TestingListLibrary = new General_libs.Array_Dict_and_list_library();
        static public void rotate_test()
            {
                Console.WriteLine(new dynamic []{8,83, 80,70}[3]);
                Console.WriteLine(TestingListLibrary.Rotate_List(new dynamic []{8,83, 80,70}, -1)[1]);
                Console.WriteLine(TestingListLibrary.Rotate_List(new dynamic []{8,83, 80,70}, -1)[3]);
                Console.WriteLine(new dynamic []{8,83, 80,70}[1]);
            }
        
        
    }
}