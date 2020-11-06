using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TagLib;

namespace Climp.Commands.Search
{
    public class ArgumentsSearchPredicate : SearchPredicate
    {
        private readonly IReadOnlyList<string> _arguments;

        public ArgumentsSearchPredicate(IReadOnlyList<string> arguments)
            => _arguments = arguments;

        public override int GetRank(File mediaFile)
        {
            var matchedArguments = _arguments.Where(argument => mediaFile.Tag.Title.Contains(argument, StringComparison.OrdinalIgnoreCase)).ToArray();
            if (matchedArguments.Any())
                return matchedArguments.Count()
                    + _arguments
                        .Except(matchedArguments)
                        .Count(remainingArgument => mediaFile.Tag.AlbumArtists.Any(albumArtist => albumArtist.Contains(remainingArgument, StringComparison.OrdinalIgnoreCase)));
            else
                return 0;
        }
    }
}