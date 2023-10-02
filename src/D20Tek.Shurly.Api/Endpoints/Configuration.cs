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

        public static ApiEndpointConfig GetByShortCode = new(
            "/",
            "GetByShortCode",
            "Get Url By ShortCode",
            Summary: "Gets the short code from the Url and redirects to the full Url.",
            Tags: GroupTags,
            Produces: new[]
            {
                Config.Produces(StatusCodes.Status307TemporaryRedirect),
                Config.ProducesProblem(StatusCodes.Status404NotFound)
            });
    }
}
