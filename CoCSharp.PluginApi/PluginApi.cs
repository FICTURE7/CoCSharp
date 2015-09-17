using System;
using CoCSharp.PluginApi.Events;

namespace CoCSharp.PluginApi
{
    public class PluginApi
    {
        // Variables/Constants
        public const uint PluginApiVersion = 0;
        // Events
        public delegate void ChatMsgHandler(ChatMsgEventArgs eventArgs);
        public static event ChatMsgHandler ChatMsg;
        // Functions
        public static void Console_WriteLine(string msg)
        {
            Console.WriteLine(msg);
        }



        //TODO: Add plugin api functions which can be accessed by the plugin

        // Event invoking
        public static void OnChatMsg(ChatMsgEventArgs eventargs)
        {
            ChatMsg?.Invoke(eventargs);
        }
    }
}
