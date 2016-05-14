using CoCSharp.Logic;
using System;
using System.IO;

namespace CoCSharp.Server.Core
{
    // Provides method to save or load avatars.
    public class AvatarSave
    {
        // Could take a user token directly instead of using an avatar.
        public AvatarSave(Avatar avatar)
        {
            Avatar = avatar;
        }

        public Avatar Avatar { get; set; }

        [Flags]
        private enum LoadState
        {
            None = 0,
            Village = 2,
            AvatarData = 4,
            All = Village | AvatarData
        }

        public void Load()
        {
            var state = LoadState.None;
            var saveDir = Path.Combine(DirectoryPaths.Avatars, Avatar.Token);
            var files = Directory.GetFiles(saveDir);

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

                        LoadAvatarData(reader);
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

        public void Save()
        {
            if (Avatar.Token == null)
            {
                FancyConsole.WriteLine("\t..Unable to save &(darkcyan){0}&(default) &(darkgreen)successfully&(default).",
                                       Avatar.ID);
                return;
            }

            var saveDir = Path.Combine("Avatars", Avatar.Token);
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            var homeJson = Avatar.Home.ToJson(true);
            File.WriteAllText(Path.Combine(saveDir, "village.json"), homeJson);

            var data = SaveAvatarData();
            File.WriteAllText(Path.Combine(saveDir, "avatar.dat"), data);
        }

        private string SaveAvatarData()
        {
            var writer = new SaveWriter();
            writer.Write("Name", Avatar.Name);
            writer.Write("IsNamed", Avatar.IsNamed);
            writer.Write("ID", Avatar.ID);
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

        private void LoadAvatarData(SaveReader reader)
        {
            Avatar.Name = reader.ReadAsString("Name");
            Avatar.IsNamed = reader.ReadAsBoolean("IsNamed");
            Avatar.ID = reader.ReadAsLong("ID");
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
