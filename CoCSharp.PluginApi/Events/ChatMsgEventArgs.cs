using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCSharp.PluginApi.Events
{
    public class ChatMsgEventArgs
    {
        public string Clan { get; set; }
        public string Playername { get; set; }
        public string Msg { get; set; }

        public ChatMsgEventArgs(string clan, string playername, string msg)
        {
            if (!String.IsNullOrEmpty(clan))
                Clan = clan;
            else
                Clan = "";
            Playername = playername;
            Msg = msg;
        }
    }
}
