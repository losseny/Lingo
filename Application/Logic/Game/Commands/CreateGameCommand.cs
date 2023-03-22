namespace Application.Logic.Game.Commands;

/// <summary>
/// Method Used to create command so we can create a new game entity in database
/// </summary>
public record CreateGameCommand : IRequest<Domain.Entities.Game>
{ 
    
}

/// <summary>
/// Base class for handling creating <see cref="Game"/> entities in database
/// </summary>
public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, Domain.Entities.Game>
{
    /// <summary>
    /// holds the context of the database. So all data and operations of you database.
    /// </summary>
    private readonly IApplicationDbContext _dbContext;
    
    public CreateGameCommandHandler(IApplicationDbContext context)
    {
        _dbContext = context;
    }
    
    /// <summary>
    /// This is where the magic happens.
    /// Here you create a <see cref="Game"/> object and the Add it to the database context.
    /// The <see cref="DbContext"/> will create the sql using magic and with SaveChangesAsync you store it in the databse
    /// </summary>
    /// <param name="request"></param> Request optionally holding data to create a new instance of <see cref="Game"/>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns game object created <see cref="Game"/> object</returns>
    public async Task<Domain.Entities.Game> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var gameCreated = new Domain.Entities.Game(1);

        _dbContext.Games.Add(gameCreated);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return gameCreated;
    }
}
