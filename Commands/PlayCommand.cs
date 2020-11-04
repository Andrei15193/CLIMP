using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Climp.Commands.Search;

namespace Climp.Commands
{
    public class PlayCommand : Command
    {
        public override IReadOnlyList<string> Names { get; } = new[] { "play", "p" };

        public override bool RequiresConfig => true;

        protected internal override string Summary => "Plays a song matching the search criteria";

        protected internal override void Execute(State state, IReadOnlyList<string> arguments)
        {
            var matchedMediaFile = _SearchMediaFiles(state.Config.MediaDirectories, new ArgumentsSearchPredicate(arguments)).FirstOrDefault();
            if (matchedMediaFile is null)
                state.Output.WriteLine("No media file was found matching the given arguments.");
            else
            {
                if (state.VlcProcess != null && !state.VlcProcess.HasExited)
                    state.VlcProcess.Kill(true);
                state.VlcProcess = Process.Start(state.Config.VlcExecutablePath.FullName, $"--intf dummy --vout dummy --dummy-quiet --no-repeat --play-and-exit \"{matchedMediaFile.FullName}\"");
                state.VlcProcess.Exited += delegate { state.VlcProcess = null; };
            }
        }

        private IEnumerable<FileInfo> _SearchMediaFiles(IEnumerable<DirectoryInfo> mediaDirectories, SearchPredicate searchPredicate)
            => mediaDirectories
                .SelectMany(mediaDirectory => mediaDirectory.EnumerateFiles("*.m4a", SearchOption.AllDirectories))
                .AsParallel()
                .OrderByDescending(mediaFile => searchPredicate.GetRank(mediaFile));
    }
}