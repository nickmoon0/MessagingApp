using System.Linq.Expressions;
using System.Reflection;
using MessagingApp.Domain.Common;

namespace MessagingApp.Test.Common;

public static class Helpers
{
    /// <summary>
    /// Sets a Guid on domain entities. Can't do this manually because domain entities/aggregates
    /// have a private set on their ID.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    public static void SetId<T>(T entity, Guid id)
    {
        SetProperty(entity, nameof(IPersistedObject.Id), id);
    }

    /// <summary>
    /// Sets a property value on a given entity. Useful for properties with inaccessible setters.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="propertyName"></param>
    /// <param name="value"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public static void SetProperty<TEntity, TValue>(TEntity entity, string propertyName, TValue value)
    {
        if (entity is null || value is null) return;

        var propertyInfo = entity.GetType().GetProperty(propertyName,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(entity, value, null);
        }
    }
    
    public static MethodInfo GetPrivateMethodInfo<T>(T entity, string methodName) where T : notnull
    {
        var methodInfo = entity.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo == null)
            throw new InvalidOperationException($"Method {methodName} does not exist");
        
        return methodInfo;
    }
}