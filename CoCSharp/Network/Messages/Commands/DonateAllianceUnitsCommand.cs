namespace CoCSharp.Network.Messages.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DonateAllianceUnitsCommand : Command
    {
        /// <summary>
        /// 
        /// </summary>
        public DonateAllianceUnitsCommand()
        {
            // Space
        }
        /// <summary>
        /// 
        /// </summary>
        public override int ID
        {
           get
           {
                return 22;
           }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public override void ReadCommand(MessageReader reader)
        {
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteCommand(MessageWriter writer)
        {
            
        }
    }
}
