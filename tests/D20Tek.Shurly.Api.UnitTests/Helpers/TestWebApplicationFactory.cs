//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Domain.ShortenedUrl;
using D20Tek.Shurly.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace D20Tek.Authentication.Individual.Api.UnitTests.Helpers;

internal class TestWebApplicationFactory : WebApplicationFactory<Program>
{
   protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace the real database context with an in-memory database
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ShurlyDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ShurlyDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
        });
    }

    public HttpClient CreateAuthenticatedClient(string? token = null)
    {
        token ??= GenerateTestAccessToken(Guid.NewGuid().ToString());
        var client = CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        return client;
    }

    public async Task SeedDatabase(IEnumerable<ShortenedUrl> shortenedUrls)
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ShurlyDbContext>();

        dbContext.ShortenedUrls.AddRange(shortenedUrls);
        await dbContext.SaveChangesAsync();
    }

    public string GenerateTestAccessToken(
        string userId,
        string userName = "TestUser",
        string givenName = "Tester",
        string familyName = "McTest",
        string email = "tester@test.com",
        string[]? roles = null)
    {
        List<Claim> claims = GetClaims(
            userId,
            givenName,
            familyName,
            email,
            userName,
            new string[] { "d20Tek.Shurly.Read", "d20Tek.Shurly.Write" },
            roles ?? new string[] { "user" });

        var token = CreateJwtToken(
            claims,
            DateTime.UtcNow.AddMinutes(60));

        return token;
    }

    private List<Claim> GetClaims(
        string userId,
        string givenName,
        string familyName,
        string email,
        string userName,
        string[] scopes,
        IEnumerable<string>? userRoles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.GivenName, givenName),
            new Claim(JwtRegisteredClaimNames.FamilyName, familyName),
            new Claim(JwtRegisteredClaimNames.Name, userName),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var scope in scopes)
        {
            claims.Add(new Claim("scope", scope));
        }

        if (userRoles is not null)
        {
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        return claims;
    }

    private string CreateJwtToken(List<Claim> claims, DateTime expiresOn)
    {
        var token = new JwtSecurityToken(
            issuer: "d20Tek.AuthenticationService",
            audience: "d20Tek.Shurly",
            expires: expiresOn,
            claims: claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    "d20Tek.Shurly.Api.4D8E0544-286C-407C-A8AB-C4A363AC7A5E")),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
