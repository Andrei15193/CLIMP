using System.IO;

namespace Climp.Commands.Search
{
    public abstract class SearchPredicate
    {
        public abstract int GetRank(FileInfo mediaFile);
    }
}