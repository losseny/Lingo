namespace Domain.Events.RoundEvents;

public class RoundDeletedEvent
{
    public RoundDeletedEvent(Round round)
    {
        Round = round;
    }
    
    private Round Round { get; }
}