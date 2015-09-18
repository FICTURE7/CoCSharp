using CoCSharp.Client.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

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
            PluginsEnabled = false;

            m_Client = client;
            m_Updates = new List<PluginUpdate>();
            m_UpdateThread = new Thread(UpdatePlugins);
            m_UpdateThread.Name = "PluginThread";
        }

        public bool PluginsEnabled { get; set; }
        public List<Plugin> LoadedPlugins { get; set; }
        public List<Assembly> LoadedPluginAssemblies { get; set; }

        private readonly ICoCClient m_Client = null;
        private readonly List<PluginUpdate> m_Updates = null;
        private readonly Thread m_UpdateThread = null;
        private readonly Type m_PluginType = typeof(Plugin);

        public void EnablePlugins()
        {
            Console.Write("Enabling plugin...");
            PluginsEnabled = true;
            for (int i = 0; i < LoadedPlugins.Count; i++)
            {
                LoadedPlugins[i].OnEnable();
            }
            m_UpdateThread.Start();
            Console.WriteLine("Done!");
        }

        public void DisablePlugins()
        {
            Console.Write("Disabling plugin...");
            PluginsEnabled = false;
            for (int i = 0; i < LoadedPlugins.Count; i++)
                LoadedPlugins[i].OnDisable();
            m_UpdateThread.Abort();
            Console.WriteLine("Done!");
        }

        public void LoadPlugins()
        {
            Console.Write("Loading plugins...");
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
                catch (PluginException)
                {
                    //TODO: Add logs.
                    //TODO: Remove plugin if the plugin was loaded.
                    Console.WriteLine("Error in plugin {0} constructor", pluginFiles[i]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while loading {0}: \n\t{1}", pluginFiles[i], ex.Message);
                }
            }

            for (int i = 0; i < LoadedPlugins.Count; i++)
                m_Updates.Add(new PluginUpdate(LoadedPlugins[i]));
            Console.WriteLine("Done!");
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

        private void UpdatePlugins()
        {
            while (PluginsEnabled)
            {
                for (int i = 0; i < LoadedPlugins.Count; i++)
                {
                    var pluginUpdate = m_Updates[i];
                    if (!pluginUpdate.UpdateTask.IsCompleted)
                       continue; // TODO: Warn user about it.
                    pluginUpdate.StartTime = DateTime.Now;
                    pluginUpdate.UpdateTask.Start();
                }
                Thread.Sleep(100);
            }
        }

        private class PluginUpdate
        {
            public PluginUpdate(Plugin plugin)
            {
                Plugin = plugin;
            }

            public Plugin Plugin { get; set; }
            public DateTime StartTime { get; set; }
            public Task UpdateTask { get { return new Task(new Action(Plugin.OnUpdate)); } }
        }
    }
}
