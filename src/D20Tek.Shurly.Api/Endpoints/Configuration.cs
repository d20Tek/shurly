﻿//---------------------------------------------------------------------------------------------------------------------
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

        public static ApiEndpointConfig GetByOwner = new(
            BaseUrl,
            "GetByOwner",
            "Get Urls By Ower",
            Summary: "Gets the list of ShortenedUrls for the owner.",
            Tags: GroupTags,
            RequiresAuthorization: true,
            Produces: new[]
            {
                Config.Produces<IEnumerable<ShortenedUrlResponse>>(StatusCodes.Status200OK),
                Config.ProducesProblem(StatusCodes.Status401Unauthorized)
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
                Config.ProducesProblem(StatusCodes.Status403Forbidden),
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

        public static ApiEndpointConfig Update = new(
            $"{BaseUrl}/{{id:Guid}}",
            "UpdateShortUrl",
            "Update Shortened Url",
            Summary: "Updates the ShortenedUrl asset based on its ShortUrlId.",
            Tags: GroupTags,
            RequiresAuthorization: true,
            Produces: new[]
            {
                Config.Produces<ShortenedUrlResponse>(StatusCodes.Status200OK),
                Config.ProducesProblem(StatusCodes.Status404NotFound),
                Config.ProducesProblem(StatusCodes.Status401Unauthorized),
                Config.ProducesProblem(StatusCodes.Status403Forbidden),
                Config.ProducesValidationProblem(StatusCodes.Status400BadRequest)
            });

        public static ApiEndpointConfig Publish = new(
            $"{BaseUrl}/{{id:Guid}}/publish",
            "PublishShortUrl",
            "Publish Shortened Url",
            Summary: "Updates the ShortenedUrl state flag to Published based on its ShortUrlId.",
            Tags: GroupTags,
            RequiresAuthorization: true,
            Produces: new[]
            {
                Config.Produces<ShortenedUrlResponse>(StatusCodes.Status200OK),
                Config.ProducesProblem(StatusCodes.Status404NotFound),
                Config.ProducesProblem(StatusCodes.Status401Unauthorized),
                Config.ProducesProblem(StatusCodes.Status403Forbidden)
            });

        public static ApiEndpointConfig Unpublish = new(
            $"{BaseUrl}/{{id:Guid}}/unpublish",
            "UnpublishShortUrl",
            "Unpublish Shortened Url",
            Summary: "Updates the ShortenedUrl state flag to Obsolete based on its ShortUrlId.",
            Tags: GroupTags,
            RequiresAuthorization: true,
            Produces: new[]
            {
                Config.Produces<ShortenedUrlResponse>(StatusCodes.Status200OK),
                Config.ProducesProblem(StatusCodes.Status404NotFound),
                Config.ProducesProblem(StatusCodes.Status401Unauthorized),
                Config.ProducesProblem(StatusCodes.Status403Forbidden)
            });

        public static ApiEndpointConfig Delete = new(
            $"{BaseUrl}/{{id:Guid}}",
            "DeleteShortUrl",
            "Delete Shortened Url",
            Summary: "Deletes the ShortenedUrl asset based on its ShortUrlId.",
            Tags: GroupTags,
            RequiresAuthorization: true,
            Produces: new[]
            {
                Config.Produces<ShortenedUrlResponse>(StatusCodes.Status200OK),
                Config.ProducesProblem(StatusCodes.Status404NotFound),
                Config.ProducesProblem(StatusCodes.Status403Forbidden),
                Config.ProducesProblem(StatusCodes.Status401Unauthorized)
            });
    }
}
