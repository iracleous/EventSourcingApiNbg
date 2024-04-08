namespace EventSourcingApiNbg.Controllers;

using System;
using System.Threading.Tasks;
using EventStore.Client;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using EventSourcingApiNbg.Models;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

[ApiController]
    [Route("api/[controller]")]
    public class EventStoreController : ControllerBase
    {
        private readonly EventStoreClient _eventStoreClient;

        public EventStoreController(EventStoreClient eventStoreClient)
        {
            _eventStoreClient = eventStoreClient ?? throw new ArgumentNullException(nameof(eventStoreClient));
        }

    [HttpGet("read/{streamName}")]
    public async Task<IActionResult> ReadEventsFromStream([FromRoute] string streamName)
    {
        try
        {
            var events = new List<WeatherForecast?>();
            // Read events from the specified stream starting from the beginning
            await _eventStoreClient
                .ReadStreamAsync( Direction.Forwards, streamName, StreamPosition.Start)
                .ForEachAsync( ev=>
                {
                    var data = ev.Event.Data;
                    events.Add(DeSerializeEvent(data));
                }
                
                
                );
            return Ok(events);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPost("write")]
        public async Task<IActionResult> WriteEventsToStream([FromQuery] string streamName, 
            [FromBody] object eventData)
        {
            try
            {
                var jsonEventData = SerializeEvent(eventData); // Serialize event data to JSON or any other format

                var eventToAdd = new EventData(
                     Uuid.NewUuid(), // Generates a new UUID for the event
                      "eventType", // Replace with your event type
                    contentType: "application/json",
                    data: jsonEventData,
                    metadata: null // You can optionally provide metadata
                );

                var result = await _eventStoreClient.AppendToStreamAsync(streamName, StreamState.Any, 
                    [eventToAdd]);

                return Ok(result.NextExpectedStreamRevision);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    // Utility method to serialize event data to JSON or any other format
    private byte[] SerializeEvent(object eventData)
    {
        // Implement serialization logic here
        return JsonSerializer.SerializeToUtf8Bytes(eventData);
    }

    // Utility method to serialize event data to JSON or any other format
    private WeatherForecast? DeSerializeEvent(ReadOnlyMemory<byte> byteArray)
    {
        // Deserialize JSON with custom options
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
            // Optional: Include this line if you have enums in your object
        };

        string decodedString = Encoding.UTF8.GetString(byteArray.ToArray());
        WeatherForecast? weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(decodedString, options);
        return weatherForecast;
    }
}
 

