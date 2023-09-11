//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;

namespace D20Tek.Authentication.Individual.Infrastructure;

internal class UserAccountRepository : IUserAccountRepository
{
    public Task<UserAccount?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<UserAccount?> GetByUserNameAsync(string userName)
    {
        throw new NotImplementedException();
    }

    public Task<UserAccount?> GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CreateAsync(UserAccount userAccount)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(UserAccount userAccount)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
