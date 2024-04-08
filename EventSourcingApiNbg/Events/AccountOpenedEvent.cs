namespace EventSourcingApiNbg.Events;

using System;
using System.Collections.Generic;

// Define events
public interface IEvent
{
    DateTime Timestamp { get; }
}

public class AccountOpenedEvent : IEvent
{
    public DateTime Timestamp { get; }= DateTime.UtcNow;
    public decimal InitialBalance { get; init; }
}

public class MoneyDepositedEvent : IEvent
{
    public DateTime Timestamp { get; }= DateTime.UtcNow;
    public decimal Amount { get; init;}
}

public class MoneyWithdrawnEvent : IEvent
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
    public decimal Amount { get; init; }
}

