using System.Collections.Generic;
using System.IO;

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
                    new ClimpJsonSerializer().Populate(configFileStreamReader, this);
        }

        public void Save()
        {
            using (var configFileStream = new FileStream(_configFile.FullName, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (var configFileStreamWriter = new StreamWriter(configFileStream))
                new ClimpJsonSerializer().Serialize(configFileStreamWriter, this);
        }
    }
}
