using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeNotes.Domain;

namespace TimeNotas.Infrastructure.Data.Mappings
{
    public class TimeEntryMapping : IEntityTypeConfiguration<TimeEntry>
    {
        public void Configure(EntityTypeBuilder<TimeEntry> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.HourPoints)
                .WithMany(h => h.TimeEntries)
                .HasForeignKey(t => t.HourPointsId);

            builder.ToTable("TimeEntries");
        }
    }
}
