using CoCSharp.Server.API;
using System.IO;
using System.Xml;

namespace CoCSharp.Server
{
    public class ServerConfiguration : IServerConfiguration
    {
        public ServerConfiguration()
        {
            StartingGems = _defaultGems;
            StartingGold = _defaultGold;
            StartingElixir = _defaultElixir;
            StartingVillage = _defaultVillage;
        }

        private static readonly string _defaultVillage = File.ReadAllText("contents/starting_village.json");
        private static readonly int _defaultGems = (int)1e7;
        private static readonly int _defaultGold = (int)1e8;
        private static readonly int _defaultElixir = (int)1e8;

        public int StartingGems { get; set; }
        public int StartingGold { get; set; }
        public int StartingElixir { get; set; }
        public string StartingVillage { get; set; }

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
            if (!startingGemsSet || !startingGoldSet || !startingElixirSet)
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

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
