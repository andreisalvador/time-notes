using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TimeNotes.Domain;

namespace TimeNotes.Data.Mappings
{
    public class HourPointConfigurationsMapping : IEntityTypeConfiguration<HourPointConfigurations>
    {
        public void Configure(EntityTypeBuilder<HourPointConfigurations> builder)
        {
            builder.HasKey(h => h.Id);
            builder.ToTable("HourPointConfigurations");
        }
    }
}
