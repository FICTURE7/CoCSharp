using CoCSharp.Data.Slots;
using CoCSharp.Network;
using CoCSharp.Network.Messages;
using System;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        public static void HandleMatchmakingCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            // TODO: Check if avatar has shield or guard.
            var avatar = server.AvatarManager.GetRandomAvatar(client.Avatar.ID);
            if (avatar == null)
            {
                // Return home if their is no one to attack.
                client.NetworkManager.SendMessage(client.Avatar.OwnHomeDataMessage);
                return;
            }

            var ehdMessage = new EnemyHomeDataMessage()
            {
                LastVisit = TimeSpan.FromSeconds(0),

                Unknown1 = null,

                Timestamp = DateTime.UtcNow,
                EnemyVillageData = new VillageMessageComponent(avatar),
                EnemyAvatarData = new AvatarMessageComponent(avatar),
                OwnAvatarData = new AvatarMessageComponent(client.Avatar)
                {
                    Units = new UnitSlot[]
                    {
                        new UnitSlot(4000001, 1),
                    }
                },

                Unknown2 = 3,
                Unknown3 = 0,
                Unknown4 = 0,
            };

            client.NetworkManager.SendMessage(ehdMessage);
        }
    }
}
