using AutoMapper;
using Laksanda.API.Application.DTOs.Recipes;
using Laksanda.API.Domain.Entities;

namespace Laksanda.API.Application.Mapping;

public class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        CreateMap<CreateRecipeItemRequest, RecipeItem>();
        CreateMap<UpdateRecipeItemRequest, RecipeItem>();

        CreateMap<RecipeItem, RecipeItemDto>()
            .ForMember(dest => dest.RawMaterialCode, opt => opt.MapFrom(src => src.RawMaterial != null ? src.RawMaterial.MaterialCode : string.Empty))
            .ForMember(dest => dest.RawMaterialName, opt => opt.MapFrom(src => src.RawMaterial != null ? src.RawMaterial.MaterialName : string.Empty));

        CreateMap<Recipe, RecipeDto>();
    }
}
