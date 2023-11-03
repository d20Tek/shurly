//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Shurly.Application.UseCases.ShortenedUrls;

namespace D20Tek.Shurly.Api.Endpoints.ShortenedUrls;

internal sealed class ShortenedUrlListResponseMapper
{
    private readonly ShortenedUrlResponseMapper _instanceMapper;

    public ShortenedUrlListResponseMapper(ShortenedUrlResponseMapper instanceMapper)
    {
        _instanceMapper = instanceMapper;
    }

    public ShortenedUrlListResponse Map(ShortenedUrlList source, HttpContext httpContext)
    {
        var baseLink = Configuration.ShortUrl.GetByOwner.RoutePattern;
        var links = new List<LinkMetadata>();

        if (source.Skip > 0)
        {
            var prevSkip = Math.Max(source.Skip - source.Take, 0);
            links.Add(new LinkMetadata(
                "prevLink",
                baseLink + $"?skip={prevSkip}; take={source.Take}"));
        }

        var nextSkip = source.Skip + source.Take;
        if (nextSkip < source.TotalItems)
        {
            links.Add(new LinkMetadata(
                "nextLink",
                baseLink + $"?skip={nextSkip}; take={source.Take}"));
        }

        links.Add(new LinkMetadata("firstLink", baseLink));
        var lastSkip = (source.TotalItems / source.Take) * source.Take;
        links.Add(new LinkMetadata(
            "lastLink",
            baseLink + $"?skip={lastSkip}; take={source.Take}"));

        var items = source.Items.Select(x => _instanceMapper.Map(x, httpContext))
                                .ToList();


        return new ShortenedUrlListResponse(
            new ListMetadata(source.TotalItems, items.Count(), source.Skip, source.Take),
            links,
            items);
    }
}
