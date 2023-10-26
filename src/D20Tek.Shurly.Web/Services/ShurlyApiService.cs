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
        return await InvokeServiceOperation<IEnumerable<ShortenedUrlResponse>>(async () =>
        {
            var serviceUrl = $"{_baseUrl}";
            return await _httpClient.GetAsync(serviceUrl);
        });
    }

    public async Task<Result<ShortenedUrlResponse>> GetByIdAsync(string urlId)
    {
        return await InvokeServiceOperation<ShortenedUrlResponse>(async () =>
        {
            var serviceUrl = $"{_baseUrl}/{urlId}";
            return await _httpClient.GetAsync(serviceUrl);
        });
    }

    public async Task<Result<ShortenedUrlResponse>> CreateAsync(CreateShortenedUrlRequest request)
    {
        return await InvokeServiceOperation<ShortenedUrlResponse>(async () =>
        {
            var serviceUrl = $"{_baseUrl}";
            return await _httpClient.PostAsJsonAsync<CreateShortenedUrlRequest>(
                serviceUrl,
                request);
        });
    }

    public async Task<Result<ShortenedUrlResponse>> UpdateAsync(
        string urlId,
        UpdateShortenedUrlRequest request)
    {
        return await InvokeServiceOperation<ShortenedUrlResponse>(async () =>
        {
            var serviceUrl = $"{_baseUrl}/{urlId}";
            return await _httpClient.PutAsJsonAsync<UpdateShortenedUrlRequest>(
                serviceUrl,
                request);
        });
    }

    public async Task<Result> DeleteAsync(string urlId)
    {
        return await InvokeServiceOperation<ShortenedUrlResponse>(async () =>
        {
            var serviceUrl = $"{_baseUrl}/{urlId}";
            return await _httpClient.DeleteAsync(serviceUrl);
        });
    }
}
