using System.IO;
using System.Xml;

namespace CoCSharp.Server
{
    public class CoCServerConfiguration
    {
        internal CoCServerConfiguration()
        {
            // Space
        }

        public CoCServerConfiguration(string path)
        {
            if (File.Exists(path))
            {
                Load(path);
            }
            else
            {
                StartingGems = _internalDefault.StartingGems;
                StartingGold = _internalDefault.StartingGold;
                StartingElixir = _internalDefault.StartingElixir;

                Create(path);
            }
        }

        private static CoCServerConfiguration _internalDefault = new CoCServerConfiguration()
        {
            StartingGems = 69696969,
            StartingGold = 69696969,
            StartingElixir = 69696969
        };

        public static CoCServerConfiguration Default
        {
            get
            {
                return new CoCServerConfiguration("config.xml")
                {
                    StartingGems = _internalDefault.StartingGems,
                    StartingGold = _internalDefault.StartingGold,
                    StartingElixir = _internalDefault.StartingElixir
                };
            }
        }

        public int StartingGems { get; set; }

        public int StartingGold { get; set; }

        public int StartingElixir { get; set; }

        private void Load(string path)
        {
            var settings = new XmlReaderSettings()
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
            };

            var startingGemsSet = false;
            var startingGoldSet = false;
            var startingElixirSet = false;

            using (var file = new FileStream(path, FileMode.Open))
            using (var reader = XmlReader.Create(file, settings))
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

            // If some configs are missing we
            // rewrite a new .xml config file with the missing configs
            // as the default value.
            var defaultConfig = _internalDefault;
            if (!startingGemsSet || !startingGoldSet || !startingElixirSet)
            {
                if (!startingGemsSet)
                {
                    StartingGems = defaultConfig.StartingGems;
                }
                if (!startingGoldSet)
                {
                    StartingGold = defaultConfig.StartingElixir;
                }
                if (!startingElixirSet)
                {
                    StartingElixir = defaultConfig.StartingElixir;
                }

                Create(path);
            }
        }

        private void Create(string path)
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
