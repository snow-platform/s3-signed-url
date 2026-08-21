using S3PresignedApi.Common.Output;

namespace S3PresignedApi.Common.Handle;

public interface IHandlerAsync<in T>
{
    Task<Effect> HandleAsync(T request);
}