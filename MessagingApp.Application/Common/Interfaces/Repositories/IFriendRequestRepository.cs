﻿using MessagingApp.Domain.Entities;

namespace MessagingApp.Application.Common.Interfaces.Repositories;

public interface IFriendRequestRepository
{
    public Task<List<FriendRequest>> GetSentFriendRequests(User user);
    public Task<List<FriendRequest>> GetReceivedFriendRequests(User user);
    public Task SetFriendRequestStatus(FriendRequest friendRequest);
}