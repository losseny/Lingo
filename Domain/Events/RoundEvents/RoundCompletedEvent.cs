namespace Domain.Events.RoundEvents;

public class RoundCompletedEvent
{
    public RoundCompletedEvent(Round round)
    {
        Round = round;
    }
    
    private Round Round { get; }
}