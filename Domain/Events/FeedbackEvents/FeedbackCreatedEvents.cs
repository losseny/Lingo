namespace Domain.Events.FeedbackEvents;

public class FeedbackCreatedEvents
{
    public FeedbackCreatedEvents(Feedback feedback)
    {
        Feedback = feedback;
    }

    private Feedback Feedback { get; }
}