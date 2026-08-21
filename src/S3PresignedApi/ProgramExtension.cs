using Amazon.S3;
using Asp.Versioning;
using S3PresignedApi.Common.Handle;
using S3PresignedApi.Features.S3.Signed;

namespace S3PresignedApi;

public static class ProgramExtension
{
    extension(IServiceCollection service)
    {
        public IServiceCollection AddVersionDefault()
        {
            service.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
                x.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            return service;
        }

        public IServiceCollection AddHandlers()
        {
            service.AddScoped<IHandlerAsync<SignedUrlInput>, SignedHandler>();

            return service;
        }

        public IServiceCollection AddAmazonS3()
        {
            var accessKeyId = Environment.GetEnvironmentVariable("R2_ACCESS_KEY_ID");
            var accessKeySecret = Environment.GetEnvironmentVariable("R2_ACCESS_KEY_SECRET");
            var r2Endpoint = Environment.GetEnvironmentVariable("R2_ENDPOINT");

            if (accessKeyId is null or ""
                || accessKeySecret is null or ""
                || r2Endpoint is null or "")
            {
                throw new Exception("The R2_ACCESS_KEY_ID, R2_ACCESS_KEY_SECRET, and R2_ENDPOINT environment variables are required.");
            }

            service.AddSingleton<IAmazonS3>(x => new AmazonS3Client(accessKeyId, accessKeySecret, new AmazonS3Config
            {
                ServiceURL = r2Endpoint,
                AuthenticationRegion = "auto",
                ForcePathStyle = true,
            }));

            return service;
        }
    }
}