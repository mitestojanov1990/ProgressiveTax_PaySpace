using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PaySpace.Calculator.Domain;

namespace PaySpace.Calculator.Infrastructure.Persistence;
public sealed class CalculatorHistoryConfiguration : IEntityTypeConfiguration<CalculatorHistory>
{
    public void Configure(EntityTypeBuilder<CalculatorHistory> builder)
    {
        builder.ToTable("CalculatorHistory");
    }
}
public sealed class CalculatorSettingConfiguration : IEntityTypeConfiguration<CalculatorSetting>
{
    public void Configure(EntityTypeBuilder<CalculatorSetting> builder)
    {
        builder.ToTable("CalculatorSetting");
    }
}
public sealed class PostalCodeConfiguration : IEntityTypeConfiguration<PostalCode>
{
    public void Configure(EntityTypeBuilder<PostalCode> builder)
    {
        builder.ToTable("PostalCode");
    }
}