using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryBuilder.Helpers
{
    public static class ThrowHelper
    {
        public static void ThrowIfNullOrEmpty<T>(IEnumerable<T> enumerable, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(argumentName))
                throw new ArgumentException(nameof(argumentName));

            if (enumerable == null || !enumerable.Any())
                throw new ArgumentException($"The parameter \"{argumentName}\" must contains at least one item.");
        }

        public static void ThrowIfNullOrWhiteSpace(string value, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(argumentName))
                throw new ArgumentException(nameof(argumentName));

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"The parameter \"{argumentName}\" cannot be null or a white space.");
        }
    }
}
