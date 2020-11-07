using System.Diagnostics;
using System.IO;

namespace Climp
{
    public class State
    {
        public bool ShouldExit { get; set; }

        public Process VlcProcess { get; set; }
    }
}