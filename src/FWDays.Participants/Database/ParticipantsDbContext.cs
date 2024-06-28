using Microsoft.EntityFrameworkCore;

namespace FWDays.Participants.Database;

public class ParticipantsDbContext: DbContext
{
    public ParticipantsDbContext(DbContextOptions<ParticipantsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Participant> Participants { get; set; } = default!;
}