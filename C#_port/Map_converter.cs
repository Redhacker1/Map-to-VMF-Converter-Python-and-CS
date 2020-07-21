using Py_embedded;
using System;
using System.Linq;
using System.Reflection;

namespace MapConverter
{
    class Map_converter

    {
        public static Assembly ExecutingAssembly = Assembly.GetExecutingAssembly();
        public static string[] EmbeddedLibraries = ExecutingAssembly.GetManifestResourceNames().Where(x => x.EndsWith(".dll")).ToArray();

        //static string beginpath = "/home/donovanariesstrawhacker/Documents/GitHub/Quake_source_Tools/maps/map_files/quake_1/";
        //static string beginpath_windows = "C:/Users/donov/Documents/GitHub/Quake_source_Tools/maps/map_files/quake_1/";
        static void Main(string[] args)
        {
            DLLIntegrator.StartFunc();
            PythonAbstractions Python_Functions = new PythonAbstractions();
            Python_Functions.RunFunction(ScriptName: "ConfigLib", FuncName: "main");

        }
    }

    class DLLIntegrator
    {

        static public void StartFunc()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name).Name + ".dll";
            var resourceName = PythonAbstractions.EmbeddedLibraries.FirstOrDefault(x => x.EndsWith(assemblyName));
            var item = LoadAssembly(resourceName);
            // Get resource name
            resourceName = "Py_embedded_v37.Python.Runtime.dll";
            item = LoadAssembly(resourceName);
            return item;
        }

        static public Assembly LoadAssembly(string Name)
        {
            // Load assembly from resource
            using (var stream = PythonAbstractions.ExecutingAssembly.GetManifestResourceStream(Name))
            {
                //if (stream !=  null)
                //{
                    var bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    return Assembly.Load(bytes);
                //}
                
            }
        }
    }
}
