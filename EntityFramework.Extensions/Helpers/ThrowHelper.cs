using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityFramework.Extensions.Helpers
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
    }
}
