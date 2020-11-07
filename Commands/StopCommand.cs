
using System.Collections.Generic;

namespace Climp.Commands
{
    public class StopCommand : Command
    {
        public override IReadOnlyList<string> Names { get; } = new[] { "stop", "s" };

        public override bool RequiresConfig => false;

        public override string Summary => "Stops the currently playing song";

        public override IEnumerable<string> Details => new[]
        {
            Summary
        };

        public override void Execute(Context context,State state, IReadOnlyList<string> arguments)
        {
            var vlcProcess = state.VlcProcess;
            if (vlcProcess is null || state.VlcProcess.HasExited)
                context.Output.WriteLine("No song is currently playing");
            else
                state.VlcProcess.Kill(true);
        }
    }
}