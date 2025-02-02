namespace LiveCoaching.Types;

public record Game
{
    public string? gameCreationDate { get; init; }
    public string? gameMode { get; set; }
}