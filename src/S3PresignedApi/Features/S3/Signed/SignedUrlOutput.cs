namespace S3PresignedApi.Features.S3.Signed;

public record SignedUrlOutput(string Url, string Bucket, string Name, DateTime Expire);