namespace Domain.Events;

public class GameCreatedEvent
{
    public GameCreatedEvent(Game game)
    {
        Game = game;
    }

    private Game Game { get; }
}