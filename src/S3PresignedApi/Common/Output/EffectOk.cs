namespace S3PresignedApi.Common.Output;

public record EffectOk(int Code) : Effect;
public record EffectOk<T>(int Code, T Value) : Effect;