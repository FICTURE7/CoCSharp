using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Messages;
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
        #region Loading
        public async Task<ClanSave> LoadClanAsync(long clanId, CancellationToken cancellationToken)
        {
            if (clanId < 1)
                throw new ArgumentOutOfRangeException(nameof(clanId));

            // If the clan is already in the cache, we return that instance.
            var clanSave = default(ClanSave);
            if (Server.Cache.TryGet(clanId, out clanSave))
                return clanSave;

            // Otherwise we query the db to find the clan save.
            using (var sql = new MySqlConnection(_connectionString))
            {
                await sql.OpenAsync(cancellationToken);

                // Search for clan with specified clanId.
                using (var command = new MySqlCommand("SELECT * FROM `clans` WHERE `clan_id` = @ClanId", sql))
                {
                    command.Parameters.AddWithValue("ClanId", clanId);

                    using (var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellationToken))
                    {
                        // If the query did not return any rows, it means the clan does not exists.
                        // So we return null;
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        else
                        {
                            if (await reader.ReadAsync())
                            {
                                clanSave = Server.Factories.GetFactory<ClanSaveFactory>().Create();
                                FromMySqlDataReader(clanSave, reader);
                            }
                        }
                    }
                }

                var shouldDelete = false;
                var members = new List<ClanMember>(4);

                // Look for entries in the clans_members table which has the same clan_id.
                using (var command = new MySqlCommand("SELECT * FROM `clan_members` WHERE `clan_id` = @ClanId", sql))
                {
                    command.Parameters.AddWithValue("ClanId", clanSave.ClanId);

                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        if (!reader.HasRows)
                        {
                            shouldDelete = true;
                        }
                        else
                        {
                            while (await reader.ReadAsync(cancellationToken))
                            {
                                var member = new ClanMember();
                                FromMySqlDataReader(member, reader);

                                var level = await Server.Db.LoadLevelAsync(member.Id, cancellationToken);

                                member.Name = level.Name;
                                member.League = level.League;
                                member.Trophies = level.Trophies;
                                member.ExpLevels = level.ExpLevels;

                                members.Add(member);
                            }

                            clanSave.Members = members;
                        }
                    }
                }

                if (shouldDelete)
                {
                    using (var command = new MySqlCommand("DELETE FROM `clans` WHERE `clan_id` = @ClanId", sql))
                    {
                        command.Parameters.AddWithValue("ClanId", clanSave.ClanId);

                        var numRows = await command.ExecuteNonQueryAsync(cancellationToken);
                        if (numRows != 1)
                            Server.Logs.Warn($"Tried to delete an empty clan however, number of rows affected was '{numRows}'.");

                        Server.Cache.Unregister<ClanSave>(clanId);
                        return null;
                    }
                }

                // Register this instance to our cache.
                Server.Cache.Register(clanId, clanSave);
                return clanSave;
            }
        }
        #endregion

        #region Saving
        public Task SaveClanAsync(ClanSave clan, CancellationToken cancellationToken)
        {
            if (clan == null)
                throw new ArgumentNullException(nameof(clan));

            return InternalSaveClanAsync(clan, false, cancellationToken);
        }

        private async Task InternalSaveClanAsync(ClanSave clanSave, bool newClan, CancellationToken cancellationToken)
        {
            using (var sql = new MySqlConnection(_connectionString))
            {
                await sql.OpenAsync();

                using (var command = ToMySqlCommand(clanSave))
                {
                    command.Connection = sql;

                    var numRows = await command.ExecuteNonQueryAsync(cancellationToken);

                    // numRows affected 1 means that the ClanSave was inserted into the db,
                    // numRows affected 2 means that the ClanSave was updated.
                    // Since its new, it shouldn't have any members.
                    if (newClan && numRows == 1)
                    {
                        var newId = command.LastInsertedId;
                        Debug.Assert(newId != 0, "Number of rows affected was 1, however last insert id was 0.");

                        clanSave.ClanId = newId;
                        Server.Cache.Register(newId, clanSave);
                    }
                    else
                    {
                        Debug.Assert(clanSave.ClanId != 0);

                        // Count how many members are in the clan to
                        // figure out if the clans needs to be deleted from the db.
                        var count = 0;
                        using (var command2 = new MySqlCommand("DELETE FROM `clan_members` WHERE `clan_id` = @ClanId", sql))
                        {
                            command2.Parameters.AddWithValue("ClanId", clanSave.ClanId);

                            var numRows3 = await command2.ExecuteNonQueryAsync(cancellationToken);

                            // Iterates through our list of clan members and store in them
                            // in the clans_members table.
                            foreach (var c in clanSave.Members)
                            {
                                var ccommand = ToMySqlCommand(c, clanSave);
                                ccommand.Connection = sql;

                                // Make sure to await since we can't do multiple queries on the same connection.
                                await ccommand.ExecuteNonQueryAsync(cancellationToken);

                                count++;
                            }
                        }

                        // If there are no members left in the clan, we delete the clan from table.
                        var shouldDelete = count == 0;
                        if (shouldDelete)
                        {
                            using (var command2 = new MySqlCommand("DELETE FROM `clans` WHERE `clan_id` = @ClanId", sql))
                            {
                                command2.Parameters.AddWithValue("ClanId", clanSave.ClanId);

                                var numRows2 = await command2.ExecuteNonQueryAsync(cancellationToken);
                                if (numRows2 != 1)
                                    Server.Logs.Warn($"Tried to delete an empty clan however, number of rows affected was '{numRows2}'.");

                                Server.Cache.Unregister<ClanSave>(clanSave.ClanId);
                            }
                        }
                        else
                        {
                            Server.Cache.Register(clanSave.ClanId, clanSave);
                        }
                    }
                }
            }
        }

        public async Task<ClanSave> NewClanAsync(CancellationToken cancellationToken)
        {
            var clan = Server.Factories.GetFactory<ClanSaveFactory>().Create();
            clan.Name = string.Empty;
            clan.Description = string.Empty;
            clan.ExpLevels = 1;
            clan.Members = new List<ClanMember>();

            await InternalSaveClanAsync(clan, true, cancellationToken);

            // Keep track of number of clans without making use of
            // SQL queries.
            Interlocked.Increment(ref _clanCount);

            return clan;
        }
        #endregion

        #region Searching
        public async Task<IEnumerable<ClanSave>> SearchClansAsync(Level level, ClanQuery search, CancellationToken cancellationToken)
        {
            using (var sql = new MySqlConnection(_connectionString))
            {
                await sql.OpenAsync();

                // Look for clan IDs with the basic criteria.
                var clanIds = new List<long>(64);
                var commandTxt = "SELECT `clan_id` FROM `clans` WHERE `invite_type` < 3 AND name LIKE @TextSearch AND `perk_points` <= @PerkPoints AND `exp_levels` <= @ExpLevels";
                using (var command = new MySqlCommand(commandTxt, sql))
                {
                    command.Parameters.AddWithValue("TextSearch", search.TextSearch + '%');
                    command.Parameters.AddWithValue("PerkPoints", search.PerkPoints);
                    command.Parameters.AddWithValue("ExpLevels", search.ExpLevels);
                    if (search.ClanLocation != null)
                    {
                        command.CommandText += " AND `location` = @ClanLocation";
                        command.Parameters.AddWithValue("ClanLocation", search.ClanLocation);
                    }

                    if (search.WarFrequency != null)
                    {
                        command.CommandText += " AND `war_frequency` = @WarFrequency";
                        command.Parameters.AddWithValue("WarFrequency", search.WarFrequency);
                    }

                    if (search.OnlyCanJoin)
                    {
                        command.CommandText += " AND `required_trophies` <= @Trophies";
                        command.Parameters.AddWithValue("Trophies", search.PerkPoints);
                    }

                    // Select 64 random clans.
                    command.CommandText += " LIMIT 64";

                    using (var reader = await command.ExecuteReaderAsync(CommandBehavior.KeyInfo, cancellationToken))
                    {
                        while (await reader.ReadAsync())
                            clanIds.Add((long)reader[0]);
                    }
                }

                if (clanIds.Count > 0)
                {
                    var clans = new List<ClanSave>();
                    var clanToDelete = new List<long>(clanIds.Count);
                    var clanToLoad = new List<long>(clanIds.Count);

                    // Load up the clan member count in the db.
                    for (int i = 0; i < clanIds.Count; i++)
                    {
                        using (var command = new MySqlCommand("SELECT COUNT(`clan_id`) FROM `clan_members` WHERE `clan_id` = @ClanId", sql))
                        {
                            command.Parameters.AddWithValue("ClanId", clanIds[i]);

                            using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                            {
                                if (await reader.ReadAsync())
                                {
                                    var count = (long)reader[0];
                                    // If clan is completely full the user can't join it.
                                    if (count >= 50 && search.OnlyCanJoin)
                                        continue;

                                    // Mark the clan for deletion if it has no clan members.
                                    if (count == 0)
                                        clanToDelete.Add(clanIds[i]);
                                    else if (count >= search.MinimumMembers && count <= search.MaximumMembers)
                                        clanToLoad.Add(clanIds[i]);
                                }
                            }
                        }
                    }

                    for (int i = 0; i < clanToLoad.Count; i++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var clanId = clanToLoad[i];
                        var clan = await LoadClanAsync(clanId, cancellationToken);

                        clans.Add(clan);
                    }

                    return clans;
                }
                return null;
            }
        }

        public Task<IEnumerable<ClanSave>> SearchClansAsync(Level level, CancellationToken cancellationToken)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            return SearchClansAsync(level, new ClanQuery
            {
                ExpLevels = 1,
                MinimumMembers = 1,
                MaximumMembers = 50,
                TextSearch = string.Empty,
                ClanLocation = null,
                WarFrequency = null,
                PerkPoints = 0,
                OnlyCanJoin = true,
            }, cancellationToken);
        }
        #endregion

        private static void FromMySqlDataReader(ClanMember member, DbDataReader reader)
        {
            member.Id = (long)reader["user_id"];
            member.Role = (ClanMemberRole)reader["role"];
            member.TroopsDonated = (int)reader["troops_donated"];
            member.TroopsReceived = (int)reader["troops_received"];
            member.Rank = (int)reader["rank"];
            member.PreviousRank = (int)reader["rank_prev"];
            member.NewMember = (bool)reader["new_member"];
            member.WarCooldown = (int)reader["war_cooldown"];
            member.WarPreference = (int)reader["war_preference"];
        }

        private static MySqlCommand ToMySqlCommand(ClanMember member, ClanSave clan)
        {
            var command = new MySqlCommand(MySqlDbManagerQueries.InsertUpdateClanMember);
            command.Parameters.AddWithValue("UserId", member.Id);
            command.Parameters.AddWithValue("ClanId", clan.ClanId);
            command.Parameters.AddWithValue("Role", (int)member.Role);
            command.Parameters.AddWithValue("TroopsDonated", member.TroopsDonated);
            command.Parameters.AddWithValue("TroopsReceived", member.TroopsReceived);
            command.Parameters.AddWithValue("Rank", member.Rank);
            command.Parameters.AddWithValue("PreviousRank", member.PreviousRank);
            command.Parameters.AddWithValue("NewMember", member.NewMember);
            command.Parameters.AddWithValue("WarCooldown", member.WarCooldown);
            command.Parameters.AddWithValue("WarPreference", member.WarPreference);

            return command;
        }

        private static void FromMySqlDataReader(ClanSave save, DbDataReader reader)
        {
            save.ClanId = (long)reader["clan_id"];

            //save.DateCreated = (DateTime)reader["create_date"];
            //save.DateLastSave = (DateTime)reader["last_save_date"];

            save.Name = (string)reader["name"];
            save.Description = (string)reader["description"];
            save.ExpLevels = (int)reader["exp_levels"];
            save.Badge = (int)reader["badge"];
            save.InviteType = (int)reader["invite_type"];
            save.TotalTrophies = (int)reader["total_trophies"];
            save.RequiredTrophies = (int)reader["required_trophies"];
            save.WarsWon = (int)reader["wars_won"];
            save.WarsLost = (int)reader["wars_lost"];
            save.WarsTried = (int)reader["wars_tried"];
            save.Location = (int)reader["location"];
            save.PerkPoints = (int)reader["perk_points"];
            save.WinStreak = (int)reader["win_streak"];
            save.WarLogsPublic = (bool)reader["war_logs_public"];

            var entries = (byte[])reader["entries"];
            var stream = new MemoryStream(entries);
            using (var r = new MessageReader(stream))
            {
                var count = r.ReadInt32();
                var list = new List<AllianceStreamEntry>(count);
                for (int i = 0; i < count; i++)
                {
                    var id = r.ReadInt32();
                    var entry = StreamEntryFactory.CreateAllianceStreamEntry(id);

                    Debug.Assert(entry != null);

                    if (entry == null)
                        break;

                    entry.ReadStreamEntry(r);
                    list.Add(entry);
                }

                save.Entries = list;
            }
        }

        private static MySqlCommand ToMySqlCommand(ClanSave save)
        {
            var command = new MySqlCommand(MySqlDbManagerQueries.InsertUpdateClan);
            command.Parameters.AddWithValue("ClanId", save.ClanId);
            command.Parameters.AddWithValue("Name", save.Name);
            command.Parameters.AddWithValue("Description", save.Description);
            command.Parameters.AddWithValue("ExpLevels", save.ExpLevels);
            command.Parameters.AddWithValue("Badge", save.Badge);
            command.Parameters.AddWithValue("InviteType", save.InviteType);
            command.Parameters.AddWithValue("TotalTrophies", save.TotalTrophies);
            command.Parameters.AddWithValue("RequiredTrophies", save.RequiredTrophies);
            command.Parameters.AddWithValue("WarsWon", save.WarsWon);
            command.Parameters.AddWithValue("WarsLost", save.WarsLost);
            command.Parameters.AddWithValue("WarsTried", save.WarsTried);
            command.Parameters.AddWithValue("Language", save.Language);
            command.Parameters.AddWithValue("WarFrequency", save.WarFrequency);
            command.Parameters.AddWithValue("Location", save.Location);
            command.Parameters.AddWithValue("PerkPoints", save.PerkPoints);
            command.Parameters.AddWithValue("WinStreak", save.WinStreak);
            command.Parameters.AddWithValue("WarLogsPublic", save.WarLogsPublic);

            var stream = new MemoryStream();
            using (var w = new MessageWriter(stream))
            {
                var entryList = save.Entries;
                if (entryList == null)
                {
                    w.Write(0);
                }
                else
                {
                    var list = new List<AllianceStreamEntry>(16);
                    foreach (var e in entryList)
                    {
                        if (e != null)
                            list.Add(e);
                    }

                    w.Write(list.Count);
                    for (int i = 0; i < list.Count; i++)
                    {
                        var entry = list[i];

                        w.Write(entry.Id);
                        entry.WriteStreamEntry(w);
                    }
                }

                command.Parameters.AddWithValue("entries", stream.ToArray());
            }
            return command;
        }
    }
}
