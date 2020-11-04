using System.Collections.Generic;
using System.Linq;

namespace Climp.Commands
{
    public abstract class Command
    {
        protected Command()
        {
        }

        public abstract IReadOnlyList<string> Names { get; }

        protected internal abstract string Summary { get; }

        protected internal abstract void Execute(State state, IReadOnlyList<string> arguments);
    }
}
