using Microsoft.EntityFrameworkCore;

namespace FWDays.Speakers;

public class SpeakersDbContext: DbContext
{
    public SpeakersDbContext(DbContextOptions<SpeakersDbContext> options)
        : base(options)
    {
    }

    public DbSet<Speaker> Speakers { get; set; } = default!;
}