using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Climp.JsonConverters;
using Newtonsoft.Json;

namespace Climp
{
    public sealed class Config : INotifyPropertyChanged
    {
        public static Config FromBoundFile(FileInfo configFile)
        {
            Config config;
            if (configFile.Exists)
            {
                using var configFileStream = new FileStream(configFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var configFileStreamReader = new StreamReader(configFileStream);

                config = JsonConvert.DeserializeObject<Config>(configFileStreamReader.ReadToEnd());
            }
            else
                config = new Config();

            config.PropertyChanged += (sender, e) =>
            {
                using var configFileStream = new FileStream(configFile.FullName, FileMode.Create, FileAccess.Write, FileShare.Read);
                using var configFileStreamWriter = new StreamWriter(configFileStream);
                configFileStreamWriter.Write(JsonConvert.SerializeObject(sender, Formatting.Indented));
            };

            return config;
        }

        private readonly IDictionary<string, object> _configs = new Dictionary<string, object>();

        public event PropertyChangedEventHandler PropertyChanged;

        [JsonConverter(typeof(FileInfoJsonConverter))]
        public FileInfo VlcExecutablePath { get => GetValue<FileInfo>(); set => SetValue(value); }

        [JsonConverter(typeof(DirectoryInfoJsonConverter))]
        public IReadOnlyList<DirectoryInfo> MediaDirectories { get => GetValue<IReadOnlyList<DirectoryInfo>>(); set => SetValue(value); }

        private TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
            => _configs.TryGetValue(propertyName, out var value) ? (TValue)value : default;

        private void SetValue<TValue>(TValue value, [CallerMemberName] string propertyName = null)
            => _configs[propertyName] = value;

        private void _NotifyPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
