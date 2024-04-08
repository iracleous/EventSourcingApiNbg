using EventStore.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();



////////////////////////////////////////////
var settings = EventStoreClientSettings.Create("esdb://localhost:2113?Tls=false");
var eventStoreClient = new EventStoreClient(settings);
builder.Services.AddSingleton(eventStoreClient);





var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
