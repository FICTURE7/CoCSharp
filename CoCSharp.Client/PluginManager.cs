using CoCSharp.Client.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CoCSharp.Client
{
    public class PluginManager
    {
        private const string PluginFolder = "plugins";
        public PluginManager(ICoCClient client)
        {
            if (!Directory.Exists(PluginFolder))
                Directory.CreateDirectory(PluginFolder);
            LoadedPlugins = new List<Plugin>();
            LoadedPluginAssemblies = new List<Assembly>();
            m_Client = client;
        }

        public List<Plugin> LoadedPlugins { get; set; }
        public List<Assembly> LoadedPluginAssemblies { get; set; }

        private readonly Type m_PluginType = typeof(Plugin);
        private ICoCClient m_Client = null;

        public void LoadPlugins()
        {
            var pluginFiles = Directory.GetFiles(PluginFolder);
            var pluginTypes = new List<Type>();
            for (int i = 0; i < pluginFiles.Length; i++)
            {
                try
                {
                    if (Path.GetExtension(pluginFiles[i]) != ".dll")
                        continue;

                    var assembly = Assembly.LoadFrom(pluginFiles[i]);
                    var types = assembly.GetExportedTypes();
                    for (int j = 0; j < types.Length; j++)
                    {
                        if (m_PluginType.IsAssignableFrom(types[j]) && !types[j].IsAbstract)
                        {
                            var plugin = Activator.CreateInstance(types[j]) as Plugin;
                            plugin.Client = m_Client;
                            LoadedPlugins.Add(plugin);

                            plugin.OnLoad();
                        }
                    }
                    LoadedPluginAssemblies.Add(assembly);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while loading {0}: {1}", pluginFiles[i], ex);
                }
            }
        }

        public void UnloadPlugins()
        {
            for (int i = 0; i < LoadedPlugins.Count; i++)
            {
                //TODO: Unload the assemblies.
                var plugin = LoadedPlugins[i];
                plugin.OnUnload();
            }
        }
    }
}
