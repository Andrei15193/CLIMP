using System.Globalization;
using Climp.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Climp
{
    public class ClimpJsonSerializer : JsonSerializer
    {
        public ClimpJsonSerializer()
        {
            Converters.Add(new DirectoryInfoJsonConverter());
            Converters.Add(new FileInfoJsonConverter());
            Converters.Add(new ProcessConverter());
            Culture = CultureInfo.InvariantCulture;
            DateFormatHandling = DateFormatHandling.IsoDateFormat;
            DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            Formatting = Formatting.Indented;
            NullValueHandling = NullValueHandling.Ignore;
            ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}