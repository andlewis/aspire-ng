﻿using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using WebApplication1.Models;

namespace WorkerService1
{

    public class ApiDbInitializer(
        IServiceProvider serviceProvider,
        IHostEnvironment hostEnvironment,
        IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
    {
        private readonly ActivitySource _activitySource = new(hostEnvironment.ApplicationName);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var activity = _activitySource.StartActivity(hostEnvironment.ApplicationName, ActivityKind.Client);

            try
            {
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApiDb>();

                await EnsureDatabaseAsync(dbContext, cancellationToken);
                await RunMigrationAsync(dbContext, cancellationToken);
            }
            catch (Exception ex)
            {
                activity?.AddException(ex);
                throw;
            }

            hostApplicationLifetime.StopApplication();
        }

        private static async Task EnsureDatabaseAsync(ApiDb dbContext, CancellationToken cancellationToken)
        {
            var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                // Create the database if it does not exist.
                // Do this first so there is then a database to start a transaction against.
                if (!await dbCreator.ExistsAsync(cancellationToken))
                {
                    await dbCreator.CreateAsync(cancellationToken);
                }
            });
        }

        private static async Task RunMigrationAsync(ApiDb dbContext, CancellationToken cancellationToken)
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                // Run migration in a transaction to avoid partial migration if it fails.
                //await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                await dbContext.Database.MigrateAsync(cancellationToken);
                //await transaction.CommitAsync(cancellationToken);
            });
        }
    }
}
