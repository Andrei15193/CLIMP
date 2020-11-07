using System.Collections.Generic;
using System.IO;

namespace Climp
{
    public class MediaFile
    {
        public FileInfo File { get; set; }

        public string Title { get; set; }

        public IReadOnlyList<string> Artists { get; set; }
    }
}