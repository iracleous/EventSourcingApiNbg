using EventStore.Client;

namespace EventSourcingApiNbg.Services
{
    public interface IEventStoreService<T>
    {
        Task<List<T?>> ReadEventsFromStream(string streamName);
        Task<StreamRevision> WriteEventsToStream(string streamName, T eventData, string eventType);
    }
}