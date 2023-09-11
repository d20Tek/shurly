//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual;
using D20Tek.Shurly.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentationServices()
                .AddIndividualAuthentication(builder.Configuration);

var app = builder.Build();

app.ConfigureMiddlewarePipeline(app.Environment);

app.MapGet("/", () => "Shurly Api - Link shortening and management");
app.Run();
