using System.Diagnostics;
using System.IO;

namespace Climp
{
    public class State
    {
        public State(Config config, TextWriter output, TextWriter error)
            => (Config, Output, Error) = (config, output, error);

        public Config Config { get; }

        public TextWriter Output { get; }

        public TextWriter Error { get; }

        public bool ShouldExit { get; set; }

        public Process VlcProcess { get; set; }
    }
}
