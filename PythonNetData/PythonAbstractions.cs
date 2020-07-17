using System;
using System.Collections.Generic;
using System.Text;
using Python.Runtime;

namespace Py_embedded_v37
{
    class PythonAbstractions
    {
        dynamic Initpython()
        {
            General_libs.Text_Modification_Library Textlib = new General_libs.Text_Modification_Library();
            string pathToPython = @"\Python37";
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
            return
        }
    }
}
