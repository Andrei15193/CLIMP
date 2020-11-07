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

        public override string Summary => "Displays this information. For information about a specific command, type 'help <command name', such as help help";

        public override IEnumerable<string> Details => new[]
        {
            Summary
        };

        public override void Execute(State state, IReadOnlyList<string> arguments)
        {
            if (arguments.Any())
            {
                var commandName = arguments.First();
                var command = _commands.SingleOrDefault(command => command.Names.Contains(commandName, StringComparer.OrdinalIgnoreCase));
                if (command is null)
                    state.Output.WriteLine($"Unknown '{commandName}' command.");
                else
                {
                    foreach (var line in command.Details)
                        state.Output.WriteLine(line);
                    state.Output.WriteLine();
                }
            }
            else
                foreach (var command in _commands)
                {
                    state.Output.WriteLine(string.Join(", ", command.Names));
                    state.Output.Write("  ");
                    state.Output.WriteLine(command.Summary);
                    state.Output.WriteLine();
                }
        }
    }
}
