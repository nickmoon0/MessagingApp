﻿using MessagingApp.Application.Common.Exceptions;
using MessagingApp.Application.Common.Interfaces.Repositories;
using MessagingApp.Domain.Entities;
using MessagingApp.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MessagingApp.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthContext _authContext;
    public UserRepository(AuthContext authContext)
    {
        _authContext = authContext;
    }
    public User? GetUserById(Guid id)
    {
        throw new NotImplementedException();
        // var user = _authContext.Users.SingleOrDefault(x => x.Id == id);
        // return user;
    }

    public User? GetUserByUsername(string username)
    {
        throw new NotImplementedException();
        
        // var user = _authContext.Users.SingleOrDefault(x => x.Username == username);
        // return user;
    }

    public Guid CreateUser(User user)
    {
        throw new NotImplementedException();
        
        // try
        // {
        //     _authContext.Users.Add(user);
        //     _authContext.SaveChanges();
        //
        //     return user.Id;
        // }
        // catch (DbUpdateException)
        // {
        //     throw new EntityAlreadyExistsException($"User with username {user.Username} already exists");
        // }
    }
}