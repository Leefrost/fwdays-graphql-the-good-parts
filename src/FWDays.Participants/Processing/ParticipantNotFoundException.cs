namespace FWDays.Participants.Processing;

internal class ParticipantNotFoundException : Exception
{
    public ParticipantNotFoundException(string message)
        :base(message)
    {
    }
}