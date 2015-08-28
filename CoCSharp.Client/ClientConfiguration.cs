using System;
using System.IO;
using System.Xml;

namespace CoCSharp.Client
{
    public class ClientConfiguration
    {
        public ClientConfiguration()
        {
            // Space
        }

        public long UserID { get; set; }
        public string UserToken { get; set; }

        private static readonly ClientConfiguration _DefaultConfiguration = new ClientConfiguration()
        {
            UserID = 0,
            UserToken = null,
        };
        public static ClientConfiguration DefaultConfiguration
        {
            get
            {
                return _DefaultConfiguration;
            }
        }

        public static ClientConfiguration LoadConfiguration(string configFilePath)
        {
            if (File.Exists(configFilePath))
            {
                var config = new ClientConfiguration();
                var readerSettings = new XmlReaderSettings()
                {
                    IgnoreComments = true,
                    IgnoreWhitespace = true
                };

                using (var reader = XmlReader.Create(configFilePath, readerSettings))
                {
                    var lastElement = reader.Name;
                    while (reader.Read())
                    {
                    ReadXmlElements:
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            lastElement = reader.Name;
                            switch (lastElement)
                            {
                                case "id":
                                    config.UserID = reader.ReadElementContentAsLong();
                                    goto ReadXmlElements;

                                case "token":
                                    var token = reader.ReadElementContentAsString();
                                    config.UserToken = token == "null" ? null : token;
                                    goto ReadXmlElements;
                            }
                        }
                    }
                }
                return config;
            }

            var writerSettings = new XmlWriterSettings()
            {
                Indent = true,
            };

            using (var writer = XmlWriter.Create(configFilePath, writerSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("clientConfiguration");

                writer.WriteElementString("id", _DefaultConfiguration.UserID.ToString());
                writer.WriteComment("The user ID that will be used for login in.");

                var token = _DefaultConfiguration.UserToken == null ? "null" : _DefaultConfiguration.UserToken;
                writer.WriteElementString("token", token);
                writer.WriteComment("The user token that will be used for login in.");

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            return _DefaultConfiguration;
        }
    }
}
