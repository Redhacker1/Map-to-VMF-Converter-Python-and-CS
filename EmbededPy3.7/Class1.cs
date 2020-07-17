using Python.Runtime;
using System;
using System.IO;

namespace Py_embedded_v37
{
    public class Class1
    {
        private static string pythonPath1 = @"\Python37";
        public static void Test()
        {
            General_libs.Text_Modification_Library Textlib = new General_libs.Text_Modification_Library();
            string pathToPython = pythonPath1;
            string path = pathToPython + ";" +
            Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToPython, EnvironmentVariableTarget.Process);

            var lib = new[]
            {
            @"\Python37\Lib",
            @"\Python37\DLLs",
            @"\Python37\Lib\site-packages",
            @"\Scripts",
            };

            string paths = string.Join("; ", lib);
            Environment.SetEnvironmentVariable("PYTHONPATH", paths, EnvironmentVariableTarget.Process);
            PythonEngine.Initialize();
            dynamic Script = Textlib.Open(@"Scripts\ConfigLib.py");
            string ScriptString = Script.ReadToEnd();

            using (Py.GIL()) //Initialize the Python engine and acquire the interpreter lock
            {

                // import your script into the process
                Py.Import("Scripts.ConfigLib");

            }
        }
    }
}
