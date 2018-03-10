using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QueryBuilder.Core.Helpers
{
    [DebuggerStepThrough]
    public static class Check
    {
        /// <summary>
        /// Check <paramref name="value"/> is null and throws <see cref="ArgumentNullException"/>.
        /// Set the <c>parameterName</c> of the exception to <paramref name="argumentName"/> value.
        /// </summary>
        /// <typeparam name="T">The parameter <paramref name="value"/> type.</typeparam>
        /// <exception cref="ArgumentNullException">Thrown when the parameter <paramref name="value"/> is null.</exception>
        /// <param name="value">The value to check.</param>
        /// <param name="argumentName">The local variable name.</param>
        public static void NotNull<T>(T value, string argumentName)
        {
            NotNullOrWhiteSpace(argumentName, nameof(argumentName));

            if (value == null)
                throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Check <paramref name="value"/> is null or white space and throws <see cref="ArgumentException"/>.
        /// Set the <c>parameterName</c> of the exception to <paramref name="argumentName"/> value.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the parameter <paramref name="value"/> is null or white space.</exception>
        /// <param name="value">The value to check.</param>
        /// <param name="argumentName">The local variable name.</param>
        public static void NotNullOrWhiteSpace(string value, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(argumentName))
                throw new ArgumentException($"The parameter \"{nameof(argumentName)}\" cannot be null or a white space.");

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"The parameter \"{argumentName}\" cannot be null or a white space.");
        }

        /// <summary>
        /// Check <paramref name="value"/> is null or empty and throws <see cref="ArgumentException"/>.
        /// Set the <c>parameterName</c> of the exception to <paramref name="argumentName"/> value.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the parameter <paramref name="value"/> is null or empty.</exception>
        /// <param name="value">The enumerable to check.</param>
        /// <param name="argumentName">The local variable name.</param>
        public static void NotNullOrEmpty<T>(IEnumerable<T> value, string argumentName)
        {
            NotNullOrWhiteSpace(argumentName, nameof(argumentName));

            if (value == null || !value.Any())
                throw new ArgumentException($"The parameter \"{argumentName}\" must contains at least one item.");
        }
    }
}
