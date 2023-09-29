﻿using AutoMapper;
using HypeHubDAL.Exeptions;
using HypeHubDAL.Repositories.Interfaces;
using HypeHubLogic.DTOs.Outfit;
using MediatR;

namespace HypeHubLogic.CQRS.Outfit.Queries;

public class GetLatestOutfitsQueryHandler : IRequestHandler<GetLatestOutfitsQuery, OutfitWithAccountAndImagesAndLikesCountReadDTO>
{
    private readonly IOutfitRepository _outfitRepository;
    private readonly IMapper _mapper;

    public GetLatestOutfitsQueryHandler(IOutfitRepository outfitRepository, IMapper mapper)
    {
        _outfitRepository = outfitRepository;
        _mapper = mapper;
    }

    public async Task<OutfitWithAccountAndImagesAndLikesCountReadDTO> Handle(GetLatestOutfitsQuery request, CancellationToken cancellationToken)
    {
        if (request.Page <= 0 || request.Count <= 0) throw new BadRequestException("Page and count parameter must be greater than zero");
        var outfits = await _outfitRepository.GetLatesOutfitsWithAccountsAndImagesAndLikes(request.Page, request.Count);
        return _mapper.Map<OutfitWithAccountAndImagesAndLikesCountReadDTO>(outfits);
    }
}