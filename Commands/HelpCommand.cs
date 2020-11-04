using System;
using System.Collections.Generic;
using System.Linq;

namespace Climp.Commands
{
    public class HelpCommand : Command
    {
        private readonly IEnumerable<Command> _commands;

        public HelpCommand(IEnumerable<Command> commands)
            => _commands = Enumerable.Repeat(this, 1).Concat(commands).ToList();

        public override IReadOnlyList<string> Names { get; } = new[] { "help", "h" };

        public override bool RequiresConfig => false;

        protected internal override string Summary => "Displays this information.";

        protected internal override void Execute(State state, IReadOnlyList<string> arguments)
        {
            if (arguments.Any())
            {
                var commandName = arguments.First();
                var command = _commands.SingleOrDefault(command => string.Equals(commandName, command.Names.Contains(commandName, StringComparer.OrdinalIgnoreCase)));
                if (command is null)
                    state.Output.WriteLine($"Unknown '{commandName}' command.");
                else
                {
                    state.Output.WriteLine(command.Summary);
                    state.Output.WriteLine(string.Empty);
                }
            }
            else
                foreach (var command in _commands)
                {
                    state.Output.WriteLine(string.Join(", ", command.Names));
                    state.Output.Write(string.Empty);
                    state.Output.WriteLine(command.Summary);
                    state.Output.Write(string.Empty);
                }
        }
    }
}
