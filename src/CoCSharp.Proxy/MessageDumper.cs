using CoCSharp.Network;
using CoCSharp.Network.Messages;
using System;
using System.IO;

namespace CoCSharp.Proxy
{
    public class MessageDumper
    {
        static MessageDumper()
        {
            if (!Directory.Exists("message-dumps"))
                Directory.CreateDirectory("message-dumps");

            if (!Directory.Exists("village-dumps"))
                Directory.CreateDirectory("village-dumps");
        }

        public MessageDumper()
        {
            // Space
        }

        public void Dump(Message message, byte[] kappa)
        {
            var time = DateTime.Now;
            var fileName = $"{time.ToString("yy_MM_dd__hh_mm_ss")} - {message.GetType().Name} ({message.Id}).bin";
            var path = Path.Combine("message-dumps", fileName);
            File.WriteAllBytes(path, kappa);

            if (message is OwnHomeDataMessage)
            {
                var ohd = message as OwnHomeDataMessage;
                if (ohd.OwnVillageData != null && ohd.OwnVillageData.VillageJson != null)
                {
                    var nfilename = $"ownhome-{DateTime.Now.ToString("yy_MM_dd__hh_mm_ss")}.json";
                    var npath = Path.Combine("village-dumps", nfilename);

                    File.WriteAllText(npath, ohd.OwnVillageData.VillageJson);
                }
            }
            else if (message is VisitHomeDataMessage)
            {
                var vhd = message as VisitHomeDataMessage;
                if (vhd.VisitVillageData != null && vhd.VisitVillageData.VillageJson != null)
                {
                    var nfilename = $"visithome-{DateTime.Now.ToString("yy_MM_dd__hh_mm_ss")}.json";
                    var npath = Path.Combine("village-dumps", nfilename);

                    File.WriteAllText(npath, vhd.VisitVillageData.VillageJson);
                }
            }
            else if (message is EnemyHomeDataMessage)
            {
                var ehd = message as EnemyHomeDataMessage;
                if (ehd.EnemyVillageData != null && ehd.EnemyVillageData.VillageJson != null)
                {
                    var nfilename = $"enemyhome-{DateTime.Now.ToString("yy_MM_dd__hh_mm_ss")}.json";
                    var npath = Path.Combine("village-dumps", nfilename);

                    File.WriteAllText(npath, ehd.EnemyVillageData.VillageJson);
                }
            }
        }
    }
}
