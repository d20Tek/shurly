//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace D20Tek.Authentication.Individual.Infrastructure;

internal class UserAccountDbContext : IdentityDbContext<UserAccount>
{
    public UserAccountDbContext(DbContextOptions<UserAccountDbContext> options)
        : base(options)
    {
    }
}
