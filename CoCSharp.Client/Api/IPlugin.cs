using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCSharp.Client.API
{
    public interface IPlugin
    {
        string Name { get; set; }
        string Desc { get; set; }
        string Author { get; set; }
        //TODO: List<string> Dependencies { get; set; }

        //Livecycle:
        void PreInit();
        void Init();
        void PostInit();
        void Update(); // Called every 100 ms
        void OnCmdRecv(string cmd, string[] param);
        void PreDeInit();
        void DeInit();
        void PostDeInit(); 
    }
}
