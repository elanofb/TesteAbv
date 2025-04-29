using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog.Templates;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Ambev.DeveloperEvaluation.Common.Logging;

/// <summary> Add default Logging configuration to project. This configuration supports Serilog logs with DataDog compatible output.</summary>
public static class LoggingExtension
{
    private static readonly DestructuringOptionsBuilder _destructuringOptionsBuilder = new DestructuringOptionsBuilder()
        .WithDefaultDestructurers()
        .WithDestructurers(new[] { new DbUpdateExceptionDestructurer() });

    private static readonly Func<LogEvent, bool> _filterPredicate = exclusionPredicate =>
    {
        if (exclusionPredicate.Level != LogEventLevel.Information) return true;

        exclusionPredicate.Properties.TryGetValue("StatusCode", out var statusCode);
        exclusionPredicate.Properties.TryGetValue("Path", out var path);

        var excludeByStatusCode = statusCode == null || statusCode.ToString().Equals("200");
        var excludeByPath = path?.ToString().Contains("/health") ?? false;

        return excludeByStatusCode && excludeByPath;
    };

    public static WebApplicationBuilder AddDefaultLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration().CreateLogger();
        
        builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
        {
            loggerConfiguration
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.WithThreadId()
                .Enrich.WithProcessId()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithMemoryUsage()
                .Enrich.WithExceptionDetails(_destructuringOptionsBuilder)
                .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
                .Enrich.WithProperty("Application", builder.Environment.ApplicationName)
                .Filter.ByExcluding(_filterPredicate);

            //Vai logar apenas quando tiver rodando sem debug.
            if (Debugger.IsAttached)
            {
                ConfigureDebugLogging(loggerConfiguration);
            }
            else
            {
                ConfigureProductionLogging(loggerConfiguration, hostingContext);
            }
        });

        return builder;
    }

    private static void ConfigureDebugLogging(LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration.WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
            theme: AnsiConsoleTheme.Code
        );
    }

    private static void ConfigureProductionLogging(LoggerConfiguration loggerConfiguration, HostBuilderContext context)
    {
        //var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var basePath = Directory.GetCurrentDirectory();
        var logPath = Path.Combine(basePath, "logs", $"log-{DateTime.UtcNow:yyyy-MM-dd}.txt");
        
        Directory.CreateDirectory(Path.GetDirectoryName(logPath));
        
        loggerConfiguration.WriteTo.File(
            path: logPath,
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30,
            buffered: false, // Desabilita o buffer para ver os logs imediatamente
            flushToDiskInterval: TimeSpan.FromSeconds(1)
        );

        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrEmpty(connectionString))
        {
            loggerConfiguration.WriteTo.PostgreSQL(
                connectionString,
                "Logs",
                needAutoCreateTable: true,
                respectCase: true
            );
        }
    }
}
