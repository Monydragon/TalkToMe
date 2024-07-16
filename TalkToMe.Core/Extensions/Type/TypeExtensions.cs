namespace TalkToMe.Core.Extensions.Type
{
    /// <summary>
    /// Provides extension methods for working with types.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the specified type is a numeric type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// <see langword="true"/> if the specified type is a numeric type; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsNumericType(this System.Type type)
        {
            return type == typeof(byte) || type == typeof(sbyte) ||
                   type == typeof(short) || type == typeof(ushort) ||
                   type == typeof(int) || type == typeof(uint) ||
                   type == typeof(long) || type == typeof(ulong) ||
                   type == typeof(float) || type == typeof(double) ||
                   type == typeof(decimal);
        }

        /// <summary>
        /// Determines whether the specified type is an integral type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// <see langword="true"/> if the specified type is an integral type; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsIntegralType(this System.Type type)
        {
            return type == typeof(byte) || type == typeof(sbyte) ||
                   type == typeof(short) || type == typeof(ushort) ||
                   type == typeof(int) || type == typeof(uint) ||
                   type == typeof(long) || type == typeof(ulong);
        }

        /// <summary>
        /// Determines whether the specified type is a floating-point type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// <see langword="true"/> if the specified type is a floating-point type; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsFloatingPointType(this System.Type type)
        {
            return type == typeof(float) || type == typeof(double) ||
                   type == typeof(decimal);
        }

        /// <summary>
        /// Determines whether the specified type is a string type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// <see langword="true"/> if the specified type is a string type; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsStringType(this System.Type type)
        {
            return type == typeof(string);
        }

        /// <summary>
        /// Determines whether the specified type is a collection type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// <see langword="true"/> if the specified type is a collection type; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsCollectionType(this System.Type type)
        {
            return typeof(System.Collections.IEnumerable).IsAssignableFrom(type) && type != typeof(string);
        }
    }
}
