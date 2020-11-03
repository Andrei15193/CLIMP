using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Clip.Commands.Search
{
    public class ArgumentsSearchPredicate : SearchPredicate
    {
        private readonly Regex _searchRegex;

        public ArgumentsSearchPredicate(IReadOnlyList<string> arguments)
            => _searchRegex = new Regex($@"(^|\W)({string.Join('|', arguments.Select(Regex.Escape))})(\W|$)", RegexOptions.IgnoreCase);

        public override int GetRank(FileInfo mediaFile)
        {
            var match = _searchRegex.Match(mediaFile.Name);
            var rank = 0;
            while (match.Success)
            {
                rank++;
                match = match.NextMatch();
            }
            return rank;
        }
    }
}