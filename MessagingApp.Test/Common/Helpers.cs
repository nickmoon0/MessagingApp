using System.Reflection;

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
        if (entity is null) return;
        
        var propertyInfo = entity.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(entity, id, null);
        }
    }
}