using CoCSharp.Client.API;
using System;
using System.Threading;

namespace CoCSharp.Client.ExamplePlugin
{
    public class ExamplePlugin : Plugin
    {
        public ExamplePlugin()
        {  
            // Space
        }

        public override string Name { get { return "Example Plugin"; } }
        public override string Description { get { return "A plugin just for sample."; } }
        public override string Author { get { return "FICTURE7 & castelbuilder123"; } }

        public override void OnUpdate()
        {
            Console.WriteLine("Updating on thread {0}!", Thread.CurrentThread.ManagedThreadId);
        }
    }
}
