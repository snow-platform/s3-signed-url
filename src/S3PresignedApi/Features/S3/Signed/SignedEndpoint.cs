using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using S3PresignedApi.Common.Endpoint;
using S3PresignedApi.Common.Handle;
using S3PresignedApi.Common.Output;

namespace S3PresignedApi.Features.S3.Signed;

public class SignedEndpoint : IMinimalEndpoint
{
    public static void Endpoint(WebApplication app)
    {
        var apiVersion = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .Build();

        app.MapPost("/api/v{version:apiVersion}/s3/signed",
                async ([FromBody] SignedUrlInput urlInput, IHandlerAsync<SignedUrlInput> handler) =>
                {
                    var result = await handler.HandleAsync(urlInput);

                    if (result is EffectError error)
                    {
                        return Results.Problem(statusCode: error.ErrorCode,
                            title: error.ErrorTitle,
                            detail: error.ErrorDescription);
                    }

                    if (result is not EffectOk<SignedUrlOutput> effect)
                    {
                        return Results.Problem(statusCode: StatusCodes.Status500InternalServerError,
                            title: "Unknown Error",
                            detail: "Unable to create signed URL");
                    }

                    return Results.Ok(effect.Value);
                })
            .WithApiVersionSet(apiVersion)
            .HasApiVersion(1);
    }
}