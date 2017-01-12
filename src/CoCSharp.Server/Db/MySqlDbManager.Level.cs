using CoCSharp.Data;
using CoCSharp.Data.Slots;
using CoCSharp.Network;
using CoCSharp.Server.Api.Core.Factories;
using CoCSharp.Server.Api.Db;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoCSharp.Server.Db
{
    public partial class MySqlDbManager
    {
        public async Task<LevelSave> LoadLevelAsync(long userId, CancellationToken cancellationToken)
        {
            // Look up instance in our cache.
            var levelSave = default(LevelSave);
            if (Server.Cache.TryGet(userId, out levelSave))
                return levelSave;

            cancellationToken.ThrowIfCancellationRequested();

            // Otherwise we look up in the db.
            using (var sql = new MySqlConnection(_connectionString))
            {
                await sql.OpenAsync(cancellationToken);

                using (var command = new MySqlCommand("SELECT * FROM `levels` WHERE `user_id` = @UserId", sql))
                {
                    command.Parameters.AddWithValue("UserId", userId);

                    using (var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellationToken))
                    {
                        if (reader.HasRows)
                        {
                            if (await reader.ReadAsync(cancellationToken))
                            {
                                // Create a new instance of the LevelSave class and sets
                                // its value from the DbDataReader.
                                levelSave = Server.Factories.GetFactory<LevelSaveFactory>().Create();
                                FromMySqlDataReader(levelSave, reader);

                                // UserId returned should be the same as the id as requested.
                                Debug.Assert(levelSave.UserId == userId, "Requested user ID and loaded user ID differ.");

                                // Register this instance to our cache.
                                Server.Cache.Register(userId, levelSave);
                                return levelSave;
                            }
                        }

                        return null;
                    }
                }
            }
        }

        public Task SaveLevelAsync(LevelSave level, CancellationToken token)
            => InternalSaveLevelAsync(level, false, token);

        private async Task InternalSaveLevelAsync(LevelSave level, bool newLevel, CancellationToken cancellationToken)
        {
            using (var sql = new MySqlConnection(_connectionString))
            {
                await sql.OpenAsync(cancellationToken);

                // Don't forget to update the DateLastSave value.
                level.DateLastSave = DateTime.UtcNow;

                // Convert the LevelSave to a MySqlCommand with the correct parameters.
                using (var command = ToMySqlCommand(level))
                {
                    command.Connection = sql;

                    var numRows = await command.ExecuteNonQueryAsync(cancellationToken);
                    if (newLevel && numRows == 1)
                    {
                        var newId = command.LastInsertedId;
                        Debug.Assert(newId != -1, "Number of rows affected was 1, however last insert id was 0.");

                        level.UserId = command.LastInsertedId;
                    }
                    else
                    {
                        Debug.Assert(level.UserId != 0);

                        // If the level isn't in a clan, we make sure its not in the clans_members table.
                        if (level.ClanId == null)
                        {
                            using (var command2 = new MySqlCommand("DELETE FROM `clan_members` WHERE `user_id` = @UserId", sql))
                            {
                                command2.Parameters.AddWithValue("UserId", level.UserId);

                                var numRows2 = await command2.ExecuteNonQueryAsync(cancellationToken);
                                if (numRows2 == 1)
                                    Server.Logs.Info("Deleting entry from clans_members since level is not in clan.");
                            }
                        }
                    }
                }
            }
        }

        public Task<LevelSave> NewLevelAsync(CancellationToken cancellationToken)
            => InternalNewLevelAsync(0, TokenUtils.GenerateToken(), cancellationToken);

        public Task<LevelSave> NewLevelAsync(long userId, string userToken, CancellationToken cancellationToken)
            => InternalNewLevelAsync(userId, userToken, cancellationToken);

        private async Task<LevelSave> InternalNewLevelAsync(long userId, string userToken, CancellationToken cancellationToken)
        {
            var save = Server.Factories.GetFactory<LevelSaveFactory>().Create();
            save.DateCreated = DateTime.UtcNow;
            save.UserId = userId;
            save.Token = userToken;
            // Prevent client from crashing.
            // Because a name == null, will cause the client to crash.
            save.Name = GetRandomName();
            save.Gems = _startingGems;
            save.FreeGems = _startingGems;
            // Prevent client from crashing.
            // Because a level less than 1 will cause the client to crash.
            save.ExpLevels = 1;
            save.LevelJson = _startingVillage;

            // Skip the initial tutorials.
            var tutorialProgress = new TutorialProgressSlot[10];
            for (int i = 0; i < 10; i++)
                tutorialProgress[i] = new TutorialProgressSlot(21000000 + i);

            var resourceAmount = new ResourceAmountSlot[]
            {
                new ResourceAmountSlot(_goldId, _startingGold),
                new ResourceAmountSlot(_elixirId, _startingElixir),
                new ResourceAmountSlot(_gemsId, _startingGems)
            };

            save.TutorialProgress = tutorialProgress;
            save.ResourcesAmount = resourceAmount;

            // Save LevelSave to set the ID using the AutoId stuff.
            await InternalSaveLevelAsync(save, true, cancellationToken);
            Interlocked.Increment(ref _levelCount);

            return save;
        }

        public async Task<LevelSave> RandomLevelAsync(CancellationToken cancellationToken)
        {
            using (var sql = new MySqlConnection(_connectionString))
            {
                await sql.OpenAsync(cancellationToken);

                using (var command = new MySqlCommand("SELECT * FROM `levels` ORDER BY RAND() LIMIT 1;", sql))
                {
                    var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellationToken);

                    if (reader.HasRows)
                    {
                        if (await reader.ReadAsync(cancellationToken))
                        {
                            // Could mess the cache up.
                            var levelSave = Server.Factories.GetFactory<LevelSaveFactory>().Create();
                            FromMySqlDataReader(levelSave, reader);

                            Server.Cache.Register(levelSave.UserId, levelSave);
                            return levelSave;
                        }
                    }
                    return null;
                }
            }
        }

        private static void FromMySqlDataReader(LevelSave save, DbDataReader reader)
        {
            save.UserId = (long)reader["user_id"];
            save.ClanId = reader["clan_id"] is DBNull ? null : (long?)reader["clan_id"];

            save.DateCreated = (DateTime)reader["create_date"];
            save.DateLastSave = (DateTime)reader["last_save_date"];
            save.PlayTime = TimeSpan.FromSeconds((int)reader["play_time_seconds"]);
            save.LoginCount = (int)reader["login_count"];

            save.LevelJson = (string)reader["level_json"];

            save.Token = (string)reader["token"];

            save.Name = (string)reader["name"];
            save.IsNamed = (bool)reader["is_named"];
            save.Trophies = (int)reader["trophies"];
            save.League = (int)reader["league"];
            save.ExpPoints = (int)reader["exp_points"];
            save.ExpLevels = (int)reader["exp_levels"];
            save.Gems = (int)reader["gems"];
            save.FreeGems = (int)reader["free_gems"];
            save.AttacksWon = (int)reader["atk_won"];
            save.AttacksLost = (int)reader["atk_lost"];
            save.DefensesWon = (int)reader["def_won"];
            save.DefensesLost = (int)reader["def_lost"];

            // Deserialize the slots.
            var slots = (byte[])reader["slots"];
            var stream = new MemoryStream(slots);
            using (var r = new MessageReader(stream))
            {
                save.ResourcesCapacity = ReadSlots<ResourceCapacitySlot>(r);
                save.ResourcesAmount = ReadSlots<ResourceAmountSlot>(r);
                save.Units = ReadSlots<UnitSlot>(r);
                save.Spells = ReadSlots<SpellSlot>(r);
                save.UnitUpgrades = ReadSlots<UnitUpgradeSlot>(r);
                save.SpellUpgrades = ReadSlots<SpellUpgradeSlot>(r);
                save.HeroUpgrades = ReadSlots<HeroUpgradeSlot>(r);
                save.HeroHealths = ReadSlots<HeroHealthSlot>(r);
                save.HeroStates = ReadSlots<HeroStateSlot>(r);
                save.AllianceUnits = ReadSlots<AllianceUnitSlot>(r);
                save.TutorialProgress = ReadSlots<TutorialProgressSlot>(r);
                save.Achievements = ReadSlots<AchievementSlot>(r);
                save.AchievementProgress = ReadSlots<AchievementProgessSlot>(r);
                save.NpcStars = ReadSlots<NpcStarSlot>(r);
                save.NpcGold = ReadSlots<NpcGoldSlot>(r);
                save.NpcElixir = ReadSlots<NpcElixirSlot>(r);
            }
        }

        private static MySqlCommand ToMySqlCommand(LevelSave save)
        {
            var command = new MySqlCommand(MySqlDbManagerQueries.InsertUpdateLevel);
            command.Parameters.AddWithValue("DateCreated", save.DateCreated);
            command.Parameters.AddWithValue("DateLastSave", save.DateLastSave);
            command.Parameters.AddWithValue("PlayTime", (int)save.PlayTime.TotalSeconds);
            command.Parameters.AddWithValue("LoginCount", save.LoginCount);
            command.Parameters.AddWithValue("UserId", save.UserId);
            command.Parameters.AddWithValue("ClanId", save.ClanId);
            command.Parameters.AddWithValue("LevelJson", save.LevelJson);
            command.Parameters.AddWithValue("Token", save.Token);
            command.Parameters.AddWithValue("Name", save.Name);
            command.Parameters.AddWithValue("IsNamed", save.IsNamed);
            command.Parameters.AddWithValue("Trophies", save.Trophies);
            command.Parameters.AddWithValue("League", save.League);
            command.Parameters.AddWithValue("ExpPoints", save.ExpPoints);
            command.Parameters.AddWithValue("ExpLevel", save.ExpLevels);
            command.Parameters.AddWithValue("Gems", save.Gems);
            command.Parameters.AddWithValue("FreeGems", save.FreeGems);
            command.Parameters.AddWithValue("AttacksWon", save.AttacksWon);
            command.Parameters.AddWithValue("AttacksLost", save.AttacksLost);
            command.Parameters.AddWithValue("DefensesWon", save.DefensesWon);
            command.Parameters.AddWithValue("DefensesLost", save.DefensesLost);

            var stream = new MemoryStream();
            using (var w = new MessageWriter(stream))
            {
                WriteSlots(w, save.ResourcesCapacity);
                WriteSlots(w, save.ResourcesAmount);
                WriteSlots(w, save.Units);
                WriteSlots(w, save.Spells);
                WriteSlots(w, save.UnitUpgrades);
                WriteSlots(w, save.SpellUpgrades);
                WriteSlots(w, save.HeroUpgrades);
                WriteSlots(w, save.HeroHealths);
                WriteSlots(w, save.HeroStates);
                WriteSlots(w, save.AllianceUnits);
                WriteSlots(w, save.TutorialProgress);
                WriteSlots(w, save.Achievements);
                WriteSlots(w, save.AchievementProgress);
                WriteSlots(w, save.NpcStars);
                WriteSlots(w, save.NpcGold);
                WriteSlots(w, save.NpcElixir);

                command.Parameters.AddWithValue("slots", stream.ToArray());
            }

            return command;
        }

        private static void WriteSlots<T>(MessageWriter writer, IEnumerable<T> slots) where T : Slot, new()
        {
            if (slots == null)
            {
                writer.Write(0);
            }
            else
            {
                var list = new List<T>(8);
                foreach (var c in slots)
                {
                    if (c != null)
                        list.Add(c);
                }

                writer.Write(list.Count);
                for (int i = 0; i < list.Count; i++)
                    list[i].WriteSlot(writer);
            }
        }

        private static IEnumerable<T> ReadSlots<T>(MessageReader reader) where T : Slot, new()
        {
            var count = reader.ReadInt32();
            if (count == 0)
                return null;

            var slots = new T[count];
            for (int i = 0; i < count; i++)
            {
                var s = new T();
                s.ReadSlot(reader);

                slots[i] = s;
            }
            return slots;
        }
    }
}
