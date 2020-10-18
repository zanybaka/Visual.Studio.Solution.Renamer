using System;
using Visual.Studio.Solution.Renamer.Library.Entity.Project;

namespace Visual.Studio.Solution.Renamer.Library.Extension
{
    public static class StringExtensions
    {
        public static bool IsNotEmpty(this string value)
        {
            return !value.IsEmpty();
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static void AssertEndsWith(this string value, string ending)
        {
            if (!value.EndsWith(ending, ignoreCase: true, culture: null))
            {
                throw new NotSupportedException($"Mask must be ended with {ProjectConstants.ProjectExtension}. Wrong value: {value}");
            }
        }

        public static string ReplaceIgnoreCase(this string input, string oldValue, string newValue)
        {
            return input
                .Replace(oldValue, newValue, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string ReplaceOnceIgnoreCase(this string input, string oldValue, string newValue)
        {
            return input
                .Replace(newValue, "-->$:temp:$<--", StringComparison.InvariantCultureIgnoreCase)
                .Replace(oldValue, newValue, StringComparison.InvariantCultureIgnoreCase)
                .Replace("-->$:temp:$<--", newValue);
        }
    }
}