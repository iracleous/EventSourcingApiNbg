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
using EventSourcingApiNbg.Helpers;
using EventSourcingApiNbg.Services;

[ApiController]
    [Route("api/[controller]")]
    public class EventStoreController : ControllerBase
    {
        private readonly IEventStoreService<WeatherForecast> _eventStoreService;
        public EventStoreController(IEventStoreService<WeatherForecast> eventStoreService)
        {
        _eventStoreService = eventStoreService ?? throw new ArgumentNullException(nameof(eventStoreService));
        }

    [HttpGet("read/{streamName}")]
    public async Task<IActionResult> ReadEventsFromStream([FromRoute] string streamName)
    {
        try
        {
            return Ok(await _eventStoreService.ReadEventsFromStream(streamName));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPost("write")]
        public async Task<IActionResult> WriteEventsToStream([FromQuery] string streamName, 
            [FromBody] WeatherForecast eventData)
        {
            try
            { 
                return Ok(await _eventStoreService.WriteEventsToStream(streamName, eventData, eventData.GetType().ToString()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
}
 

