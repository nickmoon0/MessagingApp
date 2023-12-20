using MessagingApp.Domain.Exceptions;

namespace MessagingApp.Domain.Common;

public class ActionResult<T>
{
    public bool Success { get; init; }

    public DomainException? Error { get; init; }
    public T? Result { get; init; }

    public ActionResult(T obj)
    {
        Success = true;
        Result = obj;
    }
    
    public ActionResult(DomainException ex)
    {
        Success = false;
        Error = ex;
    }
}