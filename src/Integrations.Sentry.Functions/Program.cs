using System.Net.Http.Headers;
using FluentValidation;
using Integrations.Core;
using Integrations.Core.Contracts;
using Integrations.Core.Models;
using Integrations.Infrastructure;
using Integrations.Infrastructure.Repositories;
using Integrations.Sentry.Client;
using Integrations.Sentry.Client.Contracts;
using Integrations.Sentry.Client.Models;
using Integrations.Sentry.Functions.Contracts;
using Integrations.Sentry.Functions.Factories;
using Integrations.Sentry.Functions.Models;
using Integrations.Sentry.Functions.Processors;
using Integrations.Sentry.Functions.Services;
using Integrations.Sentry.Functions.Settings;
using Integrations.Sentry.Functions.Validators;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication()
    .Configuration.AddUserSecrets<Program>();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

// Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    opts.UseSqlServer(connectionString, sqlOpts =>
    {
        sqlOpts.EnableRetryOnFailure();
        sqlOpts.CommandTimeout(30);
    });
});

// Configuration
var sentrySettings = builder.Configuration.GetSection(SentrySettings.SectionName).Get<SentrySettings>();
if (sentrySettings == null) throw new InvalidOperationException("Sentry settings are not configured.");
builder.Services.AddSingleton(sentrySettings);

// Services
builder.Services.AddScoped<IEventAggregationRepository<EventAggregation>, EventAggregationRepository>();
builder.Services.AddHttpClient<ISentryHttpClient, SentryHttpClient>(opts =>
{
    opts.BaseAddress = new Uri(sentrySettings.BaseUrl ?? throw new InvalidOperationException("Sentry BaseUrl is not configured."));
    opts.Timeout = TimeSpan.FromSeconds(10);
    opts.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sentrySettings.ApiKey);
}).AddStandardResilienceHandler();
builder.Services.AddScoped<IProcessorFactory<IProcessor<Result<EventAggregationProcessorResponse>>>, SentryProcessorFactory>();
builder.Services.AddScoped<IProcessor<Result<EventAggregationProcessorResponse>>, SentryEventAggregationAllProcessor>();
builder.Services.AddScoped<IValidator<EventAggregationRequest>, EventAggregationRequestValidator>();
builder.Services.AddScoped<ISentryService, SentryService>();

builder.Build().Run();