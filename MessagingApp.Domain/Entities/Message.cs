﻿using MessagingApp.Domain.Common;

namespace MessagingApp.Domain.Entities;

public class Message : IDomainObject
{
    public Guid Id { get; private set; }
    public bool Active { get; private set; }

    public string? Content { get; set; }
    public DateTime TimeStamp { get; set; }

    public Guid ConversationId { get; set; }
    public Guid SendingUserId { get; set; }
    public Guid ReceivingUserId { get; set; }
}