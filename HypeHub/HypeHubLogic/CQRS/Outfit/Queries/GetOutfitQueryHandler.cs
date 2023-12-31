﻿using AutoMapper;
using HypeHubDAL.Exeptions;
using HypeHubDAL.Repositories.Interfaces;
using HypeHubLogic.DTOs.Outfit;
using MediatR;

namespace HypeHubLogic.CQRS.Outfit.Queries;
public class GetOutfitQueryHandler : IRequestHandler<GetOutfitQuery, OutfitGenerallReadDTO>
{
    private readonly IOutfitRepository _outfitRepository;
    private readonly IMapper _mapper;
    public GetOutfitQueryHandler(IOutfitRepository outfitRepository, IMapper mapper)
    {
        _outfitRepository = outfitRepository;
        _mapper = mapper;
    }
    public async Task<OutfitGenerallReadDTO> Handle(GetOutfitQuery request, CancellationToken cancellationToken)
    {
        var outfit = await _outfitRepository.GetByIdAsync(request.OutfitId) ?? throw new NotFoundException($"There is no outfit with the given Id: {request.OutfitId}.");
        return _mapper.Map<OutfitGenerallReadDTO>(outfit);
    }
}