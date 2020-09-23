using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeNotes.Domain;

namespace TimeNotas.Infrastructure.Data.Mappings
{
    public class HourPointsMapping : IEntityTypeConfiguration<HourPoints>
    {
        public void Configure(EntityTypeBuilder<HourPoints> builder)
        {
            builder.HasKey(h => h.Id);
            builder.HasMany(h => h.TimeEntries)
                .WithOne(t => t.HourPoints)
                .HasForeignKey(t => t.HourPointsId);

            builder.ToTable("HourPoints");
        }
    }
}
