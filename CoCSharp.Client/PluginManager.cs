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
            m_PluginTasks = new List<PluginTasks>();
            m_UpdaterThread = new Thread(UpdatePlugins);
            m_UpdaterThread.Name = "PluginThread";
        }

        public bool PluginsEnabled { get; set; }
        public List<Plugin> LoadedPlugins { get; set; }
        public List<Assembly> LoadedPluginAssemblies { get; set; }

        private readonly ICoCClient m_Client = null;
        private readonly List<PluginTasks> m_PluginTasks = null;
        private readonly Thread m_UpdaterThread = null;
        private readonly Type m_PluginType = typeof(Plugin);

        public void EnablePlugins()
        {
            Console.Write("Enabling plugin...");
            PluginsEnabled = true;
            for (int i = 0; i < m_PluginTasks.Count; i++)
            {
                var pluginTasks = m_PluginTasks[i];
                pluginTasks.DoEnable();
            }
            m_UpdaterThread.Start();
            Console.WriteLine("Done!");
        }

        public void DisablePlugins()
        {
            Console.Write("Disabling plugin...");
            PluginsEnabled = false;
            for (int i = 0; i < m_PluginTasks.Count; i++)
            {
                var pluginTasks = m_PluginTasks[i];
                pluginTasks.DoDisable();
            }
            m_UpdaterThread.Abort();
            Console.WriteLine("Done!");
        }

        public void LoadPlugins()
        {
            Console.WriteLine("Loading plugins...");
            Console.WriteLine();
            var pluginFiles = Directory.GetFiles(PluginFolder);
            var pluginTypes = new List<Type>();
            for (int i = 0; i < pluginFiles.Length; i++)
            {
                try
                {
                    if (Path.GetExtension(pluginFiles[i]) != ".dll")
                        continue;

                    Console.WriteLine("Loading {0}...", pluginFiles[i]);
                    var assembly = Assembly.LoadFrom(pluginFiles[i]);
                    var types = assembly.GetExportedTypes();
                    for (int j = 0; j < types.Length; j++)
                    {
                        if (m_PluginType.IsAssignableFrom(types[j]) && !types[j].IsAbstract)
                        {
                            var plugin = Activator.CreateInstance(types[j]) as Plugin;
                            Console.WriteLine("\tLoading {0}...", plugin.GetType().Name);
                            Console.WriteLine("\t\tName: {0}\n\t\tDescription: {1}\n\t\tAuthor: {2}", plugin.Name, plugin.Description, plugin.Author);

                            plugin.Client = m_Client;
                            LoadedPlugins.Add(plugin);
                        }
                    }
                    Console.WriteLine();
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
            {
                var pluginTasks = new PluginTasks(LoadedPlugins[i]);
                m_PluginTasks.Add(pluginTasks);
                pluginTasks.DoLoad();
            }
        }

        public void UnloadPlugins()
        {
            for (int i = 0; i < m_PluginTasks.Count; i++)
            {
                //TODO: Unload the assemblies.
                var pluginTasks = m_PluginTasks[i];
                pluginTasks.DoUnload();
            }
        }

        private void UpdatePlugins()
        {
            while (PluginsEnabled)
            {
                for (int i = 0; i < m_PluginTasks.Count; i++)
                {
                    var pluginTasks = m_PluginTasks[i];
                    pluginTasks.DoUpdate();
                }
                Thread.Sleep(100);
            }
        }

        private class PluginTasks
        {
            public PluginTasks(Plugin plugin)
            {
                Plugin = plugin;
                m_UpdateTask = new Task(new Action(Plugin.OnUpdate));
                m_LoadTask = new Task(new Action(Plugin.OnLoad));
                m_UnloadTask = new Task(new Action(Plugin.OnUnload));
                m_EnableTask = new Task(new Action(Plugin.OnEnable));
                m_DisableTask = new Task(new Action(Plugin.OnDisable));
            }

            public Plugin Plugin { get; set; }

            private Task m_UpdateTask = null;
            private Task m_LoadTask = null;
            private Task m_UnloadTask = null;
            private Task m_EnableTask = null;
            private Task m_DisableTask = null;

            public void DoUpdate()
            {
                RunTask(m_UpdateTask);
            }

            public void DoLoad()
            {
                RunTask(m_LoadTask);
            }

            public void DoUnload()
            {
                RunTask(m_UnloadTask);
            }

            public void DoEnable()
            {
                RunTask(m_EnableTask);
            }

            public void DoDisable()
            {
                RunTask(m_DisableTask);
            }

            private void RunTask(Task task)
            {
                switch (task.Status)
                {
                    case TaskStatus.Created:
                        task.Start();
                        break;

                    case TaskStatus.RanToCompletion:
                        task = new Task(new Action(Plugin.OnUpdate));
                        task.Start();
                        break;
                }
            }
        }
    }
}
