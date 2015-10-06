namespace CoCSharp.Networking.Packets.Commands
{
    public class ClaimAchievementRewardCommand : ICommand
    {
        public int ID { get { return 0x20B; } }

        public int AchievementID;
        public int Unknown1;

        public void ReadCommand(PacketReader reader)
        {
            AchievementID = reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
        }

        public void WriteCommand(PacketWriter writer)
        {
            writer.WriteInt32(AchievementID);
            writer.WriteInt32(Unknown1);
        }
    }
}
