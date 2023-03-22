namespace Application.Logic.Game.Commands;

public class UpdateGameCommand : IRequest<Domain.Entities.Game>
{
    public int Id { get; init; }
    public string? Attempt { get; init; }
}

/// <summary>
/// Base class to create a validator. So here you check if request to the database are valid
/// </summary>
public class UpdateGameCommandValidator : AbstractValidator<UpdateGameCommand>
{

    /// <summary>
    /// Example Validator
    /// </summary>
    public UpdateGameCommandValidator(IApplicationDbContext context)
    {
        RuleFor(command => command.Attempt)// check that attempt made
            .Length(5, 7).WithMessage("Invalid input"); // is between 5 and 6;
    }
}

/// <summary>
/// Base class for handling creating <see cref="Game"/> entities in database
/// </summary>
public class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommand, Domain.Entities.Game>
{
    /// <summary>
    /// holds the context of the database. So all data(---sort of) and operations of you database.
    /// </summary>
    private readonly IApplicationDbContext _context;

    public UpdateGameCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// This is where the magic happens.
    /// Here you search a <see cref="Game"/> object from db.
    /// The <see cref="DbContext"/> will create the sql using magic and with SaveChangesAsync you store the changes in the database
    /// </summary>
    /// <param name="request"></param> Request optionally holding data to create a new instance of <see cref="Game"/>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns updated game object <see cref="Game"/> object</returns>
    public async Task<Domain.Entities.Game> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Game entity = await _context.Games.FindOrNotFoundAsync(request.Id, cancellationToken);
        
        entity.MakeGuess(request.Attempt);

        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}