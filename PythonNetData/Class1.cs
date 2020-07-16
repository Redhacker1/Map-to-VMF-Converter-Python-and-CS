using Python.Runtime;
using System;
using System.IO;

namespace PythonNetData
{
    public class Class1
    {
        private static string pythonPath1 = @"C:\Users\donov\AppData\Local\Programs\Python\Python37-64";
        public static void Test()
        {
            string pathToPython = pythonPath1;
            string path = pathToPython + ";" +
            Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToPython, EnvironmentVariableTarget.Process);

            var lib = new[]
            {
            @"C:\Users\donov\Documents",
            @"C:\Users\donov\AppData\Local\Programs\Python\Python37-64\Lib",
            @"C:\Users\donov\AppData\Local\Programs\Python\Python37-64\DLLs",
            @"C:\Users\donov\AppData\Local\Programs\Python\Python37-64\Lib\site-packages"
            };

            string paths = string.Join("; ", lib);
            Environment.SetEnvironmentVariable("PYTHONPATH", paths, EnvironmentVariableTarget.Process);
            PythonEngine.Initialize();

            //using (Py.GIL()) //Initialize the Python engine and acquire the interpreter lock
            {

                // import your script into the process
                dynamic np = PythonEngine.ImportModule("numpy");
                Console.WriteLine(np.cos(np.pi * 2));

            }
        }
    }
}
