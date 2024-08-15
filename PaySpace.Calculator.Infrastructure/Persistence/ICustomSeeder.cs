namespace PaySpace.Calculator.Infrastructure.Persistence;
public interface ICustomSeeder
{
    Task InitializeAsync(CancellationToken cancellationToken);
}