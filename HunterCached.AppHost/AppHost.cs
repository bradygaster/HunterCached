#pragma warning disable ASPIREAZUREREDIS001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets(typeof(Program).Assembly);

var azureRedisResource = builder.AddParameter("azureRedisResourceName");
var azureRedisResourceGroup = builder.AddParameter("azureRedisResourceGroup");

var cache = builder.AddAzureRedisEnterprise("cache")
                   .AsExisting(azureRedisResource, azureRedisResourceGroup);

var apiService = builder.AddProject<Projects.HunterCached_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.HunterCached_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
#pragma warning restore ASPIREAZUREREDIS001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
