//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Domain.Validations;
using D20Tek.Minimal.Result;
using D20Tek.Shurly.Domain.Errors;
using D20Tek.Shurly.Domain.ShortenedUrl;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.Update;

internal class UpdateShortenedUrlCommandValidator : IValidator<UpdateShortenedUrlCommand>
{
    public ValidationsResult Validate(UpdateShortenedUrlCommand command)
    {
        var result = new ValidationsResult();

        result.AddOnFailure(
        () => command.LongUrl.NotEmpty(),
            DomainErrors.EntityPropertyEmpty(nameof(command.LongUrl)));

        result.AddOnFailure(
            () => command.LongUrl.InMaxLength(LongUrl.MaxLength),
            DomainErrors.EntityPropertyTooLong(nameof(command.LongUrl)));

        result.AddOnFailure(
        () => command.Summary.NotEmpty(),
            DomainErrors.EntityPropertyEmpty(nameof(command.Summary)));

        result.AddOnFailure(
            () => command.Summary.InMaxLength(Summary.MaxLength),
            DomainErrors.EntityPropertyTooLong(nameof(command.Summary)));

        result.AddOnFailure(
        () => command.OwnerId.NotEmpty(),
            DomainErrors.OwnerIdInvalid);

        return result;
    }
}
