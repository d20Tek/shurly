//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Authentication.Individual.Abstractions;

internal interface IUserAccountReadRepository
{
    public Task<UserAccount?> GetByIdAsync(Guid id);

    public Task<UserAccount?> GetByUserNameAsync(string userName);

    public Task<UserAccount?> GetByEmailAsync(string email);

    public Task<bool> CheckPasswordAsync(UserAccount userAccountstring, string password);

    public Task<IEnumerable<string>> GetUserRolesAsync(UserAccount userAccount);
}
