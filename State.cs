using System.Diagnostics;
using System.IO;

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
                    new ClimpJsonSerializer().Populate(stateFileStreamReader, this);
        }

        public void Save()
        {
            using (var stateFileStream = new FileStream(_stateFileInfo.FullName, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (var stateFileStreamWriter = new StreamWriter(stateFileStream))
                new ClimpJsonSerializer().Serialize(stateFileStreamWriter, this);
        }
    }
}