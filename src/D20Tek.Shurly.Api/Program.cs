//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual;
using D20Tek.Minimal.Endpoints;
using D20Tek.Shurly.Api;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentationServices()
        .AddIndividualAuthentication(builder.Configuration);
}

var app = builder.Build();
{
    app.ConfigureMiddlewarePipeline(app.Environment);

    app.MapApiEndpoints();
}

app.Run();

[ExcludeFromCodeCoverage]
partial class Program { }