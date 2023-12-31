﻿using HypeHubLogic.DTOs.Item;
using MediatR;

namespace HypeHubLogic.CQRS.Item.Queries;
public class GetItemQuery : IRequest<ItemWithImagesAndLikesReadDTO>
{
    public Guid ItemId { get; init; }
    public GetItemQuery(Guid itemId)
    {
        ItemId = itemId;
    }
}
