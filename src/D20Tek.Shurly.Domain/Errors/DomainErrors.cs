//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;

namespace D20Tek.Shurly.Domain.Errors;

public static class DomainErrors
{
    public static Error EntityPropertyEmpty(string name) => Error.Validation(
        $"{name}.Empty",
        $"{name} cannot be empty.");

    public static Error EntityPropertyTooLong(string name) => Error.Validation(
        $"{name}.TooLong",
        $"{name} is too long.");

    public static Error EntityNotFound(string name, Guid id) => Error.NotFound(
        $"{name}.NotFound",
        $"Cannot find the entity with Id: {id}.");

    public static Error LongUrlInvalidFormat = Error.Validation(
        "LongUrl.InvalidFormat",
        "The specified LongUrl is not a valid URL format.");

    public static readonly Error OwnerIdInvalid = Error.Validation(
        "OwnerId.Empty",
        "The specified owner account id is invalid.");

    public static Error ShortUrlNotFound = Error.NotFound(
        "ShortenedUrl.NotFound",
        "Cannot find the short url with the specified code.");

    public static Error ShortUrlNotOwner = Error.Forbidden(
        "ShortenedUrl.NotOwner",
        "Cannot operate on a short url entity that user does not own.");

    public static Error CreateFailed = Error.Failure(
        "ShortenedUrl.CreateFailed",
        "Unable to create the specified short url.");

    public static Error UpdateFailed = Error.Failure(
        "ShortenedUrl.UpdateFailed",
        "Unable to update the specified short url.");

    public static Error DeleteFailed = Error.Failure(
        "ShortenedUrl.DeleteFailed",
        "Unable to delete the specified short url entity.");
}
