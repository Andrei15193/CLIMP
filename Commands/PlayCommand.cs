using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Climp.Commands.Search;

namespace Climp.Commands
{
    public class PlayCommand : Command
    {
        private readonly Config _config;

        public PlayCommand(Config config)
            => _config = config;

        public override IReadOnlyList<string> Names { get; } = new[] { "play", "p" };

        public override bool RequiresConfig => true;

        public override string Summary => "Plays a song matching the search criteria";

        public override IEnumerable<string> Details => new[]
        {
            Summary,
            string.Empty,
            "Specify multiple arguments to narrow down the search, the song that best matches the criteria is played."
        };

        public override void Execute(State state, IReadOnlyList<string> arguments)
        {
            var searchPredicate = new ArgumentsSearchPredicate(arguments);
            var matchedMediaFile = _SearchMediaFiles(_config.MediaDirectories, searchPredicate).FirstOrDefault();
            if (matchedMediaFile is null || searchPredicate.GetRank(matchedMediaFile) == 0)
                state.Output.WriteLine("No media file was found matching the given arguments.");
            else
            {
                if (state.VlcProcess != null && !state.VlcProcess.HasExited)
                    state.VlcProcess.Kill(true);
                state.VlcProcess = Process.Start(_config.VlcExecutablePath.FullName, $"--intf dummy --vout dummy --dummy-quiet --no-repeat --play-and-exit \"{matchedMediaFile.Name}\"");
                state.VlcProcess.Exited += delegate { state.VlcProcess = null; };
                state.Output.WriteLine($"Playing '{matchedMediaFile.Tag.Title}' by {string.Join(", ", matchedMediaFile.Tag.AlbumArtists)}");
            }
        }

        private IEnumerable<TagLib.File> _SearchMediaFiles(IEnumerable<DirectoryInfo> mediaDirectories, SearchPredicate searchPredicate)
            => Task
                .WhenAll(
                    mediaDirectories
                        .SelectMany(mediaDirectory => mediaDirectory.GetFiles("*.m4a", SearchOption.AllDirectories))
                        .Select(mediaFileInfo => Task.Run(() => TagLib.File.Create(mediaFileInfo.FullName, TagLib.ReadStyle.PictureLazy)))
                )
                .Result
                .OrderByDescending(searchPredicate.GetRank);
    }
}