﻿using HypeHubLogic.DTOs.AccountItemLike;
using MediatR;
using Microsoft.AspNetCore.Http.Features;

namespace HypeHubLogic.CQRS.Item.Commands.Post;

public class LikeOrUnlikeItemCommand : IRequest
{
    public AccountItemLikeCreateDTO AccountItemLike { get; init; }

    public LikeOrUnlikeItemCommand(AccountItemLikeCreateDTO accountItemLike)
    {
        AccountItemLike = accountItemLike;
    }
}
