using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService, IIdentityService identityService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _identityService = identityService;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId.ToString() ?? string.Empty;
        var userName = string.IsNullOrEmpty(userId) ? string.Empty : await _identityService.GetUserNameAsync(userId);

        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("Request: {RequestName} {@UserId} {@UserName} {@Request}", requestName, userId, userName, request);
    }
}