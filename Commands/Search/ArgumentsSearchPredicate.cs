using System;
using System.Collections.Generic;
using System.Linq;

namespace Climp.Commands.Search
{
    public class ArgumentsSearchPredicate : SearchPredicate
    {
        private readonly IReadOnlyList<string> _arguments;

        public ArgumentsSearchPredicate(IReadOnlyList<string> arguments)
            => _arguments = arguments;

        public override int GetRank(MediaFile mediaFile)
        {
            var matchedArguments = _arguments.Where(argument => mediaFile.Title.Contains(argument, StringComparison.OrdinalIgnoreCase)).ToArray();
            if (matchedArguments.Any())
                return matchedArguments.Count()
                    + _arguments
                        .Except(matchedArguments)
                        .Count(remainingArgument => mediaFile.Artists.Any(albumArtist => albumArtist.Contains(remainingArgument, StringComparison.OrdinalIgnoreCase)));
            else
                return 0;
        }
    }
}