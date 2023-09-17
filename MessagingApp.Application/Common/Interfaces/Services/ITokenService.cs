﻿using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Common.Interfaces.Services;

public interface ITokenService
{
    public string GenerateToken(User user);
}