using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Climp.Commands;
using Newtonsoft.Json;

namespace Climp
{
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Welcome to CLIMP (Command Line Interface Media Player), to get started type help for a list of commands.");
            Console.WriteLine();

            var commands = _GetCommands()
                .SelectMany(command => Enumerable.Repeat(command.Name, 1).Concat(command.Aliases).Select(name => new { Name = name, Command = command }))
                .ToDictionary(pair => pair.Name, pair => pair.Command, StringComparer.OrdinalIgnoreCase);

            var state = new State(
                Config.FromBoundFile(new FileInfo(Path.Combine(Environment.GetEnvironmentVariable("userProfile"), ".climp"))),
                Console.Out,
                Console.Error
            );
            do
                try
                {
                    Console.Write("> ");
                    var commandLine = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(commandLine))
                    {
                        var commandLineParts = commandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        var commandName = commandLineParts[0];
                        if (commands.TryGetValue(commandName, out var command))
                            command.Execute(state, commandLineParts.Skip(1).ToArray());
                        else
                            Console.Error.WriteLine($"Unknown '{commandName}' command.");
                    }
                }
                catch (ArgumentException argumentException)
                {
                    Console.Error.WriteLine(argumentException.Message);
                }
            while (!state.ShouldExit);
        }

        private static IEnumerable<Command> _GetCommands()
        {
            var commands = new Command[]
            {
                new ExitCommand(),
                new PlayCommand(),
                new StopCommand()
            };

            yield return new HelpCommand(commands);
            foreach (var command in commands)
                yield return command;
        }
    }
}
