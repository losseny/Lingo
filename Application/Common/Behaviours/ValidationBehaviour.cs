using FluentValidation.Results;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.Common.Behaviours;
/// <summary>
/// To make a base validation behaviour
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly IRequestService _requestService;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, IRequestService requestService)
    {
        _validators = validators;
        _requestService = requestService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(_validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

            List<ValidationFailure> failures = validationResults
                .Where(validationResult => validationResult.Errors.Any())
                .SelectMany(validationResult => validationResult.Errors)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures, _requestService.AcceptLanguage is not null);
        }
        return await next();
    }
}