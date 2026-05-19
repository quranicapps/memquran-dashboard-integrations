using Integrations.Core.Contracts;
using Integrations.Core.Models;
using Integrations.Infrastructure;
using Integrations.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Integrations.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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
        
        // Services
        builder.Services.AddScoped<IEventAggregationRepository<EventAggregation>, EventAggregationRepository>();

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