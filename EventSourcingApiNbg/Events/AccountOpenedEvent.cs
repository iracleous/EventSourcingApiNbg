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
    public DateTime Timestamp { get; }
    public decimal InitialBalance { get; }

    public AccountOpenedEvent(decimal initialBalance)
    {
        Timestamp = DateTime.UtcNow;
        InitialBalance = initialBalance;
    }
}

public class MoneyDepositedEvent : IEvent
{
    public DateTime Timestamp { get; }
    public decimal Amount { get; }

    public MoneyDepositedEvent(decimal amount)
    {
        Timestamp = DateTime.UtcNow;
        Amount = amount;
    }
}

public class MoneyWithdrawnEvent : IEvent
{
    public DateTime Timestamp { get; }
    public decimal Amount { get; }

    public MoneyWithdrawnEvent(decimal amount)
    {
        Timestamp = DateTime.UtcNow;
        Amount = amount;
    }
}

