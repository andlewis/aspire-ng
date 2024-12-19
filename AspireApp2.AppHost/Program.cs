using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var weatherApi = builder.AddProject<Projects.WebApplication1>("WebApplication1")
    .WithExternalHttpEndpoints();

builder.AddNpmApp("FrontEnd", "../angularoo")
        .WithReference(weatherApi)
        .WaitFor(weatherApi)
        .WithHttpEndpoint(env: "PORT")
        .WithExternalHttpEndpoints()
        .PublishAsDockerFile();

builder.Build().Run();
