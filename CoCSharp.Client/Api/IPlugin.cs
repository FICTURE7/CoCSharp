using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCSharp.Client.Api
{
    public interface IPlugin
    {
        string Name { get; set; }
        string Desc { get; set; }
        string Author { get; set; }
        //TODO: List<string> Dependencies { get; set; }

        //Livecycle:
        void preInit();
        void Init();
        void postInit();
        void update(); // Called every 100 ms
        void onCmdRecv(string cmd, string[] param);
        void preDeInit();
        void DeInit();
        void postDeInit(); 
    }
}
