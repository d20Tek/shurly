﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace D20Tek.Authentication.Individual.Infrastructure;

internal class UserAccountRepository : IUserAccountRepository
{
    private readonly UserManager<UserAccount> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<UserAccountRepository> _logger;

    public UserAccountRepository(
        UserManager<UserAccount> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<UserAccountRepository> logger)
    {
        _userManager = userManager;
        _logger = logger;
        _roleManager = roleManager;
    }

    public async Task<UserAccount?> GetByIdAsync(Guid id)
    {
        return await RepositoryOperationAsync<UserAccount?>(async () =>
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return user;
        });
    }

    public async Task<UserAccount?> GetByUserNameAsync(string userName)
    {
        return await RepositoryOperationAsync<UserAccount?>(async () =>
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user;
        });
    }

    public async Task<UserAccount?> GetByEmailAsync(string email)
    {
        return await RepositoryOperationAsync<UserAccount?>(async () =>
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        });
    }

    public async Task<bool> CreateAsync(UserAccount userAccount)
    {
        return await RepositoryOperationAsync<bool>(async () =>
        {
            var result = await _userManager.CreateAsync(userAccount);
            if (result.Succeeded is false)
            {
                _logger.LogError(
                    "UserAccountRepository.CreateAsync failed: {error}",
                    result.Errors);
            }

            return result.Succeeded;
        });
    }

    public async Task<bool> AttachUserRole(UserAccount userAccount, string userRole)
    {
        return await RepositoryOperationAsync<bool>(async () =>
        {
            if (!await _roleManager.RoleExistsAsync(userRole))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(userRole));
                if (result.Succeeded is false)
                {
                    _logger.LogError(
                        "UserAccountRepository.AttachUserRole unable to create new role: {error}",
                        result.Errors);
                    return false;
                }
            }

            if (await _roleManager.RoleExistsAsync(userRole))
            {
                var result = await _userManager.AddToRoleAsync(userAccount, userRole);
                if (result.Succeeded is false)
                {
                    _logger.LogError(
                        "UserAccountRepository.AttachUserRole unable to add new role: {error}",
                        result.Errors);
                    return false;
                }
            }

            return true;
        });
    }

    public async Task<bool> UpdateAsync(UserAccount userAccount)
    {
        return await RepositoryOperationAsync<bool>(async () =>
        {
            var result = await _userManager.UpdateAsync(userAccount);
            if (result.Succeeded is false)
            {
                _logger.LogError(
                    "UserAccountRepository.UpdateAsync failed: {error}",
                    result.Errors);
            }

            return result.Succeeded;
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await RepositoryOperationAsync<bool>(async () =>
        {
            var userAccount = await GetByIdAsync(id);
            if (userAccount is null) return false;

            var result = await _userManager.DeleteAsync(userAccount);
            if (result.Succeeded is false)
            {
                _logger.LogError(
                    "UserAccountRepository.DeleteAsync failed: {error}",
                    result.Errors);
            }

            return result.Succeeded;
        });
    }

    private async Task<TResult?> RepositoryOperationAsync<TResult>(
        Func<Task<TResult>> operation,
        [CallerMemberName] string caller = "method")
    {
        try
        {
            return await operation.Invoke();
        }
        catch (Exception ex)
        {
            var className = this.GetType().Name;
            _logger.LogError(
                ex,
                $"Error processing {className}.{caller} operation.");
            return default;
        }
    }
}
