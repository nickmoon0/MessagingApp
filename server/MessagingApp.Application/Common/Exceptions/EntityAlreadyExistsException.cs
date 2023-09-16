﻿namespace MessagingApp.Application.Common.Exceptions;

public class EntityAlreadyExistsException : Exception
{
    public EntityAlreadyExistsException(string message) : base(message) { }
}