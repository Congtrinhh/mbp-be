using System.Text.RegularExpressions;

namespace Domain.Extensions
{
    /// <summary>
    /// Extension methods for string operations
    /// Created by: tqcong 28/04/2025
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Converts a camelCase string to snake_case
        /// Example: "firstName" becomes "first_name"
        /// </summary>
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var result = input[0].ToString().ToLower();
            for (var i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                {
                    result += "_" + char.ToLower(input[i]);
                }
                else
                {
                    result += input[i];
                }
            }
            return result;
        }
    }
}