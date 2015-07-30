using Newtonsoft.Json.Linq;
using SevenZip;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace CoCSharp.Data
{
    public class DatabaseManager
    {
        public const string AssetServer = "http://b46f744d64acd2191eda-3720c0374d47e9a0dd52be4d281c260f.r11.cf2.rackcdn.com/";

        public DatabaseManager(string fingerprintHash)
            : this("databases", fingerprintHash)
        {
            // Space
        }

        public DatabaseManager(string directory, string fingerprintHash)
        {
            var dbPath = Path.Combine(directory, fingerprintHash);
            if (!Directory.Exists(dbPath))
                Directory.CreateDirectory(dbPath);

            if (!File.Exists(Path.Combine(dbPath, "fingerprint.json")))
                DownloadAndLoadDatabasesAsync(directory, fingerprintHash);

            FingerprintHash = fingerprintHash;
            DatabaseDirectory = dbPath;
        }

        public bool IsDownloading { get; private set; }
        public string FingerprintHash { get; private set; }
        public string DatabaseDirectory { get; private set; }
        public BuildingDatabase BuildingDatabase { get; set; }
        public TrapDatabase TrapDatabase { get; set; }
        public DecorationDatabase DecorationDatabase { get; set; }
        public ObstacleDatabase ObstacleDatabase { get; set; }

        public async void DownloadAndLoadDatabasesAsync(string directory, string fingerprintHash)
        {
            var dbPath = Path.Combine(directory, fingerprintHash);
            if (!Directory.Exists(dbPath))
                Directory.CreateDirectory(dbPath);

            var rootUrl = new Uri(AssetServer + fingerprintHash + "/");
            var fingerprintUrl = new Uri(rootUrl + "fingerprint.json");
            using (var webClient = new WebClient())
            {
                IsDownloading = true;
                var fingerprintJson = await webClient.DownloadStringTaskAsync(fingerprintUrl);
                dynamic fingerprint = JObject.Parse(fingerprintJson); //TODO: Make Fingerprint.cs
                var files = fingerprint.files;
                
                File.WriteAllText(Path.Combine(dbPath, "fingerprint.json"), fingerprintJson);

                for (int i = 0; i < files.Count; i++) // iterate through fingerprint
                {
                    var file = files[i].file;
                    if (Path.GetExtension(file.Value) == ".csv") // could download whole asset stuff but nah
                    {
                        var csvUrl = new Uri(rootUrl + file.Value);
                        var comBytes = await webClient.DownloadDataTaskAsync(csvUrl);
                        var comBytesList = comBytes.ToList();
                        comBytesList.InsertRange(9, new byte[4]); // fix lzma header

                        var csvFile = LzmaUtils.Decompress(comBytesList.ToArray());
                        File.WriteAllBytes(Path.Combine(dbPath, Path.GetFileName(file.Value)), csvFile);
                    }
                }
                IsDownloading = false;
            }
            LoadDatabases();
        }

        public void LoadDatabases()
        {
            BuildingDatabase = new BuildingDatabase(Path.Combine(DatabaseDirectory, "buildings.csv"));
            TrapDatabase = new TrapDatabase(Path.Combine(DatabaseDirectory, "traps.csv"));
            DecorationDatabase = new DecorationDatabase(Path.Combine(DatabaseDirectory, "decos.csv"));
            ObstacleDatabase = new ObstacleDatabase(Path.Combine(DatabaseDirectory, "obstacles.csv"));

            BuildingDatabase.LoadDatabase();
            TrapDatabase.LoadDatabase();
            DecorationDatabase.LoadDatabase();
            ObstacleDatabase.LoadDatabase();
        }

        private void ReloadDatabases()
        {
            BuildingDatabase.ReloadDatabase();
            TrapDatabase.ReloadDatabase();
            DecorationDatabase.ReloadDatabase();
            ObstacleDatabase.ReloadDatabase();
        }
    }
}
