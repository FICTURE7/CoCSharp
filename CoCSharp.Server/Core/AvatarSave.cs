using CoCSharp.Logic;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace CoCSharp.Server.Core
{
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
            var saveDir = Path.Combine("Avatars", Avatar.Token);
            var files = Directory.GetFiles(saveDir);

            for (int i = 0; i < files.Length; i++)
            {
                var filePath = files[i];
                var fileName = Path.GetFileName(filePath);

                switch (fileName)
                {
                    case "village.json":
                        var homeJson = File.ReadAllText(filePath);
                        var home = Village.FromJson(homeJson);

                        Avatar.Home = home;
                        state |= LoadState.Village;
                        break;

                    case "avatar.dat":
                        var table = new Hashtable();
                        var data = File.ReadAllLines(filePath);
                        for (int j = 0; j < data.Length; j++)
                        {
                            var split = data[j].Split('=');
                            table.Add(split[0], split[1]);
                        }

                        LoadAvatarData(table);
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
            var builder = new StringBuilder();
            builder.AppendFormat("Name={0}\n", Avatar.Name);
            builder.AppendFormat("IsNamed={0}\n", Avatar.IsNamed);
            builder.AppendFormat("ID={0}\n", Avatar.ID);
            builder.AppendFormat("Token={0}\n", Avatar.Token);
            builder.AppendFormat("League={0}\n", Avatar.League);
            builder.AppendFormat("Level={0}\n", Avatar.Level);
            builder.AppendFormat("Experience={0}\n", Avatar.Experience);
            builder.AppendFormat("Gems={0}\n", Avatar.Gems);
            builder.AppendFormat("FreeGems={0}\n", Avatar.FreeGems);
            builder.AppendFormat("Trophies={0}\n", Avatar.Trophies);

            builder.AppendFormat("AttacksWon={0}\n", Avatar.AttacksWon);
            builder.AppendFormat("AttacksLost={0}\n", Avatar.AttacksLost);
            builder.AppendFormat("DefensesWon={0}\n", Avatar.DefensesWon);
            builder.AppendFormat("DefensesLost={0}\n", Avatar.DefensesLost);
            return builder.ToString();
        }

        private void LoadAvatarData(Hashtable table)
        {
            Avatar.Name = (string)table["Name"];
            Avatar.IsNamed = Convert.ToBoolean(table["IsNamed"]);
            Avatar.ID = Convert.ToInt64(table["ID"]);
            Avatar.Token = (string)table["Token"];
            Avatar.League = Convert.ToInt32(table["League"]);
            Avatar.Level = Convert.ToInt32(table["Level"]);
            Avatar.Experience = Convert.ToInt32(table["Experience"]);
            Avatar.Gems = Convert.ToInt32(table["Gems"]);
            Avatar.FreeGems = Convert.ToInt32(table["FreeGems"]);
            Avatar.Trophies = Convert.ToInt32(table["Trophies"]);
            Avatar.AttacksWon = Convert.ToInt32(table["AttacksWon"]);
            Avatar.AttacksLost = Convert.ToInt32(table["AttacksLost"]);
            Avatar.DefensesWon = Convert.ToInt32(table["DefensesWon"]);
            Avatar.DefensesLost = Convert.ToInt32(table["DefensesLost"]);
        }
    }
}
