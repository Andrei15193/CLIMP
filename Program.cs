using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Climp.Commands;

namespace Climp
{
    public static class Program
    {
        public static void Main(params string[] args)
        {
            var config = new Config(new FileInfo(Path.Combine(Environment.GetEnvironmentVariable("userProfile"), ".climp-config")));
            config.Load();
            var state = new State(new FileInfo(Path.Combine(Environment.GetEnvironmentVariable("userProfile"), ".climp-state")));
            state.Load();
            var context = new Context(Console.Out, Console.Error);

            var mediaIndex = new MediaIndex(config, new FileInfo(Path.Combine(Environment.GetEnvironmentVariable("userProfile"), ".climp-index")));
            mediaIndex.Refresh(context);

            var commands = _GetCommands(config, state, mediaIndex)
                .SelectMany(command => command.Names.Select(name => new { Name = name, Command = command }))
                .ToDictionary(pair => pair.Name, pair => pair.Command, StringComparer.OrdinalIgnoreCase);

            try
            {
                if (args.Length > 0)
                {
                    var commandName = args[0];
                    if (commands.TryGetValue(commandName, out var command))
                        if (command.RequiresConfig && !config.IsConfigured())
                            Console.Error.WriteLine($"Command '{commandName}' requires config, run config command for setup");
                        else
                            command.Execute(context, args.Skip(1).ToArray());
                    else
                        Console.Error.WriteLine($"Unknown '{commandName}' command.");
                }
                else
                    Console.WriteLine("Welcome to CLIMP (Command Line Interface Media Player), to get started type climp help for a list of commands.");
            }
            catch (ArgumentException argumentException)
            {
                Console.Error.WriteLine(argumentException.Message);
            }
        }

        private static IEnumerable<Command> _GetCommands(Config config, State state, MediaIndex mediaIndex)
        {
            var commands = new Command[]
            {
                new ConfigCommand(config),
                new PlayCommand(config, state, mediaIndex),
                new StopCommand(state)
            };

            yield return new HelpCommand(commands);
            foreach (var command in commands)
                yield return command;
        }
    }
}
