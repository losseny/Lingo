namespace Domain.Events.GameEvents;

public class GameCompletedEvent
{
    public GameCompletedEvent(Game game)
    {
        Game = game;
    }

    private Game Game { get; }
}