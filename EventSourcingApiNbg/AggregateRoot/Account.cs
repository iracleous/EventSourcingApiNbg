using EventSourcingApiNbg.Events;

namespace EventSourcingApiNbg.AggregateRoot;


// Define aggregate root
public class Account
{
    private readonly List<IEvent> _changes = new List<IEvent>();
    public Guid Id { get; private set; }
    public decimal Balance { get; private set; }


    // Constructor for creating new account
    public Account(Guid id, decimal initialBalance)
    {
        ApplyChange(new AccountOpenedEvent { InitialBalance = initialBalance });
    }

    // Constructor for rebuilding account from events
    public Account(Guid id, IEnumerable<IEvent> events)
    {
        Id = id;
        foreach (var @event in events)
        {
            ApplyChange(@event);
        }
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        ApplyChange(new MoneyDepositedEvent { Amount= amount });
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        if (Balance - amount < 0)
            throw new InvalidOperationException("Insufficient funds.");

        ApplyChange(new MoneyWithdrawnEvent { Amount = amount });
    }

    // Apply event to update state
    private void ApplyChange(IEvent @event)
    {
        switch (@event)
        {
            case AccountOpenedEvent openedEvent:
                Id = Guid.NewGuid();
                Balance = openedEvent.InitialBalance;
                break;
            case MoneyDepositedEvent depositedEvent:
                Balance += depositedEvent.Amount;
                break;
            case MoneyWithdrawnEvent withdrawnEvent:
                Balance -= withdrawnEvent.Amount;
                break;
        }

        _changes.Add(@event);
    }

    // Get the list of changes (events)
    public IEnumerable<IEvent> GetChanges() => _changes;
}

