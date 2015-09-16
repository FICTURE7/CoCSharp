using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoCSharp.PluginApi;

namespace ExamplePlugin
{
    public class ExamplePlugin : IPlugin
    {
        /*
            Note: File must be put in the plugins folder
        */
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Author { get; set; }
        public void PreStart()
        {
            Name = "Example Plugin";
            Desc = "For testing purpose only!";
            Author = "Castelbuilder123";
            PluginApi.Console_WriteLine(Name + " PreStart!");
        }

        public void OnStart()
        {
            PluginApi.Console_WriteLine(Name + " OnStart!");
        }

        public void PostStart()
        {
            PluginApi.Console_WriteLine(Name + " PostStart!");
        }

        public void Update()
        {
            PluginApi.Console_WriteLine(Name + " Update!");
        }

        public void OnCmdRecv(string cmd, string[] param)
        {
            PluginApi.Console_WriteLine(Name + " CMD RECV: " + cmd + "!");
        }

        public void PreStop()
        {
            PluginApi.Console_WriteLine(Name + " PreStop!");
        }

        public void OnStop()
        {
            PluginApi.Console_WriteLine(Name + " OnStop!");
        }

        public void PostStop()
        {
            PluginApi.Console_WriteLine(Name + " PostStop!");
        }
    }
}
