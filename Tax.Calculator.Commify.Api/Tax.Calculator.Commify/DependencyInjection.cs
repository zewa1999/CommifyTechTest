using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Tax.Calculator.Commify.Business;
using Tax.Calculator.Commify.Data;
using Tax.Calculator.Commify.Data.Repositories;
using Tax.Calculator.Commify.Validators;

namespace Tax.Calculator.Commify;

public static class DependencyInjection
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<TaxCalculatorDbContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during migration: {ex.Message}");
            }
        }
    }

    public static IHostApplicationBuilder AddOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

        string apiToken = Environment.GetEnvironmentVariable("SEQ_API_TOKEN") ?? "H8bsdO5s2R4dIkmm2Fnj";
        string seqEndpoint = Environment.GetEnvironmentVariable("SEQ_API_ENDPOINT") ?? "http://commify-seq:80/ingest/otlp/v1/logs";

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.AddConsoleExporter();

            logging.AddOtlpExporter(a =>
            {
                a.Endpoint = new Uri(seqEndpoint);
                a.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                a.Headers = $"X-Seq-ApiKey={apiToken}";
            });

            logging.SetResourceBuilder(ResourceBuilder.CreateEmpty()
                .AddService("TaxCalculatorService"));
        });

        return builder;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();

        services.AddValidatorsFromAssemblyContaining<CreateTaxBandRequestValidator>();

        return services;
    }

    public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
    {
        var corsSection = configuration.GetSection("Cors:Origins");

        services.AddCors(options =>
        {
            options.AddPolicy(name: "AllowAll",
                policy =>
                {
                    policy.WithOrigins(corsSection.Get<string[]>()!)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
        });

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITaxCalculatorService, TaxCalculatorService>();
        services.AddScoped<ITaxBandService, TaxBandService>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITaxBandRepository, TaxBandRepository>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = GetPostgresConnectionString();

        services.AddDbContext<TaxCalculatorDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }

    private static string GetPostgresConnectionString()
    {
        string host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        string port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
        string database = Environment.GetEnvironmentVariable("DB_NAME") ?? "commify-db";
        string user = Environment.GetEnvironmentVariable("DB_USER") ?? "commify_admin";
        string password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "Secret123@";

        return $"Host={host};Port={port};Database={database};Username={user};Password={password};";
    }
}