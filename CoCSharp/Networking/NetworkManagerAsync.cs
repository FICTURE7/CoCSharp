namespace CoCSharp.Networking
{
    public class NetworkManagerAsync
    {
        public NetworkManagerAsync()
        {

        }

        public void SendMessage(Message message)
        {
            var length = 0;
            if (length > Message.MaxSize)
                throw new InvalidMessageException("Length of message is greater than Message.MaxSize.");
        }
    }
}
