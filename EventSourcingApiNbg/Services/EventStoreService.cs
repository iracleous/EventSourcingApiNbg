using EventSourcingApiNbg.Helpers;
using EventSourcingApiNbg.Models;
using EventStore.Client;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcingApiNbg.Services
{
    public class EventStoreService<T> : IEventStoreService<T>
    {
        private readonly EventStoreClient _eventStoreClient;

        public EventStoreService(EventStoreClient eventStoreClient)
        {
            _eventStoreClient = eventStoreClient ?? throw new ArgumentNullException(nameof(eventStoreClient));
        }

        public async Task<List<T?>> ReadEventsFromStream(string streamName)
        {
            try
            {
                var events = new List<T?>();
                // Read events from the specified stream starting from the beginning
                await _eventStoreClient
                    .ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.Start)
                    .ForEachAsync(ev =>
                    {
                        var data = ev.Event.Data;
                        events.Add(SerializationHelper<T>.DeSerializeEvent(data));
                    }


                    );
                return events;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<StreamRevision> WriteEventsToStream(string streamName, T eventData, string eventType)
        {
            try
            {
                var jsonEventData = SerializationHelper<T>.SerializeEvent(eventData);
                var eventToAdd = new EventData(
                    Uuid.NewUuid(),
                    eventType,
                    data: jsonEventData,
                    metadata: null
                );

                var result = await _eventStoreClient.AppendToStreamAsync(streamName, StreamState.Any,
                    [eventToAdd]);

                return result.NextExpectedStreamRevision;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
