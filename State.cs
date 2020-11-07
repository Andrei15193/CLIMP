using System.Diagnostics;
using System.IO;

namespace Climp
{
    public class State
    {
        public State(TextWriter output, TextWriter error)
            => (Output, Error) = (output, error);

        public TextWriter Output { get; }

        public TextWriter Error { get; }

        public bool ShouldExit { get; set; }

        public Process VlcProcess { get; set; }
    }
}
