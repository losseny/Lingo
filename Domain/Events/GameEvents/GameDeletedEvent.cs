namespace Domain.Events;

public class GameDeletedEvent
{
    public GameDeletedEvent(Game game)
    {
        Game = game;
    }

    private Game Game { get; }
}