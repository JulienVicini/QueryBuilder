using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryBuilder.Helpers
{
    public static class ThrowHelper
    {
        public static void ThrowIfNull<T>(T value, string argumentName)
        {
            ThrowIfNullOrWhiteSpace(argumentName, nameof(argumentName));

            if (value == null)
                throw new ArgumentNullException(nameof(value));
        }

        public static void ThrowIfNullOrWhiteSpace(string value, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(argumentName))
                throw new ArgumentException($"The parameter \"{nameof(argumentName)}\" cannot be null or a white space.");

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"The parameter \"{argumentName}\" cannot be null or a white space.");
        }

        public static void ThrowIfNullOrEmpty<T>(IEnumerable<T> value, string argumentName)
        {
            ThrowIfNullOrWhiteSpace(argumentName, nameof(argumentName));

            if (value == null || !value.Any())
                throw new ArgumentException($"The parameter \"{argumentName}\" must contains at least one item.");
        }
    }
}
