using S3PresignedApi.Common.Output;

namespace S3PresignedApi.Common.Handle;

public interface IHandler<in T>
{
    Effect Handle(T request);
}