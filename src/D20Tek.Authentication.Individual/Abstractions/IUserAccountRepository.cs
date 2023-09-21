//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Identity;

namespace D20Tek.Authentication.Individual.Abstractions;

internal interface IUserAccountRepository : IUserAccountReadRepository
{
    public Task<bool> AttachUserRoleAsync(UserAccount userAccount, string userRole);

    public Task<IdentityResult> ChangePasswordAsync(
        UserAccount userAccount,
        string currentPassword,
        string newPassword);

    public Task<IdentityResult> CreateAsync(UserAccount userAccount, string password);

    public Task<IdentityResult> DeleteAsync(UserAccount userAccount);

    public Task<string?> GeneratePasswordResetTokenAsync(UserAccount userAccount);

    public Task<bool> RemoveUserRolesAsync(UserAccount userAccount, IEnumerable<string> userRoles);

    public Task<IdentityResult> ResetPasswordAsync(
        UserAccount userAccount,
        string resetToken,
        string newPassword);

    public Task<IdentityResult> UpdateAsync(UserAccount userAccount);
}
