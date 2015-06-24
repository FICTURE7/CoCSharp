using System.IO;
using System.Xml;

namespace CoCSharp
{
    public class ProxyConfiguration
    {
        public ProxyConfiguration() { }

        public bool DeleteLogOnStartup { get; set; }
        public bool LogRawPacket { get; set; }
        public bool LogPrivatePacketFields { get; set; }
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public int ProxyPort { get; set; }

        public static ProxyConfiguration DefaultConfiguration
        {
            get
            {
                return new ProxyConfiguration()
                {
                    DeleteLogOnStartup = true,
                    LogRawPacket = true,
                    LogPrivatePacketFields = false,
                    ServerAddress = "gamea.clashofclans.com",
                    ServerPort = 9339,
                    ProxyPort = 9339
                };
            }
        }

        public static ProxyConfiguration LoadConfiguration(string configFileName)
        {
            var config = ProxyConfiguration.DefaultConfiguration;

            if (File.Exists(configFileName))
            {
                var readerSettings = new XmlReaderSettings()
                {
                    IgnoreComments = true,
                    IgnoreWhitespace = true
                };

                using (var reader = XmlReader.Create(configFileName, readerSettings))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            switch (reader.Name)
                            {
                                case "deleteLogOnStartup":
                                    config.DeleteLogOnStartup = reader.ReadElementContentAsBoolean();
                                    break;

                                case "logRawPacket":
                                    config.LogRawPacket = reader.ReadElementContentAsBoolean();
                                    break;

                                case "logPrivatePacketFields":
                                    config.LogPrivatePacketFields = reader.ReadElementContentAsBoolean();
                                    break;

                                case "serverAddress":
                                    config.ServerAddress = reader.ReadElementContentAsString();
                                    break;

                                case "serverPort":
                                    config.ServerPort = reader.ReadElementContentAsInt();
                                    break;

                                case "proxyPort":
                                    config.ProxyPort = reader.ReadElementContentAsInt();
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                var writerSettings = new XmlWriterSettings()
                {
                    Indent = true,
                };

                using (var writer = XmlWriter.Create(configFileName, writerSettings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("proxyConfiguration");

                    writer.WriteElementString("deleteLogOnStartup", config.DeleteLogOnStartup.ToString().ToLower());
                    writer.WriteComment("Will delete packets.log file on startup.");

                    writer.WriteElementString("logRawPacket", config.LogRawPacket.ToString().ToLower());
                    writer.WriteComment("Will log raw decrypted packet bytes in packet_dump folder.");

                    writer.WriteElementString("logPrivatePacketFields", config.LogPrivatePacketFields.ToString().ToLower());
                    writer.WriteComment("Will log private fields.");

                    writer.WriteElementString("serverAddress", config.ServerAddress.ToString().ToLower());
                    writer.WriteComment("The real Clash of Clan server address.");

                    writer.WriteElementString("serverPort", config.ServerPort.ToString().ToLower());
                    writer.WriteComment("The real Clash of Clan server port.");

                    writer.WriteElementString("proxyPort", config.ServerPort.ToString().ToLower());
                    writer.WriteComment("Port on which the proxy will run.");

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }

            return config;
        }
    }
}
