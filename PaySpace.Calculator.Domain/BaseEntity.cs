using PaySpace.Calculator.Domain.Contracts;

namespace PaySpace.Calculator.Domain;
public abstract class BaseEntity<TId> : IEntity<TId>
{
    public TId Id { get; protected init; } = default!;
}

public abstract class BaseEntity : BaseEntity<int>
{ 
    protected BaseEntity() => Id = default(int);
}
