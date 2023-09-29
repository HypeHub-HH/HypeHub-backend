﻿using AutoMapper;
using Azure.Core;
using FluentValidation;
using HypeHubDAL.Exeptions;
using HypeHubDAL.Repositories;
using HypeHubDAL.Repositories.Interfaces;
using HypeHubLogic.DTOs.Item;
using MediatR;

namespace HypeHubLogic.CQRS.Item.Commands.Update;

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ItemGenerallReadDTO>
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<ItemUpdateDTO> _validator;
    public UpdateItemCommandHandler(IItemRepository itemRepository, IMapper mapper, IValidator<ItemUpdateDTO> validator)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<ItemGenerallReadDTO> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request.Item);
        if (!validationResult.IsValid) throw new ValidationFailedException("Validation failed", validationResult.Errors.Select(error => error.ErrorMessage));
        var itemForUpdate = await _itemRepository.GetByIdAsync(request.Item.Id) ?? throw new NotFoundException($"There is no item with the given Id: {request.Item.Id}.");
        var update = request.Item;

        itemForUpdate.Name = update.Name;
        itemForUpdate.CloathingType = update.CloathingType;
        itemForUpdate.Brand = update.Brand;
        itemForUpdate.Model = update.Model;
        itemForUpdate.Colorway = update.Colorway;
        itemForUpdate.Price = update.Price;
        itemForUpdate.PurchaseDate = update.PurchaseDate;

        var item = await _itemRepository.UpdateAsync(itemForUpdate);
        var updatedItem = _mapper.Map<ItemGenerallReadDTO>(item);

        return updatedItem;
    }
}