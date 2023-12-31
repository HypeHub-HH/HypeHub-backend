﻿using HypeHubLogic.DTOs.AccountItemLike;
using MediatR;
using System.Security.Claims;

namespace HypeHubLogic.CQRS.Item.Commands.Update;
public class LikeOrUnlikeItemCommand : IRequest<List<AccountItemLikeReadDTO>>
{
    public Guid ItemId { get; init; }
    public IEnumerable<Claim> Claims { get; init; }
    public LikeOrUnlikeItemCommand(Guid itemId, IEnumerable<Claim> claims)
    {
        ItemId = itemId;
        Claims = claims;
    }
}
