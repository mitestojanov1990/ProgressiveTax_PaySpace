using Microsoft.EntityFrameworkCore;
using PaySpace.Calculator.Domain;

namespace PaySpace.Calculator.Infrastructure.Persistence;

public sealed class CalculatorContext : DbContext
{
    public CalculatorContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<PostalCode> PostalCodes => Set<PostalCode>();
    public DbSet<CalculatorSetting> CalculatorSettings => Set<CalculatorSetting>();
    public DbSet<CalculatorHistory> CalculatorHistories => Set<CalculatorHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}