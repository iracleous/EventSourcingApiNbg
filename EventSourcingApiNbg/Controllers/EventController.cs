 

namespace EventSourcingApiNbg.Controllers;


using System;
using System.Threading.Tasks;
using EventStore.Client;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using EventSourcingApiNbg.Models;
using System.Text.Json;

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
    public async Task<IActionResult> ReadEventsFromStream(string streamName)
    {
        try
        {
            var events = new List<WeatherForecast>();
            // Read events from the specified stream starting from the beginning
            await _eventStoreClient.ReadStreamAsync(
              Direction.Forwards, streamName, StreamPosition.Start)
                .ForEachAsync(
             ev=> {
                 byte[] yy = ConcatenateReadOnlyMemories(  ev.Event.Data);
                 WeatherForecast xx =JsonSerializer.Deserialize<WeatherForecast>(  yy);
                 events.Add(xx);
             });

            return Ok(events);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPost("write")]
        public async Task<IActionResult> WriteEventsToStream(string streamName, [FromBody] object eventData)
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
            return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(eventData);
        }

    // Utility method to serialize event data to JSON or any other format
    private WeatherForecast? DeSerializeEvent(byte[]eventData)
    {
        // Implement serialization logic here
        return System.Text.Json.JsonSerializer.Deserialize<WeatherForecast>(eventData);
    }



    public static byte[]
    ConcatenateReadOnlyMemories(params ReadOnlyMemory<byte>[] readOnlyMemories)
    {
        // Calculate the total length of all ReadOnlyMemory<byte> instances
        int totalLength = 0;
        foreach (var memory in readOnlyMemories)
        {
            totalLength += memory.Length;
        }

        // Create a byte array to hold the concatenated bytes
        byte[] byteArray = new byte[totalLength];

        // Copy bytes from each ReadOnlyMemory<byte> to the byte array
        int currentIndex = 0;
        foreach (var memory in readOnlyMemories)
        {
            memory.Span.CopyTo(byteArray.AsSpan().Slice(currentIndex));
            currentIndex += memory.Length;
        }

        return byteArray;
    }
}
 

