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

        public abstract string Summary { get; }

        public abstract IEnumerable<string> Details { get; }

        public abstract void Execute(Context context, State state, IReadOnlyList<string> arguments);
    }
}
