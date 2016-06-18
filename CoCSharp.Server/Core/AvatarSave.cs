using CoCSharp.Logic;
using System;
using System.IO;

namespace CoCSharp.Server.Core
{
    // Represents an avatar save directory.
    internal class AvatarSave
    {
        public AvatarSave(string saveDirPath)
        {
            if (saveDirPath == null)
                throw new ArgumentNullException("saveDirPath");

            SaveDirectory = saveDirPath;
        }

        public AvatarSave(Avatar avatar, string saveDirPath)
        {
            if (avatar == null)
                throw new ArgumentNullException("avatar");
            if (saveDirPath == null)
                throw new ArgumentNullException("saveDirPath");

            Avatar = avatar;
            SaveDirectory = saveDirPath;
        }

        // Reference to the avatar that got loaded or saved.
        public Avatar Avatar { get; set; }

        // Path to the save directory of the avatar.
        public string SaveDirectory
        {
            get
            {
                return _saveDirPath;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (!Directory.Exists(value))
                    throw new DirectoryNotFoundException("'" + value + "' does not exists.");

                _saveDirPath = value;
                _saveDirName = Path.GetFileName(value);

                // Doing this stuff to remove extra calculations.
                // Set _token from _saveDirName.
                var token = Utils.GetDirectoryToken(_saveDirName);
                if (!TokenUtils.CheckToken(token))
                    throw new ArgumentException("value does not contain a valid token.");

                // Set _userID from _saveDirName.
                var userID = -1L;
                var userIDStr = Utils.GetDirectoryUserID(_saveDirName);
                if (userIDStr == null)
                    throw new ArgumentException("value does not contain a valid user ID.");
                if (!long.TryParse(userIDStr, out userID))
                    userID = -1L;

                _token = token;
                _userID = userID;
            }
        }
        private string _saveDirPath;
        private string _saveDirName;

        // Token of the avatar according to the SaveDirectory.
        public string Token
        {
            get
            {
                return _token;
            }
        }
        private string _token;

        // User ID of the avatar according to the SaveDirectory.
        public long ID
        {
            get
            {
                return _userID;
            }
        }
        private long _userID;

        [Flags]
        private enum LoadState
        {
            None = 0,
            Village = 2,
            AvatarData = 4,
            All = Village | AvatarData
        }

        // Loads the avatar completely.
        public void LoadFull()
        {
            var state = LoadState.None;
            var saveDir = SaveDirectory;
            var files = Directory.GetFiles(saveDir);

            //Avatar = new Avatar();
            Avatar.Token = Token;
            Avatar.ID = ID;

            for (int i = 0; i < files.Length; i++)
            {
                var filePath = files[i];
                var fileName = Path.GetFileName(filePath);

                switch (fileName)
                {
                    case "village.json":

                        // Load village.json from disk.
                        var homeJson = File.ReadAllText(filePath);
                        var home = Village.FromJson(homeJson);

                        Avatar.Home = home;
                        state |= LoadState.Village;
                        break;

                    case "avatar.dat":

                        // Loads avatar.dat from disk.
                        var data = File.ReadAllText(filePath);
                        var reader = new SaveReader(data);

                        ReadAvatarData(reader);
                        state |= LoadState.AvatarData;
                        break;
                }
            }

            switch (state)
            {
                case LoadState.None:
                    FancyConsole.WriteLine("\t..Could not find &(darkcyan)village.json&(default) and &(darkcyan)avatar.dat&(default) for &(magenta){0}&(default).",
                                           Avatar.Token);
                    break;

                case LoadState.AvatarData:
                    FancyConsole.WriteLine("\t..Could not find &(darkcyan)avatar.dat&(default) for &(darkcyan){0}&(default).",
                                           Avatar.Token);
                    break;

                case LoadState.Village:
                    FancyConsole.WriteLine("\t..Could not find &(darkcyan)village.json&(default) for &(darkcyan){0}&(default).",
                                           Avatar.Token);
                    break;

                case LoadState.All:
                    FancyConsole.WriteLine("\t..Loaded &(darkcyan){0}&(default) successfully.",
                                           Avatar.Token);
                    break;
            }
        }

        // Saves the whole avatar.
        public void SaveFull()
        {
            if (Avatar.Token == null)
            {
                FancyConsole.WriteLine("\t..Unable to save &(darkcyan){0}&(default) &(darkgreen)successfully&(default).",
                                       Avatar.ID);
                return;
            }

            //var saveDir = Path.Combine("Avatars", Avatar.ID.ToString() + "-" + Avatar.Token);
            if (!Directory.Exists(SaveDirectory))
                Directory.CreateDirectory(SaveDirectory);

            SaveAvatarData();
            SaveVillage();
        }

        // Saves the avatar data only.
        public void SaveAvatarData()
        {
            var data = WriteAvatarData();
            File.WriteAllText(Path.Combine(SaveDirectory, "avatar.dat"), data);
        }

        // Saves the avatar village only.
        public void SaveVillage()
        {
            var homeJson = Avatar.Home.ToJson(true);
            File.WriteAllText(Path.Combine(SaveDirectory, "village.json"), homeJson);
        }

        private string WriteAvatarData()
        {
            var writer = new SaveWriter();
            writer.Write("Name", Avatar.Name);
            writer.Write("IsNamed", Avatar.IsNamed);
            //writer.Write("ID", Avatar.ID);
            writer.Write("League", Avatar.League);
            writer.Write("Level", Avatar.Level);
            writer.Write("Experience", Avatar.Experience);

            writer.Write("Gems", Avatar.Gems);
            writer.Write("FreeGems", Avatar.FreeGems);

            writer.Write("Trophies", Avatar.Trophies);
            writer.Write("AttacksWon", Avatar.AttacksWon);
            writer.Write("AttacksLost", Avatar.AttacksLost);
            writer.Write("DefensesWon", Avatar.DefensesWon);
            writer.Write("DefensesLost", Avatar.DefensesLost);
            return writer.ToString();
        }

        private void ReadAvatarData(SaveReader reader)
        {
            Avatar.Name = reader.ReadAsString("Name");
            Avatar.IsNamed = reader.ReadAsBoolean("IsNamed");
            //Avatar.ID = reader.ReadAsLong("ID");
            Avatar.League = reader.ReadAsInt("League");
            Avatar.Level = reader.ReadAsInt("Level");
            Avatar.Experience = reader.ReadAsInt("Experience");

            Avatar.Gems = reader.ReadAsInt("Gems");
            Avatar.FreeGems = reader.ReadAsInt("FreeGems");

            Avatar.Trophies = reader.ReadAsInt("Trophies");
            Avatar.AttacksWon = reader.ReadAsInt("AttacksWon");
            Avatar.AttacksLost = reader.ReadAsInt("AttacksLost");
            Avatar.DefensesWon = reader.ReadAsInt("DefensesWon");
            Avatar.DefensesLost = reader.ReadAsInt("DefensesLost");
        }
    }
}
