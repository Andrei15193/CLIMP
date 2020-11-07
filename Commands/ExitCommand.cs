using System.Collections.Generic;

namespace Climp.Commands
{
    public class ExitCommand : Command
    {
        public ExitCommand()
        {
        }

        public override IReadOnlyList<string> Names { get; } = new[] { "exit", "close" };

        public override bool RequiresConfig => false;

        public override string Summary => "Closes the application, type this if you no longer wish to listen to music *sad face*";

        public override IEnumerable<string> Details => new[]
        {
            Summary
        };

        public override void Execute(Context context, State state, IReadOnlyList<string> arguments)
        {
            if (state.VlcProcess != null && !state.VlcProcess.HasExited)
                state.VlcProcess.Kill(true);
            state.ShouldExit = true;
        }
    }
}
