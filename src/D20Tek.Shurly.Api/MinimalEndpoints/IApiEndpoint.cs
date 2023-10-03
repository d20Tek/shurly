﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Minimal.Endpoints;

public interface IApiEndpoint<TRequest, THandler> : IApiEndpoint
    where TRequest : IRequest<IResult>
{
    public Task<IResult> HandleAsync(
        TRequest request,
        THandler handler,
        CancellationToken cancellation);
}
