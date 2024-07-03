namespace FWDays.Tracks.Database;

public class Track
{
    public int Id { get; set; }

    public string? Topic { get; set; }

    public TrackSpeaker Speaker { get; set; }
    
    public List<TrackParticipant> Participants { get; set; }
}

public class TrackSpeaker
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class TrackParticipant
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}