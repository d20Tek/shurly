//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace D20Tek.Authentication.Individual.Infrastructure;

internal sealed class UserAccountRepository : UserAccountReadRepository, IUserAccountRepository
{
    public UserAccountRepository(
        UserManager<UserAccount> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<UserAccountRepository> logger)
        : base(userManager, roleManager, logger)
    {
    }

    public async Task<IdentityResult> ChangePasswordAsync(
        UserAccount userAccount,
        string currentPassword,
        string newPassword)
    {
        return await _opsManager.OperationAsync(async () =>
            await _userManager.ChangePasswordAsync(userAccount, currentPassword, newPassword));
    }

    public async Task<IdentityResult> CreateAsync(UserAccount userAccount, string password)
    {
        return await _opsManager.OperationAsync(async () =>
            await _userManager.CreateAsync(userAccount, password));
    }

    public async Task<bool> AttachUserRoleAsync(UserAccount userAccount, string userRole)
    {
        return await _opsManager.OperationAsync<bool>(async () =>
        {
            if (!await _roleManager.RoleExistsAsync(userRole))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(userRole));
                if (result.Succeeded is false) return false;
            }

            if (await _roleManager.RoleExistsAsync(userRole))
            {
                var result = await _userManager.AddToRoleAsync(userAccount, userRole);
                if (result.Succeeded is false) return false;
            }

            return true;
        });
    }

    public async Task<bool> RemoveUserRolesAsync(UserAccount userAccount, IEnumerable<string> userRoles)
    {
        return await _opsManager.OperationAsync<bool>(async () =>
        {
            var result = await _userManager.RemoveFromRolesAsync(userAccount, userRoles);
            return result.Succeeded;
        });
    }

    public async Task<IdentityResult> UpdateAsync(UserAccount userAccount)
    {
        return await _opsManager.OperationAsync(async () =>
        {
            return await _userManager.UpdateAsync(userAccount);
        });
    }

    public async Task<IdentityResult> DeleteAsync(Guid id)
    {
        return await _opsManager.OperationAsync(async () =>
        {
            var userAccount = await GetByIdAsync(id);
            if (userAccount is null)
            {
                return IdentityResult.Failed(Errors.Authentication.AccountNotFound);
            }

            return await _userManager.DeleteAsync(userAccount);
        });
    }
}
