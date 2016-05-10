using System.IO;

namespace CoCSharp.Server
{
    public static class DirectoryPaths
    {
        private static string _content = "Content";
        public static string Content
        {
            get { return _content; }
        }

        private static string _npcVillages = Path.Combine(_content, "level");
        public static string Npc
        {
            get { return _npcVillages; }
        }

        private static string _avatars = "Avatars";
        public static string Avatars
        {
            get { return _avatars; }
        }
    }
}
