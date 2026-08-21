namespace S3PresignedApi.Common.Output;

public record EffectError(int ErrorCode, string? ErrorTitle, string? ErrorDescription) : Effect;