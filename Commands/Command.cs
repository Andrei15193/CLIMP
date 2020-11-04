using System.Collections.Generic;

namespace Climp.Commands
{
    public abstract class Command
    {
        protected Command()
        {
        }

        public abstract IReadOnlyList<string> Names { get; }

        public abstract bool RequiresConfig { get; }

        protected internal abstract string Summary { get; }

        protected internal abstract void Execute(State state, IReadOnlyList<string> arguments);
    }
}
