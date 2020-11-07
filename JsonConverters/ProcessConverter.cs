using System;
using System.Diagnostics;
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
            if (processInfo is null)
                return null;
            else
            {
                var process = Process.GetProcessById(processInfo.Id);
                return string.Equals(processInfo.Name, process.ProcessName, StringComparison.OrdinalIgnoreCase) ? process : null;
            }
        }

        public override bool CanConvert(Type objectType)
            => objectType == typeof(Process);
    }
}