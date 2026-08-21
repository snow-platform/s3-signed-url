using Amazon.S3;
using Amazon.S3.Model;
using Polly.Registry;
using S3PresignedApi.Common.Handle;
using S3PresignedApi.Common.Output;

namespace S3PresignedApi.Features.S3.Signed;

public class SignedHandler : IHandlerAsync<SignedUrlInput>
{
    private readonly IAmazonS3 _amazonS3;
    private readonly ResiliencePipelineProvider<string> _resiliencePipelineProvider;

    public SignedHandler(IAmazonS3 amazonS3, ResiliencePipelineProvider<string> resiliencePipelineProvider)
    {
        _amazonS3 = amazonS3;
        _resiliencePipelineProvider = resiliencePipelineProvider;
    }

    public async Task<Effect> HandleAsync(SignedUrlInput request)
    {
        var pipeline = _resiliencePipelineProvider.GetPipeline("signed");
        var signedGet = new GetPreSignedUrlRequest
        {
            BucketName = "db-backup",
            Key = request.Key,
            Expires = DateTime.UtcNow.AddMinutes(15),
            Verb = HttpVerb.PUT,
        };
        var signed = await pipeline.ExecuteAsync(async _ => await _amazonS3.GetPreSignedURLAsync(signedGet));

        return new EffectOk<SignedUrlOutput>(StatusCodes.Status200OK,
            new SignedUrlOutput(signed, signedGet.BucketName, signedGet.Key, signedGet.Expires ?? DateTime.UtcNow));
    }
}