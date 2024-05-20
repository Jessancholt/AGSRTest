using Test.DataAccess.Entities.Predefined;

namespace Test.Core.Models.Predefined
{
    internal static class GenderConstants
    {
        public static readonly Dictionary<Gender, string> GendersToStrings = new Dictionary<Gender, string>()
        {
            { Gender.Male, "male" },
            { Gender.Female, "female" },
            { Gender.Other, "other" },
            { Gender.Unknown, "unknown" },
        };

        public static readonly Dictionary<string, Gender> StringToGenders = GendersToStrings.ToDictionary(x => x.Value.ToLower(), y => y.Key);
    }
}
