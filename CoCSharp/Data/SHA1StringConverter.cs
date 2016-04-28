using System;
using Newtonsoft.Json;
using System.Globalization;

namespace CoCSharp.Data
{
    // Newtonsoft.Json converter to convert byte array stings into byte[] mostly for fingerprint's SHA1 strings.
    internal class SHA1StringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(byte[]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = (string)reader.Value;
            if (value == null || value.Length != 40)
                return null;

            var bytes = new byte[value.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = byte.Parse(value.Substring(i * 2, 2), NumberStyles.HexNumber);
            return bytes;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var byteArray = (byte[])value;

            if (byteArray == null || byteArray.Length != 20)
            {
                writer.WriteNull();
                return;
            }

            var byteString = string.Empty;
            for (int i = 0; i < byteArray.Length; i++)
                byteString += byteArray[i].ToString("x2");

            writer.WriteRawValue("\"" + byteString + "\"");
        }
    }
}
