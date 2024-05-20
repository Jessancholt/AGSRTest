using Test.DataAccess.Entities;

namespace Test.Core.Models
{
    public sealed class NameContext
    {
        public Guid Guid { get; }
        public string Use { get; }
        public string Family { get; }
        public List<string> Given { get; }

        public NameContext(Guid guid, string use, string family, List<Given> given)
        {
            ArgumentNullException.ThrowIfNull(family);

            Guid = guid;
            Use = use;
            Family = family;
            Given = given?.Select(x => x.Name).ToList();
        }
    }
}
