using System.IO;

namespace Clip.Commands.Search
{
    public abstract class SearchPredicate
    {
        public abstract int GetRank(FileInfo mediaFile);
    }
}