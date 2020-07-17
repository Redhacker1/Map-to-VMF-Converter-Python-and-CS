using Python.Runtime;
using System;
using System.Collections.Generic;

namespace Py_embedded_v37
{
    /// <summary>
    /// Example of Embedding Python inside of a .NET program.
    /// </summary>
    /// <remarks>
    /// It has similar functionality to doing `import clr` from within Python, but this does it
    /// the other way around; That is, it loads Python inside of .NET program.
    /// See https://github.com/pythonnet/pythonnet/issues/358 for more info.
    /// </remarks>
    public class PythonConsole
    {
#if NET40
        private static AssemblyLoader assemblyLoader = new AssemblyLoader();
#endif
        private PythonConsole()
        {
        }

        [STAThread]
        public static int Hello()
        {
            return 0;
        }

#if NET40
        // Register a callback function to load embedded assemblies.
        // (Python.Runtime.dll is included as a resource)
        private sealed class AssemblyLoader
        {
            private Dictionary<string, Assembly> loadedAssemblies;

            public AssemblyLoader()
            {
                loadedAssemblies = new Dictionary<string, Assembly>();

                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    string shortName = args.Name.Split(',')[0];
                    string resourceName = $"{shortName}.dll";

                    if (loadedAssemblies.ContainsKey(resourceName))
                    {
                        return loadedAssemblies[resourceName];
                    }

                    // looks for the assembly from the resources and load it
                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                    {
                        if (stream != null)
                        {
                            var assemblyData = new byte[stream.Length];
                            stream.Read(assemblyData, 0, assemblyData.Length);
                            Assembly assembly = Assembly.Load(assemblyData);
                            loadedAssemblies[resourceName] = assembly;
                            return assembly;
                        }
                    }
                    return null;
                };
            }
        }
#endif
    }

    class Program
    {
        static void Runpython()
        {
            var pythonPath = @"C:\Users\andrea\Envs\pynet\Scripts";
            //var pythonPath = @"C:\Users\andrea\AppData\Local\Programs\Python\Python37";

            Environment.SetEnvironmentVariable("PATH", $@"{pythonPath};" + Environment.GetEnvironmentVariable("PATH"));
            Environment.SetEnvironmentVariable("PYTHONHOME", pythonPath);

            Environment.SetEnvironmentVariable("PYTHONPATH ", $@"{pythonPath}\..\Lib;{pythonPath}\..\Lib\site-packages;");
            //Environment.SetEnvironmentVariable("PYTHONPATH ", $@"{pythonPath}\Lib");

            PythonEngine.PythonHome = Environment.GetEnvironmentVariable("PYTHONHOME", EnvironmentVariableTarget.Process);
            PythonEngine.PythonPath = Environment.GetEnvironmentVariable("PYTHONPATH", EnvironmentVariableTarget.Process);
        }
    }
}

