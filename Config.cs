using System.Collections.Generic;
using System.IO;
using Climp.JsonConverters;
using Newtonsoft.Json;

namespace Climp
{
    public sealed class Config
    {
        private readonly FileInfo _configFile;

        public Config(FileInfo configFile)
            => _configFile = configFile;

        public FileInfo VlcExecutablePath { get; set; }

        public IReadOnlyList<DirectoryInfo> MediaDirectories { get; set; }

        public bool IsConfigured()
            => (VlcExecutablePath != null && MediaDirectories != null);

        public void Load()
        {
            if (_configFile.Exists)
                using (var configFileStream = new FileStream(_configFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var configFileStreamReader = new StreamReader(configFileStream))
                    JsonConvert.PopulateObject(configFileStreamReader.ReadToEnd(), this, new JsonSerializerSettings { Converters = { new FileInfoJsonConverter(), new DirectoryInfoJsonConverter() } });
        }

        public void Save()
        {
            using (var configFileStream = new FileStream(_configFile.FullName, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (var configFileStreamWriter = new StreamWriter(configFileStream))
                configFileStreamWriter.Write(JsonConvert.SerializeObject(this, Formatting.Indented, new FileInfoJsonConverter(), new DirectoryInfoJsonConverter()));
        }
    }
}
