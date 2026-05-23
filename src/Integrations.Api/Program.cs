using System.Net.Http.Headers;
using Integrations.Api.Settings;
using Integrations.Core.Contracts;
using Integrations.Core.Models;
using Integrations.Infrastructure;
using Integrations.Infrastructure.Repositories;
using Integrations.Sentry.Client;
using Integrations.Sentry.Client.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Integrations.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

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

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        // app.UseAuthorization();

        // Map application endpoints
        app.MapEventAggregationEndpoints();

        app.Run();
    }
}