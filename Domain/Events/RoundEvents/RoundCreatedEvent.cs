namespace Domain.Events.RoundEvents;

public class RoundCreatedEvent
{
    public RoundCreatedEvent(Round round)
    {
        Round = round;
    }
    
    private Round Round { get; }
}