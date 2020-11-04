using System.Collections.Generic;
using System.Linq;

namespace Climp.Commands
{
    public abstract class Command
    {
        protected Command()
        {
        }

        public abstract string Name { get; }

        public virtual IReadOnlyList<string> Aliases => Enumerable.Empty<string>() as IReadOnlyList<string> ?? new string[0];

        protected internal abstract string Summary { get; }

        protected internal abstract void Execute(State state, IReadOnlyList<string> arguments);
    }
}
