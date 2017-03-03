using CoCSharp.Csv;
using CoCSharp.Data.Models;
using CoCSharp.Server.Api;
using CoCSharp.Server.Api.Db;
using CoCSharp.Server.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoCSharp.Server.Db
{
    public partial class MySqlDbManager : IDbManager
    {
        //TODO: Create a LevelFactory instead, that will handle all of the naming and stuff.

        private static readonly Random s_random = new Random();
        private static readonly string[] s_names =
        {
            "Patrik",
            "Jean",
            "Kenny",
            "Levi",
            "Edward",
            "Dan",
            "Vincent",
            "Luke",
            "Vulcan",
            "Kukli",
            "Davi",
            "Osie",
            "Keem",
            "Alfred",
            "Dimitri",
            "Valdimir"
        };
        private static readonly int s_nameCount = s_names.Length;

        public MySqlDbManager(IServer server)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            _server = server;

            var builder = new MySqlConnectionStringBuilder();

            var host = Server.Configuration.MySqlHost;
            if (host == null)
                Server.Logs.Error("Configuration did not contain 'mysql_host' entry.");

            var user = Server.Configuration.MySqlUser;
            if (user == null)
                Server.Logs.Error("Configuration did not contain 'mysql_user' entry.");

            var pwd = Server.Configuration.MySqlPassword;
            if (pwd == null)
                Server.Logs.Error("Configuration did not contain 'mysql_pwd' entry.");

            var port = Server.Configuration.MySqlPort;
            if (pwd == null)
                Server.Logs.Error("Configuration did not contain 'mysql_port' entry.");

            var configError = host == null || user == null || pwd == null;
            if (configError)
                throw new InvalidOperationException("Missing config information.");

            _userIds = new List<long>();
            _logger = Server.Logs.GetLogger<ClanLogger>();

            builder.Server = host;
            builder.UserID = user;
            if (!string.IsNullOrWhiteSpace(pwd))
            builder.Password = pwd;
            builder.Port = (uint)port;
            // Disable pooling, reopening a connection from the pool,
            // seems to be causing issues.
            builder.Pooling = false;
            builder.Database = "cocsharp";
            builder.MinimumPoolSize = 1;

            _connectionString = builder.ToString();
            using (var sql = new MySqlConnection(_connectionString))
            {
                sql.Open();

                Server.Logs.Info($"Successfully made a MySqlConnection to {user}@{host}.");

                Server.Logs.Info("Counting number of levels in the database...");
                var command = new MySqlCommand("SELECT COUNT(*) FROM `levels`", sql);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        _levelCount = (long)reader["COUNT(*)"];
                }

                Server.Logs.Info("Counting number of clans in the database...");
                command = new MySqlCommand("SELECT COUNT(*) FROM `clans`", sql);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        _clanCount = (long)reader["COUNT(*)"];
                }
            }

            // Look up the data ID of resources such as Gold, Elixir and Diamonds.
            var resourceTable = _server.Assets.Get<CsvDataTable<ResourceData>>();
            _goldId = resourceTable.Rows["Gold"].Id;
            _elixirId = resourceTable.Rows["Elixir"].Id;
            _gemsId = resourceTable.Rows["Diamonds"].Id;

            _startingGold = _server.Configuration.StartingGold;
            _startingElixir = _server.Configuration.StartingElixir;
            _startingGems = _server.Configuration.StartingGems;
            _startingVillage = _server.Configuration.StartingVillage;
        }

        private List<long> _userIds;
        private long _levelCount;
        private long _clanCount;

        private readonly IServer _server;
        private readonly ClanLogger _logger;
        private readonly string _connectionString;

        private readonly int _goldId;
        private readonly int _elixirId;
        private readonly int _gemsId;

        private readonly int _startingGold;
        private readonly int _startingElixir;
        private readonly int _startingGems;
        private readonly string _startingVillage;

        private static string GetRandomName() => s_names[s_random.Next(s_nameCount)];

        public IServer Server => _server;

        public Task<long> GetLevelCountAsync() => Task.FromResult(Thread.VolatileRead(ref _levelCount));
        public Task<long> GetClanCountAsync() => Task.FromResult(Thread.VolatileRead(ref _clanCount));

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Space
        }
    }
}
