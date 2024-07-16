namespace TalkToMe.Core.Extensions.Int
{
    /// <summary>
    ///     Extension methods for the <see cref="int" /> data type.
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        ///     Clamps a value between a minimum and maximum value. If the value is less than the minimum, it will return the
        ///     minimum. If the value is greater than the maximum, it will return the maximum.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Clamp(this int value, int min, int max)
        {
            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }
    }
}