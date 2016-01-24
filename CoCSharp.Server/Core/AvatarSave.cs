using CoCSharp.Logic;

namespace CoCSharp.Server.Core
{
    public class AvatarSave
    {
        public AvatarSave(Avatar avatar)
        {
            Avatar = avatar;
        }

        public Avatar Avatar { get; set; }

        public void Save()
        {
            
        }

        public void Load(string path)
        {

        }
    }
}
