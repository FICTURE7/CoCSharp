using CoCSharp.Databases.Csv;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

namespace CoCSharp.Databases
{
    public abstract class Database
    {
        public const string AssetServer = "http://b46f744d64acd2191eda-3720c0374d47e9a0dd52be4d281c260f.r11.cf2.rackcdn.com/";

        public Database(string path)
        {
            this.CsvTable = new CsvTable(path);
            this.CsvPath = path;
        }

        public string CsvPath { get; set; }
        public virtual CsvTable CsvTable { get; set; }

        public virtual void ReloadDatabase()
        {
            CsvTable = new CsvTable(CsvPath);
            LoadDatabase();
        }

        public abstract void LoadDatabase();

        public static void DownloadDatabasesAysnc(string directory, string fingerprintHash)
        {
            var dbPath = Path.Combine(directory, fingerprintHash);
            if (!Directory.Exists(dbPath))
                Directory.CreateDirectory(dbPath);

            var rootUrl = AssetServer + fingerprintHash + "/";
            var fingerprintUrl = new Uri(rootUrl + "fingerprint.json");
            using (var webClient = new WebClient())
            {
                webClient.DownloadStringCompleted += DownloadStringAsyncCompleted;
                webClient.DownloadStringAsync(fingerprintUrl, new string[] { rootUrl, dbPath });
            }
        }

        private static void DownloadStringAsyncCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null) throw e.Error;

            var webClient = (WebClient)sender;
            var rootUrl = ((string[])e.UserState)[0];
            var dbPath = ((string[])e.UserState)[1];
            var fingerprintJson = e.Result;
            dynamic fingerprint = JObject.Parse(fingerprintJson); //TODO: Make Fingerprint.cs
            var files = fingerprint.files;

            File.WriteAllText(dbPath + "/fingerprint.json", fingerprintJson);

            webClient.DownloadDataCompleted += DownloadDataAsyncCompleted;
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i].file;
                if (Path.GetExtension(file.Value) == ".csv") // could download whole asset stuff but nah
                {
                    var csvUrl = new Uri(rootUrl + file.Value);
                    while (webClient.IsBusy) ;
                    webClient.DownloadDataAsync(csvUrl, new string[] { Path.GetFileName(file.Value), dbPath });
                }
            }
        }

        private static void DownloadDataAsyncCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            var csvFile = e.Result;
            var file = ((string[])e.UserState)[0];
            var path = ((string[])e.UserState)[1];
            File.WriteAllBytes(Path.Combine(path, Path.GetFileName(file)), csvFile);
        }
    }
}
