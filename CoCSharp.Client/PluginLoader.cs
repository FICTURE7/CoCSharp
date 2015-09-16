using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Threading;
using CoCSharp.PluginApi;

namespace CoCSharp.Client
{
    class PluginLoader
    {
        private List<IPlugin> loadedPlugins = new List<IPlugin>(); 
        private List<Thread> updateThreads = new List<Thread>();

        public void LoadPlugins()
        {
            if (!Directory.Exists("plugins"))
                Directory.CreateDirectory("plugins");
            else
            {
                string[] pluginFiles = Directory.GetFiles("plugins");
                List<Type> pluginTypes = new List<Type>();
                foreach (var pluginFile in pluginFiles) // Load all types which implement IPlugin
                {
                    try
                    {
                        pluginTypes.AddRange((from t in Assembly.LoadFrom(pluginFile).GetExportedTypes()
                            where !t.IsInterface && !t.IsAbstract
                            where typeof (IPlugin).IsAssignableFrom(t)
                            select t).ToArray());
                    }
                    catch { }
                }
                loadedPlugins.AddRange(pluginTypes.Select(t => (IPlugin) Activator.CreateInstance(t)).ToArray());
                // Create a instance of them
                foreach (var plugin in loadedPlugins)
                {
                    try
                    {
                        plugin.PreStart();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error occured: " + ex);
                    }
                }
                foreach (var plugin in loadedPlugins)
                {
                    try
                    {
                        plugin.OnStart();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error occured: " + ex);
                    }
                }
                foreach (var plugin in loadedPlugins)
                {
                    try
                    {
                        plugin.PostStart();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error occured: " + ex);
                    }
                }
                foreach (var plugin in loadedPlugins)
                {
                    try
                    {
                        Thread updateThread;
                        (updateThread = new Thread(PushUpdates)).Start(plugin);
                        updateThreads.Add(updateThread);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error occured: " + ex);
                    }
                }
            }
        }

        public void PushUpdates(object pl)
        {
            if (pl != null)
            {
                try
                {
                    IPlugin plugin = pl as IPlugin;
                    while (true)
                    {
                        try
                        {
                            plugin.Update();
                        }
                        catch (Exception ex)
                        {
                            if (plugin.Name != null) Console.WriteLine("Error while updateing {0}: {1}", plugin.Name, ex);
                            else Console.WriteLine("Error while updateing UNKNOWN: {0}", ex);
                        }
                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex);
                }
            }
        }

        public void UnloadPlugins()
        {
            if (loadedPlugins.Count == 0) return;
            foreach (var updateThread in updateThreads)
            {
                updateThread.Abort();
            }
            foreach (var plugin in loadedPlugins)
            {
                try
                {
                    plugin.PreStop();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex);
                }
            }
            foreach (var plugin in loadedPlugins)
            {
                try
                {
                    plugin.OnStop();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex);
                }
            }
            foreach (var plugin in loadedPlugins)
            {
                try
                {
                    plugin.PostStop();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex);
                }
            }
        }
    }
}
