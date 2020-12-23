using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeNotes.Domain;

namespace TimeNotas.Infrastructure.Data.Identity
{
    public class ApplicationDbContext : IdentityDbContext<TimeNotesUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
