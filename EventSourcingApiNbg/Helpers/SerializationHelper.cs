using EventSourcingApiNbg.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

namespace EventSourcingApiNbg.Helpers;

    public static class SerializationHelper<T>
    {

        // Utility method to serialize event data to JSON or any other format
        public static byte[] SerializeEvent(T eventData)
        {
            // Implement serialization logic here
            return JsonSerializer.SerializeToUtf8Bytes(eventData);
        }

        // Utility method to serialize event data to JSON or any other format
        public static T? DeSerializeEvent(ReadOnlyMemory<byte> byteArray)
        {
            // Deserialize JSON with custom options
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter() }
                // Optional: Include this line if you have enums in your object
            };
            var options = jsonSerializerOptions;

            string decodedString = Encoding.UTF8.GetString(byteArray.ToArray());
            T? t = JsonSerializer.Deserialize<T>(decodedString); //, options);
            return t;
        }
    }

