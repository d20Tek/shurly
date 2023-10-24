//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using D20Tek.Shurly.Web.Services.Contracts;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace D20Tek.Shurly.Web.Services;

internal class ShurlyApiService : ServiceBase
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ShurlyApiService(
        HttpClient httpClient,
        IOptions<ServiceEndpointSettings> endpointOptions,
        ILogger<ShurlyApiService> logger)
        : base(logger)
    {
        _httpClient = httpClient;
        _baseUrl = endpointOptions.Value.ShorturlEndpoint;
    }

    public async Task<Result<IEnumerable<ShortenedUrlResponse>>> GetByOwnerAsync()
    {
        var serviceUrl = $"{_baseUrl}{Configuration.Shurly.GetByOwner}";
        var result = await _httpClient.GetAsync(serviceUrl);
        if (result.IsSuccessStatusCode is false)
        {
            return Errors.ShortenedUrlService.GetShortUrlFailure;
        }

        var response = await result.Content.ReadFromJsonAsync<List<ShortenedUrlResponse>>();
        return response!;
    }

    public async Task<Result<ShortenedUrlResponse>> GetByIdAsync(string urlId)
    {
        var serviceUrl = $"{_baseUrl}/{urlId}";
        var result = await _httpClient.GetAsync(serviceUrl);
        if (result.IsSuccessStatusCode is false)
        {
            return Errors.ShortenedUrlService.GetShortUrlFailure;
        }

        var response = await result.Content.ReadFromJsonAsync<ShortenedUrlResponse>();
        return response!;
    }

    public async Task<Result> DeleteAsync(string urlId)
    {
        var serviceUrl = $"{_baseUrl}/{urlId}";
        var result = await _httpClient.DeleteAsync(serviceUrl);
        if (result.IsSuccessStatusCode is false)
        {
            return Errors.ShortenedUrlService.DeleteShortUrlFailure;
        }

        return Result.Success();
    }
}
