using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WorkerService1;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddHostedService<ApiDbInitializer>();

builder.Services.AddDbContextPool<ApiDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Todos"), sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("WebApplication1");
        // Workaround for https://github.com/dotnet/aspire/issues/1023
        sqlOptions.ExecutionStrategy(c => new RetryingSqlServerRetryingExecutionStrategy(c));
    }));
builder.EnrichSqlServerDbContext<ApiDb>(settings =>
    // Disable Aspire default retries as we're using a custom execution strategy
    settings.DisableRetry = true);

var app = builder.Build();
app.Run();
