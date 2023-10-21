//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Domain.Abstractions;
using D20Tek.Minimal.Domain.Validations;
using D20Tek.Minimal.Result;
using D20Tek.Shurly.Domain.Errors;
using D20Tek.Shurly.Domain.ShortenedUrl;

namespace D20Tek.Shurly.Application.UseCases.ShortenedUrls.Create;

internal class CreateShortenedUrlCommandValidator : IValidator<CreateShortenedUrlCommand>
{
    public ValidationsResult Validate(CreateShortenedUrlCommand command)
    {
        var result = new ValidationsResult();

        result.AddOnFailure(
        () => command.Title.NotEmpty(),
            DomainErrors.EntityPropertyEmpty(nameof(command.Title)));

        result.AddOnFailure(
            () => command.Title.InMaxLength(Title.MaxLength),
            DomainErrors.EntityPropertyTooLong(nameof(command.Title)));

        result.AddOnFailure(
        () => command.LongUrl.NotEmpty(),
            DomainErrors.EntityPropertyEmpty(nameof(command.LongUrl)));

        result.AddOnFailure(
            () => command.LongUrl.InMaxLength(LongUrl.MaxLength),
            DomainErrors.EntityPropertyTooLong(nameof(command.LongUrl)));

        if (command.Summary is not null)
        {
            result.AddOnFailure(
            () => command.Summary.NotEmpty(),
                DomainErrors.EntityPropertyEmpty(nameof(command.Summary)));

            result.AddOnFailure(
                () => command.Summary.InMaxLength(Summary.MaxLength),
                DomainErrors.EntityPropertyTooLong(nameof(command.Summary)));
        }

        result.AddOnFailure(
        () => command.CreatorId.NotEmpty(),
            DomainErrors.OwnerIdInvalid);

        return result;
    }
}
