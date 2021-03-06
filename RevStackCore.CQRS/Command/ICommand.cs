﻿using System;
using RevStackCore.CQRS.Message;


namespace RevStackCore.CQRS.Command
{
    public interface ICommand : IMessage
    {
        int AggregateId { get; }
    }
}
