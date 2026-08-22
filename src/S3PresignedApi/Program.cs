using Polly;
using Polly.Retry;
using S3PresignedApi;
using S3PresignedApi.Extension;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// version
builder.Services.AddVersionDefault();

// handlers
builder.Services.AddHandlers();

builder.Services.AddAmazonS3();

// resilience
builder.Services.AddResiliencePipeline("signed", static x =>
{
    x.AddRetry(new RetryStrategyOptions
    {
        MaxRetryAttempts = 2,
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true,
        MaxDelay = TimeSpan.FromSeconds(5)
    });
    x.AddTimeout(TimeSpan.FromSeconds(30));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapMinimalEndpoint();

app.Run();