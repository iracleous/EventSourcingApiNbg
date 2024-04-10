using EventSourcingApiNbg.Events;
using EventSourcingApiNbg.Models;
using EventSourcingApiNbg.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcingApiNbg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountEventController : ControllerBase
    {
        private readonly IEventStoreService<EventDto> _eventStoreService;
        public AccountEventController(IEventStoreService<EventDto> eventStoreService)
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
                [FromBody] EventDto eventData)
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
}
