using System.ComponentModel;

namespace TalkToMe.Core.Extensions.String
{
    public static class StringExtensions
    {
        /// <summary>
        /// Validates the input against boolean options (e.g., Yes/No).
        /// </summary>
        /// <typeparam name="T">The type of the input to be returned.</typeparam>
        /// <param name="input">The input string to validate.</param>
        /// <param name="availableOptions">The available string options.</param>
        /// <param name="parsedValue">The parsed boolean value.</param>
        /// <returns>True if the input is valid, otherwise false.</returns>
        public static bool ValidateBooleanInput<T>(this string input, string[] availableOptions, out T parsedValue)
        {
            parsedValue = default;

            if (availableOptions.Length == 2)
            {
                if (string.Equals(input, availableOptions[0], StringComparison.OrdinalIgnoreCase))
                {
                    parsedValue = (T)(object)true;
                    return true;
                }

                if (string.Equals(input, availableOptions[1], StringComparison.OrdinalIgnoreCase))
                {
                    parsedValue = (T)(object)false;
                    return true;
                }

                if (int.TryParse(input, out var index))
                {
                    if (index == 1) // Adjusted to start from 1
                    {
                        parsedValue = (T)(object)true;
                        return true;
                    }

                    if (index == 2) // Adjusted to start from 1
                    {
                        parsedValue = (T)(object)false;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Validates the input against a numeric range.
        /// </summary>
        /// <typeparam name="T">The type of the input to be returned.</typeparam>
        /// <param name="input">The input string to validate.</param>
        /// <param name="minValue">The minimum value in the range.</param>
        /// <param name="maxValue">The maximum value in the range.</param>
        /// <param name="parsedValue">The parsed value.</param>
        /// <returns>True if the input is within the range, otherwise false.</returns>
        public static bool ValidateRange<T>(this string input, T minValue, T maxValue, out T parsedValue) where T : IComparable
        {
            parsedValue = default;
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter.IsValid(input))
            {
                parsedValue = (T)converter.ConvertFromString(input);
                if (parsedValue.CompareTo(minValue) >= 0 && parsedValue.CompareTo(maxValue) <= 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Validates the input against available string options.
        /// </summary>
        /// <typeparam name="T">The type of the input to be returned.</typeparam>
        /// <param name="input">The input string to validate.</param>
        /// <param name="availableOptions">The available string options.</param>
        /// <param name="parsedValue">The parsed value.</param>
        /// <param name="allowNumberInput">Indicates if numeric inputs are allowed.</param>
        /// <param name="startFromOne">Indicates if numbering should start from 1.</param>
        /// <returns>True if the input is valid, otherwise false.</returns>
        public static bool ValidateInput<T>(this string input, string[] availableOptions, out T parsedValue, bool allowNumberInput, bool startFromOne = true) where T : IComparable
        {
            parsedValue = default;

            for (var index = 0; index < availableOptions.Length; index++)
            {
                var option = availableOptions[index];
                if (string.Equals(input, option, StringComparison.OrdinalIgnoreCase))
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter.IsValid(option))
                    {
                        parsedValue = (T)converter.ConvertFromString(option);
                        return true;
                    }
                }

                // Also allow numeric selection of the options, but only show indices for non-numeric types
                if (allowNumberInput && int.TryParse(input, out var inputIndex) && inputIndex == (startFromOne ? index + 1 : index))
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter.IsValid(option))
                    {
                        parsedValue = (T)converter.ConvertFromString(option);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Validates the input against available enum options.
        /// </summary>
        /// <typeparam name="T">The type of the input to be returned.</typeparam>
        /// <param name="input">The input string to validate.</param>
        /// <param name="enumResult">The parsed enum value.</param>
        /// <param name="allowNumberInput">Indicates if numeric inputs are allowed.</param>
        /// <param name="startFromOne">Indicates if numbering should start from 1.</param>
        /// <returns>True if the input is valid, otherwise false.</returns>
        public static bool ValidateEnum<T>(this string input, out T enumResult, bool allowNumberInput, bool startFromOne = true) where T : IComparable
        {
            enumResult = default;

            if (Enum.TryParse(typeof(T), input, true, out var result) && Enum.IsDefined(typeof(T), result))
            {
                enumResult = (T)result;
                return true;
            }

            if (allowNumberInput && int.TryParse(input, out var index) && Enum.IsDefined(typeof(T), index - (startFromOne ? 1 : 0)))
            {
                enumResult = (T)Enum.ToObject(typeof(T), index - (startFromOne ? 1 : 0));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validates the input against available enum options.
        /// </summary>
        /// <typeparam name="T">The type of the input to be returned.</typeparam>
        /// <param name="input">The input string to validate.</param>
        /// <param name="availableEnumOptions">The available enum options.</param>
        /// <param name="parsedValue">The parsed enum value.</param>
        /// <param name="allowNumberInput">Indicates if numeric inputs are allowed.</param>
        /// <param name="startFromOne">Indicates if numbering should start from 1.</param>
        /// <returns>True if the input is valid, otherwise false.</returns>
        public static bool ValidateEnumOptions<T>(this string input, T[] availableEnumOptions, out T parsedValue, bool allowNumberInput, bool startFromOne = true) where T : IComparable
        {
            parsedValue = default;

            for (var index = 0; index < availableEnumOptions.Length; index++)
            {
                var option = availableEnumOptions[index];
                if (string.Equals(input, option.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    parsedValue = option;
                    return true;
                }

                if (allowNumberInput && int.TryParse(input, out var inputIndex) && inputIndex == (startFromOne ? index + 1 : index))
                {
                    parsedValue = option;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Validates the input against a range of enum values.
        /// </summary>
        /// <typeparam name="T">The type of the input to be returned.</typeparam>
        /// <param name="input">The input string to validate.</param>
        /// <param name="minEnum">The minimum enum value in the range.</param>
        /// <param name="maxEnum">The maximum enum value in the range.</param>
        /// <param name="parsedValue">The parsed enum value.</param>
        /// <returns>True if the input is within the range, otherwise false.</returns>
        public static bool ValidateEnumRange<T>(this string input, T minEnum, T maxEnum, out T parsedValue) where T : IComparable
        {
            parsedValue = default;

            if (input.ValidateEnum(out T enumResult, true))
            {
                var enumIntValue = Convert.ToInt32(enumResult);
                if (enumIntValue >= Convert.ToInt32(minEnum) && enumIntValue <= Convert.ToInt32(maxEnum))
                {
                    parsedValue = enumResult;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Converts a pair of string inputs to a numeric range.
        /// </summary>
        /// <typeparam name="T">The type of the input to be returned.</typeparam>
        /// <param name="inputs">The array of string inputs to convert.</param>
        /// <param name="minValue">The minimum value in the range.</param>
        /// <param name="maxValue">The maximum value in the range.</param>
        /// <returns>True if the conversion was successful, otherwise false.</returns>
        public static bool TryConvert<T>(this string[] inputs, out T minValue, out T maxValue) where T : IComparable
        {
            minValue = default;
            maxValue = default;
            if (inputs.Length == 2 && TypeDescriptor.GetConverter(typeof(T)).IsValid(inputs[0]) &&
                TypeDescriptor.GetConverter(typeof(T)).IsValid(inputs[1]))
            {
                minValue = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(inputs[0]);
                maxValue = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(inputs[1]);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Converts a pair of enum inputs to a range of enum values.
        /// </summary>
        /// <typeparam name="T">The type of the input to be returned.</typeparam>
        /// <param name="inputs">The array of enum inputs to convert.</param>
        /// <param name="minValue">The minimum enum value in the range.</param>
        /// <param name="maxValue">The maximum enum value in the range.</param>
        /// <returns>True if the conversion was successful, otherwise false.</returns>
        public static bool TryConvertEnum<T>(this T[] inputs, out T minValue, out T maxValue) where T : IComparable
        {
            minValue = default;
            maxValue = default;
            if (inputs.Length == 2 && Enum.TryParse(typeof(T), inputs[0].ToString(), out var minEnum) &&
                Enum.TryParse(typeof(T), inputs[1].ToString(), out var maxEnum))
            {
                minValue = (T)minEnum;
                maxValue = (T)maxEnum;
                return true;
            }

            return false;
        }
    }
}
