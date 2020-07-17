using System;
using System.Collections.Generic;
using System.Text;
using Python.Runtime;
using static Python.Runtime.Py;

namespace Py_embedded_v37
{
    public class PythonAbstractions
    {
        public void Initpython(string custom_PATH = "")
        {
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
                @"\Scripts"
                };

            if (custom_PATH != "")
            {
                lib = new[]
                {
                @"\Python37\Lib",
                @"\Python37\DLLs",
                @"\Python37\Lib\site-packages",
                @"\Scripts",
                custom_PATH
                };
            }

            string paths = string.Join("; ", lib);
            Environment.SetEnvironmentVariable("PYTHONPATH", paths, EnvironmentVariableTarget.Process);
            PythonEngine.Initialize();
            Console.WriteLine(PythonEngine.Version);
        }

        string FSpath_to_PyPath(string FSPath)
        {
            FSPath = FSPath.Replace('\\', '/');
            string[] Path_Split = FSPath.Split('/');
            string pyPath = Path_Split[Path_Split.Length - 1];
            if (pyPath != "/")
            {
                pyPath = pyPath.Trim().Replace("/", "");
            }

            else
            {
                pyPath = Path_Split[Path_Split.Length - 2];
                pyPath = pyPath.Trim().Remove('/');
            }
            

            return pyPath;
        }

        public void TerminatePython()
        {
            try
            {
                PythonEngine.Shutdown();
            }
            catch(PythonException Exception)
            {
                Console.WriteLine(Exception.Message);
            }
            
        }

        public bool RunScript(string ScriptLocation = "Scripts", string ScriptName = "Main.py")
        {
            Initpython();
            ScriptLocation = FSpath_to_PyPath(ScriptLocation);
            if (ScriptLocation != "Scripts")
            {
                Initpython(ScriptLocation);
            }

            try
            {
                PythonEngine.ImportModule(ScriptLocation + "." + ScriptName);
            }
            catch (PythonException Exception)
            {
                Console.WriteLine(Exception.Message);
                TerminatePython();
                return false;
            }
            TerminatePython();
            return true;

        }

        public dynamic RunFunction(string ScriptLocation = "Scripts", string ScriptName = "Main.py", string FuncName = "Main()")
        {
            dynamic return_value = null;
            try
            {
                dynamic script = PythonEngine.ImportModule(ScriptLocation + "." + ScriptName);
                Console.WriteLine("Function Starting");
                return_value = script.FuncName;
            }
            catch (PythonException Exception)
            {
                Console.WriteLine(Exception.Message);
                TerminatePython();
                return false;
            }

            return return_value;
        }

        
    }
    // Direct Access to PythonEngine and PythonException to allow for more advanced functionality
    public class Manual_Access
    {
        public readonly PythonEngine pythonEngine = new PythonEngine();

        public readonly PythonException pyException = new PythonException();
    }
}
