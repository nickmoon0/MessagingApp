using System.Reflection;
using MessagingApp.Domain.Aggregates;
using MessagingApp.Domain.Entities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using BindingFlags = System.Reflection.BindingFlags;

namespace MessagingApp.Test.Common;

/// <summary>
/// Contains methods for creating objects when its not important how the objects are created
/// (e.g., Hashed passwords are not typically required for most unit tests so when creating
/// a user you can dramatically speed up tests by not hashing a password)
/// </summary>
public static class DomainObjectFactory
{
    public static User CreateUser(
        Guid? id = null,
        bool? active = true,
        string? username = null,
        string? hashedPassword = null,
        string? bio = null,
        ICollection<Conversation>? conversations = null,
        ICollection<FriendRequest>? friendRequests = null,
        ICollection<User>? friends = null)
    {
        var constructor = GetConstructor<User>();
        var user = (User)constructor.Invoke([]);

        if (id != null) Helpers.SetProperty(user, nameof(user.Id), id);
        if (active != null) Helpers.SetProperty(user, nameof(user.Active), active);
        if (username != null) Helpers.SetProperty(user, nameof(user.Username), username);
        if (hashedPassword != null) Helpers.SetProperty(user, nameof(user.HashedPassword), hashedPassword);
        if (bio != null) Helpers.SetProperty(user, nameof(user.Bio), bio);
        if (conversations != null) Helpers.SetProperty(user, nameof(user.Conversations), conversations);
        if (friendRequests != null) Helpers.SetProperty(user, nameof(user.FriendRequests), friendRequests);
        if (friends != null) Helpers.SetProperty(user, nameof(user.Friends), friends);

        return user;
    }
    
    private static ConstructorInfo GetConstructor<T>()
    {
        var type = typeof(T);
        var constructor = type.GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            Type.EmptyTypes,
            null);

        if (constructor == null)
            throw new InvalidOperationException("Failed to retrieve user constructor");

        return constructor;
    }
}