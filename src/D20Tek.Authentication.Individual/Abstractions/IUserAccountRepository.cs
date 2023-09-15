//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Identity;

namespace D20Tek.Authentication.Individual.Abstractions;

internal interface IUserAccountRepository : IUserAccountReadRepository
{
    public Task<IdentityResult> ChangePasswordAsync(
        UserAccount userAccount,
        string currentPassword,
        string newPassword);

    public Task<IdentityResult> CreateAsync(UserAccount userAccount, string password);

    public Task<bool> AttachUserRoleAsync(UserAccount userAccount, string userRole);

    public Task<bool> RemoveUserRolesAsync(UserAccount userAccount, IEnumerable<string> userRoles);

    public Task<IdentityResult> UpdateAsync(UserAccount userAccount);

    public Task<IdentityResult> DeleteAsync(UserAccount userAccount);
}
