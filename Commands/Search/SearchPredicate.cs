using TagLib;

namespace Climp.Commands.Search
{
    public abstract class SearchPredicate
    {
        public abstract int GetRank(File mediaFile);

        public abstract int GetRank(MediaFile mediaFile);
    }
}