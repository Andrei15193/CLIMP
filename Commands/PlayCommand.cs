using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Climp.Commands.Search;

namespace Climp.Commands
{
    public class PlayCommand : Command
    {
        private readonly Config _config;
        private readonly State _state;
        private readonly MediaIndex _mediaIndex;

        public PlayCommand(Config config, State state, MediaIndex mediaIndex)
            => (_config, _state, _mediaIndex) = (config, state, mediaIndex);

        public override IReadOnlyList<string> Names { get; } = new[] { "play", "p" };

        public override bool RequiresConfig => true;

        public override string Summary => "Plays a song matching the search criteria";

        public override IEnumerable<string> Details => new[]
        {
            Summary,
            string.Empty,
            "Specify multiple arguments to narrow down the search, the song that best matches the criteria is played."
        };

        public override void Execute(Context context, IReadOnlyList<string> arguments)
        {
            var matchedMediaFile = _mediaIndex.SearchForFiles(new ArgumentsSearchPredicate(arguments)).FirstOrDefault();
            if (matchedMediaFile is null)
                context.Output.WriteLine("No media file was found matching the given arguments.");
            else
            {
                if (_state.VlcProcess != null && !_state.VlcProcess.HasExited)
                    _state.VlcProcess.Kill(true);
                _state.VlcProcess = Process.Start(_config.VlcExecutablePath.FullName, $"--intf dummy --vout dummy --dummy-quiet --no-repeat --play-and-exit \"{matchedMediaFile.File.FullName}\"");
                _state.Save();
                context.Output.WriteLine($"Playing '{matchedMediaFile.Title}' by {string.Join(", ", matchedMediaFile.Artists)}");
            }
        }
    }
}