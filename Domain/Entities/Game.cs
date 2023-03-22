namespace Domain.Entities;

/// <summary>
/// basic entity class for creating games that implements <see cref="BaseAuditableEntity"/>
/// </summary>
public class Game : BaseAuditableEntity
{
    
    public ICollection<Round> Rounds { get; }
    private int Score { get; }

    public Game(int gameId)
    {
        Id = gameId;
        Rounds = new List<Round>();
        Score = 0;
    }

    public void MakeGuess(string attempt)
    {
        
    }
}

