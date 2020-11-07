using System.Diagnostics;
using System.IO;
using Climp.JsonConverters;
using Newtonsoft.Json;

namespace Climp
{
    public class State
    {
        private readonly FileInfo _stateFileInfo;

        public State(FileInfo stateFileInfo)
            => _stateFileInfo = stateFileInfo;

        public Process VlcProcess { get; set; }

        public void Load()
        {
            if (_stateFileInfo.Exists)
                using (var stateFileStream = new FileStream(_stateFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var stateFileStreamReader = new StreamReader(stateFileStream))
                    _JsonSerializer().Populate(stateFileStreamReader, this);
        }

        public void Save()
        {
            using (var stateFileStream = new FileStream(_stateFileInfo.FullName, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (var stateFileStreamWriter = new StreamWriter(stateFileStream))
                _JsonSerializer().Serialize(stateFileStreamWriter, this);
        }

        private static JsonSerializer _JsonSerializer()
            => new JsonSerializer
            {
                Converters = { new ProcessConverter() },
                Formatting = Formatting.Indented
            };
    }
}