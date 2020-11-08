using System;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Climp.JsonConverters
{
    public class ProcessConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var process = (Process)value;
            serializer.Serialize(writer, new ProcessInfo { Id = process.Id, Name = process.ProcessName });
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var processInfo = serializer.Deserialize<ProcessInfo>(reader);
            return processInfo is null ? null : Process.GetProcessesByName(processInfo.Name).FirstOrDefault(process => process.Id == processInfo.Id);
        }

        public override bool CanConvert(Type objectType)
            => objectType == typeof(Process);
    }
}