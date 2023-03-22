using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

public class IdentityService  : IIdentityService
{
    	private readonly UserManager<IdentityUser> _userManager;
	private readonly SignInManager<IdentityUser> _signInManager;
	private readonly IUserClaimsPrincipalFactory<IdentityUser> _userClaimsPrincipalFactory;
	private readonly IAuthorizationService _authorizationService;

	public IdentityService(
		UserManager<IdentityUser> userManager,
		SignInManager<IdentityUser> signInManager,
		IUserClaimsPrincipalFactory<IdentityUser> userClaimsPrincipalFactory,
		IAuthorizationService authorizationService)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_userClaimsPrincipalFactory = userClaimsPrincipalFactory;
		_authorizationService = authorizationService;
	}

	internal async Task<IdentityUser?> FindByNameAsync(string userName)
	{
		return await _userManager.FindByNameAsync(userName);
	}

	public async Task<string?> GetUserNameAsync(string userId)
	{
		var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

		return user?.Email;
	}

	public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
	{
		var user = new IdentityUser
		{
			UserName = userName,
			Email = userName,
		};

		var result = await _userManager.CreateAsync(user, password);

		return (result.ToApplicationResult(), user.Id);
	}

	public async Task<bool> CheckPasswordSignInAsync(string userName, string password)
	{
		var user = await _userManager.FindByNameAsync(userName);

		if (user is null)
			return false;

		var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);

		return result.Succeeded;
	}

	public async Task<IList<string>> GetRolesAsync(string userName)
	{
		var user = await _userManager.FindByNameAsync(userName);

		return user is not null
			? await _userManager.GetRolesAsync(user)
			: new List<string>();
	}

	public async Task<bool> IsInRoleAsync(string userId, string role)
	{
		var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

		return user is not null && await _userManager.IsInRoleAsync(user, role);
	}

	public async Task<bool> AuthorizeAsync(string userId, string policyName)
	{
		var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

		if (user == null)
		{
			return false;
		}

		var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

		var result = await _authorizationService.AuthorizeAsync(principal, policyName);

		return result.Succeeded;
	}

	public async Task<Result> DeleteUserAsync(string userId)
	{
		var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

		return user is not null
			? await DeleteUserAsync(user)
			: Result.Success();
	}

	internal async Task<Result> DeleteUserAsync(IdentityUser user)
	{
		var result = await _userManager.DeleteAsync(user);

		return result.ToApplicationResult();
	}

}