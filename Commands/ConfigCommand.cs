using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Climp.Commands
{
    public class ConfigCommand : Command
    {
        public ConfigCommand()
        {
        }

        public override IReadOnlyList<string> Names { get; } = new[] { "config" };

        public override bool RequiresConfig => false;

        protected internal override string Summary => "Configures the application. Set the VLC path using -vlcPath argument followed by the path to the VLC executable, set media directories using -mediaDirectories argument followed by the media directories.";

        protected internal override void Execute(State state, IReadOnlyList<string> arguments)
        {
            if (arguments.Count == 0)
            {
                state.Output.WriteLine($"VLC path: {state.Config.VlcExecutablePath?.FullName}");
                state.Output.WriteLine($"Media directories ({state.Config.MediaDirectories?.Count ?? 0}):");
                foreach (var mediaDirectory in state.Config.MediaDirectories ?? Enumerable.Empty<DirectoryInfo>())
                    state.Output.WriteLine($"* {mediaDirectory.FullName}");
            }
            else
            {
                FileInfo vlcPath = null;
                List<DirectoryInfo> mediaDirectories = null;
                Action<string> argumentProcessor = null;
                foreach (var argument in arguments)
                    if ("-vlcPath".Equals(argument, StringComparison.OrdinalIgnoreCase))
                        argumentProcessor = arg => { vlcPath = new FileInfo(arg); argumentProcessor = null; };
                    else if ("-mediaDirectories".Equals(argument, StringComparison.OrdinalIgnoreCase))
                    {
                        if (mediaDirectories is null)
                            mediaDirectories = new List<DirectoryInfo>();
                        argumentProcessor = arg => mediaDirectories.Add(new DirectoryInfo(arg));
                    }
                    else if (argumentProcessor != null)
                        argumentProcessor(argument);

                if (vlcPath != null)
                    state.Config.VlcExecutablePath = vlcPath;
                if (mediaDirectories != null)
                    state.Config.MediaDirectories = mediaDirectories;
            }
        }
    }
}
