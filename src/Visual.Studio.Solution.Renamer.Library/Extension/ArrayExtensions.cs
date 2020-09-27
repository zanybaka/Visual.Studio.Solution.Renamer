using System;

namespace Visual.Studio.Solution.Renamer.Library.Extension
{
    public static class ArrayExtensions
    {
        public static void AssertAtLeastOneElement<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(array));
            }
        }
    }
}