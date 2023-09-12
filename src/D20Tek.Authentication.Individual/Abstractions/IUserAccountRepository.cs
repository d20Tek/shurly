//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Identity;

namespace D20Tek.Authentication.Individual.Abstractions;

internal interface IUserAccountRepository
{
    public Task<UserAccount?> GetByIdAsync(Guid id);

    public Task<UserAccount?> GetByUserNameAsync(string userName);

    public Task<UserAccount?> GetByEmailAsync(string email);

    public Task<bool> CheckPasswordAsync(UserAccount userAccountstring, string password);

    public Task<IdentityResult> CreateAsync(UserAccount userAccount, string password);

    public Task<bool> AttachUserRole(UserAccount userAccount, string roleName);

    public Task<IEnumerable<string>> GetUserRoles(UserAccount userAccount);

    public Task<bool> UpdateAsync(UserAccount userAccount);

    public Task<bool> DeleteAsync(Guid id);
}
