//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Client.Result;
using D20Tek.Minimal.Result;
using System.Net.Http.Json;

namespace D20Tek.Authentication.Individual.Client;

internal abstract class ServiceBase
{
    protected async Task<Result<T>> ProcessHttpResponse<T>(HttpResponseMessage message)
        where T : class
    {
        try
        {
            if (message.IsSuccessStatusCode is false)
            {
                var problem = await message.Content.ReadFromJsonAsync<ProblemDetails>();
                if (problem is not null)
                {
                    var result = problem.ToResult<T>();
                    Console.WriteLine("Login error:");
                    Console.WriteLine(string.Join(Environment.NewLine, result.Errors));

                    return result;
                }

                return Errors.AuthenticationService.CannotParseProblem;
            }

            var response = await message.Content.ReadFromJsonAsync<T>();
            if (response is null)
            {
                return Errors.AuthenticationService.CannotConvertPayload;
            }

            return response;
        }
        catch (Exception ex)
        {
            return DefaultErrors.UnhandledExpection(ex.Message);
        }
    }
}
