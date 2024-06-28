namespace FWDays.Participants.Database;

public class Participant
{
    public int Id { get; set; }
    
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? UserName { get; set; }

    public string? EmailAddress { get; set; }
}