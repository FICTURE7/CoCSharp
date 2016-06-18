using CoCSharp.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CoCSharp.Server.Core
{
    // Provides method to save & load avatars.
    public class AvatarManager // : IAvatarManager
    {
        //TODO: Remove extra calls to TokenUtils.CheckToken(string).

        public AvatarManager()
        {
            _avatarDirs = new List<AvatarSave>();

            if (!Directory.Exists(DirectoryPaths.Avatars))
            {
                // Create directory "Avatars" if it does not exists.
                Directory.CreateDirectory(DirectoryPaths.Avatars);
            }
            else
            {
                // Get the max UserID by iterating through the directory paths.
                var directories = Directory.GetDirectories(DirectoryPaths.Avatars);
                for (int i = 0; i < directories.Length; i++)
                {
                    var directory = Path.GetFileName(directories[i]);
                    var dirUserID = Utils.GetDirectoryUserID(directory);
                    var userID = default(long);

                    if (long.TryParse(dirUserID, out userID))
                    {
                        if (userID < 1)
                        {
                            // We got an invalid save directory.
                            // Should probably delete it.
                        }

                        if (userID > _maxUserID)
                            _maxUserID = userID;
                    }
                }
            }
        }

        // Known avatar save directories.
        private List<AvatarSave> _avatarDirs;

        // Maximum/Latest value of user ID.
        private long _maxUserID = 1;

        // Creates a new Avatar with a random Token & UserID.
        public Avatar CreateNewAvatar()
        {
            // Generate a token & make sure its unique.
            var token = TokenUtils.GenerateToken();
            while (Exists(token))
                token = TokenUtils.GenerateToken();

            var userID = Interlocked.Increment(ref _maxUserID);

            return CreateNewAvatar(token, userID);
        }

        // Creates a new Avatar with the specified Token & UserID.
        public Avatar CreateNewAvatar(string token, long userID)
        {
            if (token == null)
                throw new ArgumentNullException("token");
            if (!TokenUtils.CheckToken(token))
                throw new ArgumentException("token must be a valid token.", "token");

            var villagePath = Path.Combine(DirectoryPaths.Content, "starting_village.json");
            var avatar = new Avatar();

            avatar.ShieldEndTime = DateTime.UtcNow.AddDays(3);
            avatar.Token = token;
            avatar.ID = userID;
            avatar.Level = 10; // Skip the tutorials.
            avatar.Home = Village.FromJson(File.ReadAllText(villagePath));
            avatar.Name = "Patrik"; // :]
            avatar.Gems = 300;
            avatar.FreeGems = 300;

            return avatar;
        }

        // Loads the avatar from disk with the specified Token.
        public Avatar LoadAvatar(string token)
        {
            // Make sure it exists because AvatarSave does not check it.
            if (!Exists(token))
                throw new ArgumentException("Avatar with token '" + token + "' does not exists.", "token");

            FancyConsole.WriteLine("[&(magenta)Avatar&(default)] Loading avatar ->");

            var saveDir = GetAvatarDirectory(token);
            if (saveDir == null)
                return null;

            var avatar = new Avatar();
            var avatarSave = new AvatarSave(avatar, saveDir);
            avatarSave.LoadFull();

            return avatar;
        }

        // Loads the avatar from disk with the specified user ID.
        public Avatar LoadAvatar(long userID)
        {
            if (!Exists(userID))
                throw new ArgumentException("Avatar with user ID '" + userID + "' does not exists.", "userID");

            var saveDir = GetAvatarDirectory(userID);
            if (saveDir == null)
                return null;

            var avatar = new Avatar();
            var avatarSave = new AvatarSave(avatar, saveDir);
            avatarSave.LoadFull();

            return avatar;
        }

        // Saves the avatar to disk.
        public void SaveAvatar(Avatar avatar)
        {
            var saveDir = Path.Combine(DirectoryPaths.Avatars, avatar.ID + "-" + avatar.Token);
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            var avatarSave = new AvatarSave(avatar, saveDir);
            avatarSave.SaveFull();
        }

        private string GetAvatarDirectory(string token)
        {
            var directories = Directory.GetDirectories(DirectoryPaths.Avatars);
            for (int i = 0; i < directories.Length; i++)
            {
                var directory = Path.GetFileName(directories[i]);

                var dirToken = Utils.GetDirectoryToken(directory);
                if (dirToken == null)
                    continue;

                if (dirToken == token)
                    return directories[i];
            }

            return null;
        }

        private string GetAvatarDirectory(long userID)
        {
            var directories = Directory.GetDirectories(DirectoryPaths.Avatars);
            for (int i = 0; i < directories.Length; i++)
            {
                var directory = Path.GetFileName(directories[i]);

                var dirUserID = Utils.GetDirectoryUserID(directory);
                if (dirUserID == null)
                    continue;

                if (dirUserID == userID.ToString())
                    return directories[i];
            }

            return null;
        }

        // Determines if an Avatar with the specified Token exists.
        public bool Exists(string token)
        {
            if (!TokenUtils.CheckToken(token))
                return false;

            return GetAvatarDirectory(token) != null;
        }

        // Determines if an Avatar with the specified UserID exists.
        public bool Exists(long userID)
        {
            if (userID < 0 || userID > _maxUserID)
                return false;

            return GetAvatarDirectory(userID) != null;
        }

        // Determines if an Avatar with the specified Token & UserID exists.
        public bool Exists(string token, long userID)
        {
            if (!TokenUtils.CheckToken(token))
                return false;

            var directories = Directory.GetDirectories(DirectoryPaths.Avatars);
            for (int i = 0; i < directories.Length; i++)
            {
                var directory = Path.GetFileName(directories[i]);

                var dirUserID = Utils.GetDirectoryUserID(directory);
                if (dirUserID != userID.ToString())
                    continue;

                var dirToken = Utils.GetDirectoryToken(directory);
                if (dirToken != token)
                    continue;

                return true;
            }
            return false;
        }
    }
}
