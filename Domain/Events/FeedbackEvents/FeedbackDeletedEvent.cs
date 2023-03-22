namespace Domain.Events.FeedbackEvents;

public class FeedbackDeletedEvent
{
    public FeedbackDeletedEvent(Feedback feedback)
    {
        Feedback = feedback;
    }

    private Feedback Feedback { get; }
}