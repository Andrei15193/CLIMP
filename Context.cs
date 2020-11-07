using System.IO;

namespace Climp
{
    public class Context
    {
        public Context(TextWriter output, TextWriter error)
            => (Output, Error) = (output, error);

        public TextWriter Output { get; }

        public TextWriter Error { get; }
    }
}