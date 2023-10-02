//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Endpoints.Configuration;
using D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal class Configuration
{
    public class ShortUrl
    {
        private const string BaseUrl = "/api/v1/short-url";
        private static readonly string[] GroupTags = { "Shortened Url" };

        public static ApiEndpointConfig Create = new(
            BaseUrl,
            "CreateShortUrl",
            "Create Shortened Url",
            Summary: "Creates a shortened Url authored by the logged in user.",
            Tags: GroupTags,
            RequiresAuthorization: true,
            Produces: new[]
            {
                Config.Produces<ShortenedUrlResponse>(StatusCodes.Status201Created),
                Config.ProducesProblem(StatusCodes.Status401Unauthorized),
                Config.ProducesValidationProblem(StatusCodes.Status400BadRequest)
            });

        public static ApiEndpointConfig GetById = new(
            $"{BaseUrl}/{{id:Guid}}",
            "GetById",
            "Get Url By Id",
            Summary: "Gets the ShortenedUrl asset based on its ShortUrlId.",
            Tags: GroupTags,
            RequiresAuthorization: true,
            Produces: new[]
            {
                Config.Produces<ShortenedUrlResponse>(StatusCodes.Status200OK),
                Config.ProducesProblem(StatusCodes.Status404NotFound),
                Config.ProducesProblem(StatusCodes.Status401Unauthorized)
            });

        public static ApiEndpointConfig GetByShortCode = new(
            "/{shortCode}",
            "GetByShortCode",
            "Get Url By ShortCode",
            Summary: "Gets the short code from the Url and redirects to the full Url.",
            Tags: GroupTags,
            Produces: new[]
            {
                Config.Produces<string>(StatusCodes.Status307TemporaryRedirect, "text/plain"),
                Config.ProducesProblem(StatusCodes.Status404NotFound)
            });
    }
}
