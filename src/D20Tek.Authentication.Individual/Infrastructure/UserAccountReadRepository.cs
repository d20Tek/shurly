//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace D20Tek.Authentication.Individual.Infrastructure;

internal class UserAccountReadRepository
{
    protected readonly UserManager<UserAccount> _userManager;
    protected readonly RoleManager<IdentityRole> _roleManager;
    protected readonly ILogger<UserAccountRepository> _logger;
    protected readonly OperationManager _opsManager;

    public UserAccountReadRepository(
        UserManager<UserAccount> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<UserAccountRepository> logger)
    {
        _userManager = userManager;
        _logger = logger;
        _roleManager = roleManager;

        _opsManager = new OperationManager(logger, nameof(UserAccountRepository));
    }

    public async Task<UserAccount?> GetByIdAsync(Guid id)
    {
        return await _opsManager.OperationAsync<UserAccount?>(async () =>
            await _userManager.FindByIdAsync(id.ToString()));
    }

    public async Task<UserAccount?> GetByUserNameAsync(string userName)
    {
        return await _opsManager.OperationAsync<UserAccount?>(async () =>
            await _userManager.FindByNameAsync(userName));
    }

    public async Task<UserAccount?> GetByEmailAsync(string email)
    {
        return await _opsManager.OperationAsync<UserAccount?>(async () =>
            await _userManager.FindByEmailAsync(email));
    }

    public async Task<bool> CheckPasswordAsync(UserAccount userAccount, string password)
    {
        return await _opsManager.OperationAsync<bool>(async () =>
            await _userManager.CheckPasswordAsync(userAccount, password));
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(UserAccount userAccount)
    {
        return await _opsManager.OperationAsync<IEnumerable<string>>(async () =>
            await _userManager.GetRolesAsync(userAccount))
                ?? Array.Empty<string>();
    }
}
