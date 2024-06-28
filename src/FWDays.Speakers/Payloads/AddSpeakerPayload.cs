namespace FWDays.Speakers.Payloads;

public record AddSpeakerPayload(string Name, string? Bio, string? Company);