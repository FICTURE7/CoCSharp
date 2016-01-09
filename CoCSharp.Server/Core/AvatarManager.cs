using CoCSharp.Logic;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoCSharp.Server.Core
{
    public class AvatarManager
    {
        //TODO: Improve this thing.

        private const string ValidTokenChar = "abcdefghijklmnopqrstuvwxyz1234567890";
        private const int ValidTokenLength = 40;

        public AvatarManager()
        {
            Avatars = new Dictionary<string, Avatar>();
            LoadAllAvatars();
        }

        public Dictionary<string, Avatar> Avatars { get; set; } // usertoken to avatar

        public Avatar CreateNewAvatar()
        {
            return CreateNewAvatar(GenerateUserToken(), GenerateUserID());
        }

        public Avatar CreateNewAvatar(string token, long id)
        {
            var avatar = new Avatar();
            avatar.ShieldDuration = TimeSpan.FromDays(3);
            avatar.Token = token;
            avatar.ID = id;
            avatar.Level = 10; // bypass tut
            avatar.Home = Village.FromJson(File.ReadAllText("Content\\default_village.json"));
            avatar.Name = "Patrik"; // :]

            Avatars.Add(avatar.Token, avatar);
            return avatar;
        }

        public void SaveAllAvatars()
        {
            var avatars = Avatars.Values.ToArray();
            for (int i = 0; i < avatars.Length; i++)
                SaveAvatar(avatars[i]);
        }

        public void SaveAvatar(Avatar avatar)
        {
            var path = Path.Combine("Avatars", avatar.Token);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var village = avatar.Home.ToJson(true);
            File.WriteAllText(Path.Combine(path, "Village.json"), village);

            var builder = new StringBuilder();

            var type = avatar.GetType();
            var properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                if (property.PropertyType == typeof(Village))
                    continue;

                var name = property.Name;
                var value = property.GetValue(avatar);

                if (value == null)
                    value = "null";

                builder.AppendFormat("{0} = {1}\n", name, value);
            }

            var savePath = Path.Combine(path, avatar.Name + ".txt");
            File.WriteAllText(savePath, builder.ToString());
        }

        public void LoadAllAvatars()
        {
            if (!Directory.Exists("Avatars"))
            {
                Directory.CreateDirectory("Avatars");
                return; // exit early cause the file didnt exist
            }

            var avatarDirectories = Directory.GetDirectories("Avatars");
            for (int i = 0; i < avatarDirectories.Length; i++)
            {
                var directory = avatarDirectories[i];
                var files = Directory.GetFiles(directory);

                var homePath = (string)null;
                var dataPath = (string)null;

                if (files.Length > 2)
                    Console.WriteLine("WARNING: Found avatar save directory with more than 2 files. {0}", directory);

                for (int j = 0; j < files.Length; j++)
                {
                    var file = files[j];
                    if (Path.GetFileName(file) == "Village.json")
                        homePath = file;
                    else if (Path.GetExtension(file) == ".txt")
                        dataPath = file;
                }

                if (homePath == null || dataPath == null)
                {
                    Console.WriteLine("WARNING: Couldn't find a save file. Skipping {0}", directory);
                    continue;
                }

                var home = Village.FromJson(File.ReadAllText(homePath));
                var avatar = new Avatar();
                avatar.Home = home;

                var type = avatar.GetType();
                var saveProperties = File.ReadAllLines(dataPath);
                for (int j = 0; j < saveProperties.Length; j++)
                {
                    var saveValues = ParseSaveProperty(saveProperties[j]);
                    var property = type.GetProperty(saveValues[0]);
                    var value = (object)saveValues[1];

                    if (saveValues[1] == "null")
                    {
                        value = null;
                    }
                    else if (property.PropertyType == typeof(TimeSpan))
                    {
                        value = TimeSpan.Parse(saveValues[1]);
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        value = DateTime.Parse(saveValues[1]);
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        value = int.Parse(saveValues[1]);
                    }
                    else if (property.PropertyType == typeof(long))
                    {
                        value = long.Parse(saveValues[1]);
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        value = bool.Parse(saveValues[1]);
                    }

                    property.SetValue(avatar, value);
                }

                Avatars.Add(avatar.Token, avatar);
            }
        }

        private string[] ParseSaveProperty(string saveProperty)
        {
            var saveValues = saveProperty.Split('=');

            saveValues[0] = saveValues[0].Remove(saveValues[0].Length - 1);
            saveValues[1] = saveValues[1].Remove(0, 1);

            return saveValues;
        }

        private long GenerateUserID()
        {
            return Utils.Random.Next();
        }

        private string GenerateUserToken()
        {
            var token = string.Empty;
            for (int i = 0; i < ValidTokenLength; i++)
                token += ValidTokenChar[Utils.Random.Next(ValidTokenChar.Length - 1)];

            if (Avatars.ContainsKey(token)) // chances are slim but possible ;]
                token = GenerateUserToken();

            return token;
        }
    }
}
