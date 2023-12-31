﻿using AutoMapper;
using HypeHubDAL.Models;
using HypeHubDAL.Models.Relations;
using HypeHubLogic.DTOs.Account;
using HypeHubLogic.DTOs.AccountItemLike;
using HypeHubLogic.DTOs.AccountOutfitLike;
using HypeHubLogic.DTOs.Item;
using HypeHubLogic.DTOs.ItemImage;
using HypeHubLogic.DTOs.Outfit;
using HypeHubLogic.DTOs.OutfitImage;
using HypeHubLogic.DTOs.Registration;

namespace HypeHubLogic.Mappers;
public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Account, AccountGeneralInfoReadDTO>();
        CreateMap<Account, AccountWithOutfitsReadDTO>();
        CreateMap<AccountCreateDTO, Account>();

        CreateMap<AccountItemLike, AccountItemLikeReadDTO>();
        CreateMap<AccountItemLikeCreateDTO, AccountItemLike>();

        CreateMap<AccountOutfitLike, AccountOutfitLikeReadDTO>();
        CreateMap<AccountOutfitLikeCreateDTO, AccountOutfitLike>();

        CreateMap<Item, ItemGenerallReadDTO>();
        CreateMap<Item, ItemWithImagesAndLikesReadDTO>();
        CreateMap<ItemCreateDTO, Item>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));

        CreateMap<ItemImage, ItemImageReadDTO>();
        CreateMap<ItemImageCreateDTO, ItemImage>();

        CreateMap<Outfit, OutfitGenerallReadDTO>();
        CreateMap<Outfit, OutfitWithImagesAndLikesReadDTO>();
        CreateMap<Outfit, OutfitWithAccountAndImagesAndLikesReadDTO>();
        CreateMap<Outfit, OutfitWithAccountAndImagesAndLikesAndItemsReadDTO>();
        CreateMap<OutfitCreateDTO, Outfit>();
        CreateMap<OutfitImage, OutfitImageReadDTO>();
        CreateMap<OutfitImageCreateDTO, OutfitImage>();

        CreateMap<RegistrationCreateDTO, RegistrationReadDTO>();
    }
}
