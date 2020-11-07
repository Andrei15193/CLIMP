
using System.Collections.Generic;

namespace Climp.Commands
{
    public class StopCommand : Command
    {
        private readonly State _state;

        public StopCommand(State state)
            => _state = state;

        public override IReadOnlyList<string> Names { get; } = new[] { "stop", "s" };

        public override bool RequiresConfig => false;

        public override string Summary => "Stops the currently playing song";

        public override IEnumerable<string> Details => new[]
        {
            Summary
        };

        public override void Execute(Context context, IReadOnlyList<string> arguments)
        {
            var vlcProcess = _state.VlcProcess;
            if (vlcProcess is null || _state.VlcProcess.HasExited)
                context.Output.WriteLine("No song is currently playing");
            else
            {
                _state.VlcProcess.Kill(true);
                _state.VlcProcess = null;
                _state.Save();
            }
        }
    }
}