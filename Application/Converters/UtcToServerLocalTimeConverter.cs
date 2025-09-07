using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Converters
{
    /// <summary>
    /// Converts a UTC DateTime string from JSON to the server's local DateTime 
    /// during deserialization. Ensures the DateTimeKind is Local.
    /// When serializing, it converts server local DateTime back to a UTC string in ISO 8601 format.
    /// created by: Roo <<03/06/2025>>
    /// </summary>
    public class UtcToServerLocalTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                string? dateString = reader.GetString();
                if (string.IsNullOrWhiteSpace(dateString))
                {
                    return null;
                }

                // Attempt to parse the string. Assume UTC if no offset is present (ISO 8601 'Z' or implicit UTC).
                // DateTimeStyles.AdjustToUniversal ensures it's treated as UTC if kind is Unspecified.
                if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out DateTime utcDateTime))
                {
                    // Convert the UTC DateTime to the server's local time.
                    // ToLocalTime() automatically sets Kind to DateTimeKind.Local.
                    DateTime serverLocalDateTime = utcDateTime.ToLocalTime(); 
                    return serverLocalDateTime;
                }
                else
                {
                    throw new JsonException($"Unable to parse DateTime string: {dateString}");
                }
            }
            
            throw new JsonException($"Unexpected token type {reader.TokenType} when parsing DateTime.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                DateTime dateTimeValue = value.Value;
                DateTime utcValue;

                switch (dateTimeValue.Kind)
                {
                    case DateTimeKind.Utc:
                        utcValue = dateTimeValue; // Already UTC
                        break;
                    case DateTimeKind.Local:
                        // Convert server's local time back to UTC.
                        utcValue = dateTimeValue.ToUniversalTime();
                        break;
                    default: // DateTimeKind.Unspecified
                        // If Kind is Unspecified, it's ambiguous. 
                        // Assume it's local as per how Read method would have set it.
                        var tempLocal = DateTime.SpecifyKind(dateTimeValue, DateTimeKind.Local);
                        utcValue = tempLocal.ToUniversalTime();
                        // Consider logging a warning if Kind is Unspecified and not from this converter.
                        break;
                }
                
                // Write in ISO 8601 format ("o" round-trip format specifier)
                writer.WriteStringValue(utcValue.ToString("o", CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}