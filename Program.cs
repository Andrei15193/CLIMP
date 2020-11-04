using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                .SelectMany(command => command.Names.Select(name => new { Name = name, Command = command }))
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
                        var commandLineParts = ReadCommandArguments(commandLine);
                        var commandName = commandLineParts[0];
                        if (commands.TryGetValue(commandName, out var command))
                            if (command.RequiresConfig && !state.Config.IsConfigured)
                                Console.Error.WriteLine($"Command '{commandName}' requires config, run config command for setup");
                            else
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

        private static IReadOnlyList<string> ReadCommandArguments(string inputLine)
        {
            var arguments = new List<string>();
            char? quotedChar = null;
            var argumentBuilder = new StringBuilder();
            foreach (var @char in inputLine)
            {
                if (quotedChar != null)
                    if (@char == quotedChar)
                        quotedChar = null;
                    else
                        argumentBuilder.Append(@char);
                else if (@char == '"' || @char == '\'')
                    quotedChar = @char;
                else if (@char == ' ' && argumentBuilder.Length > 0)
                {
                    arguments.Add(argumentBuilder.ToString());
                    argumentBuilder.Clear();
                }
                else
                    argumentBuilder.Append(@char);
            }
            if (argumentBuilder.Length > 0)
                arguments.Add(argumentBuilder.ToString());

            return arguments;
        }

        private static IEnumerable<Command> _GetCommands()
        {
            var commands = new Command[]
            {
                new ConfigCommand(),
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
