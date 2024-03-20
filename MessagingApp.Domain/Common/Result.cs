namespace MessagingApp.Domain.Common;

public readonly struct Result<TSuccess, TError> 
    where TSuccess : class?
    where TError : class? 
{
    public readonly TSuccess Value;
    public readonly TError Error;

    private Result(TSuccess v, TError e, bool success)
    {
        Value = v;
        Error = e;
        IsOk = success;
    }

    public bool IsOk { get; }

    public static Result<TSuccess, TError> Ok(TSuccess v)
    {
        return new Result<TSuccess, TError>(v, default!, true);
    }

    public static Result<TSuccess, TError> Err(TError e)
    {
        return new Result<TSuccess, TError>(default!, e, false);
    }

    public static implicit operator Result<TSuccess, TError>(TSuccess v) => new(v, default!, true);
    public static implicit operator Result<TSuccess, TError>(TError e) => new(default!, e, false);

    public TReturn Match<TReturn>(Func<TSuccess, TReturn> success, Func<TError, TReturn> failure) =>
        IsOk ? success(Value) : failure(Error);

    public TReturn Match<TReturn>(Func<TSuccess, TReturn> success) where TReturn : class? 
        => IsOk ? success(Value) : default!;

    public TReturn Match<TReturn>(Func<TError, TReturn> failure) where TReturn : class?
        => IsOk ? default! : failure(Error);
}