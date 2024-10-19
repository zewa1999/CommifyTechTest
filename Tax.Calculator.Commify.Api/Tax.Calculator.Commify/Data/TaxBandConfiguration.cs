using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tax.Calculator.Commify.Data.Entities;

namespace Tax.Calculator.Commify.Data;

internal class TaxBandConfiguration : IEntityTypeConfiguration<TaxBand>
{
    public void Configure(EntityTypeBuilder<TaxBand> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.BandName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.LowerSalaryRange)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.LastUpdatedAt)
            .IsRequired();
    }
}