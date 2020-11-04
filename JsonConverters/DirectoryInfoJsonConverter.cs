using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Climp.JsonConverters
{
    public class DirectoryInfoJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IEnumerable<DirectoryInfo> directoryInfos)
                serializer.Serialize(writer, directoryInfos.Select(directoryInfo => directoryInfo.FullName));
            else
                writer.WriteValue(((DirectoryInfo)value).FullName);

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
                return serializer.Deserialize<string[]>(reader)
                    .Select(directoryFullName => new DirectoryInfo(directoryFullName))
                    .ToArray();
            else
                return new DirectoryInfo((string)reader.Value);
        }

        public override bool CanConvert(Type objectType)
            => objectType == typeof(DirectoryInfo) || typeof(IEnumerable<DirectoryInfo>).IsAssignableFrom(objectType);
    }
}