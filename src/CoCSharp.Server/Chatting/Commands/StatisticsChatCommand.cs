using CoCSharp.Server.Api;
using CoCSharp.Server.Api.Chatting;
using System.Text;

namespace CoCSharp.Server.Chatting.Commands
{
    public class StatisticsChatCommand : ChatCommand
    {
        private static readonly string[] s_alias = new string[] { "stats" };

        public override string Name => "statistics";
        public override string[] Alias => s_alias;
        public override string Description => "Displays the statistics of the user that issued the command.";

        public override void Execute(IClient client, params string[] args)
        {
            var builder = new StringBuilder();
            var level = client.Session.Level;
            var playTime = level.PlayTime;
            var loginCount = level.LoginCount;
            var dateJoined = level.DateCreated;
            var dateSaved = level.DateLastSave;

            builder.AppendLine($"Statistics for user {level.Avatar.Name}: ");
            builder.AppendLine($"Play time: {playTime.Hours}h {playTime.Minutes}m {playTime.Seconds}s");
            builder.AppendLine($"Login count: {loginCount}");
            builder.AppendLine($"Date joined: {dateJoined}");
            builder.AppendLine($"Date saved: {dateSaved}");
            Manager.SendChatMessage(client, builder.ToString());
        }
    }
}
