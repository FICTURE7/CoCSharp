using CoCSharp.Server.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace CoCSharp.Server
{
    public class ServerConfiguration : IServerConfiguration
    {
        public ServerConfiguration()
        {
            _configs = new Dictionary<string, string>();

            StartingGems = _defaultGems;
            StartingGold = _defaultGold;
            StartingElixir = _defaultElixir;
            StartingVillage = _defaultVillage;

            SynchronizeAssets = _defaultSynchronizeAssets;
            ContentUrl = _defaultContentUrl;
            MasterHash = _defaultMasterHash;

            MySqlHost = _defaultMySqlHost;
            MySqlUser = _defaultMySqlUser;
            MySqlPassword = _defaultMySqlPass;
            MySqlPort = _defaultMySqlPort;
        }

        private static readonly int _defaultGems = (int)1e7;
        private static readonly int _defaultGold = (int)1e8;
        private static readonly int _defaultElixir = (int)1e8;

        private static readonly string _defaultVillage = File.ReadAllText("contents/starting_village.json");

        private static readonly bool _defaultSynchronizeAssets = true;
        private static readonly string _defaultContentUrl = "http://b46f744d64acd2191eda-3720c0374d47e9a0dd52be4d281c260f.r11.cf2.rackcdn.com/";
        private static readonly string _defaultMasterHash = "2f2c3464104feb771097b42ebf4dfe871bd56062";

        private static readonly string _defaultMySqlHost = "127.0.0.1";
        private static readonly string _defaultMySqlUser = "root";
        private static readonly string _defaultMySqlPass = " ";
        private static readonly int _defaultMySqlPort = 3306;


        private readonly Dictionary<string, string> _configs;

        public string this[string configName]
        {
            get
            {
                if (configName == null)
                    throw new ArgumentNullException(nameof(configName));

                var value = (string)null;
                _configs.TryGetValue(configName, out value);
                return value;
            }
        }

        public int StartingGems { get; set; }
        public int StartingGold { get; set; }
        public int StartingElixir { get; set; }

        public bool SynchronizeAssets { get; set; }
        public string ContentUrl { get; set; }
        public string MasterHash { get; set; }
        public string StartingVillage { get; set; }

        public string MySqlHost { get; set; }
        public int MySqlPort { get; set; }
        public string MySqlUser { get; set; }
        public string MySqlPassword { get; set; }

        public bool Load(string path)
        {
            var settings = new XmlReaderSettings()
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
            };

            var startingGemsSet = false;
            var startingGoldSet = false;
            var startingElixirSet = false;

            var synchronizeAssetsSet = false;
            var contentUrlSet = false;
            var masterHashSet = false;

            var mysqlHostSet = false;
            var mysqlUserSet = false;
            var mysqlPassSet = false;
            var mysqlPortSet = false;


            if (!File.Exists(path))
                return false;

            using (var file = new FileStream(path, FileMode.Open))
            using (var reader = XmlReader.Create(file, settings))
            {
                try
                {
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                switch (reader.Name)
                                {
                                    case "starting_gems":
                                        if (reader.Read())
                                        {
                                            StartingGems = reader.ReadContentAsInt();
                                            startingGemsSet = true;
                                        }
                                        break;

                                    case "starting_gold":
                                        if (reader.Read())
                                        {
                                            StartingGold = reader.ReadContentAsInt();
                                            startingGoldSet = true;
                                        }
                                        break;

                                    case "starting_elixir":
                                        if (reader.Read())
                                        {
                                            StartingElixir = reader.ReadContentAsInt();
                                            startingElixirSet = true;
                                        }
                                        break;

                                    case "synchronize_assets":
                                        if (reader.Read())
                                        {
                                            SynchronizeAssets = reader.ReadContentAsBoolean();
                                            synchronizeAssetsSet = true;
                                        }
                                        break;

                                    case "content_url":
                                        if (reader.Read())
                                        {
                                            ContentUrl = reader.ReadContentAsString();
                                            contentUrlSet = true;
                                        }
                                        break;

                                    case "master_hash":
                                        if (reader.Read())
                                        {
                                            MasterHash = reader.ReadContentAsString();
                                            masterHashSet = true;
                                        }
                                        break;

                                    case "mysql_host":
                                        if (reader.Read())
                                        {
                                            MySqlHost = reader.ReadContentAsString();
                                            mysqlHostSet = true;
                                        }
                                        break;

                                    case "mysql_user":
                                        if (reader.Read())
                                        {
                                            MySqlUser = reader.ReadContentAsString();
                                            mysqlUserSet = true;
                                        }
                                        break;

                                    case "mysql_pwd":
                                        if (reader.Read())
                                        {
                                            MySqlPassword = reader.ReadContentAsString();
                                            mysqlPassSet = true;
                                        }
                                        break;

                                    case "mysql_port":
                                        if (reader.Read())
                                        {
                                            MySqlPort = reader.ReadContentAsInt();
                                            mysqlPortSet = true;
                                        }
                                        break;

                                    case "server_config":
                                        break;

                                    default:
                                        var name = reader.Name;
                                        if (reader.Read())
                                            _configs.Add(name, reader.ReadContentAsString());
                                        break;
                                }
                                break;
                        }
                    }
                }
                catch (XmlException)
                {
                    return false;
                }
            }

            // If some configs are missing we
            // rewrite a new .xml config file with the missing configs
            // as the default value.
            if (!startingGemsSet || !startingGoldSet || !startingElixirSet
               || !contentUrlSet || !masterHashSet || !mysqlHostSet
               || !mysqlUserSet || !mysqlPassSet || !mysqlPortSet || !synchronizeAssetsSet)
                return false;

            return true;
        }

        public void Save(string path)
        {
            var settings = new XmlWriterSettings()
            {
                Indent = true,
            };

            using (var file = new FileStream(path, FileMode.OpenOrCreate))
            using (var writer = XmlWriter.Create(file, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("server_config");

                writer.WriteElementString("starting_gems", StartingGems.ToString());
                writer.WriteElementString("starting_gold", StartingGold.ToString());
                writer.WriteElementString("starting_elixir", StartingElixir.ToString());

                writer.WriteElementString("synchronize_assets", SynchronizeAssets.ToString().ToLower());
                writer.WriteElementString("content_url", ContentUrl);
                writer.WriteElementString("master_hash", MasterHash);

                writer.WriteElementString("mysql_host", MySqlHost);
                writer.WriteElementString("mysql_user", MySqlUser);
                writer.WriteElementString("mysql_pwd", MySqlPassword);
                writer.WriteElementString("mysql_port", MySqlPort.ToString());

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
