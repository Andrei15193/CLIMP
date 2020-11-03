
using System.Collections.Generic;

namespace Clip.Commands
{
    public class StopCommand : Command
    {
        public override string Name
            => "stop";

        public override IReadOnlyList<string> Aliases
            => new[] { "s" };

        protected internal override string Summary
            => "Stops the currently playing song";

        protected internal override void Execute(State state, IReadOnlyList<string> arguments)
        {
            var vlcProcess = state.VlcProcess;
            if (vlcProcess is null || state.VlcProcess.HasExited)
                state.Output.WriteLine("No song is currently playing");
            else
                state.VlcProcess.Kill(true);
        }
    }
}