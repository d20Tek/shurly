//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Authentication.Individual.Abstractions;

internal interface IUserAccountRepository
{
    public Task<UserAccount?> GetByIdAsync(Guid id);

    public Task<UserAccount?> GetByUserNameAsync(string userName);

    public Task<UserAccount?> GetByEmailAsync(string email);

    public Task<bool> CreateAsync(UserAccount userAccount);

    public Task<bool> UpdateAsync(UserAccount userAccount);

    public Task<bool> DeleteAsync(Guid id);
}
