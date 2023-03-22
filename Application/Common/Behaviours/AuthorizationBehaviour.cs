using System.Reflection;

namespace Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;

    public AuthorizationBehaviour(
        ICurrentUserService currentUserService,
        IIdentityService identityService)
    {
        _currentUserService = currentUserService;
        _identityService = identityService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>().ToList();

        if (!authorizeAttributes.Any()) return await next();
        // Must be authenticated user
        if (_currentUserService.UserId.ToString() is null)
            throw new UnauthorizedAccessException();

        // Role-based authorization
        var authorizeAttributesWithRoles = authorizeAttributes.Where(authorizeAttribute => !string.IsNullOrWhiteSpace(authorizeAttribute.Roles)).ToList();

        if (authorizeAttributesWithRoles.Any())
        {
            var authorized = false;

            foreach (var roles in authorizeAttributesWithRoles.Select(authorizeAttribute => authorizeAttribute.Roles.Split(',')))
            {
                foreach (var role in roles)
                {
                    if (!await _identityService.IsInRoleAsync(_currentUserService.UserId.ToString(), role.Trim()))
                        continue;
                    authorized = true;
                    break;
                }
            }

            // Must be a member of at least one role in roles
            if (!authorized)
                throw new ForbiddenAccessException();
        }

        // Policy-based authorization
        var authorizeAttributesWithPolicies = authorizeAttributes.Where(authorizeAttribute => !string.IsNullOrWhiteSpace(authorizeAttribute.Policy)).ToList();
        if (!authorizeAttributesWithPolicies.Any()) return await next();
        {
            foreach (var policy in authorizeAttributesWithPolicies.Select(authorizeAttribute => authorizeAttribute.Policy))
            {
                if (!await _identityService.AuthorizeAsync(_currentUserService.UserId.ToString(), policy))
                    throw new ForbiddenAccessException();
            }
        }

        // User is authorized / authorization not required
        return await next();
    }
}