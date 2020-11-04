using System;
using System.IO;
using Newtonsoft.Json;

namespace Climp.JsonConverters
{
    public class FileInfoJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var fileInfo = (FileInfo)value;
            writer.WriteValue(fileInfo.FullName);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var fileFullName = (string)reader.Value;
            return new FileInfo(fileFullName);
        }

        public override bool CanConvert(Type objectType)
            => objectType == typeof(FileInfo);
    }
}