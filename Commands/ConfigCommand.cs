using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Climp.Commands
{
    public class ConfigCommand : Command
    {
        private readonly Config _config;

        public ConfigCommand(Config config)
            => _config = config;

        public override IReadOnlyList<string> Names { get; } = new[] { "config" };

        public override bool RequiresConfig => false;

        protected internal override string Summary => "Configures the application. Set the VLC path using -vlcPath argument followed by the path to the VLC executable, set media directories using -mediaDirectories argument followed by the media directories.";

        protected internal override void Execute(State state, IReadOnlyList<string> arguments)
        {
            if (arguments.Count == 0)
            {
                state.Output.WriteLine($"VLC path: {_config.VlcExecutablePath?.FullName}");
                state.Output.WriteLine($"Media directories ({_config.MediaDirectories?.Count ?? 0}):");
                foreach (var mediaDirectory in _config.MediaDirectories ?? Enumerable.Empty<DirectoryInfo>())
                    state.Output.WriteLine($"* {mediaDirectory.FullName}");
            }
            else
            {
                FileInfo vlcPath = null;
                List<DirectoryInfo> mediaDirectories = null;
                Action<string> argumentProcessor = null;
                foreach (var argument in arguments)
                    if ("--vlc-path".Equals(argument, StringComparison.OrdinalIgnoreCase))
                        argumentProcessor = arg => { vlcPath = new FileInfo(arg); argumentProcessor = null; };
                    else if ("--media-directories".Equals(argument, StringComparison.OrdinalIgnoreCase))
                    {
                        if (mediaDirectories is null)
                            mediaDirectories = new List<DirectoryInfo>();
                        argumentProcessor = arg => mediaDirectories.Add(new DirectoryInfo(arg));
                    }
                    else if (argumentProcessor != null)
                        argumentProcessor(argument);

                if (vlcPath != null)
                    _config.VlcExecutablePath = vlcPath;
                if (mediaDirectories != null)
                    _config.MediaDirectories = mediaDirectories;
                _config.Save();
            }
        }
    }
}
