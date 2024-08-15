namespace PaySpace.Calculator.Infrastructure.Persistence;
internal interface IDatabaseInitializer
{
    Task InitializeDatabaseAsync(CancellationToken cancellationToken);
}