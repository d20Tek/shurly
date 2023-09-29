//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Shurly.Application.Abstractions;

public interface IUrlShorteningService
{
    public Task<string> GenerateUniqueCodeAsync(Guid? guid = null);
}
