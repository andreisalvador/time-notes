using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeNotes.Domain;

namespace TimeNotas.Infrastructure.Data.Mappings
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
