using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);


// https://learn.microsoft.com/en-us/dotnet/aspire/database/sql-server-integration
var sqlServer = builder.AddSqlServer("sqlserver")
    .WithLifetime(ContainerLifetime.Persistent);

var db = sqlServer.AddDatabase("Todos");

var migrationService = builder.AddProject<Projects.WorkerService1>("migration")
    .WithReference(db)
    .WaitFor(db);

var weatherApi = builder.AddProject<Projects.WebApplication1>("WebApplication1")
    .WithReference(db)
    .WaitForCompletion(migrationService)
    .WithExternalHttpEndpoints();

builder.AddNpmApp("FrontEnd", "../angularoo")
        .WithReference(weatherApi)
        .WaitFor(weatherApi)
        .WithHttpEndpoint(env: "PORT")
        .WithExternalHttpEndpoints()
        .PublishAsDockerFile();


builder.Build().Run();
