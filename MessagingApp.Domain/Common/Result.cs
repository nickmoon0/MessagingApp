namespace MessagingApp.Domain.Common;

public readonly struct Result<TSuccess> where TSuccess : class?
{
    public readonly TSuccess Value;
    public readonly Exception Error;
    
    public bool IsOk { get; }

    private Result(TSuccess value, Exception error, bool success)
    {
        Value = value;
        Error = error;
        IsOk = success;
    }

    public static implicit operator Result<TSuccess>(TSuccess value) => new(value, default!, true);
    public static implicit operator Result<TSuccess>(Exception exception) => new(default!, exception, false);

    public TReturn Match<TReturn>(Func<TSuccess, TReturn> success, Func<Exception, TReturn> failure) =>
        IsOk ? success(Value) : failure(Error);
    
    public TReturn Match<TReturn>(Func<TSuccess, TReturn> success) where TReturn : class? 
        => IsOk ? success(Value) : default!;

    public TReturn Match<TReturn>(Func<Exception, TReturn> failure) where TReturn : class?
        => IsOk ? default! : failure(Error);
}