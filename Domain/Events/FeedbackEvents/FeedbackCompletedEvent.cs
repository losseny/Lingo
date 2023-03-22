namespace Domain.Events.FeedbackEvents;

public class FeedbackCompletedEvent
{
    public FeedbackCompletedEvent(Feedback feedback)
    {
        Feedback = feedback;
    }

    private Feedback Feedback { get; }
}