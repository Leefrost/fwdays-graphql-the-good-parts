using Microsoft.EntityFrameworkCore;

namespace FWDays.Tracks.Database;

public class TracksDbContext: DbContext
{
    public TracksDbContext(DbContextOptions<TracksDbContext> options)
        : base(options)
    {
    }

    public DbSet<Track> Tracks { get; set; } = default!;
}