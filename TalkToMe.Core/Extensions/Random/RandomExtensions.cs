namespace TalkToMe.Core.Extensions.Random
{
    public static class RandomExtensions
    {
        /// <summary>
        ///     Returns a random long integer that is within a specified range. Inclusive min value, exclusive max value.
        /// </summary>
        /// <param name="rnd"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static long NextInt64(this System.Random rnd, long minValue, long maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException(paramName: nameof(minValue),
                    message: "minValue must be less than or equal to maxValue");
            }

            var range = (ulong)(maxValue - minValue);
            ulong ulongRand;

            do
            {
                var buffer = new byte[8];
                rnd.NextBytes(buffer: buffer);
                ulongRand = (ulong)BitConverter.ToInt64(value: buffer, startIndex: 0);
            } while (ulongRand > ulong.MaxValue - (ulong.MaxValue % range + 1) % range);

            return (long)(ulongRand % range) + minValue;
        }
    }
}