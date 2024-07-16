using System.ComponentModel;
using TalkToMe.Core.Extensions.String;
using TalkToMe.Core.Extensions.Type;

namespace TalkToMe.Core.ConsoleHandlers.Input
{
    /// <summary>
    /// Provides methods to handle input from the console.
    /// </summary>
    public static class ConsoleInputHandler
    {
        /// <summary>
        /// Reads a line of input from the console.
        /// </summary>
        /// <returns>The input string read from the console, or null if no input was read.</returns>
        public static string? GetInput()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// Gets a typed input from the console with various options and configurations.
        /// </summary>
        /// <typeparam name="T">The type of the input to be returned.</typeparam>
        /// <param name="message">The message to display to the user.</param>
        /// <param name="isRange">Indicates if the input should be within a range.</param>
        /// <param name="allowNumberInput">Indicates if numeric inputs are allowed.</param>
        /// <param name="showOptions">Indicates if the available options should be displayed.</param>
        /// <param name="availableStringOptions">Array of available string options.</param>
        /// <param name="availableEnumOptions">Array of available enum options.</param>
        /// <returns>The validated and parsed input of type <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// This method provides flexible input handling for different types, including ranges and specific options.
        /// </remarks>
        public static T? GetInput<T>(
            string? message = null,
            bool isRange = false,
            bool allowNumberInput = true,
            bool showOptions = true,
            string[]? availableStringOptions = null,
            T[]? availableEnumOptions = null
        ) where T : IComparable
        {
            // Display the message if provided
            if (message != null)
            {
                Console.WriteLine(message);
            }

            T minValue = default;
            T maxValue = default;

            // Handle ranges for enum types
            if (isRange && availableEnumOptions != null && availableEnumOptions.Length == 2 && typeof(T).IsEnum && availableEnumOptions.TryConvertEnum(out minValue, out maxValue))
            {
                if (showOptions)
                {
                    Console.WriteLine("Please enter one of the available options:");
                    for (int i = Convert.ToInt32(minValue); i <= Convert.ToInt32(maxValue); i++)
                    {
                        var value = (T)Enum.ToObject(typeof(T), i);
                        Console.WriteLine($"{i + 1}: {value}"); // Adjusted to start from 1
                    }
                }
            }
            // Handle ranges for numeric types
            else if (isRange && availableStringOptions != null && availableStringOptions.Length == 2 && typeof(T).IsNumericType() && availableStringOptions.TryConvert(out minValue, out maxValue))
            {
                if (showOptions)
                {
                    Console.WriteLine($"Please enter a value between {minValue} and {maxValue}:");
                }
            }
            // Handle enum options
            else if (availableEnumOptions != null && availableEnumOptions.Length > 0)
            {
                if (showOptions)
                {
                    Console.WriteLine("Please enter one of the available options:");
                    for (var index = 0; index < availableEnumOptions.Length; index++)
                    {
                        if (allowNumberInput)
                        {
                            Console.WriteLine($"{index + 1}: {availableEnumOptions[index]}"); // Adjusted to start from 1
                        }
                        else
                        {
                            Console.WriteLine($"{availableEnumOptions[index]}");
                        }
                    }
                }
            }
            // Handle string options
            else if (availableStringOptions != null && availableStringOptions.Length > 0)
            {
                if (showOptions)
                {
                    Console.WriteLine("Please enter one of the available options:");
                    for (var index = 0; index < availableStringOptions.Length; index++)
                    {
                        if (allowNumberInput)
                        {
                            Console.WriteLine($"{index + 1}: {availableStringOptions[index]}"); // Adjusted to start from 1
                        }
                        else
                        {
                            Console.WriteLine($"{availableStringOptions[index]}");
                        }
                    }
                }
            }
            // Handle enum types without options
            else if (typeof(T).IsEnum)
            {
                if (showOptions)
                {
                    Console.WriteLine("Please enter one of the available options:");
                    foreach (var value in Enum.GetValues(typeof(T)))
                    {
                        Console.WriteLine($"{((int)value) + 1}: {value}"); // Adjusted to start from 1
                    }
                }
            }
            // Handle array types
            else if (typeof(T).IsArray)
            {
                var elementType = typeof(T).GetElementType();
                if (showOptions)
                {
                    Console.WriteLine($"Please enter values of type: {elementType?.Name}");
                }
            }
            // Handle general types
            else
            {
                if (showOptions)
                {
                    Console.WriteLine($"Please enter a value of type: {typeof(T).Name}");
                }
            }

            // Loop to get and validate the user input
            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    continue;
                }

                try
                {
                    // Handle boolean types with string options
                    if (typeof(T) == typeof(bool) && availableStringOptions != null && availableStringOptions.Length == 2)
                    {
                        if (input.ValidateBooleanInput(availableStringOptions, out T boolResult))
                        {
                            return boolResult;
                        }
                    }
                    // Handle ranges for numeric types
                    else if (isRange && typeof(T).IsNumericType())
                    {
                        if (input.ValidateRange(minValue, maxValue, out var parsedValue))
                        {
                            return parsedValue;
                        }
                    }
                    // Handle ranges for enum types
                    else if (isRange && typeof(T).IsEnum)
                    {
                        if (input.ValidateEnumRange(minValue, maxValue, out var parsedValue))
                        {
                            return parsedValue;
                        }
                    }
                    // Handle enum options
                    else if (availableEnumOptions != null && availableEnumOptions.Length > 0)
                    {
                        if (input.ValidateEnumOptions(availableEnumOptions, out T parsedValue, allowNumberInput, startFromOne: true))
                        {
                            return parsedValue;
                        }
                    }
                    // Handle string options
                    else if (availableStringOptions != null && availableStringOptions.Length > 0)
                    {
                        if (input.ValidateInput(availableStringOptions, out T parsedValue, allowNumberInput, startFromOne: true))
                        {
                            return parsedValue;
                        }
                    }
                    // Handle general enum types
                    else if (typeof(T).IsEnum)
                    {
                        if (input.ValidateEnum(out T enumResult, allowNumberInput, startFromOne: true))
                        {
                            return enumResult;
                        }
                    }
                    // Handle general types
                    else
                    {
                        var converter = TypeDescriptor.GetConverter(typeof(T));
                        if (converter.IsValid(input))
                        {
                            return (T)converter.ConvertFromString(input);
                        }
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Invalid format for type {typeof(T).Name}. Please try again.");
                }
                catch (InvalidCastException)
                {
                    Console.WriteLine($"Invalid value for type {typeof(T).Name}. Please try again.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                Console.WriteLine("Invalid input. Please try again.");
            }
        }
    }
}