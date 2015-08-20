using System;

namespace CoCSharp.Logic
{
    public class Avatar
    {
        public Avatar()
        {

        }

        public Avatar(long id)
        {
            ID = id;
        }

        public long ID { get; set; }
        public string Username { get; set; }
    }
}
