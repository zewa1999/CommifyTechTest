using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using Tax.Calculator.Commify.Data.Entities;

namespace Tax.Calculator.Commify.Data;

public class TaxCalculatorDbContext : DbContext
{
    public virtual DbSet<TaxBand> TaxBands { get; set; } = null!;

    public TaxCalculatorDbContext()
    {
    }

    public TaxCalculatorDbContext(DbContextOptions<TaxCalculatorDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        ApplyAuditToDatabase(builder);

        base.OnModelCreating(builder);
    }

    private void ApplyAuditToDatabase(ModelBuilder builder)
    {
        foreach (IMutableEntityType mutableEntityType in builder.Model.GetEntityTypes())
        {
            if (mutableEntityType.ClrType.IsAssignableTo(typeof(IBaseEntity)))
            {
                IMutableProperty createdAtProperty = mutableEntityType.GetProperty(nameof(IBaseEntity.CreatedAt));
                createdAtProperty.ValueGenerated = ValueGenerated.OnAdd;
                createdAtProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                createdAtProperty.SetDefaultValueSql("NOW()");

                IMutableProperty updatedAtProperty =
                    mutableEntityType.GetProperty(nameof(IBaseEntity.LastUpdatedAt));
                updatedAtProperty.ValueGenerated = ValueGenerated.OnAddOrUpdate;
                updatedAtProperty.SetAfterSaveBehavior(PropertySaveBehavior.Save);
                updatedAtProperty.SetValueGeneratorFactory((_, _) => new DateTimeNowValueGenerator());
                updatedAtProperty.SetDefaultValueSql("NOW()");
            }
        }
    }
}