using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TimeNotes.Domain;

namespace TimeNotes.Data.Mappings
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
