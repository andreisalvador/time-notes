using Microsoft.EntityFrameworkCore;
using System;
using TimeNotes.Domain;

namespace TimeNotes.Data
{
    public class TimeNotesContext : DbContext
    {
        public TimeNotesContext(DbContextOptions<TimeNotesContext> options)
            : base(options) { }

        public DbSet<TimeEntry> TimeEntries{ get; set; }
        public DbSet<HourPoints> HourPoints { get; private set; }
        public DbSet<HourPointConfigurations> HourPointConfigurations { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TimeNotesContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
